/*
# -------------------------------------------------------------------------------
# Author:      Cody Martin <cody.martin@blacklanternsecurity.com>
#
# Created:     10-15-2020
# Copyright:   (c) BLS OPS LLC. 2020
# Licence:     GPL
# -------------------------------------------------------------------------------
*/

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Enter_The_Matrix.Models;
using Microsoft.Extensions.Options;
using Enter_The_Matrix.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.HttpOverrides;
using System;

namespace Enter_The_Matrix
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
            services.AddControllersWithViews();

            // LDAP
            services.Configure<LdapConfig>(Configuration.GetSection("Ldap"));
            services.AddScoped<IAuthenticationService, LdapAuthenticationService>();

            // MongoDB
            services.Configure<ETMDatabaseSettings>(
                Configuration.GetSection(nameof(ETMDatabaseSettings)));
            services.AddSingleton<IETMDatabaseSettings>(provider =>
                provider.GetRequiredService<IOptions<ETMDatabaseSettings>>().Value);

            services.AddControllers();

            // Local Authentication
            services.Configure<LocalAuthSettings>(
                Configuration.GetSection(nameof(LocalAuthSettings)));
            services.AddSingleton<ILocalAuthSettings>(provider =>
                provider.GetRequiredService<IOptions<LocalAuthSettings>>().Value);
            services.AddScoped<UsersService>();

            services.AddScoped<AssessmentsService>();
            services.AddScoped<ScenariosService>();
            services.AddScoped<StepsService>();
            services.AddScoped<SteplatesService>();
            services.AddScoped<TreeService>();

            // Auth Cookies
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.Cookie.Name = "BLS.AuthCokieAspNetCore";
                options.LoginPath = "/Security/Login";
                options.LogoutPath = "/Security/Logout";
                options.ExpireTimeSpan = TimeSpan.FromHours(2);
            });

            // Default Cookies
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.Strict;
                options.HttpOnly = HttpOnlyPolicy.Always;
                options.Secure = CookieSecurePolicy.Always;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseAuthentication();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // LDAP Authorization
            app.UseAuthorization();

            // Cookie Authentication
            app.UseCookiePolicy();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "login",
                    pattern: "{controller=Security}/{action=Login}");
                endpoints.MapControllerRoute(
                    name: "logout",
                    pattern: "{controller=Security}/{action=Logout}");
            });
        }
    }
}
