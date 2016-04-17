using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Routing;
using Sambucks.Data;
using Sambucks.Data.Entities;
using Sambucks.Models;

namespace Sambucks.Models
{
    public class ModelFactory
    {
        private UrlHelper _urlHelper;
        private ISambucksRepository _repo;
        public ModelFactory(HttpRequestMessage request, ISambucksRepository repo)
        {
            _urlHelper = new UrlHelper(request);
            _repo = repo;
        }

        public FoodModel Create(Food food)
        {
            return new FoodModel()
            {
                Url = _urlHelper.Link("menu", new { foodid = food.Id }),
                Description = food.Description,
                Category = food.Category,
                Sizes = food.Sizes.Select(m => Create(m))
            };
        }

        public SizeModel Create(Size size)
        {
            return new SizeModel()
            {
                Links = new List<LinkModel>()
        {
          CreateLink(_urlHelper.Link("Size", new { foodid = size.Food.Id, id = size.Id }), "self")
        },
                Description = size.Description,
                Price = size.Price
            };
        }

        public OrderModel Create(Order d)
        {
            return new OrderModel()
            {
                Links = new List<LinkModel>()
        {
          CreateLink(_urlHelper.Link("Diaries", new { diaryid = d.CurrentDate.ToString("yyyy-MM-dd") }),
            "self"),
          CreateLink(_urlHelper.Link("DiaryEntries", new { diaryid = d.CurrentDate.ToString("yyyy-MM-dd") }),
            "newDiaryEntry", "POST"),

        },
                CurrentDate = d.CurrentDate,
                Entries = d.Entries.Select(e => Create(e))
            };
        }

        public LinkModel CreateLink(string href, string rel, string method = "GET", bool isTemplated = false)
        {
            return new LinkModel()
            {
                Href = href,
                Rel = rel,
                Method = method,
                IsTemplated = isTemplated
            };
        }

        public OrderEntryModel Create(OrderEntry entry)
        {
            return new OrderEntryModel()
            {
                Links = new List<LinkModel>()
        {
          CreateLink(_urlHelper.Link("OrderEntries", new { diaryid = entry.Order.CurrentDate.ToString("yyyy-MM-dd"), id = entry.Id }), "self")
        },
                FoodDescription = entry.FoodItem.Description,
                SizeDescription = entry.Size.Description,
                SizeUrl = _urlHelper.Link("Size", new { foodid = entry.FoodItem.Id, id = entry.Size.Id })
            };
        }

        public OrderEntry Parse(OrderEntryModel model)
        {
            try
            {
                var entry = new OrderEntry();


                if (!string.IsNullOrWhiteSpace(model.SizeUrl))
                {
                    var uri = new Uri(model.SizeUrl);
                    var sizeId = int.Parse(uri.Segments.Last());
                    var size = _repo.GetSize(sizeId);
                    entry.Size = size;
                    entry.FoodItem = size.Food;
                }

                return entry;
            }
            catch
            {
                return null;
            }
        }

        public Order Parse(OrderModel model)
        {
            try
            {
                var entity = new Order();

                var selfLink = model.Links.FirstOrDefault(l => l.Rel == "self");
                if (selfLink != null && !string.IsNullOrWhiteSpace(selfLink.Href))
                {
                    var uri = new Uri(selfLink.Href);
                    entity.Id = int.Parse(uri.Segments.Last());
                }

                entity.CurrentDate = model.CurrentDate;

                if (model.Entries != null)
                {
                    foreach (var entry in model.Entries) entity.Entries.Add(Parse(entry));
                }

                return entity;
            }
            catch
            {
                return null;
            }
        }

        public Food Parse(FoodModel model)
        {
            throw new NotImplementedException();
        }
    }

}