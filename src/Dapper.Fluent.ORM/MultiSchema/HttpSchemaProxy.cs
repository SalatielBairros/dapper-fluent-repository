using System;
using Dapper.Fluent.ORM.Contracts;
using Microsoft.AspNetCore.Http;

namespace Dapper.Fluent.ORM.MultiSchema;

public class HttpSchemaProxy : ISchema
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IRepositorySettings _settings;

    public HttpSchemaProxy(IHttpContextAccessor httpContextAccessor, IRepositorySettings settings)
    {
        _httpContextAccessor = httpContextAccessor;
        _settings = settings;
    }

    public string GetSchema()
    {
        var headers = _httpContextAccessor.HttpContext.Request.Headers;
        return headers["schema"].ToString() ?? _settings.DefaultSchema;
    }
}

