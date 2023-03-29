using BooksyAPI.Models;

namespace BooksyAPI.Services.Interfaces
{
    public interface IOrderHeaderService<OrderHeader>
    {

        public Task<IEnumerable<OrderHeader>> GetOrderHeaders();
        public Task<OrderHeader> GetById(int id);
        public Task Add(OrderHeader orderheader);
        public Task Update(OrderHeader orderheader);
        public Task Delete(int id);
        public Task<bool> OrderHeaderExists(int id);
    }
}
