using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Api.Response
{
    public interface IFilterDataPayLoad<TFilter, TItem> : IDataPayLoad<TItem>
    {
        TFilter Filter { get; set; }
    }
}
