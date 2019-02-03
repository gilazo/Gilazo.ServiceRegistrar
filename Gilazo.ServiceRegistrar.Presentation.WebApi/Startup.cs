using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Gilazo.ServiceRegistrar.Application;
using Gilazo.ServiceRegistrar.Infrastructure;
using Gilazo.ServiceRegistrar.Presentation.WebApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
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
            services.AddHttpClient();

            BsonClassMap.RegisterClassMap<MongoService>(cm =>
            {
                cm.AutoMap();
                cm.MapIdMember(m => m.MongoId)
                  .SetSerializer(new StringSerializer(BsonType.ObjectId))
                  .SetIdGenerator(StringObjectIdGenerator.Instance)
                  .SetIgnoreIfDefault(true);
            });

            services.AddScoped<HttpClient>(s => s.GetService<IHttpClientFactory>().CreateClient());
            
            services.AddScoped<MongoClient>(_ => new MongoClient("mongodb://mongo:27017"));
            services.AddScoped<IMongoDatabase>(s => s.GetService<MongoClient>().GetDatabase("Gilazo-ServiceRegistrar"));
            services.AddScoped<IMongoCollection<MongoService>>(
                s => s.GetService<IMongoDatabase>().GetCollection<MongoService>("services"));
         
            services.AddScoped<IRegisterable<Service>, UpsertableMongoService>();
            services.AddScoped<Application.IQueryable<MongoService, Service>, FindableMongoService>();

            services.AddHostedService<HeartbeatUpdatableService>();
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
