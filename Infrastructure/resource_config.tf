# create key vault
resource "azurerm_key_vault" "ecommerce_kv" {
  name                        = "${var.kv_name}"
  location                    = "${azurerm_resource_group.rg.location}"
  resource_group_name         = "${azurerm_resource_group.rg.name}"
  enabled_for_disk_encryption = true
  tenant_id                   = "${data.azurerm_client_config.current.tenant_id}"

  sku {
      name = "standard"
  }
  
  tags = {
    environment = "Production"
  }
}

resource "azurerm_key_vault_access_policy" "admin_access" {
  key_vault_id = "${azurerm_key_vault.ecommerce_kv.id}"

  tenant_id = "${data.azurerm_client_config.current.tenant_id}"
  object_id = "${data.external.thisAccount.result.objectId}"

  key_permissions = [
      "create",
      "get",
      "list",
      "update"
  ]

  secret_permissions = [
      "set",
      "get",
      "list",
      "delete",
  ]
}

# This creates Azure App config resource, TF does not yet support this natively
# hence ARM is used directly
resource "azurerm_template_deployment" "ecommerce_app_config" {
  name                = "${var.appconfig_name}"
  resource_group_name = "${azurerm_resource_group.rg.name}"

  template_body = <<DEPLOY
{
  "$schema": "https://schema.management.azure.com/schemas/2015-01-01/deploymentTemplate.json#",
    "contentVersion": "1.0.0.0",
    "parameters": {
        "appconfig_name": {
            "defaultValue": "ecommerce-appconfig",
            "type": "String"
        },
        "appconfig_location": {
            "defaultValue": "westcentralus",
            "type": "String"
        }
    },
    "variables": {},
    "resources": [
        {
            "type": "Microsoft.AppConfiguration/configurationStores",
            "apiVersion": "2019-02-01-preview",
            "name": "[parameters('appconfig_name')]",
            "location": "[parameters('appconfig_location')]",
            "properties": {
                "endpoint": "[concat('https://', parameters('appconfig_name'), '.azconfig.io')]"
            }
        }
    ]
}
DEPLOY

  # these key-value pairs are passed into the ARM Template's `parameters` block
  parameters = {
    "appconfig_name" = "${var.appconfig_name}"
    "appconfig_location" = "${azurerm_resource_group.rg.location}"
  }

  deployment_mode = "Incremental"
}