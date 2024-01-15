using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faerie.Core.Game
{
    /// <summary>
    /// https://wiki.vg/Launching_the_game#JVM_Arguments
    /// </summary>
    public class FaerieArgumentFactory
    {
        //private readonly List<FaerieArgument> availableArguments = new();
        private readonly List<FaerieArgument> arguments = new();

        public FaerieArgumentFactory AddArgument(FaerieArgument arg)
        {
            logger.LogInformation($"Available argument: {arg.GetArgument()}");
            arguments.Add(arg);
            return this;
        }

        public FaerieArgumentFactory SetArgument(FaerieArgument arg)
        {
            arguments.Add(arg);
            return this;
        }

        public string GetArgumentList()
        {
            return string.Join(" ", arguments.Select(arg => arg.GetArgument()).ToArray());
        }

        public string Build()
        {
            return string.Join(" ", arguments.Select(arg => arg.GetArgument()).ToArray());
        }

        public void RemoteKey(string key)
        {
            arguments.Remove(arguments.First(x => x.GetKey() == key));
        }

        public void GetKeyAndSetValue(string key, string? value)
        {
            try
            {
                if (!string.IsNullOrEmpty(value))
                {
                    var arg = arguments
                        .First(x => x.GetKey() == key);

                    arg.SetValue(value);
                }
            }
            catch (Exception)
            {
                logger.LogWarning($"Argument {key} does not exist!");
            }

        }
        public void GetKeyAndSetValueAndSplitter(string key, string? splitter, string? value)
        {
            try
            {
                if (!string.IsNullOrEmpty(splitter) && !string.IsNullOrEmpty(value))
                {
                    var arg = arguments
                        .First(x => x.GetKey() == key);

                    arg
                        .SetSplitter(splitter)
                        .SetValue(value);
                }
            }
            catch (Exception)
            {
                logger.LogWarning($"Argument {key} does not exist!");
            }

        }
    }
}
