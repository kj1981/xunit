using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Dynamo.Helper;
using Dynamo.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Dynamo
{
    public class Startup
    {

        public IConfiguration Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(env.ContentRootPath)
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddMvc();
            services.AddSingleton<IConfiguration>(Configuration);
           
            services.Configure<Dictionary<string, string>>(op => Configuration.GetSection("AppSettings")?.Bind(op));
           
            services.AddSingleton<IDbClientInitialization, DbClientInitialization>();
            services.Configure<DbClientSettings>(c =>
            {
                c.Id = Configuration.GetValue<string>("AppSettings:awsId");
                c.Password = Configuration.GetValue<string>("AppSettings:awsPassword");
                c.Region = RegionEndpoint.GetBySystemName(Configuration.GetValue<string>("AppSettings:dynamoDbRegion"));
            });

            services.AddScoped(typeof(IDynamoDbRepository<>), typeof(DynamoDbRepository<>));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
