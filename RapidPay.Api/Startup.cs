using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RapidPay.Domain.Interfaces;
using RapidPay.Persistance;
using RapidPay.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;

namespace RapidPay.Api
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
            services.AddScoped<ICardManager, CardManager>();
            services.AddScoped<IAuthenticationManager, AuthenticationManager>();
            services.AddSingleton<IPaymentFeeManager, PaymentFeeManager>();

            services.AddDbContext<DatabaseContext>(options => options.
                   UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                           b => b.MigrationsAssembly("RapidPay.Persistance")));

            services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, Handlers.BasicAuthenticationHandler>("BasicAuthentication", null);

            services.AddControllers();
       

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
