using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gilazo.ServiceRegistrar.Application;
using Gilazo.ServiceRegistrar.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Gilazo.ServiceRegistrar.Presentation.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc()
                .AddJsonOptions(opt =>
                {
                    opt.SerializerSettings.Converters.Add(new StringEnumConverter());
                    opt.SerializerSettings.ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() };
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddApiVersioning();
            
            BsonClassMap.RegisterClassMap<Service>(cm =>
            {
                cm.AutoMap();
                cm.MapIdMember(c => c.Id);
                cm.MapCreator(s => new Service(s.Id, s.Name, s.DocumentationUrl, s.StatusUrl, s.Status));
            });
            
            services.AddScoped<MongoClient>(_ => new MongoClient("mongodb://localhost:27017"));
            services.AddScoped<IMongoDatabase>(s => s.GetService<MongoClient>().GetDatabase("Gilazo-ServiceRegistrar"));
            services.AddScoped<IMongoCollection<Service>>(
                s => s.GetService<IMongoDatabase>().GetCollection<Service>("services"));
         
            services.AddScoped<UpsertableMongoService>();
            services.AddScoped<FindableMongoService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
