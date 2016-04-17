using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Sambucks.Controllers;
using Sambucks.Data;
using Sambucks.Models;
using Sambucks.Services;

namespace Sambucks.Controllers
{
    public class OrderController : BaseApiController
    {
        private readonly ISambucksIdentityService _identityService;

        const int PageSize = 10;

        public OrderController(ISambucksRepository repo, ISambucksIdentityService identityService)
            : base(repo)
        {
            _identityService = identityService;
        }

        [Authorize(Roles = "Barista")]
        public IEnumerable<OrderModel> Get(bool includeCompleted = false, int page = 0)
        {
            IEnumerable<OrderModel> results;
            if (includeCompleted)
            {
                results = Repository.GetAllOrders()
                                    .Select(d => this.ModelFactory.Create(d))
                                    .Skip(PageSize * page)
                                    .Take(PageSize)
                                    .OrderBy(d => d.CurrentDate)
                                    .ToList();
            }
            else
            {
                results = Repository.GetAllOrders()
                                    .Select(d => this.ModelFactory.Create(d))
                                    .Where(d => d.Status != "Complete" && d.Status != "Canceled")
                                    .Skip(PageSize * page)
                                    .Take(PageSize)
                                    .OrderBy(d => d.CurrentDate)
                                    .ToList();
            }
            return results;
        }

        [Authorize]
        public HttpResponseMessage Get(int orderId)
        {
            var username = _identityService.CurrentUser;
            var result = Repository.GetOrder(username, orderId);

            if (result == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK,
              this.ModelFactory.Create(result));
        }

        [HttpPut]
        [HttpPatch]
        [Authorize]
        public HttpResponseMessage Patch(int orderId, [FromBody]OrderModel orderModel)
        {
            try
            {
                var newEntity = this.ModelFactory.Parse(orderModel);
                
                if (newEntity == null) 
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Could not read order entry in body");
                var oldEntity = Repository.GetOrder(_identityService.CurrentUser, orderId);
                if (oldEntity == null)
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Order not found");
                newEntity.UserName = _identityService.CurrentUser;
                var order = Repository.Update(newEntity);

                if (order == null)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Could not update order.");
                return Request.CreateResponse(HttpStatusCode.OK, this.ModelFactory.Create(order));
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [Authorize]
        public HttpResponseMessage Delete(int orderId)
        {
            try
            {
                var orderEntity = Repository.GetOrder(_identityService.CurrentUser, orderId);
                if (orderEntity == null)
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Order not found");
                orderEntity.Status = "Cancelled";
                if (Repository.Update(orderEntity) != null)
                    return Request.CreateResponse(HttpStatusCode.OK);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [Authorize]
        public HttpResponseMessage Post([FromBody]OrderModel orderModel)
        {
            try
            {
                var entity = this.ModelFactory.Parse(orderModel);

                if (entity == null) Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Could not read order entry in body");

                var newEntity = Repository.Insert(entity);

                if (newEntity == null) Request.CreateResponse(HttpStatusCode.NotFound);

                return Request.CreateResponse(HttpStatusCode.Created, this.ModelFactory.Create(newEntity));
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}