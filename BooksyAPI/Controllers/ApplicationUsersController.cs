using BooksyAPI.Models;
using BooksyAPI.Repo.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BooksyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUsersController : ControllerBase
    {
        private readonly IUnitOfWorkRepo _unitOfWork;
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(ApplicationUsersController));

        public ApplicationUsersController(IUnitOfWorkRepo unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/<ApplicationUsersController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetApplicationUsers()
        {
            try
            {
                _log4net.Info("get application users list is being called");
                //var ApplicationUsers = await _ApplicationUserService.GetApplicationUsers();
                var ApplicationUsers = await _unitOfWork.ApplicationUser.GetApplicationUsers();
                return Ok(ApplicationUsers);
            }
            catch (Exception ex)
            {
                _log4net.Error(ex.Message, ex);
                return BadRequest(ex.Message);
            }

        }

        // GET api/<ApplicationUsersController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationUser>> GetApplicationUser(int id)
        {
            try
            {
                _log4net.Info("put application users"+id+"list is being called");
                //var ApplicationUser = await _ApplicationUserService.GetById(id);
                var ApplicationUser = await _unitOfWork.ApplicationUser.GetById(id);

                if (ApplicationUser == null)
                {
                    return NotFound();
                }

                return Ok(ApplicationUser);
            }
            catch (Exception e)
            {_log4net.Error(e.Message, e);
                return BadRequest(e.Message);
            }

        }

        // POST api/<ApplicationUsersController>
        [HttpPost]
        public async Task<ActionResult<ApplicationUser>> PostApplicationUser(ApplicationUser ApplicationUser)
        {
            try
            {
                _log4net.Info("post application users"+ApplicationUser+" is being called");
                //await _ApplicationUserService.Add(ApplicationUser);
                await _unitOfWork.ApplicationUser.Add(ApplicationUser);
                return CreatedAtAction("GetApplicationUser", new { id = ApplicationUser.Id }, ApplicationUser);
            }
            catch (DBConcurrencyException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception e)
            {
                _log4net.Error(e.Message, e);
                return BadRequest(e.Message);
            }
        }

        // PUT api/<ApplicationUsersController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompany(int id, ApplicationUser ApplicationUser)
        {
            if (id != ApplicationUser.Id)
            {
                return BadRequest();
            }
            try
            {
                _log4net.Info("put application users"+id+" "+ApplicationUser+"is being called");
                //await _ApplicationUserService.Update(ApplicationUser);
                await _unitOfWork.ApplicationUser.Update(ApplicationUser);
                return Ok(ApplicationUser);
            }
            catch (DBConcurrencyException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception e)
            {
                _log4net.Error(e.Message, e);
                return BadRequest(e.Message);
            }
        }

        // DELETE api/<ApplicationUsersController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApplicationUser(int id)
        {
            //var ApplicationUser = await _ApplicationUserService.ApplicationUserExists(id);
            var ApplicationUser = await _unitOfWork.ApplicationUser.ApplicationUserExists(id);
            if (!ApplicationUser)
            {
                return NotFound();
            }
            try
            {
                _log4net.Info("delete application user for " + id + " is beign called");
                //await _ApplicationUserService.Delete(id);
                await _unitOfWork.ApplicationUser.Delete(id);
                return Ok();
            }
            catch (DBConcurrencyException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception e)
            {
                _log4net.Error(e.Message, e);
                return BadRequest(e.Message);
            }
        }
    }
}
