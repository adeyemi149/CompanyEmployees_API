﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyEmployees.Entities.Exceptions
{
    public class IdParameterBadRequestException : BadRequestException
    {
        public IdParameterBadRequestException()
            :base("Parameter ids is null")
        {

        }
    }
}
