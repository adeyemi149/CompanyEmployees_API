namespace CompanyEmployees.Contract.Interface
{
    public interface IRepositoryManager
    {
        public ICompanyRepository Company { get; }
        public IEmployeeRepository Employee { get; }
        Task SaveAsync();
    }
}
