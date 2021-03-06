using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ASP.NetCoreTwitterOAuth.Data;
using ASP.NetCoreTwitterOAuth.Services;
using Tweetinvi;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Twitter;
using Tweetinvi.Models;

namespace ASP.NetCoreTwitterOAuth
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            services.AddAuthentication().AddTwitter(i => {
                i.Events = new Microsoft.AspNetCore.Authentication.Twitter.TwitterEvents() {
                    //OnRedirectToAuthorizationEndpoint = TwitterRedirectToAuthorizationEndpointEventHandler, Redirection fails. Submitted a feedback to Microsoft.
                    OnCreatingTicket = TwitterCreatingTicketEventHandler
                };
                i.RetrieveUserDetails = true;
                i.ConsumerKey = Configuration["Authentication:Twitter:ConsumerKey"];
                i.ConsumerSecret = Configuration["Authentication:Twitter:ConsumerSecret"];
            });
            services.AddMvc()
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AuthorizeFolder("/Account/Manage");
                    options.Conventions.AuthorizePage("/Account/Logout");
                });
            Auth.SetUserCredentials(Configuration["Authentication:Twitter:ConsumerKey"], Configuration["Authentication:Twitter:ConsumerSecret"], Configuration["Authentication:Twitter.AccessToken"], Configuration["Authentication:Twitter.AccessTokenSecret"]);
            // Register no-op EmailSender used by account confirmation and password reset during development
            // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=532713
            services.AddSingleton<IEmailSender, EmailSender>();
        }
        private async Task TwitterRedirectToAuthorizationEndpointEventHandler(RedirectContext<TwitterOptions> context)
        {
        }
        private async Task TwitterCreatingTicketEventHandler(TwitterCreatingTicketContext context)
        {
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            } else
                app.UseExceptionHandler("/Error");
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
