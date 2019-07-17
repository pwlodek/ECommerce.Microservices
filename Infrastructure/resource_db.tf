resource "azurerm_sql_server" "ecommerce_dbserver" {
  name                         = "${var.dbserver_name}"
  location                     = "${azurerm_resource_group.rg.location}"
  resource_group_name          = "${azurerm_resource_group.rg.name}"
  version                      = "12.0"
  administrator_login          = "${var.db_admin_username}"
  administrator_login_password = "${var.db_admin_password}"
}

# Allow external access
resource "null_resource" "dbserver_setup_rule_external_access" {

  # runs after database and security group providing external access is created
  depends_on = ["azurerm_sql_server.ecommerce_dbserver"]

    provisioner "local-exec" {
        command = "az sql server firewall-rule create -g ${azurerm_resource_group.rg.name} -s ${azurerm_sql_server.ecommerce_dbserver.name} -n allow_rule --start-ip-address ${chomp(data.http.myip.body)} --end-ip-address ${chomp(data.http.myip.body)}"
        
    }
}

/*
 * Database Products
 *
 */
resource "azurerm_sql_database" "products" {
  name                = "products"
  location            = "${azurerm_resource_group.rg.location}"
  resource_group_name = "${azurerm_resource_group.rg.name}"
  server_name         = "${azurerm_sql_server.ecommerce_dbserver.name}"
  edition             = "Basic"
  tags = {
    environment = "production"
  }
}

resource "azurerm_key_vault_secret" "products_db" {
  name         = "ConnectionStrings-ProductsDb"
  value        = "Server=tcp:${azurerm_sql_server.ecommerce_dbserver.fully_qualified_domain_name},1433;Initial Catalog=${azurerm_sql_database.products.name};Persist Security Info=False;User ID=${azurerm_sql_server.ecommerce_dbserver.administrator_login};Password=${azurerm_sql_server.ecommerce_dbserver.administrator_login_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
  key_vault_id = "${azurerm_key_vault.ecommerce_kv.id}"
  depends_on   = ["azurerm_key_vault_access_policy.admin_access"]

  tags = {
    environment = "Production"
    source = "terraform"
  }
}

resource "null_resource" "db_products_setup" {

  # runs after database and security group providing external access is created
  depends_on = ["azurerm_sql_database.products", "null_resource.dbserver_setup_rule_external_access"]

    provisioner "local-exec" {
        command = "sqlcmd -S ${azurerm_sql_server.ecommerce_dbserver.fully_qualified_domain_name} -U ${azurerm_sql_server.ecommerce_dbserver.administrator_login} -P ${azurerm_sql_server.ecommerce_dbserver.administrator_login_password} -d ${azurerm_sql_database.products.name} -i db_products.sql"
        
    }
}

/*
 * Database Customers
 *
 */
resource "azurerm_sql_database" "customers" {
  name                = "customers"
  location            = "${azurerm_resource_group.rg.location}"
  resource_group_name = "${azurerm_resource_group.rg.name}"
  server_name         = "${azurerm_sql_server.ecommerce_dbserver.name}"
  edition             = "Basic"

  tags = {
    environment = "production"
  }
}

resource "azurerm_key_vault_secret" "customers_db" {
  name         = "ConnectionStrings-CustomersDb"
  value        = "Server=tcp:${azurerm_sql_server.ecommerce_dbserver.fully_qualified_domain_name},1433;Initial Catalog=${azurerm_sql_database.customers.name};Persist Security Info=False;User ID=${azurerm_sql_server.ecommerce_dbserver.administrator_login};Password=${azurerm_sql_server.ecommerce_dbserver.administrator_login_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
  key_vault_id = "${azurerm_key_vault.ecommerce_kv.id}"
  depends_on   = ["azurerm_key_vault_access_policy.admin_access"]

  tags = {
    environment = "Production"
    source = "terraform"
  }
}

resource "null_resource" "db_customers_setup" {

  # runs after database and security group providing external access is created
  depends_on = ["azurerm_sql_database.customers", "null_resource.dbserver_setup_rule_external_access"]

    provisioner "local-exec" {
        command = "sqlcmd -S ${azurerm_sql_server.ecommerce_dbserver.fully_qualified_domain_name} -U ${azurerm_sql_server.ecommerce_dbserver.administrator_login} -P ${azurerm_sql_server.ecommerce_dbserver.administrator_login_password} -d ${azurerm_sql_database.customers.name} -i db_customers.sql"
        
    }
}

/*
 * Database Reporting
 *
 */
resource "azurerm_sql_database" "reporting" {
  name                = "reporting"
  location            = "${azurerm_resource_group.rg.location}"
  resource_group_name = "${azurerm_resource_group.rg.name}"
  server_name         = "${azurerm_sql_server.ecommerce_dbserver.name}"
  edition             = "Basic"

  tags = {
    environment = "production"
  }
}

resource "azurerm_key_vault_secret" "reporting_db" {
  name         = "ConnectionStrings-ReportingDb"
  value        = "Server=tcp:${azurerm_sql_server.ecommerce_dbserver.fully_qualified_domain_name},1433;Initial Catalog=${azurerm_sql_database.reporting.name};Persist Security Info=False;User ID=${azurerm_sql_server.ecommerce_dbserver.administrator_login};Password=${azurerm_sql_server.ecommerce_dbserver.administrator_login_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
  key_vault_id = "${azurerm_key_vault.ecommerce_kv.id}"
  depends_on   = ["azurerm_key_vault_access_policy.admin_access"]

  tags = {
    environment = "Production"
    source = "terraform"
  }
}

resource "null_resource" "db_reporting_setup" {

  # runs after database and security group providing external access is created
  depends_on = ["azurerm_sql_database.reporting", "null_resource.dbserver_setup_rule_external_access"]

    provisioner "local-exec" {
        command = "sqlcmd -S ${azurerm_sql_server.ecommerce_dbserver.fully_qualified_domain_name} -U ${azurerm_sql_server.ecommerce_dbserver.administrator_login} -P ${azurerm_sql_server.ecommerce_dbserver.administrator_login_password} -d ${azurerm_sql_database.reporting.name} -i db_reporting.sql"
        
    }
}

/*
 * Database Sales
 *
 */
resource "azurerm_sql_database" "sales" {
  name                = "sales"
  location            = "${azurerm_resource_group.rg.location}"
  resource_group_name = "${azurerm_resource_group.rg.name}"
  server_name         = "${azurerm_sql_server.ecommerce_dbserver.name}"
  edition             = "Basic"

  tags = {
    environment = "production"
  }
}

resource "azurerm_key_vault_secret" "sales_db" {
  name         = "ConnectionStrings-SalesDb"
  value        = "Server=tcp:${azurerm_sql_server.ecommerce_dbserver.fully_qualified_domain_name},1433;Initial Catalog=${azurerm_sql_database.sales.name};Persist Security Info=False;User ID=${azurerm_sql_server.ecommerce_dbserver.administrator_login};Password=${azurerm_sql_server.ecommerce_dbserver.administrator_login_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
  key_vault_id = "${azurerm_key_vault.ecommerce_kv.id}"
  depends_on   = ["azurerm_key_vault_access_policy.admin_access"]

  tags = {
    environment = "Production"
    source = "terraform"
  }
}

resource "null_resource" "db_sales_setup" {

  # runs after database and security group providing external access is created
  depends_on = ["azurerm_sql_database.sales", "null_resource.dbserver_setup_rule_external_access"]

    provisioner "local-exec" {
        command = "sqlcmd -S ${azurerm_sql_server.ecommerce_dbserver.fully_qualified_domain_name} -U ${azurerm_sql_server.ecommerce_dbserver.administrator_login} -P ${azurerm_sql_server.ecommerce_dbserver.administrator_login_password} -d ${azurerm_sql_database.sales.name} -i db_sales.sql"
        
    }
}