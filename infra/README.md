### Login to subscription
- <code>az login --tenant TENANT_ID --use-device-code</code>
- <code>az ad sp create-for-rbac --name "azure-ai-toolkit-sp" --role Owner --scopes /subscriptions/$subscriptionId --sdk-auth</code>

### Repository Secrets
- Secret Name: AZURE_CREDENTIALS
- Secret Value: 
<code>
{
  "clientId": "",
  "clientSecret": "",
  "subscriptionId": "",
  "tenantId": ""
}
</code>