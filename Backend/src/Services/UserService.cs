using DoughBro.src.Models;
using DoughBro.src.Repositories.Interfaces;
using DoughBro.src.Services.Interfaces;

namespace DoughBro.src.Services
{
    public class UserService: IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<string?> FetchPlaidAccessToken(string userId)
        {
            throw new NotImplementedException();
        }

        public Task SavePlaidAccessToken(PlaidAccessTokenModel token)
        {
            return _userRepository.SavePlaidAccessToken(token);
        }
    }
}
