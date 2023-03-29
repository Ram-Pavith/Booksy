using BooksyAPI.Data;
using BooksyAPI.Models;
using BooksyAPI.Repo.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BooksyAPI.Repo.Classes
{
    public class OrderDetailRepo : IOrderDetailRepo<OrderDetail>
    {
        private readonly ApplicationDbContext _context;
        public OrderDetailRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Add(OrderDetail orderdetail)
        {

            await _context.OrderDetails.AddAsync(orderdetail);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            OrderDetail orderdetail = _context.OrderDetails.Find(id);
            _context.OrderDetails.Remove(orderdetail);
            await _context.SaveChangesAsync();
        }

        public async Task<OrderDetail> GetById(int id)
        {
			List<OrderDetail> orderDetails= await _context.OrderDetails.Include(o => o.OrderHeader).Include(o => o.Product).ToListAsync();


			return orderDetails.SingleOrDefault(o=>o.Id==id);
        }

        public async Task<IEnumerable<OrderDetail>> GetOrderDetails()
        {
            return await _context.OrderDetails.Include(o => o.OrderHeader).Include(o=>o.Product).ToListAsync();
        }

        public async Task<bool> OrderDetailExists(int id)
        {
            return await _context.OrderDetails.FindAsync(id) != null;
        }

        public async Task Update(OrderDetail orderdetail)
        {
            _context.OrderDetails.Update(orderdetail);
            await _context.SaveChangesAsync();
        }
    }
}
