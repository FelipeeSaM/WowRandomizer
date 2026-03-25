using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildingBlocks.Concerns.Redis
{
    public class RedisConfig
    {
        public string Configuration { get; set; } = string.Empty;
        public string InstanceName { get; set; } = string.Empty;
    }
}
