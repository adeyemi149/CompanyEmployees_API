using AutoMapper;
using Serilog;
using Service.Contract;
using Shared.DataTransferObject;
using Shared.RequestFeatures;
using CompanyEmployees.Contract.Interface;
using CompanyEmployees.Entities.Exceptions;
using CompanyEmployees.Entities.Models;

namespace Services
{
    public class CompanyService : ICompanyService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public CompanyService(
            IRepositoryManager repository, ILogger logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<CompanyDto> CreateCompanyAsync(CompanyForCreationDto company)
        {
            var companyEntity = _mapper.Map<Company>(company);

            _repository.Company.CreateCompany(companyEntity);
            await _repository.SaveAsync();

            var companyToReturn = _mapper.Map<CompanyDto>(companyEntity);
            return companyToReturn;
        }

        public async Task<(IEnumerable<CompanyDto> companies, string ids)> CreateCompanyCollectionAsync(IEnumerable<CompanyForCreationDto> companyCollection)
        {
            if (companyCollection is null)
                throw new CompanyCollectionBadRequest();

            var companyEntities = _mapper.Map<IEnumerable<Company>>(companyCollection);
            foreach(var company in companyEntities)
            {
                _repository.Company.CreateCompany(company);
            }
            await _repository.SaveAsync();

            var companyCollectionReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
            var ids = string.Join(",", companyCollectionReturn.Select(c => c.Id));
            
            return (companies: companyCollectionReturn, ids: ids);
        }

        public async Task DeleteCompanyAsync(Guid companyId, bool trackChanges)
        {
            var company = await GetCompanyAndCheckIfItExists(companyId, trackChanges);

            _repository.Company.DeleteCompany(company);
            await _repository.SaveAsync();
        }

        public async Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync(bool trackChanges, CompanyParameters companyParameters)
        {
                var companies = await _repository.Company.GetAllCompaniesAsync(trackChanges, companyParameters);

                var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);

                return companiesDto;
        }

        public async Task<IEnumerable<CompanyDto>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges)
        {
            if (ids is null)
                throw new IdParameterBadRequestException();

            var companyEntites = await _repository.Company.GetByIdsAsync(ids, trackChanges);
            if (ids.Count() != companyEntites.Count())
                throw new CollectionByIdsBadRequestException();

            var employeeReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntites);
            return employeeReturn;
        }

        public async Task<CompanyDto> GetCompanyAsync(Guid id, bool trackChanges)
        {
            var company = await GetCompanyAndCheckIfItExists(id, trackChanges);

            var companyDto = _mapper.Map<CompanyDto>(company);
            return companyDto;
        }

        private async Task<Company> GetCompanyAndCheckIfItExists(Guid id, bool trackChanges)
        {
            var company = await _repository.Company.GetCompanyAsync(id, trackChanges);
            if (company is null)
            {
                throw new CompanyNotFoundException(id);
            }

            return company;
        }

        public async Task UpdateCompanyAsync(Guid companyId, CompanyForUpdateDto companyForUpdate, bool trackChanges)
        {
            var company = await GetCompanyAndCheckIfItExists(companyId, trackChanges);

            _mapper.Map(companyForUpdate, company);
            await _repository.SaveAsync();
        }
    }
}