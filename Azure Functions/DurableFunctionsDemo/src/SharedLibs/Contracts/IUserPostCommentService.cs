using SharedLibs.Models;

namespace SharedLibs.Contracts
{
    public interface IUserPostCommentService
    {
        public Task<IEnumerable<Comment>> FetchCommentsByUserPostId(IEnumerable<int> postIds, CancellationToken cancellationToken = default);
    }
}
