using Microsoft.EntityFrameworkCore;
using Repository;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompanyEmployees.Contract.Interface;
using CompanyEmployees.Data;
using CompanyEmployees.Entities.Models;
using CompanyEmployees.Repository.Extension;

namespace CompanyEmployees.Repository.RepositoryUser
{
    public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
    {
        public CompanyRepository(DatabaseContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Company>> GetAllCompaniesAsync(bool trackChanges, CompanyParameters companyParameter ) =>
            await FindAll(trackChanges)
            .Search(companyParameter.searchTerm)
            .Sort(companyParameter.OrderBy)
            .ToListAsync();
        

        public async Task<Company> GetCompanyAsync(Guid companyId, bool trackChanges) =>
           await FindByCondition(c => c.Id.Equals(companyId), trackChanges)
            .SingleOrDefaultAsync();
        
        public void CreateCompany(Company company) => Create(company);

        public async Task<IEnumerable<Company>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges) =>
            await FindByCondition(c => ids.Contains(c.Id), trackChanges)
            .ToListAsync();

        public void DeleteCompany(Company company) =>
            Delete(company);
    }
}
