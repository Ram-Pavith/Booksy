using BooksyAPI.Models;
using BooksyAPI.Repo.Interfaces;
using BooksyAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BooksyAPI.Services.Classes
{
    public class ShoppingCartService : IShoppingCartService<ShoppingCart>
    {
        private readonly IShoppingCartRepo<ShoppingCart> _repoobj;
        public ShoppingCartService(IShoppingCartRepo<ShoppingCart> repoobj)
        {
            _repoobj = repoobj;
        }

        public async Task Add(ShoppingCart shoppingcart)
        {
            try
            {
                await _repoobj.Add(shoppingcart);
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

        public async Task<ShoppingCart> GetById(int id)
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

        public async Task<IEnumerable<ShoppingCart>> GetShoppingCarts()
        {
            try
            {
                return await _repoobj.GetShoppingCarts();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public Task<bool> ShoppingCartExists(int id)
        {
            return _repoobj.ShoppingCartExists(id);
        }

        public async Task Update(ShoppingCart shoppingcart)
        {
            try
            {
                await _repoobj.Update(shoppingcart);
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
