using SharedLibs.Models;

namespace SharedLibs.Contracts
{
    public interface IUserService
    {
        public Task<User?> FetchUserByIdAsync(int id, CancellationToken cancellationToken = default);
    }
}
