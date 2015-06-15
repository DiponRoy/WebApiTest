using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Api.Response
{
    public interface IDataPayLoad<TItem>
    {
        IEnumerable<TItem> Items { get; set; }
        int? PageSize { get; set; }
        int? PageNo { get; set; }
        int? Total { get; set; }
    }
}
