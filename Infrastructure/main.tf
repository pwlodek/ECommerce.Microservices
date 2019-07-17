/*
 * Azure infrastructure required to run Ecommerce services in Azure (just the services)
 *
 */

provider "azurerm" {
    version = "=1.28.0"
}

# main resource group
resource "azurerm_resource_group" "rg" {
    name     = "${var.rg_name}"
    location = "${var.location}"
}

data "azurerm_client_config" "current" {
}

# IP visible outside
data "http" "myip" {
  url = "http://ipv4.icanhazip.com"
}

# Fetch current user info using the Azure cli
data "external" "thisAccount" {
  program = ["az","ad","signed-in-user","show","--query","{displayName: displayName,objectId: objectId,objectType: objectType}"]
}

/*
 * Azure Service Bus
 *
 */

resource "azurerm_servicebus_namespace" "ecommerce_broker" {
  name                = "${var.broker_name}"
  location            = "${azurerm_resource_group.rg.location}"
  resource_group_name = "${azurerm_resource_group.rg.name}"
  sku                 = "Standard"

  tags = {
    environment = "Production"
    source = "terraform"
  }
}

# Store SB connection string in key vault

resource "azurerm_key_vault_secret" "ecommerce_broker" {
  name         = "Brokers-ServiceBus-Url"
  value        = "${azurerm_servicebus_namespace.ecommerce_broker.default_primary_connection_string}"
  key_vault_id = "${azurerm_key_vault.ecommerce_kv.id}"
  depends_on   = ["azurerm_key_vault_access_policy.admin_access"]

  tags = {
    environment = "Production"
    source = "terraform"
  }
}

/*
 * Azure Application Insights
 *
 */

resource "azurerm_application_insights" "ecommerce_ai" {
  name                = "${var.ai_name}"
  location            = "${azurerm_resource_group.rg.location}"
  resource_group_name = "${azurerm_resource_group.rg.name}"
  application_type    = "Web"
}

resource "azurerm_key_vault_secret" "ecommerce_ai" {
  name         = "ApplicationInsights-InstrumentationKey"
  value        = "${azurerm_application_insights.ecommerce_ai.instrumentation_key}"
  key_vault_id = "${azurerm_key_vault.ecommerce_kv.id}"
  depends_on   = ["azurerm_key_vault_access_policy.admin_access"]

  tags = {
    environment = "Production"
    source      = "terraform"
  }
}

/*
 * Redis cache
 *
 */

resource "azurerm_redis_cache" "ecommerce_redis" {
  name                = "${var.redis_name}"
  location            = "${azurerm_resource_group.rg.location}"
  resource_group_name = "${azurerm_resource_group.rg.name}"
  capacity            = 0
  family              = "C"
  sku_name            = "Basic"
  enable_non_ssl_port = false
  minimum_tls_version = "1.2"

  redis_configuration {}

  tags = {
    environment = "Production"
    source = "terraform"
  }
}

resource "azurerm_key_vault_secret" "ecommerce_redis" {
  name         = "Cache-Redis"
  value        = "${azurerm_redis_cache.ecommerce_redis.name}.redis.cache.windows.net:6380,password=${azurerm_redis_cache.ecommerce_redis.primary_access_key},ssl=True,abortConnect=False"
  key_vault_id = "${azurerm_key_vault.ecommerce_kv.id}"
  depends_on   = ["azurerm_key_vault_access_policy.admin_access"]

  tags = {
    environment = "Production"
    source = "terraform"
  }
}