using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using System.Collections.Generic;
using System.Threading.Tasks;
using SharedLibs.Models;
using SharedLibs.Contracts;
using System.Linq;

namespace FunctionApp.Functions
{
    public class UserPostCommentActivityFunction
    {
        private readonly ILogger<UserPostCommentActivityFunction> _logger;
        private readonly IUserPostCommentService _userPostCommentService;

        public UserPostCommentActivityFunction(
            ILogger<UserPostCommentActivityFunction> logger,
            IUserPostCommentService userPostCommentService
        )
        {
            _logger = logger;
            _userPostCommentService = userPostCommentService;
        }

        [FunctionName("FetchUserPostCommentDetails")]
        public async Task<IEnumerable<Comment>> FetchUserPostCommentDetails([ActivityTrigger] IEnumerable<Comment> req)
        {
            _logger.LogInformation($"Fetching comment on posts for user");

            IEnumerable<int> postIds = req.Select(x => x.PostId);

            return await _userPostCommentService.FetchCommentsByUserPostId(postIds);
        }
    }
}
