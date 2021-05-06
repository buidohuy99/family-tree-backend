using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Helpers.ConfigModels
{
    public class ImageKitAccounts
    {
        public class ImageKitAccount{
            public string URLEndpoint { get; set; }
            public string PublicKey { get; set; }
            public string PrivateKey { get; set; }
        }

        public List<ImageKitAccount> Accounts { get; set; }
    }
}
