using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Domain.Enum
{
    public enum StatusCode
    {
        OK = 200,
        InternalServerError = 500,
        GameNotFound = 30
    }
}
