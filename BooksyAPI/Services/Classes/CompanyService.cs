using BooksyAPI.Models;
using BooksyAPI.Repo.Interfaces;
using BooksyAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BooksyAPI.Services.Classes
{
    public class CompanyService : ICompanyService<Company>
    {
        private readonly ICompanyRepo<Company> _companyRepo;
        public CompanyService(ICompanyRepo<Company> companyRepo)
        {
            _companyRepo = companyRepo;
        }
        public async Task Add(Company company)
        {
            try
            {
                await _companyRepo.Add(company);
            }
            catch(DBConcurrencyException e){
                throw new DBConcurrencyException(e.Message);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<bool> CompanyExists(int id)
        {
            try
            {
                return await _companyRepo.CompanyExists(id);
            }
            catch(Exception e) {
                throw new Exception(e.Message);
            }
        }

        public async Task Delete(int id)
        {
            try
            {
                await _companyRepo.Delete(id);
            }
            catch(DBConcurrencyException e)
            {
                throw new DBConcurrencyException(e.Message);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<Company> GetById(int id)
        {
            try
            {
                return await _companyRepo.GetById(id);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<Company>> GetCompanies()
        {
            try
            {
                return await _companyRepo.GetCompanies();
            }
            catch(Exception e){
                throw new Exception(e.Message);
            }
        }

        public async Task Update(Company company)
        {
            try
            {
                await _companyRepo.Update(company);
            }
            catch( DbUpdateException e)
            {
                throw new DbUpdateException(e.Message);
            }
            catch(DBConcurrencyException e)
            {
                throw new DBConcurrencyException(e.Message);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }

        }
    }
}
