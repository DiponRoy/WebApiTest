using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Api.Model
{
    public interface IResponse<TSource>
    {
        bool IsSuccess { get; set; }
        TSource Data { get; set; }
        Exception Exception { get; set; }
    }
}
