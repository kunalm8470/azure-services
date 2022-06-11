using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FunctionApp.Models.Requests;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using SharedLibs.Models;

namespace FunctionApp.Functions
{
    public class OrchestratorFunction
    {
        private readonly ILogger<OrchestratorFunction> _logger;
        public OrchestratorFunction(ILogger<OrchestratorFunction> logger)
        {
            _logger = logger;
        }

        [FunctionName("OrchestratorFunction")]
        public async Task<User?> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context
        )
        {
            OrchestratorFunctionRequest input = context.GetInput<OrchestratorFunctionRequest>();
            int userId = input.UserId;

            // Fetch user details
            User found = await context.CallActivityAsync<User>("FetchUserDetails", userId);

            if (found == default)
            {
                return default;
            }

            // Fetch user post details
            IEnumerable<Post> userPosts = await context.CallActivityAsync<IEnumerable<Post>>("FetchUserPostDetails", userId);

            if (!userPosts.Any())
            {
                found.Posts = new List<UserPost>().AsReadOnly();
                return found;
            }

            IEnumerable<Comment> commentRequests = userPosts.Select(post => new Comment
            {
                PostId = post.Id
            });

            // Fetch users' posts' comments details
            IEnumerable<Comment> comments = await context.CallActivityAsync<IEnumerable<Comment>>("FetchUserPostCommentDetails", commentRequests);

            // Map the required data
            List<UserPost> userPostsList = new();
            foreach (Post post in userPosts)
            {
                UserPost userPost = new();
                userPost.Id = post.Id;
                userPost.Title = post.Title;
                userPost.Body = post.Body;
                List<UserComment> userComments = new(comments.Count());

                foreach (Comment comment in comments)
                {
                    userComments.Add(new UserComment
                    {
                        Id = comment.Id,
                        Body = comment.Body,
                        Name = comment.Name,
                        Email = comment.Email
                    });
                }

                userPost.Comments = userComments.AsReadOnly();
                userPostsList.Add(userPost);
            }

            found.Posts = userPostsList.AsReadOnly();
            return found;
        }
    }
}
