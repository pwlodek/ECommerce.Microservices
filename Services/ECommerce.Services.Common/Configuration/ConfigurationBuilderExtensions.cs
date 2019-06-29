using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Configuration.AzureKeyVault;

namespace ECommerce.Services.Common.Configuration
{
    public static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddCloud(this IConfigurationBuilder builder)
        {
            var configuration = builder.Build();
            var useCloud = configuration.GetValue<bool>("UseCloudServices");

            if (useCloud)
            {
                var azureServiceTokenProvider = new AzureServiceTokenProvider();
                var kv = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));

                builder.AddAzureKeyVault(configuration["ConnectionStrings:KeyVault"], kv, new KeyValueSecretManager());
                builder.AddAzureAppConfiguration(options =>
                {
                    options.ConnectWithManagedIdentity(configuration["ConnectionStrings:AppConfiguration"])
                        .Use(KeyFilter.Any, LabelFilter.Null);
                });
            }

            return builder;
        }
    }
}
