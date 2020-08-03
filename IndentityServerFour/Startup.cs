using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography.X509Certificates;
using IdentityServer4;
using IdentityServerFour;
using IdentityServerFour.Misc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sustainsys.Saml2;
using Sustainsys.Saml2.Metadata;

namespace IndentityServerFour
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

            var builder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
                options.AccessTokenJwtType = "JWT";
            })
              .AddTestUsers(TestUsers.Users)
              .AddInMemoryIdentityResources(Config.IdentityResources)
              .AddInMemoryApiScopes(Config.ApiScopes)
              .AddInMemoryClients(Config.Clients)
              .AddSigningCredential(new X509Certificate2("testclient.pfx", "test"));
            
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            builder.AddDeveloperSigningCredential();
            services.AddAuthentication(options =>
               {
                    options.DefaultScheme = "Cookies";
                    options.DefaultChallengeScheme = "oidc";
               })
              .AddSaml2(options =>
              {
                  options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                  options.SignOutScheme = IdentityServerConstants.DefaultCookieAuthenticationScheme;
                  options.SPOptions.EntityId = new EntityId("https://stubidp.sustainsys.com/Metadata");
                  options.IdentityProviders.Add(
                    new IdentityProvider(
                        new EntityId("https://stubidp.sustainsys.com/Metadata"), options.SPOptions)
                    {
                        LoadMetadata = true
                    });
                  options.SPOptions.ServiceCertificates.Add(new X509Certificate2("Sustainsys.Saml2.Tests.pfx"));
              })
              .AddCookie("Cookies")
               .AddOpenIdConnect("oidc", options =>
               {
                   options.Authority = "https://localhost:44359/";
                   options.RequireHttpsMetadata = false;
                   options.ClientId = "mvc";
                   options.SaveTokens = true;
                   options.ResponseType = "id_token token";
               });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if(env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
