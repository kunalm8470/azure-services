using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using SharedLibs.Contracts;
using SharedLibs.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunctionApp.Functions
{
    public class UserPostActivityFunction
    {
        private readonly ILogger<UserPostActivityFunction> _logger;
        private readonly IUserPostService _userPostService;

        public UserPostActivityFunction(
            ILogger<UserPostActivityFunction> logger,
            IUserPostService userPostService
        )
        {
            _logger = logger;
            _userPostService = userPostService;
        }

        [FunctionName("FetchUserPostDetails")]
        public async Task<IEnumerable<Post>> FetchUserPostDetails([ActivityTrigger] int userId)
        {
            _logger.LogInformation($"Fetching posts for user with userId: {userId}");
            return await _userPostService.FetchPostsByUserIdAsync(userId);
        }
    }
}
