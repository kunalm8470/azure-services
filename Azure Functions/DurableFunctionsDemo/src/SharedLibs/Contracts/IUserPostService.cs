using SharedLibs.Models;

namespace SharedLibs.Contracts
{
    public interface IUserPostService
    {
        public Task<IEnumerable<Post>> FetchPostsByUserIdAsync(int userId, CancellationToken cancellationToken = default);
    }
}
