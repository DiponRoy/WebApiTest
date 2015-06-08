using System;

namespace Web.Api.Model
{
    public class Response<TSource> : IResponse<TSource>
    {
        //*if any response data and not exception type, success true*/
        public bool IsSuccess { get; set; }
        public TSource Data { get; set; }
        //*if any exception data, success false*/
        public Exception Exception { get; set; }

        private Response()
        {
            Data = default(TSource);
            Exception = null;
        }

        private Response(bool isSuccess)
            : this()
        {
            IsSuccess = isSuccess;
        }

        /*sets response data*/

        public Response(TSource data)
            : this(true)
        {
            Data = data;
        }

        /*sets error*/

        public Response(Exception exception)
            : this(false)
        {
            Exception = exception;
        }
    }
}