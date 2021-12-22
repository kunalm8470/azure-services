# Azure Services

## Table of Contents
- [Azure Key Vault](#azure-key-vault)

## Azure Key Vault

Key vault is a fully managed service in Azure to store secrets and keys using strong cryptographic algorithms or optional hardware based keys.

Steps to integrate in .NET 6 application -
1. Create a resource group.
2. Create a Key Vault app in the Azure portal.
3. Install [`az-cli`](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli).
4. Login first time using the az cli by typing 
    ```
    az login
    ```
5. Add Service principal for using az cli - 
    ```
    az ad sp create-for-rbac --name "<YOUR_SERVICE_PRINCIPAL_NAME>"
    ```
6. The `az-cli` will return a JSON object on executing the above command which looks like (Store this for later use) -
    ```javascript
    {
        "appId": "<uuidv4>",
        "displayName": "<YOUR_SERVICE_PRINCIPAL_NAME>",
        "name": "<uuidv4>",
        "password": "<random_string>",
        "tenant": "<uuidv4>"
    }
    ```
7. Head over to **Access Policies** tab in the Key vault app in Azure portal and select the Service Principal name in the dropdown and add `GET` and `LIST` permissions.
8. Navigate to **Secrets** tab and add any application secrets.
9. Copy the Vault URI in the Key vault app and add it into the `appsettings.json` file.
10. To authenticate we will use managed identities which will read 3 environment variables (`AZURE_CLIENT_ID`, `AZURE_CLIENT_SECRET`, and `AZURE_TENANT_ID`) to ascertain our identity.
11. `AZURE_CLIENT_ID` corresponds to `appId`, `AZURE_CLIENT_SECRET` corresponds to `password`, and `AZURE_TENANT_ID` corresponds to `tenant` from step 6, copy this in to the `launchSettings.json` file as we will be reading them as environment variables for the current process.
    ```javascript
    "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development",
        "AZURE_CLIENT_ID": "<VALUE>",
        "AZURE_CLIENT_SECRET": "<VALUE>",
        "AZURE_TENANT_ID": "<VALUE>"
    }
    ```
12. Install the following nuget packages for Azure SDK -
    - `Azure.Extensions.AspNetCore.Configuration.Secrets` (For `.AddAzureKeyVault` extension method for using Azure key vault as partial configuation)
    - Azure.Identity (For `DefaultAzureCredential`)
    - Azure.Security.KeyVault.Secrets (For creating key vault client)
13. Inject `IConfiguration` into the controllers' constructor and fetch the value as any other configuration.