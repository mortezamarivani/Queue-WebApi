using System.IO;
using Gx.Core.Utilities.Extensions.Connection;
using Gx.DataLayer.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Gx.DataLayer.Repository;
using Gx.Core.Services.Interfaces;
using Gx.Core.Services.Implementations;
using Gx.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Gx.Core.Security;

namespace AngularEshop.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region Authentication

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "https://localhost:44381",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("AngularEshopJwtBearer"))
                    };
                });

            #endregion

            #region CORS

            services.AddCors(options =>
            {
                options.AddPolicy("EnableCors", builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .Build();
                });
            });

            #endregion


            services.AddSingleton<IConfiguration>(
                new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile($"appsettings.json")
                    .Build()
            );

            services.AddApplicationDbContext(Configuration);

            /*services.AddDbContext<AngularEshopDbContext>(options =>
            {
                var connectionString = "ConnectionStrings:AngularEshopConnection:Development";
                options.UseSqlServer(Configuration[connectionString]);
            });*/

            #region Add DbContext

            services.AddApplicationDbContext(Configuration);
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            #endregion

            #region Application Services

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IQueueService, QueueService>();
            services.AddScoped<IPasswordHelper, PasswordHelper>();

            

            #endregion

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
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
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseCors("EnableCors");
            app.UseAuthentication();

            app.UseMvc();
            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller=Users}/{action=Users}/{id?}"
            //    );
            //});
        }
    }
}