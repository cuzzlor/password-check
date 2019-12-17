using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace PasswordCheck
{
    public interface IForbiddenPasswordService
    {
        bool IsPasswordForbidden(string password);
    }

    public class ForbiddenPasswordService : IForbiddenPasswordService
    {
        private readonly List<Regex> _forbiddenPasswordRegexs = new List<Regex>();

        public ForbiddenPasswordService(IOptions<ForbiddenPasswordOptions> forbiddenPasswordOptions)
        {
            var options = forbiddenPasswordOptions.Value;

            if (options.ForbiddenPasswords != null)
                _forbiddenPasswordRegexs.AddRange(options.ForbiddenPasswords.Select(BuildRegex));

            if (!string.IsNullOrWhiteSpace(options.ForbiddenPasswordsFile))
                _forbiddenPasswordRegexs.AddRange(LoadFromFile(options.ForbiddenPasswordsFile));
        }

        private static Regex BuildRegex(string passwordPattern)
        {
            return new Regex(passwordPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
        }

        private IEnumerable<Regex> LoadFromFile(string forbiddenPasswordsFile)
        {
            return File
                .ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), forbiddenPasswordsFile))
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Select(BuildRegex);
        }

        public bool IsPasswordForbidden(string password)
        {
            return _forbiddenPasswordRegexs.Any(regex => regex.IsMatch(password));
        }
    }
}
