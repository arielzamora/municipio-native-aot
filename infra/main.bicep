param location string = resourceGroup().location
param environmentName string = 'env-municipio-poc'
param sqlServerName string = 'sql-municipio-poc-${uniqueString(resourceGroup().id)}'
param sqlDatabaseName string = 'db-municipio-payments'
param sqlAdminLogin string = 'sqladmin'
@secure()
param sqlAdminPassword string

param serviceBusNamespaceName string = 'sb-municipio-poc-${uniqueString(resourceGroup().id)}'
param containerRegistryName string = 'cr-municipio-poc${uniqueString(resourceGroup().id)}'

// 1. Azure Container Registry (ACR)
resource acr 'Microsoft.ContainerRegistry/registries@2023-07-01' = {
  name: containerRegistryName
  location: location
  sku: {
    name: 'Basic'
  }
  properties: {
    adminUserEnabled: true
  }
}

// 2. Azure SQL Database
resource sqlServer 'Microsoft.Sql/servers@2021-11-01' = {
  name: sqlServerName
  location: location
  properties: {
    administratorLogin: sqlAdminLogin
    administratorLoginPassword: sqlAdminPassword
  }
}

resource sqlFirewallAllowAll 'Microsoft.Sql/servers/firewallRules@2021-11-01' = {
  parent: sqlServer
  name: 'AllowAllAzureIPs'
  properties: {
    startIpAddress: '0.0.0.0'
    endIpAddress: '0.0.0.0'
  }
}

resource sqlDatabase 'Microsoft.Sql/servers/databases@2021-11-01' = {
  parent: sqlServer
  name: sqlDatabaseName
  location: location
  sku: {
    name: 'Basic'
    tier: 'Basic'
    capacity: 5
  }
}

// 3. Azure Service Bus
resource serviceBusNamespace 'Microsoft.ServiceBus/namespaces@2021-11-01' = {
  name: serviceBusNamespaceName
  location: location
  sku: {
    name: 'Standard'
  }
}

resource auditQueue 'Microsoft.ServiceBus/namespaces/queues@2021-11-01' = {
  parent: serviceBusNamespace
  name: 'audit-payments-queue'
}

// 4. Log Analytics Workspace
resource logAnalyticsWorkspace 'Microsoft.OperationalInsights/workspaces@2021-06-01' = {
  name: 'log-municipio-poc'
  location: location
  properties: {
    sku: {
      name: 'PerGB2018'
    }
  }
}

// 5. Container Apps Environment
resource environment 'Microsoft.App/managedEnvironments@2023-05-01' = {
  name: environmentName
  location: location
  properties: {
    appLogsConfiguration: {
      destination: 'log-analytics'
      logAnalyticsConfiguration: {
        customerId: logAnalyticsWorkspace.properties.customerId
        sharedKey: logAnalyticsWorkspace.listKeys().primarySharedKey
      }
    }
  }
}

// Outputs
output acrName string = acr.name
output acrLoginServer string = acr.properties.loginServer
output sqlServerName string = sqlServer.name
output serviceBusNamespace string = serviceBusNamespace.name
