using System;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PasswordCheck
{
    public interface IBreachedPasswordService
    {
        Task<int> GetBreachCountAsync(string password);
    }

    public class BreachedPasswordService : IBreachedPasswordService, IDisposable
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly SHA1Managed _sha1Managed = new SHA1Managed();

        public BreachedPasswordService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<int> GetBreachCountAsync(string password)
        {
            var hash = _sha1Managed.ComputeHash(Encoding.UTF8.GetBytes(password));
            var hashString = string.Concat(hash.Select(b => b.ToString("X2")));
            var first5 = hashString.Substring(0, 5);
            var remainder = hashString.Substring(5);

            using var client = _httpClientFactory.CreateClient("hibp-range");
            var response = await client.GetAsync($"range/{first5}");
            response.EnsureSuccessStatusCode();
            var text = await response.Content.ReadAsStringAsync();

            var lines = Regex.Split(text, "\r\n|\r|\n");
            var match = lines.FirstOrDefault(line => line.Split(':')[0].Equals(remainder));

            return match == null ? 0 : Convert.ToInt32(match.Split(':')[1]);
        }

        public void Dispose()
        {
            _sha1Managed.Dispose();
        }
    }
}
