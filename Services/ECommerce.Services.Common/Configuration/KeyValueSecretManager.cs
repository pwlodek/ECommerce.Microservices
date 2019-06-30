using Microsoft.Azure.KeyVault.Models;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Services.Common.Configuration
{
    public class KeyValueSecretManager : IKeyVaultSecretManager
    {
        public string GetKey(SecretBundle secret)
        {
            var val = secret.SecretIdentifier.Name.Replace("-", ":");
            return val;
        }

        public bool Load(SecretItem secret)
        {
            return true;
        }
    }
}
