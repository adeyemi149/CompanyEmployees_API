using Microsoft.EntityFrameworkCore;
using Repository;
using Shared.RequestFeatures;
using CompanyEmployees.Contract.Interface;
using CompanyEmployees.Data;
using CompanyEmployees.Entities.Models;
using CompanyEmployees.Repository.Extension;

namespace CompanyEmployees.Repository.RepositoryUser
{
    public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(DatabaseContext context) : base(context)
        {
        }

        public async Task<PagedList<Employee>> GetEmployeesAsync(Guid companyId, EmployeeParameters employeeParameters, bool trackChanges)
        {
            var employee =  await FindByCondition(c => c.CompanyId.Equals(companyId), trackChanges)
            .FilterEmployees(employeeParameters.MinAge, employeeParameters.MaxAge)
            .Search(employeeParameters.SearchTerm)
            .Sort(employeeParameters.OrderBy)
            .ToListAsync();

            var count = await FindByCondition(e => e.CompanyId.Equals(companyId), trackChanges).CountAsync();

            return new PagedList<Employee>(employee, count, employeeParameters.PageNumber, employeeParameters.PageSize);
        }

        public async Task<Employee> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges) =>
            await FindByCondition(c => c.CompanyId.Equals(companyId) && c.Id.Equals(id), trackChanges)
            .SingleOrDefaultAsync();

        public void CreateEmployeeForCompany(Guid companyId, Employee employee)
        {
            employee.CompanyId = companyId;
            Create(employee);
        }

        public void DeleteEmployee(Employee employee) =>
            Delete(employee);
        
    }
}
