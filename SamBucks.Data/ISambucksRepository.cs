using System;
using System.Collections.Generic;
using System.Linq;
using Sambucks.Data.Entities;

namespace Sambucks.Data
{
    public interface ISambucksRepository
    {
        IQueryable<Food> GetAllFoods();
        Food GetFood(int id);
        Food Insert(Food food);
        Food Update(Food food);
        bool DeleteFood(int id);
        IQueryable<Order> GetAllOrders();
        IQueryable<Order> GetOrders(string username);
        Order GetOrder(string username, int orderId);
        Order Insert(Order order);
        Order Update(Order order);
        bool DeleteOrder(int id);
        Size GetSize(int id);
    }
}
