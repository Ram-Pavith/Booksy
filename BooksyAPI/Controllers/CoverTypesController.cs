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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using BooksyAPI.Repo.Interfaces;

namespace BooksyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoverTypesController : ControllerBase
    {
/*        private readonly ICoverTypeService<CoverType> _service;

        public CoverTypesController(ICoverTypeService<CoverType> service)
        {
            _service = service;
        }*/

        private readonly IUnitOfWorkRepo _unitOfWork;
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(CoverTypesController));

        public CoverTypesController(IUnitOfWorkRepo unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/CoverTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CoverType>>> GetCoverTypes()
        {
            try
            {
                _log4net.Info("Get Covertypes is invoked");
                //var covertypes = await _service.GetCoverTypes();
                var covertypes = await _unitOfWork.CoverType.GetCoverTypes();
                return Ok(covertypes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            };
        }

        // GET: api/CoverTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CoverType>> GetCoverType(int id)
        {
            try
            {
                _log4net.Info("Get covertype by " + id + " is invoked");
                //var covertype = await _service.GetById(id);
                var covertype = await _unitOfWork.CoverType.GetById(id);

                if (covertype == null)
                {
                    return NotFound();
                }

                return Ok(covertype);
            }
            catch (Exception e)
            {
                _log4net.Error(e.Message, e);
                return BadRequest(e.Message);
            }
        }

        // PUT: api/CoverTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCoverType(int id, CoverType coverType)
        {
            if (id != coverType.Id)
            {
                return BadRequest();
            }
            if (!CoverTypeExists(id))
            {
                return NotFound();
            }
            try
            {
                _log4net.Info("Put CoverType by " + id + coverType+ " is invoked");
                //await _service.Update(coverType);
                await _unitOfWork.CoverType.Update(coverType);
                return Ok(coverType);
            }
            catch (Exception e)
            {
                _log4net.Error(e.Message, e);
                return BadRequest(e.Message);
            }
        }

        // POST: api/CoverTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CoverType>> PostCoverType(CoverType covertype)
        {
            try
            {
                _log4net.Info("Post Covertype of " + covertype + " is invoked");
                // await _service.Add(covertype);
                await _unitOfWork.CoverType.Add(covertype);
                return CreatedAtAction("GetCoverType", new { id = covertype.Id }, covertype);
            }
            catch (Exception e)
            {
                _log4net.Error(e.Message, e);
                return BadRequest(e.InnerException.Message);
            }
        }

        // DELETE: api/CoverTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCoverType(int id)
        {
            //var covertype = await _service.CoverTypeExists(id);
            var covertype = await _unitOfWork.CoverType.CoverTypeExists(id);
            if (!covertype)
            {
                return NotFound();
            }

            try
            {
                _log4net.Info("Delete Covertype by " + id + "is invoked");
                //await _service.Delete(id);
                await _unitOfWork.CoverType.Delete(id);
                return Ok();
            }
            catch (Exception e)
            {
                _log4net.Error(e.Message, e);
                return BadRequest(e.Message);
            }
        }

        private bool CoverTypeExists(int id)
        {
            _log4net.Info("Covertype exists check for " + id + "id");
            //return (_service.GetById(id) != null);
            return (_unitOfWork.CoverType.GetById(id) != null);
        }
    }
}
