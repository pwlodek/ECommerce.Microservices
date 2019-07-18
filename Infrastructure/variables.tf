variable "location" {
    default = "West Europe"
}

/*
 * NOTE: It is very poor practice to hardcode sensitive information
 * such as user name, password, etc. Hardcoded values are used here
 * only to simplify the sample.
 */
variable "db_admin_username" {
    default = "db_admin"
}
variable "db_admin_password" {
    default = "Password1234!"
}

variable "azuread_app_password" {
    default = "l6&lStl38C3WWE11Dc7x"
}

/*
 * Resource names
 */
variable "azuread_app_name" {
    default = "EcommerceApplication"
}

variable "rg_name" {
    default = "ecommerce-services"
}

variable "broker_name" {
    default = "ecommerce-broker"
}

variable "kv_name" {
    default = "ecommerce-keyvault"
}

variable "appconfig_name" {
    default = "ecommerce-appconfig"
}

variable "ai_name" {
    default = "ecommerce-ai"
}

variable "redis_name" {
    default = "ecommerce-redis"
}

variable "dbserver_name" {
    default = "ecommerce-dbserver"
}