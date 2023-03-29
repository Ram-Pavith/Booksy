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
using System.Net.NetworkInformation;

namespace BooksyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(CompaniesController));
        /* private readonly ICompanyService<Company> _service;

         public CompaniesController(ICompanyService<Company> service)
         {
             _service = service;
         }*/
        private readonly IUnitOfWorkRepo _unitOfWork;

        public CompaniesController(IUnitOfWorkRepo unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/Companies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Company>>> GetCompanies()
        {
            try
            {
                _log4net.Info("get companies list is being called");
                //var companies = await _service.GetCompanies();
var companies = await _unitOfWork.Company.GetCompanies();
                return Ok(companies);
            }
            catch(Exception ex)
            {
                _log4net.Error(ex.Message, ex);
                return BadRequest(ex.Message);
            }
        }

        // GET: api/Companies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Company>> GetCompany(int id)
        {

            try
            {
                _log4net.Info("post company " +id+" is being called");
                //var company = await _service.GetById(id);
var company = await _unitOfWork.Company.GetById(id);

                if (company == null)
                {
                    return NotFound();
                }

                return Ok(company);
            }
            catch(Exception e)
            {_log4net.Error(e.Message, e);
                return BadRequest(e.Message);
            }
        }

        // PUT: api/Companies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompany(int id, Company company)
        {
            if (id != company.Id)
            {
                return BadRequest();
            }
            if (!CompanyExists(id))
            {
                return NotFound();
            }
            try
            {
                _log4net.Info("put company " + id +" "+ company + " is being called");
                //await _service.Update(company);
                await _unitOfWork.Company.Update(company);
                return Ok(company);
            }
            catch (Exception e)
            {
               return BadRequest(e.Message);
            }
        }

        // POST: api/Companies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Company>> PostCompany(Company company)
        {

            try
            {
                _log4net.Info("post company " + company + " is being called");
                //await _service.Add(company);
                await _unitOfWork.Company.Add(company);
                return CreatedAtAction("GetCompany", new { id = company.Id }, company);
            }
            catch (Exception e)
            {
                _log4net.Error(e.Message, e);
                return BadRequest(e.Message);
            }
        }

        // DELETE: api/Companies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            //var company = await _service.CompanyExists(id);
            var company = await _unitOfWork.Company.CompanyExists(id);

            if (!company)
            {
                return NotFound();
            }

            try
            {
                _log4net.Info("delete company for " + id + " is being called");
                //await _service.Delete(id);
                await _unitOfWork.Company.Delete(id);
                return Ok();
            }
            catch(Exception e) {_log4net.Error(e.Message, e);
                return BadRequest(e.Message);
            }
        }

        private bool CompanyExists(int id)
        {
            _log4net.Info(" company exists for " + id + " is being called");
            //return (_service.GetById(id)!=null);
            return (_unitOfWork.Company.GetById(id)!=null);
        }
    }
}
