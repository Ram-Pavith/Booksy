using BooksyAPI.Models;
using BooksyAPI.Repo.Interfaces;
using BooksyAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BooksyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(CategoriesController));
        /*        private readonly ICategoryService<Category> _categoryService;
                public CategoriesController(ICategoryService<Category> categoryService)
                {
                    _categoryService = categoryService;
                }*/
        private readonly IUnitOfWorkRepo _unitOfWork;
        
        public CategoriesController(IUnitOfWorkRepo unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/<CategoriesController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            try
            {
                _log4net.Info("get categories list is being called");
                //var categories = await _categoryService.GetCategories();
                var categories = await _unitOfWork.Category.GetCategories();
                return Ok(categories);
            }   
            catch(Exception ex)
            {
                _log4net.Error(ex.Message, ex);
                return BadRequest(ex.Message);                
            }
            
        }

        // GET api/<CategoriesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            try
            {
                _log4net.Info("get category for"+id+"  is being called");
                //var category = await _categoryService.GetById(id);
                var category = await _unitOfWork.Category.GetById(id);

                if (category == null)
                {
                    return NotFound();
                }

                return Ok(category);
            }
            catch (Exception e)
            {
                _log4net.Error(e.Message, e);
                return BadRequest(e.Message);
            }

        }

        // POST api/<CategoriesController>
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
            try
            {
                _log4net.Info("post category "+category+" is being called");
                //await _categoryService.Add(category);
                await _unitOfWork.Category.Add(category);
                return CreatedAtAction("GetCategory", new { id = category.Id }, category);
            }
            catch (DBConcurrencyException ex) {
                return BadRequest(ex.Message);
            }
            catch(Exception e)
            {_log4net.Error(e.Message, e);
                return BadRequest(e.Message);
            }
        }

        // PUT api/<CategoriesController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, Category category)
        {
            if(id != category.Id)
            {
                return BadRequest();
            }
            try
            {
                _log4net.Info("put category for"+id+" "+category+" is being called");
                //await _categoryService.Update(category);
                await _unitOfWork.Category.Update(category);
                return Ok(category);
            }
            catch(DBConcurrencyException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception e)
            {
                _log4net.Error(e.Message, e);
                return BadRequest(e.Message);
            }
        }

        // DELETE api/<CategoriesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            //var category = await _categoryService.CategoryExists(id);
            var category = await _unitOfWork.Category.CategoryExists(id);
            if (!category)
            {
                return NotFound();
            }
            try
            {
                _log4net.Info("delete for company "+id+" is being called");
                //await _categoryService.Delete(id);
                await _unitOfWork.Category.Delete(id);
                return Ok();
            }
            catch(DBConcurrencyException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
