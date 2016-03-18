using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformDev.ssh
{
    public class SshConnectionInfo
    {
        public SshConnectionInfo(string host, string user, string pass, string identityFile)
        {
            Host = host;
            User = user;
            Pass = pass;
            IdentityFile = identityFile;
        }
        public string Host;
        public string User;
        public string Pass;
        public string IdentityFile;
    }
}
