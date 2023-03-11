using AutoMapper;
using Contract.Interface;
using Serilog;
using Service.Contract;
using Shared.DataTransferObject;
using CompanyEmployees.Contract.Interface;

namespace Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<ICompanyService> _companyService;
        private readonly Lazy<IEmployeeService> _employeeService;

        public ServiceManager(IRepositoryManager repositoryManager, ILogger logger, IMapper mapper, IDataShaper<EmployeeDto> dataShaper)
        {
            _companyService = new Lazy<ICompanyService>(() => new CompanyService(repositoryManager, logger, mapper));
            _employeeService = new Lazy<IEmployeeService>(() => new EmployeeService(repositoryManager, logger, mapper, dataShaper));
        }

        public ICompanyService CompanyService => _companyService.Value;
        public IEmployeeService EmployeeService => _employeeService.Value;
    }
}
