using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
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
                   

            // AWS Options
            var awsOptions = Configuration.GetAWSOptions();
            services.AddDefaultAWSOptions(awsOptions);

            var client = awsOptions.CreateServiceClient<IAmazonDynamoDB>();
            var dynamoDbOptions = new DynamoDbOptions();
            ConfigurationBinder.Bind(Configuration.GetSection("DynamoDbTables"), dynamoDbOptions);

            // This is where the magic happens
            services.AddScoped<IDynamoDbManager<MyModel>>(provider => new DynamoDbManager<MyModel>(client, dynamoDbOptions.MyModel));
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
