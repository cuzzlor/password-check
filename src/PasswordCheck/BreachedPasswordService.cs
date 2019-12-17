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
        private static readonly Regex LineRegex = new Regex("\r\n|\r|\n", RegexOptions.Compiled);

        private readonly HttpClient _httpClient;
        private readonly SHA1Managed _sha1Managed = new SHA1Managed();

        public BreachedPasswordService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<int> GetBreachCountAsync(string password)
        {
            var hash = _sha1Managed.ComputeHash(Encoding.UTF8.GetBytes(password));
            var hashString = string.Concat(hash.Select(b => b.ToString("X2")));
            var first5 = hashString.Substring(0, 5);
            var remainder = hashString.Substring(5);

            var response = await _httpClient.GetAsync($"range/{first5}");
            response.EnsureSuccessStatusCode();
            var text = await response.Content.ReadAsStringAsync();

            var lines = LineRegex.Split(text);
            var match = lines.FirstOrDefault(line => line.Split(':')[0].Equals(remainder));

            return match == null ? 0 : Convert.ToInt32(match.Split(':')[1]);
        }

        public void Dispose()
        {
            _sha1Managed.Dispose();
        }
    }
}
