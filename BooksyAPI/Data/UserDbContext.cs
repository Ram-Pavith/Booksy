using BooksyAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BooksyAPI.Data
{
    public class UserDbContext:IdentityDbContext
    {

        public UserDbContext() { }
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-LJOJLTJ\\SQLEXPRESS;Database=Users;Trusted_Connection=True;");
            }
        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        
    }

}
