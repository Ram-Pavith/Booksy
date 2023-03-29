using BooksyAPI.Repo.Interfaces;
using BooksyAPI.Models;
namespace BooksyAPI.Repo.Interfaces
{
    public interface IUnitOfWorkRepo
    {
        ICategoryRepo<Category> Category { get; }
        ICoverTypeRepo<CoverType> CoverType { get; }
        IProductRepo<Product> Product { get; }
        ICompanyRepo<Company> Company { get; }
        IShoppingCartRepo<ShoppingCart> ShoppingCart { get; }
        IApplicationUserRepo<ApplicationUser> ApplicationUser { get; }
        IOrderDetailRepo<OrderDetail> OrderDetail { get; }
        IOrderHeaderRepo<OrderHeader> OrderHeader { get; }

        void Save();
    }
}
