using BankRecord.Communication.Configuration;
using BankRecord.Data.Repositorys;
using BuyRequest.Application.Interfaces;
using BuyRequest.Application.Services;
using BuyRequest.Data.Context;
using BuyRequest.Data.Repositorys;
using BuyRequest.Data.Repositorys.BuyRequest;
using BuyRequest.Data.Repositorys.ProductRequest;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Text.Json.Serialization;

namespace BuyRequestAPI
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

            services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BuyRequestAPI", Version = "v1" });
            });
            services.AddDbContext<BuyRequestDataContext>(cfg =>
            {
                cfg.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddBankRecordConfig(Configuration);
            services.AddScoped<IBuyRequestRepository, BuyRequestRepository>();
            services.AddScoped<IProductRequestRepository, ProductRequestRepository>();
            services.AddScoped<IBuyRequestService, BuyRequestService>();
            services.AddScoped<IProductRequestService, ProductRequestService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BuyRequestAPI v1"));
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
