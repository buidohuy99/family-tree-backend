using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Helpers.ConfigModels
{
    public class CloudinaryAccounts
    {
        public class CloudinaryAccount{
            public string CloudName { get; set; }
            public string APIKey { get; set; }
            public string APISecret { get; set; }
        }

        public List<CloudinaryAccount> Accounts { get; set; }
    }
}
