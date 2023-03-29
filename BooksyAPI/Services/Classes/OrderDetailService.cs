using BooksyAPI.Models;
using BooksyAPI.Repo.Interfaces;
using BooksyAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BooksyAPI.Services.Classes
{
    public class OrderDetailService:IOrderDetailService<OrderDetail>
    {
        private readonly IOrderDetailRepo<OrderDetail> _repoobj;
        public OrderDetailService(IOrderDetailRepo<OrderDetail> repoobj)
        {
            _repoobj = repoobj;
        }

        public async Task Add(OrderDetail orderdetail)
        {
            try
            {
                await _repoobj.Add(orderdetail);
            }
            catch (DBConcurrencyException e)
            {
                throw new DBConcurrencyException(e.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task Delete(int id)
        {
            try
            {
                await _repoobj.Delete(id);
            }
            catch (DBConcurrencyException e)
            {
                throw new DBConcurrencyException(e.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<OrderDetail> GetById(int id)
        {
            try
            {
                return await _repoobj.GetById(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<OrderDetail>> GetOrderDetails()
        {
            try
            {
                return await _repoobj.GetOrderDetails();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<bool> OrderDetailExists(int id)
        {
            try
            {
                return await _repoobj.OrderDetailExists(id);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task Update(OrderDetail orderdetail)
        {
            try
            {
                await _repoobj.Update(orderdetail);
            }
            catch (DbUpdateException e)
            {
                throw new DbUpdateException(e.Message);
            }
            catch (DBConcurrencyException e)
            {
                throw new DBConcurrencyException(e.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
