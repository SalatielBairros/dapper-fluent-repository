using System;
using Microsoft.AspNetCore.Http;

namespace Dapper.Fluent.ORM.MultiSchema
{
    public class RequestInfo : IRequestInfo
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RequestInfo(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetSchema()
        {
            var headers = _httpContextAccessor.HttpContext.Request.Headers;
            return headers["schema"].ToString() ?? "dapper";
        }
    }
}
