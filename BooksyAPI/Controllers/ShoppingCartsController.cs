using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BooksyAPI.Data;
using BooksyAPI.Models;
using BooksyAPI.Services.Interfaces;
using BooksyAPI.Repo.Interfaces;
using BooksyAPI.Repo.Classes;

namespace BooksyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartsController : ControllerBase
    {
/*        private readonly IShoppingCartService<ShoppingCart> _service;

        public ShoppingCartsController(IShoppingCartService<ShoppingCart> service)
        {
            _service= service;
        }
*/
        private readonly IUnitOfWorkRepo _unitOfWork;
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(ShoppingCartsController));


        public ShoppingCartsController(IUnitOfWorkRepo unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/ShoppingCarts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShoppingCart>>> GetShoppingCarts()
        {
            try
            {
                _log4net.Info("get shopping cart list is being called");
                //var companies = await _service.GetShoppingCarts();
                var shoppingcart = await _unitOfWork.ShoppingCart.GetShoppingCarts();
                return Ok(shoppingcart);
            }
            catch (Exception ex)
            {
                _log4net.Error(ex.Message, ex);
                return BadRequest(ex.Message);
            }
        }

        // GET: api/Companies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ShoppingCart>> GetShoppingCart(int id)
        {

            try
            {
                _log4net.Info("get shopping cart for "+id+" is being called");
                //var ShoppingCart = await _service.GetById(id);
                var ShoppingCart = await _unitOfWork.ShoppingCart.GetById(id);

                if (ShoppingCart == null)
                {
                    return NotFound();
                }

                return Ok(ShoppingCart);
            }
            catch (Exception e)
            {
                _log4net.Error(e.Message, e);
                return BadRequest(e.Message);
            }
        }

        // PUT: api/Companies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutShoppingCart(int id, ShoppingCart ShoppingCart)
        {
            if (id != ShoppingCart.Id)
            {
                return BadRequest();
            }
            if (!ShoppingCartExists(id))
            {
                return NotFound();
            }
            try
            {
                _log4net.Info("put shopping cart  is being called");
                //await _service.Update(ShoppingCart);
                await _unitOfWork.ShoppingCart.Update(ShoppingCart);
                return Ok(ShoppingCart);
            }
            catch (Exception e)
            {
                _log4net.Error(e.Message, e);
                return BadRequest(e.Message);
            }
        }

        [HttpPut("increment_count/{id}/{count}")]
        public async Task<IActionResult> IncrementShoppingCartCount(int id,int count, ShoppingCart shoppingCart)
        {
            Task<ShoppingCart> ShoppingCartAsync = _unitOfWork.ShoppingCart.GetById(id);
            ShoppingCart ShoppingCart = await ShoppingCartAsync;
            if (id != ShoppingCart.Id)
            {
                return BadRequest();
            }
            if (!ShoppingCartExists(id))
            {
                return NotFound();
            }
            try
            {
                _log4net.Info("increment shopping cart count for "+id+" is being called");
                //await _service.Update(ShoppingCart);
                _unitOfWork.ShoppingCart.IncrementCount(ShoppingCart,count);
                return Ok(ShoppingCart);
            }
            catch (Exception e)
            {_log4net.Error(e.Message, e);
                return BadRequest(e.Message);
            }
        }
        [HttpPut("decrement_count/{id}/{count}")]
        public async Task<IActionResult> DecrementShoppingCartCount(int id, int count, ShoppingCart shoppingCart)
        {
            Task<ShoppingCart> ShoppingCartAsync = _unitOfWork.ShoppingCart.GetById(id);
            ShoppingCart ShoppingCart = await ShoppingCartAsync;
            if (id != ShoppingCart.Id)
            {
                return BadRequest();
            }
            if (!ShoppingCartExists(id))
            {
                return NotFound();
            }
            try
            {
                _log4net.Info("decrement shopping cart for"+id+" list is being called");
                //await _service.Update(ShoppingCart);
                _unitOfWork.ShoppingCart.DecrementCount(ShoppingCart, count);
                return Ok(ShoppingCart);
            }
            catch (Exception e)
            {
                _log4net.Error(e.Message, e);
                return BadRequest(e.Message);
            }
        }


        // POST: api/Companies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ShoppingCart>> PostShoppingCart(ShoppingCart ShoppingCart)
        {
            
            try
            {
                _log4net.Info("post shopping cart list for"+ShoppingCart+" is being called");
                //await _service.Add(ShoppingCart);
                await _unitOfWork.ShoppingCart.Add(ShoppingCart);
                return CreatedAtAction("GetShoppingCart", new { id = ShoppingCart.Id }, ShoppingCart);                    
            }
            catch (Exception e)
            {
                _log4net.Error(e.Message, e);
                return BadRequest(e.Message);
            }
        }

        // DELETE: api/Companies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShoppingCart(int id)
        {
           // var ShoppingCart = await _service.ShoppingCartExists(id);
            var ShoppingCart = await _unitOfWork.ShoppingCart.ShoppingCartExists(id);
            if (!ShoppingCart)
            {
                return NotFound();
            }

            try
            {
                _log4net.Info("delete shopping cart for "+id+" list is being called");
                //await _service.Delete(id);
                await _unitOfWork.ShoppingCart.Delete(id);
                return Ok();
            }
            catch (Exception e)
            {
                _log4net.Error(e.Message, e);
                return BadRequest(e.Message);
            }
        }

        private bool ShoppingCartExists(int id)
        {
            _log4net.Info(" shopping cart exists for"+id+"  is being called");
            //return (_service.GetById(id) != null);
            return (_unitOfWork.ShoppingCart.GetById(id) != null);
        }
    }
}
