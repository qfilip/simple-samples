using System.Reflection;
using MediatorPipeline.MediatorBase;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MediatorPipeline
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
            services.AddControllers();
            services.AddMediatR(typeof(BaseHandler<,>).GetTypeInfo().Assembly);
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(BasePipelineBehavior<,>));
            services.AddScoped(typeof(IRequestPreProcessor<>), typeof(BaseRequestPreProcessor<>));
            services.AddScoped(typeof(IRequestPostProcessor<,>), typeof(BaseRequestPostProcessor<,>));
            services.AddScoped(typeof(IRequestExceptionHandler<,>), typeof(BaseExceptionHandler<,>));
            services.AddScoped(typeof(IRequestExceptionAction<,>), typeof(BaseExceptionAction<,>));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

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
