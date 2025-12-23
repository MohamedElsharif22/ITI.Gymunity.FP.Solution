using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.Configuration
{
    public class PayPalSettings
    {
        public string Mode { get; set; } = "Sandbox"; // "Sandbox" or "Live"
        public string ClientId { get; set; } = null!;
        public string ClientSecret { get; set; } = null!;
        public string ReturnUrl { get; set; } = null!;
        public string CancelUrl { get; set; } = null!;
    }
}
