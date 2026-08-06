using DoughBro.src.Models;
using DoughBro.src.Repositories.Interfaces;
using DoughBro.src.Services.Interfaces;

namespace DoughBro.src.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<PlaidAccessTokenModel>?> FetchPlaidAccessTokens(string userId)
        {
            return await _userRepository.FetchPlaidAccessTokens(userId);
        }

        public Task SavePlaidAccessToken(PlaidAccessTokenModel token)
        {
            return _userRepository.SavePlaidAccessToken(token);
        }

        public Task UpdatePlaidCursor(PlaidAccessTokenModel token, string currentCursor)
        {
            return _userRepository.UpdatePlaidCursor(token, currentCursor);
        }
    }
}
