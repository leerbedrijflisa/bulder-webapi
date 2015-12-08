using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Cors.Core;
using Microsoft.Framework.DependencyInjection;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json.Serialization;

namespace Lisa.Bulder.WebApi
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            var policy = new CorsPolicy();
            policy.Origins.Add("*");
            policy.Methods.Add("*");
            policy.Headers.Add("*");
            services.AddCors(config => config.AddPolicy("allowAll", policy));

            services.AddInstance<IEmailService>(new SmtpEmailService());
            services.AddInstance(new Database());
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseIISPlatformHandler();
            app.UseCors("allowAll");
            app.UseMvc();
        }
    }
}