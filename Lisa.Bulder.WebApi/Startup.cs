using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Cors.Core;
using Microsoft.Framework.DependencyInjection;
using Microsoft.WindowsAzure.Storage;
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
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseIISPlatformHandler();
            app.UseCors("allowAll");
            app.UseMvc();

            var account = CloudStorageAccount.Parse("UseDevelopmentStorage=true");
            var client = account.CreateCloudTableClient();

            _messages = client.GetTableReference("messages");
            _channels = client.GetTableReference("channels");
            _subscriptions = client.GetTableReference("subscriptions");
            _users = client.GetTableReference("users");

            _channels.CreateIfNotExistsAsync();
            _messages.CreateIfNotExistsAsync();
            _users.CreateIfNotExistsAsync();
            _subscriptions.CreateIfNotExistsAsync();
        }

        private CloudTable _messages;
        private CloudTable _channels;
        private CloudTable _subscriptions;
        private CloudTable _users;
    }
}