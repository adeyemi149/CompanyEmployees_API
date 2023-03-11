using Shared.DataTransferObject;
using Shared.RequestFeatures;

namespace Service.Contract
{
    public interface ICompanyService
    {
        Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync(bool trackChanges, CompanyParameters companyParameters);
        Task<CompanyDto> GetCompanyAsync(Guid companyId, bool trackChanges);
        Task<CompanyDto> CreateCompanyAsync(CompanyForCreationDto company);
        Task<IEnumerable<CompanyDto>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
        Task<(IEnumerable<CompanyDto> companies, string ids)> CreateCompanyCollectionAsync(IEnumerable<CompanyForCreationDto> companyCollection);
        Task DeleteCompanyAsync(Guid companyId, bool trackChanges);
        Task UpdateCompanyAsync(Guid companyId, CompanyForUpdateDto companyForUpdate, bool trackChanges);
    }
}
