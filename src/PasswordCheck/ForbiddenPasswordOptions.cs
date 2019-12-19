using System.Collections.Generic;

namespace PasswordCheck
{
    public class ForbiddenPasswordOptions
    {
        public List<string> ForbiddenPasswords { get; set; }
        public string ForbiddenPasswordsFile { get; set; }
    }
}
