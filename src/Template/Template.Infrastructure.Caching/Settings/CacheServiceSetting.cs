using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Infrastructure.Caching.Settings
{
    public class CacheServiceSettingOptions
    {
        public const string Key = "CacheServiceSettings";

        public bool Enabled { get; set; }
        public int? ExpirationScanFrequencyInMinutes { get; set; }
        public long? SizeLimit { get; set; }

    }
}
