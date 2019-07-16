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

/*
 * Resource names
 */
variable "azuread_app_name" {
    default = "EcommerceApplication"
}

variable "azuread_app_password" {
    default = "l6&lStl38C3WWE11Dc7x"
}

variable "rg_name" {
    default = "ecommerce-services"
}

variable "broker_name" {
    default = "ecommerce-broker1"
}

variable "kv_name" {
    default = "ecommerce-keyvault1"
}

variable "appconfig_name" {
    default = "ecommerce-appconfig1"
}

variable "ai_name" {
    default = "ecommerce-appinsights"
}

variable "redis_name" {
    default = "ecommerce-redis"
}