using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Helpers.ConfigModels
{
    public class JWT
    {
        public string AccessTokenKey { get; set; }
        public string RefreshTokenKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public double AccessTokenDurationInMinutes { get; set; }
        public double RefreshTokenDurationInMinutes { get; set; }
    }
}
