using AlivelyMVC.Models;
using Ardalis.GuardClauses;
using System.Text;

namespace AlivelyMVC.Services
{
    public class UserService
    {
        private HttpClient _httpClient;

        private readonly string _userApiUrl = "https://alivelyuser.azure-api.net/api/User";

        public UserService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<HttpResponseMessage> LoginUserAsync(string username, string password, CancellationToken token = default)
        {
            Guard.Against.NullOrEmpty(username, nameof(username));

            Guard.Against.NullOrEmpty(password, nameof(password));

            var values = new Dictionary<string, string>
            {
                { "username", username },
                { "password", password }
            };

            var content = new FormUrlEncodedContent(values);

            return await _httpClient.PostAsync(_userApiUrl + $"/Login?username={username}&password={password}",content, token).ConfigureAwait(false);
        }

        public async Task<HttpResponseMessage> SignupUserAsync(User user, CancellationToken token = default)
        {
            Guard.Against.Null(user);

            return await _httpClient.PostAsJsonAsync<User>(_userApiUrl, user, token).ConfigureAwait(false);
        }

        public async Task<HttpResponseMessage> GetUser(Guid uuid, CancellationToken token = default)
        {
            if(uuid == Guid.Empty)
            {
                throw new ArgumentException("User uuid is missing. ");
            }

            return await _httpClient.GetAsync(_userApiUrl + $"?uuid={uuid}",token).ConfigureAwait(false);
        }

        public async Task<HttpResponseMessage> UpdateUser(User user, CancellationToken token = default)
        {
            Guard.Against.Null(user);

            return await _httpClient.PutAsJsonAsync<User>(_userApiUrl + $"/{user.Uuid}", user, token).ConfigureAwait(false);
        }

        public async Task<HttpResponseMessage> DeleteUser(Guid uuid, CancellationToken token = default)
        {
            if(uuid == Guid.Empty)
            {
                throw new ArgumentException("User uuid is missing. ");
            }

            return await _httpClient.DeleteAsync(_userApiUrl + $"?uuid={uuid}", token).ConfigureAwait(false);
        }

        public async Task<HttpResponseMessage> ChangePassword(Guid uuid, string password, CancellationToken token = default)
        {
            if (uuid == Guid.Empty)
            {
                throw new ArgumentException("User uuid is missing. ");
            }

            Guard.Against.NullOrWhiteSpace(password, nameof(password), "Password is missing. ");

            var values = new Dictionary<string, string>
            {
                { "newpassword", password }
            };

            return await _httpClient.PostAsJsonAsync(_userApiUrl + $"/ChangePassword/{uuid}?newpassword={password}", values, token);
        }
    }
}
