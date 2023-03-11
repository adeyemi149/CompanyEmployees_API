using AutoMapper;
using Contract.Interface;
using Serilog;
using Service.Contract;
using Services.DataShaping;
using Shared.DataTransferObject;
using Shared.RequestFeatures;
using System.Dynamic;
using CompanyEmployees.Contract.Interface;
using CompanyEmployees.Entities.Exceptions;
using CompanyEmployees.Entities.Models;

namespace Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        public readonly IDataShaper<EmployeeDto> _dataShaper;

        public EmployeeService(
            IRepositoryManager repository, 
            ILogger logger,
            IMapper mapper,
            IDataShaper<EmployeeDto> dataShaper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _dataShaper = dataShaper;
        }

        public async Task<EmployeeDto> CreateEmployeeForServiceAsync(Guid companyId, EmployeeForCreationDto employeeForCreation, bool trackChanges)
        {
            await CheckIfCompanyExists(companyId, trackChanges);

            var employeeEntity = _mapper.Map<Employee>(employeeForCreation);

            _repository.Employee.CreateEmployeeForCompany(companyId, employeeEntity);
            await _repository.SaveAsync();

            var employeeReturn = _mapper.Map<EmployeeDto>(employeeEntity);
            return employeeReturn;
        }

        public async Task DeleteEmployeeForCompanyAsync(Guid companyId, Guid id, bool trackChanges)
        {
            await CheckIfCompanyExists(companyId, trackChanges);

            var employeeForCompany = await GetEmployeeForCompanyAndCheckIfItExists(companyId, id, trackChanges);

            _repository.Employee.DeleteEmployee(employeeForCompany);
            await _repository.SaveAsync();
        }

        public async Task<EmployeeDto> GetEmployeeAsync(Guid companyId,Guid id, bool trackChanges)
        {
            await CheckIfCompanyExists(companyId, trackChanges);

            var employeeDb = await GetEmployeeForCompanyAndCheckIfItExists(companyId, id, trackChanges);

            var employee = _mapper.Map<EmployeeDto>(employeeDb);

            return employee;
        }

        public async Task<(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)> GetEmployeeForPatchAsync(Guid companyId, Guid id, bool comptrackChanges, bool empTrackChanges)
        {
            await CheckIfCompanyExists(companyId, comptrackChanges);

            var employeeEntity = await GetEmployeeForCompanyAndCheckIfItExists(companyId, id, empTrackChanges);

            var employeeToPatch = _mapper.Map<EmployeeForUpdateDto>(employeeEntity);

            return (employeeToPatch, employeeEntity);
        }

        public async Task SaveChangesForPatchAsync(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)
        {
            _mapper.Map(employeeToPatch, employeeEntity);
            await _repository.SaveAsync();
        }

        public async Task<(IEnumerable<ExpandoObject> employees, MetaData metaData)> GetEmployeesAsync(Guid companyId, EmployeeParameters employeeParameter, bool trackChanges)
        {
            if (!employeeParameter.ValidAge)
                throw new MaxAgeRangeBadRequestException();

            await CheckIfCompanyExists(companyId, trackChanges);

            var employeesWithMetadata= await _repository.Employee.GetEmployeesAsync(companyId, employeeParameter, trackChanges);
            
            var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employeesWithMetadata);

            var shapedData = _dataShaper.ShapeData(employeesDto, employeeParameter.Fields);

            return (employees: shapedData, metaData: employeesWithMetadata.MetaData);
        }

        public async Task UpdateEmployeeForCompanyAsync(Guid companyId, Guid id, EmployeeForUpdateDto employeeForUpdateDto, bool compTrackChanges, bool empTrackChanges)
        {
            await CheckIfCompanyExists(companyId, compTrackChanges);

            var employeeEntity = await GetEmployeeForCompanyAndCheckIfItExists(companyId, id, empTrackChanges);

            _mapper.Map(employeeForUpdateDto, employeeEntity);
            await _repository.SaveAsync();
        }

        private async Task CheckIfCompanyExists(Guid companyId, bool trackChanges)
        {
            var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges);
            if (company is null)
                throw new CompanyNotFoundException(companyId);
        }

        private async Task<Employee> GetEmployeeForCompanyAndCheckIfItExists(Guid companyId, Guid id, bool trackChanges)
        {
            var employee = await _repository.Employee.GetEmployeeAsync(companyId, id, trackChanges);
            if (employee is null)
                throw new EmployeeNotFoundException(id);

            return employee;
        }
    }
}   
