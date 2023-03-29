 namespace BooksyAPI.Services.Interfaces
{
    public interface IProductService<Product>
    {
        public Task<IEnumerable<Product>> GetProducts();
        public Task<Product> GetById(int id);
        public Task Add(Product product);
        public Task Update(Product product);
        public Task Delete(int id);
        public Task<bool> ProductExists(int id);
    }
}
