using System;
using System.Collections.Generic;
using System.Text;

namespace VincereSharp
{
    public class VincereConfig
    {
        public string ClientId { get; set; }
        public string ApiKey { get; set; }
        public string RedirectUrl { get; set; }
        public string DomainId { get; set; }
        public bool UseTest { get; set; }
    }
}
