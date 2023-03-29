using BooksyAPI.Models;

namespace BooksyAPI.Repo.Interfaces
{
    public interface IOrderHeaderRepo<OrderHeader>
    {
        public Task<IEnumerable<OrderHeader>> GetOrderHeaders();
        public Task<OrderHeader> GetById(int id);
        public Task Add(OrderHeader orderheader);
        public Task Update(OrderHeader orderheader);
        public Task Delete(int id);
        public Task<bool> OrderHeaderExists(int id);
        void UpdateStripePaymentID(int id, string sessionId, string paymentIntentId);
    }
}
