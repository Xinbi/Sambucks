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
    public class MenuController : BaseApiController
    {
       
        /// <summary>
        /// Initializes a new instance of the <see cref="MenuController"/> class.
        /// </summary>
        public MenuController()
            : base(new SambucksRepository())
        {
            
        }

        public MenuController(ISambucksRepository repo)
            : base(repo)
        {
            
        }

        public IEnumerable<FoodModel> Get()
        {
            var allFoods = Repository.GetAllFoods();
            return allFoods.Select(item => this.ModelFactory.Create(item)).ToList();
        }

        public FoodModel Get(int foodId) 
        {
            return this.ModelFactory.Create(Repository.GetFood(foodId));
        }

        [Authorize(Roles = "Barista")]
        public HttpResponseMessage Post([FromBody]FoodModel model)
        {
            try
            {
                var entity = this.ModelFactory.Parse(model);

                if (entity == null) Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Could not read food entry in body");

                var food = Repository.Insert(entity);

                if (food == null) Request.CreateResponse(HttpStatusCode.NotFound);

                return Request.CreateResponse(HttpStatusCode.Created, this.ModelFactory.Create(food));
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpPut]
        [HttpPatch]
        [Authorize(Roles = "Barista")]
        public HttpResponseMessage Patch(int foodId, [FromBody]FoodModel model) {
            try
            {
                var entity = this.ModelFactory.Parse(model);

                if (entity == null) return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Could not read food entry in body");
                if(Repository.GetFood(foodId) == null)
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Food not found");
                var food = Repository.Update(entity);

                if (food == null)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Could not add food.");
                return Request.CreateResponse(HttpStatusCode.OK, this.ModelFactory.Create(food));
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [Authorize(Roles = "Barista")]
        public HttpResponseMessage Delete(int foodId)
        {
            try
            {
                if (Repository.GetFood(foodId) == null)
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Food not found");
                if (Repository.DeleteFood(foodId))
                    return Request.CreateResponse(HttpStatusCode.OK);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
