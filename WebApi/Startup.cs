using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Text;
using WebApi.Authorization;
using WebApi.DB;
using WebApi.Models;

namespace WebApi
{
    public class Startup
    {
        private static string AllowedCors = "_allowedCors";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.Configure<ApplicationSettings>(Configuration.GetSection("ApplicationSettings"));
            services.AddDbContext<ApiContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApiContext>();


            //User Manager içindeki password validasyonlarýný kaldýralým
            services.Configure<IdentityOptions>(options =>
            {
                //options.SignIn.RequireConfirmedPhoneNumber = true;
                //options.SignIn.RequireConfirmedEmail = true;
                //options.SignIn.RequireConfirmedAccount = true;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
            });

            //var key = Encoding.UTF8.GetBytes(Configuration["ApplicationSettings:SecretKey"].ToString());
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




            foreach (var item in Permissions.ModuleList)
            {
                List<string> modulePermissions = Permissions.GeneratePermissionsForModule(item);
                foreach (var subPermission in modulePermissions)
                {
                    services.AddAuthorization(options =>
                    {
                        options.AddPolicy(subPermission, builder =>
                        {
                            builder.Requirements.Add(new PermissionRequirement(subPermission));
                        });
                    });
                    //options.AddPolicy(subPermission,
                    //    policy => policy.RequireClaim(subPermission, "true"));
                }
            }


            //Custom Policy Provider
            //services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
            //Customer Authorization Handler
            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();


            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "WebApi",
                    Version = "v1"
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                       {
                         new OpenApiSecurityScheme
                         {
                           Reference = new OpenApiReference
                           {
                             Type = ReferenceType.SecurityScheme,
                             Id = "Bearer"
                           }
                          },
                          new string[] { }
                        }
                      });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi v1"));
            }

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            app.UseHttpsRedirection();
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
