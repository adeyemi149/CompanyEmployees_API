using CompanyEmployees.Entities.Models;
using CompanyEmployees.Repository.Extension.Utility;
using System.Linq.Dynamic.Core;

namespace CompanyEmployees.Repository.Extension
{
    public static class RepositoryCompanyExtension
    {
        public static IQueryable<Company> Search(this IQueryable<Company> company, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return company;

            var lowerCase = searchTerm.Trim().ToLower();

            return company.Where(e => e.Name.ToLower().Contains(lowerCase));
        }

        public static IQueryable<Company> Sort(this IQueryable<Company> companies, string sortTerm)
        {
            if (string.IsNullOrWhiteSpace(sortTerm))
                return companies.OrderBy(e => e.Name);

            var orderQuery = OrderQueryBuilder.CreateOrderQuery<Company>(sortTerm);

            if (string.IsNullOrWhiteSpace(orderQuery))
                return companies.OrderBy(e => e.Name);

            return companies.OrderBy(orderQuery);
        }
    }
}
