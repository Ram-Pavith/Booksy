﻿using BooksyAPI.Models;

namespace BooksyAPI.Services.Interfaces
{
    public interface IOrderDetailService<OrderDetail>
    {

        public Task<IEnumerable<OrderDetail>> GetOrderDetails();
        public Task<OrderDetail> GetById(int id);
        public Task Add(OrderDetail orderdetail);
        public Task Update(OrderDetail orderdetail);
        public Task Delete(int id);
        public Task<bool> OrderDetailExists(int id);
    }
}
