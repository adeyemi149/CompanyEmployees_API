using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyEmployees.Entities.Exceptions
{
    public class CollectionByIdsBadRequestException : BadRequestException
    {
        public CollectionByIdsBadRequestException()
            :base("Collection count mismatch comparing to ids.")
        {

        }
    }
}
