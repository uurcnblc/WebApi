using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiAuth.Authorization;
using WebApiAuth.DB;
using WebApiAuth.Models;
using WebApiAuth.Repository.Interfaces;

namespace WebApiAuth
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
            services.Configure<ApplicationSettings>(Configuration.GetSection("ApplicationSettings"));
            services.AddDbContext<MarketContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            var key = Configuration["ApplicationSettings:SecretKey"].ToString();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["ApplicationSettings:Issuer"],
                        ValidAudience = Configuration["ApplicationSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(key))
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(CustomPolicyTypes.Orders.Manage, policy => { policy.RequireClaim(CustomClaimTypes.Permission, StaticPermissions.Orders.Create); });
                options.AddPolicy(CustomPolicyTypes.Orders.Edit, policy => { policy.RequireClaim(CustomClaimTypes.Permission, StaticPermissions.Orders.Edit); });
            });

            services.AddTransient<IUserRepository, IUserRepository>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApiAuth", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApiAuth v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
