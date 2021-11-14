using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using HAdmin.Data;
using HAdmin.Repositories;
using HAdmin.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace HAdmin
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

            services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(Configuration.GetConnectionString("context")));
            services.AddScoped<IDataContext>(provider => provider.GetService<DataContext>());
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IHasher, Hasher>();
            services.AddScoped<ITokenManager, TokenManager>();


            services.AddHttpContextAccessor();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = Configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])),
                    //other settings
                    ClockSkew = TimeSpan.Zero
                };
                options.Events = new JwtBearerEvents();
                options.Events.OnMessageReceived = context =>
                {
                    if (context.Request.Cookies.ContainsKey("APP-AUTH-COOKIE"))
                    {
                        context.Token = context.Request.Cookies["APP-AUTH-COOKIE"];
                    }
                    return Task.CompletedTask;
                };
            });

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddCors();
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddControllers();

            services.AddSwaggerDocument(config =>
            {
                config.PostProcess = document =>
                {
                    document.Info.Version = "v1";
                    document.Info.Title = "HAdmin";
                    document.Info.Description = "admin for test vuetify";
                    //document.Info.TermsOfService = "None";
                    document.Info.Contact = new NSwag.OpenApiContact
                    {
                        Name = "HWH",
                        Email = "cloudtom@gmail.com",
                    };
                };
            });
            services.AddSpaStaticFiles(config =>
{
    config.RootPath = "wwwroot";
});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 404 &&
                !System.IO.Path.HasExtension(context.Request.Path.Value) &&
                !context.Request.Path.Value.StartsWith("/api"))
                {
                    context.Request.Path = "/index.html";
                    context.Response.StatusCode = 200;
                    await next();
                }
            });

            app.UseStaticFiles();
            app.UseDefaultFiles();
            app.UseSpaStaticFiles();

            app.MapWhen(context =>
    context.Request.Path == "/",
    mainApp =>
    {
        mainApp.UseSpa(spa =>
            {
                spa.Options.SourcePath = "wwwroot";
                spa.Options.DefaultPageStaticFileOptions = new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "wwwroot"))
                };
            });
    }
);

            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors(x =>
            {
                var origin = new string[] {
                    "https://localhost:5001", "http://localhost:3000", "http://localhost:3001",
                };
                x
                .WithOrigins(origin)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
