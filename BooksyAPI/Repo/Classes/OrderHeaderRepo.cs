using BooksyAPI.Data;
using BooksyAPI.Models;
using BooksyAPI.Repo.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BooksyAPI.Repo.Classes
{
    public class OrderHeaderRepo : IOrderHeaderRepo<OrderHeader>
    {
        private readonly ApplicationDbContext _context;
        public OrderHeaderRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Add(OrderHeader orderheader)
        {
            await _context.OrderHeaders.AddAsync(orderheader);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            OrderHeader orderheader = _context.OrderHeaders.Find(id);
            _context.OrderHeaders.Remove(orderheader);
            await _context.SaveChangesAsync();
        }

        public async Task<OrderHeader> GetById(int id)
        {
			List<OrderHeader> orderHeaders=  await _context.OrderHeaders.Include(o => o.ApplicationUser).ToListAsync();

			return orderHeaders.SingleOrDefault(o=>o.Id==id);
        }

        public async Task<IEnumerable<OrderHeader>> GetOrderHeaders()
        {
            return await _context.OrderHeaders.Include(o => o.ApplicationUser).ToListAsync();
        }

        public async Task<bool> OrderHeaderExists(int id)
        {
            return await _context.OrderHeaders.FindAsync(id) != null;
        }

        public Task Update(OrderHeader orderheader)
        {
            throw new NotImplementedException();
        }

        public void UpdateStripePaymentID(int id, string sessionId, string paymentIntentId)
        {
            var orderFromDb = _context.OrderHeaders.FirstOrDefault(u => u.Id == id);
            orderFromDb.PaymentDate = DateTime.Now;
            orderFromDb.SessionId = sessionId;
            orderFromDb.PaymentIntentId = paymentIntentId;
            _context.OrderHeaders.Update(orderFromDb);
            _context.SaveChanges();
        }
    }
}
