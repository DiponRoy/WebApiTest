using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Api.Response
{
    internal class DataPayLoad<TItem> : IDataPayLoad<TItem>
    {
        public IEnumerable<TItem> Items { get; set; }
        public int? PageSize { get; set; }
        public int? PageNo { get; set; }
        public int? Total { get; set; }
    }
}