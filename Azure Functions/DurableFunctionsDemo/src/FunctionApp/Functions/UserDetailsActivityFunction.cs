using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using SharedLibs.Contracts;
using SharedLibs.Models;

namespace FunctionApp.Functions
{
    public class UserDetailsActivityFunction
    {
        private readonly ILogger<UserDetailsActivityFunction> _logger;
        private readonly IUserService _userService;
        
        public UserDetailsActivityFunction(
            ILogger<UserDetailsActivityFunction> logger,
            IUserService userService
        )
        {
            _logger = logger;
            _userService = userService;
        }

        [FunctionName("FetchUserDetails")]
        public async Task<User?> FetchUserDetails([ActivityTrigger] int userId)
        {
            _logger.LogInformation($"Fetching details for user with userId: {userId}");

            return await _userService.FetchUserByIdAsync(userId);
        }
    }
}