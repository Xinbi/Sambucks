using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sambucks.Data.Entities;

namespace Sambucks.Data
{
    public class SambucksRepository : ISambucksRepository 
    {
        private Dictionary<int, Food> foodList;
        private Dictionary<int, Order> orderList;
        private Dictionary<int, Size> sizeList;
        private int nextFoodIndex;
        private int nextSizeIndex;
        private int nextOrderIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="SambucksRepository"/> class.
        /// </summary>
        public SambucksRepository()
        {
            orderList = new Dictionary<int, Order>();
            
            foodList = new Dictionary<int, Food>()
            {   
                {0, new Food { Id = 0, Category = "Coffee", Description = "Brewed Coffee" }},
                {1, new Food { Id = 1, Category = "Coffee", Description = "Misto" }},
                {2, new Food { Id = 2, Category = "Coffee", Description = "Americano" }},
                {3, new Food { Id = 3, Category = "Coffee", Description = "Late" }},
                {4, new Food { Id = 4, Category = "Tea", Description = "Black Tea" }},
                {5, new Food { Id = 5, Category = "Tea", Description = "Green Tea" }},
                {6, new Food { Id = 6, Category = "Extra", Description = "Additional Flavor" }},
                {7, new Food { Id = 7, Category = "Extra", Description = "Additional Shot" }},
            };

            sizeList = new Dictionary<int, Size> 
            {
                {0, new Size {Id = 0, Description = "Small", Price = 2, Food = foodList[0]}},
                {1, new Size {Id = 1, Description = "Medium", Price = 3, Food = foodList[0]}},
                {2, new Size {Id = 2, Description = "Large", Price = 4, Food = foodList[0]}},
                {3, new Size {Id = 3, Description = "Small", Price = 3.5m, Food = foodList[1]}},
                {4, new Size {Id = 4, Description = "Medium", Price = 4.5m, Food = foodList[1]}},
                {5, new Size {Id = 5, Description = "Large", Price = 5.5m, Food = foodList[1]}},
                {6, new Size {Id = 6, Description = "Small", Price = 3, Food = foodList[2]}},
                {7, new Size {Id = 7, Description = "Medium", Price = 4, Food = foodList[2]}},
                {8, new Size {Id = 8, Description = "Large", Price = 5, Food = foodList[2]}},
                {9, new Size {Id = 9, Description = "Small", Price = 3.5m, Food = foodList[3]}},
                {10, new Size {Id = 10, Description = "Medium", Price = 4.5m, Food = foodList[3]}},
                {11, new Size {Id = 11, Description = "Large", Price = 5.5m, Food = foodList[3]}},
                {12, new Size {Id = 12, Description = "Small", Price = 1m, Food = foodList[4]}},
                {13, new Size {Id = 13, Description = "Medium", Price = 1.5m, Food = foodList[4]}},
                {14, new Size {Id = 14, Description = "Large", Price = 2m, Food = foodList[4]}},
                {15, new Size {Id = 15, Description = "Small", Price = 1.5m, Food = foodList[5]}},
                {16, new Size {Id = 16, Description = "Medium", Price = 2m, Food = foodList[5]}},
                {17, new Size {Id = 17, Description = "Large", Price = 2.5m, Food = foodList[5]}},
                {18, new Size {Id = 18, Description = "", Price = 0.5m, Food = foodList[6]}},
                {19, new Size {Id = 19, Description = "", Price = 0.5m, Food = foodList[7]}}
            };

            foreach (KeyValuePair<int, Food> foodItem in foodList)
            {
                var foodSizeList = sizeList.Where(s => s.Value.Food.Id == foodItem.Value.Id);
                foreach (KeyValuePair<int, Size> sizeItem in foodSizeList)
                {
                    if (foodItem.Value.Sizes == null)
                        foodItem.Value.Sizes = new List<Size>();
                    foodItem.Value.Sizes.Add(sizeItem.Value);
                }
            }

            nextFoodIndex = 8;
            nextSizeIndex = 20;
            nextOrderIndex = 0;
        }

        public IQueryable<Food> GetAllFoods()
        {
            return foodList.Values.AsQueryable();  
        }

        public Food GetFood(int id)
        {
            if (!foodList.ContainsKey(id))
                return null;
            return foodList.FirstOrDefault(f => f.Key == id).Value;  
        }

        public Food Insert(Food food)
        {
            try
            {
                food.Id = nextFoodIndex++;
                if (food.Sizes.Count > 0)
                {
                    foreach (Size foodSize in food.Sizes)
                    {
                        foodSize.Id = nextSizeIndex++;
                        foodSize.Food = food;
                        sizeList.Add(nextSizeIndex++, foodSize);
                    }
                }
                foodList.Add(food.Id, food);
                return food;
            }
            catch
            {
                return null;
            }
        }

        public Food Update(Food food)
        {
            try
            {
                foodList[food.Id] = food;
                return foodList[food.Id];
            }
            catch
            {
                return null;
            }
        }

        public bool DeleteFood(int id) {
            try
            {
                if (foodList.ContainsKey(id))
                {
                    var deletedSizes = sizeList.Select(s => s.Value).Where(s => s.Food.Id == id).ToList();
                    foreach (var size in deletedSizes)
                    {
                        sizeList.Remove(size.Id);
                    }
                }
                foodList.Remove(id);

                return true;
            }
            catch 
            {
                return false;
            }
        }
        public IQueryable<Order> GetAllOrders()
        {
            return orderList.Select(o => o.Value).AsQueryable();
        }
        public IQueryable<Order> GetOrders(string username)
        {
            return orderList.Select(o => o.Value).Where(o => o.UserName == username).AsQueryable();
        }

        public Order GetOrder(string username, int orderId)
        {
            return orderList.Select(o => o.Value).SingleOrDefault(o => o.UserName == username && o.Id == orderId);
        }

        public Order Insert(Order order)
        {
            var orderId = nextOrderIndex++;
            order.Id = orderId;
            try
            {
                orderList.Add(orderId, order);
                return order;
            }
            catch
            {
                return null;
            }
        }

        public Order Update(Order order)
        {
            try
            {
                orderList[order.Id] = order;
                return orderList[order.Id];
            }
            catch
            {
                return null;
            }
        }

        public bool DeleteOrder(int id)
        {
            try
            {
                orderList.Remove(id);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Size GetSize(int id)
        {
            return sizeList.Select(s => s.Value).SingleOrDefault(s => s.Id == id);
        }

    }
}
