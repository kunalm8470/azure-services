**Steps to integrate Azure Key Vault in .NET 6 application**

1. Create Key vault resource with the following details -
	- Subscription: Choose your subscription
	- Resource group: Create/use existing
	- Key vault name: Provide unique name, this will also act as DNS name
	- Region: East US
	- Pricing Tier / SKU size: Standard
	- Soft-Delete: (Default) Enabled
	- Retention period of deletion: 90 days default
	- Purge protection: (Hard/Soft delete)
	
2. Create .NET 6 API project

3. Add the following nuget packages into that project
	- [Azure.Extensions.AspNetCore.Configuration.Secrets](https://www.nuget.org/packages/Azure.Extensions.AspNetCore.Configuration.Secrets)
	- [Azure.Identity](https://www.nuget.org/packages/Azure.Identity/)
	- [Azure.Security.KeyVault.Secrets](https://www.nuget.org/packages/Azure.Security.KeyVault.Secrets/)

4. Add the extension method in `Program.cs` -
```
    .ConfigureAppConfiguration((context, config) =>
    {
        string vaultUri = Environment.GetEnvironmentVariable("KEYVAULT_ENDPOINT");

        SecretClient client = new(new Uri(vaultUri), new DefaultAzureCredential());
        config.AddAzureKeyVault(client, new KeyVaultSecretManager());
    });
```
				
5. Make a app service and obtain publisher profile and deploy the API project.

6. Toggle Status to on `Settings > Identity > System Assigned` (Generate the Service principal)

7. Obtain the Vault URI from the Overview page of Key vault

8. Add the Vault URI into App service configuration, `Settings > Configuration > Application Settings > New application settings` (Add key KEYVAULT_ENDPOINT)

9. Link the Service principal generated in step 6 to Azure key vault 
`Settings > Access Policy > Add access policy`
search using the Service principal Guid
Select the secret permissions to **Get** and **List** following the [Principle of Least Privilege](https://en.wikipedia.org/wiki/Principle_of_least_privilege) and save it.

10. Generate a secret in this format in the Generate Secret page -`parentkey--childkey1--childkey2`

11. Add a controller

    ```
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ValuesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult GetSecretValue()
        {
            return Ok(_configuration.GetValue<string>("mysecret:supersecret"));
        }
    }
    ```