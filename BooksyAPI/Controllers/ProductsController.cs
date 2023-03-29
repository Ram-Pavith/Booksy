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

namespace BooksyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        /*        private readonly IProductService<Product> _service;

                public ProductsController(IProductService<Product> service)
                {
                    _service= service;
                }*/
        private readonly IUnitOfWorkRepo _unitOfWork;
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(ProductsController));


        public ProductsController(IUnitOfWorkRepo unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            try
            {
                _log4net.Info("get products list is being called");
                //var companies = await _service.GetProducts();
                var companies = await _unitOfWork.Product.GetProducts();
                return Ok(companies);
            }
            catch (Exception ex)
            {
                _log4net.Error(ex.Message, ex);
                return BadRequest(ex.Message);
            }
        }

        // GET: api/Companies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {

            try
            {
                _log4net.Info("get product for "+id+" is being called");
                //var Product = await _service.GetById(id);
                var Product = await _unitOfWork.Product.GetById(id);

                if (Product == null)
                {
                    return NotFound();
                }

                return Ok(Product);
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
        public async Task<IActionResult> PutProduct(int id, Product Product)
        {
            if (id != Product.Id)
            {
                return BadRequest();
            }
            if (!ProductExists(id))
            {
                return NotFound();
            }
            try
            {
                _log4net.Info("put product for "+id+" " +Product+" being called");
                //await _service.Update(Product);
                await _unitOfWork.Product.Update(Product);
                return Ok(Product);
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
        public async Task<ActionResult<Product>> PostProduct(Product Product)
        {

            try
            {
                _log4net.Info("post product for"+Product+" is being called");
                //await _service.Add(Product);
                await _unitOfWork.Product.Add(Product);
                return CreatedAtAction("GetProduct", new { id = Product.Id }, Product);
            }
            catch (Exception e)
            {
                _log4net.Error(e.Message, e);
                return BadRequest(e.Message);
            }
        }

        // DELETE: api/Companies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            //var Product = await _service.ProductExists(id);
            var Product = await _unitOfWork.Product.ProductExists(id);
            if (!Product)
            {
                return NotFound();
            }

            try
            {
                _log4net.Info("delete product for"+id+" is being called");
                //await _service.Delete(id);
                await _unitOfWork.Product.Delete(id);
                return Ok();
            }
            catch (Exception e)
            {
                _log4net.Error(e.Message, e);
                return BadRequest(e.Message);
            }
        }

        private bool ProductExists(int id)
        {
            _log4net.Info("Product exists for "+id+" is being called");
            // return (_service.GetById(id) != null);
            return (_unitOfWork.Product.GetById(id) != null);
        }
    }
}
