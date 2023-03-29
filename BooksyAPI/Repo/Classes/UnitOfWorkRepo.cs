using BooksyAPI.Data;
using BooksyAPI.Models;
using BooksyAPI.Repo.Interfaces;

namespace BooksyAPI.Repo.Classes
{
    public class UnitOfWorkRepo:IUnitOfWorkRepo
    {
        private ApplicationDbContext _db;

        public UnitOfWorkRepo(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepo(_db);
            CoverType = new CoverTypeRepo(_db);
            Product = new ProductRepo(_db);
            Company = new CompanyRepo(_db);
            ApplicationUser = new ApplicationUserRepo(_db);
            ShoppingCart = new ShoppingCartRepo(_db);
            OrderHeader = new OrderHeaderRepo(_db);
            OrderDetail = new OrderDetailRepo(_db);
        }
        public ICategoryRepo<Category> Category { get; private set; }
        public ICoverTypeRepo<CoverType> CoverType { get; private set; }
        public IProductRepo<Product> Product { get; private set; }
        public ICompanyRepo<Company> Company { get; private set; }

        public IShoppingCartRepo<ShoppingCart> ShoppingCart { get; private set; }

        public IApplicationUserRepo<ApplicationUser> ApplicationUser { get; private set; }
        public IOrderHeaderRepo<OrderHeader> OrderHeader { get; private set; }
        public IOrderDetailRepo<OrderDetail> OrderDetail { get; private set; }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
