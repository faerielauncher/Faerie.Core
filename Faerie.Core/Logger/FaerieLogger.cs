using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faerie.Core.Logger
{
    internal class FaerieLogger
    {
        public static ILogger logger = LoggerFactory.Create(config =>
        {
            config.ClearProviders();
            config.AddSimpleConsole();
            config.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Debug);
        }).CreateLogger("Faerie.Core");
    }
}
