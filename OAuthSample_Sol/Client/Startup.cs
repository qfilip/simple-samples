using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace Client
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(cfg =>
            {
                cfg.DefaultAuthenticateScheme = "ClientCookie";
                cfg.DefaultSignInScheme = "ClientCookie";
                cfg.DefaultChallengeScheme = "server-schema";
            })
                .AddCookie("ClientCookie")
                .AddOAuth("server-schema", cfg =>
                {
                    cfg.ClientId = "client_id";
                    cfg.ClientSecret = "client_secret";
                    cfg.CallbackPath = "/oauth/callback";
                    cfg.Scope.Add("name");
                    cfg.AuthorizationEndpoint = "https://localhost:44355/oauth/authorize";
                    cfg.TokenEndpoint = "https://localhost:44355/oauth/token";
                    cfg.CorrelationCookie.SameSite = SameSiteMode.Lax; 
                    cfg.SaveTokens = true;

                    cfg.Events = new OAuthEvents()
                    {
                        OnTicketReceived = context =>
                        {
                            return Task.CompletedTask;
                        },
                        OnCreatingTicket = context =>
                        {
                            return Task.CompletedTask;
                        },
                        OnRemoteFailure = context =>
                        {
                            return Task.CompletedTask;
                        },
                    };
                });


            services.AddControllers();
            services.AddCors(options => options.AddDefaultPolicy(builder => builder.AllowAnyHeader().AllowAnyMethod().SetIsOriginAllowed(origin => true)));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors();
            app.UseCookiePolicy(new CookiePolicyOptions()
            {
                HttpOnly = HttpOnlyPolicy.Always,
                Secure = CookieSecurePolicy.Always,
                MinimumSameSitePolicy = SameSiteMode.Lax
            });
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action}");
            });
        }
    }
}
