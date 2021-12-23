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
using Microsoft.OpenApi.Models;
using System.Reflection;

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
            //declaring the dependencies
            services.AddScoped<ICardManager, CardManager>();
            services.AddScoped<IAuthenticationManager, AuthenticationManager>();
            //declaring the singleton for the fee manager
            services.AddSingleton<IPaymentFeeManager, PaymentFeeManager>();

            //declaring sql Server as the provider for Entity framework
            services.AddDbContext<DatabaseContext>(options => options.
                   UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                           b => b.MigrationsAssembly("RapidPay.Persistance")));

            //declaring our handler for basic auth
            services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, Handlers.BasicAuthenticationHandler>("BasicAuthentication", null);

            //Adding the swagger
            services.AddSwaggerGen(options =>
            {
               
                //including basic auth option in swagger
                options.AddSecurityDefinition("basic", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "basic",
                    In = ParameterLocation.Header,
                    Description = "Basic Authorization header using the Bearer scheme."
                });

                //including basic auth requeriment in swagger
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "basic"
                            }
                    },
                    new string[] {}
                }
                });

            });

            services.AddControllers();
       

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseRouting();
            //Configuring authentication and authorization for the app
            app.UseAuthentication();
            app.UseAuthorization();
        
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
