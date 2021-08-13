using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper.Fluent.API.Util;
using Dapper.Fluent.Application;
using Dapper.Fluent.Domain.Contracts;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Dapper.Fluent.Domain;
using Dapper.Fluent.Repository.Contracts;
using Dapper.Fluent.Repository.Impl;
using Dapper.Fluent.ORM.Extensions;
using Dapper.Fluent.ORM.Contracts;
using Dapper.Fluent.ORM.Postgres.Extensions;

namespace Dapper.Fluent.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddPostgresRepositoryWithMigration(Configuration["ConnectionString"])
                .AddMapperConfiguration<MapperConfiguration>()
                .AddDapperORM();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Dapper.Fluent.API", Version = "v1" });
            });

            services.AddScoped<IRequestInfo, RequestInfo>();
            services.AddScoped<IDapperFluentService, DapperFluentService>();
            services.AddScoped<IPublicSchemaEntityRepository, PublicSchemaEntityRepository>();
            services.AddScoped<ILogRepository, LogRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDapperORMRunner dapper)
        {
            dapper.AddMapsAndRunMigrations();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Dapper.Fluent.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
