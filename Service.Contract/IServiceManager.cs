using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contract
{
    public interface IServiceManager
    {
        public ICompanyService CompanyService { get; }
        public IEmployeeService EmployeeService { get; }
    }
}
