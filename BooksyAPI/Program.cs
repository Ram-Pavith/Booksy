using BooksyAPI.Data;
using BooksyAPI.Models;
using BooksyAPI.Repo.Classes;
using BooksyAPI.Repo.Interfaces;
using BooksyAPI.Services.Classes;
using BooksyAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<UserDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("UserConnection")));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Logging.AddLog4Net(); //creating the provider for logging


builder.Services.AddControllers();
builder.Services.AddScoped<IApplicationUserRepo<ApplicationUser>, ApplicationUserRepo>();
builder.Services.AddScoped<IApplicationUserService<ApplicationUser>, ApplicationUserService>();
builder.Services.AddScoped<ICategoryRepo<Category>, CategoryRepo>();
builder.Services.AddScoped<ICategoryService<Category>, CategoryService>();
builder.Services.AddScoped<ICompanyRepo<Company>, CompanyRepo>();
builder.Services.AddScoped<ICompanyService<Company>, CompanyService>();
builder.Services.AddTransient<ICoverTypeRepo<CoverType>, CoverTypeRepo>();
builder.Services.AddTransient<ICoverTypeService<CoverType>, CoverTypeService>();
builder.Services.AddScoped<IOrderDetailRepo<OrderDetail>, OrderDetailRepo>();
builder.Services.AddScoped<IOrderDetailService<OrderDetail>, OrderDetailService>();
builder.Services.AddScoped<IOrderHeaderRepo<OrderHeader>, OrderHeaderRepo>();
builder.Services.AddScoped<IOrderHeaderService<OrderHeader>, OrderHeaderService>();
builder.Services.AddScoped<IProductRepo<Product>, ProductRepo>();
builder.Services.AddScoped<IProductService<Product>, ProductService>();
builder.Services.AddScoped<IShoppingCartRepo<ShoppingCart>, ShoppingCartRepo>();
builder.Services.AddScoped<IShoppingCartService<ShoppingCart>, ShoppingCartService>();
builder.Services.AddTransient<IUnitOfWorkRepo, UnitOfWorkRepo>();

//controller for api include
//builder.Services.AddControllers().AddJsonOptions(options => { options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles; });


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();;

app.UseAuthorization();
app.MapRazorPages();
app.MapControllers();

app.Run();
