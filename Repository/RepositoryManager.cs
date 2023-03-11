using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompanyEmployees.Contract.Interface;
using CompanyEmployees.Repository.RepositoryUser;

namespace CompanyEmployees.Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly DatabaseContext _context;
        private readonly Lazy<ICompanyRepository> _companyRepository;
        private readonly Lazy<IEmployeeRepository> _employeeRepository;
        public RepositoryManager(DatabaseContext context)
        {
            _context = context;
            _companyRepository = new Lazy<ICompanyRepository>(() => new CompanyRepository(_context));
            _employeeRepository = new Lazy<IEmployeeRepository>(() => new EmployeeRepository(_context));
        }
        public ICompanyRepository Company => _companyRepository.Value;
        public IEmployeeRepository Employee => _employeeRepository.Value;
        public async Task SaveAsync() => await _context.SaveChangesAsync();
    }
}
