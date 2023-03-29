using BooksyAPI.Models;
using BooksyAPI.Repo.Interfaces;
using BooksyAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BooksyAPI.Services.Classes
{
    public class OrderHeaderService: IOrderHeaderService<OrderHeader>
    {
        private readonly IOrderHeaderRepo<OrderHeader> _repoobj;
        public OrderHeaderService(IOrderHeaderRepo<OrderHeader> repoobj)
        {
            _repoobj = repoobj;
        }

        public async Task Add(OrderHeader orderheader)
        {
            try
            {
                await _repoobj.Add(orderheader);
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

        public async Task<OrderHeader> GetById(int id)
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

        public async Task<IEnumerable<OrderHeader>> GetOrderHeaders()
        {
                try
                {
                    return await _repoobj.GetOrderHeaders();
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
        }

        public async Task<bool> OrderHeaderExists(int id)
        {
                try
                {
                    return await _repoobj.OrderHeaderExists(id);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
        }

        public async Task Update(OrderHeader orderheader)
        {
                try
                {
                    await _repoobj.Update(orderheader);
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
