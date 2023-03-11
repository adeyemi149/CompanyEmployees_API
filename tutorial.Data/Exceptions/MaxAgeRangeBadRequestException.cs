using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyEmployees.Entities.Exceptions
{
    public class MaxAgeRangeBadRequestException : BadRequestException
    {
        public MaxAgeRangeBadRequestException()
            :base("Max age can't be less than min age.")
        {

        }
    }
}
