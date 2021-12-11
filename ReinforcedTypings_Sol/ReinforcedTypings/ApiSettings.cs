using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReinforcedTypings
{
    public class ApiSettings
    {
        public ApiSettings()
        {
        }

        //public eEnvironment Environment { get; set; }
        public string ApiId { get; set; }
        public string ClientId { get; set; }
        public string TenantId { get; set; }
        public string GoogleAnalyticsKey { get; set; }
    }
}
