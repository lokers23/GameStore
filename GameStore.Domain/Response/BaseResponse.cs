using GameStore.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Domain.Response
{
    public class BaseResponse<T> : IBaseResponse<T>
    {
        public string Discription { get; set; }
        public StatusCode StatusCode { get; set; }
        public T Data { get; set; }

    }

    public interface IBaseResponse<T> 
    {
        string Discription { get; set; }
        StatusCode StatusCode { get; set; }
        T Data { get; set; }
    }

}
