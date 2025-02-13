// Load abbreviated names for resources
var _abbrs = loadJsonContent('./abbreviations.json')

// Limit the scope of the deployment to the subscription level
targetScope = 'subscription'


// Define the parameters for the deployment
@minLength(1)
@maxLength(64)
@description('Name of the the environment which is used to generate a short unique hash used in all resources.')
param environmentName string

@minLength(1)
@description('Primary location for all resources')
param location string

@minLength(1)
@description('Name of the resource group to deploy all resources to')
param resourceGroupName string

@description('The current UTC timestamp in the format YYYY-MM-DDTHH:MM:SSZ')
param deploymentTimestamp string = utcNow()

@description('User or service principal identity to assign application roles')
param principalId string = ''
param principalType string = 'ServicePrincipal'
 

// Tags that should be applied to all resources.
var _tags = {
  // Tag all resources with the environment name.
  'azd-env-name': environmentName
}


// Organize resources in a resource group
resource rg 'Microsoft.Resources/resourceGroups@2021-04-01' = {
  name: !empty(resourceGroupName) ? resourceGroupName : '${_abbrs.resourcesResourceGroups}${environmentName}'
  location: location
  tags: _tags
}


// Outputs for post deployment actions
output AZURE_TENANT_ID string = tenant().tenantId
output AZURE_SUBSCRIPTION_ID string = subscription().subscriptionId
output AZURE_PRINCIPAL_ID string = principalId
output AZURE_PRINCIPAL_TYPE string = principalType
output AZURE_DEPLOYMENT_TIMESTAMP string = deploymentTimestamp

output AZURE_ENVIRONMENT_NAME string = environmentName
output AZURE_RESOURCE_GROUP string = rg.name
output AZURE_LOCATION string = rg.location




