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
    internal class FaerieArgumentFactory
    {
        private List<FaerieArgument> arguments = new();

        public FaerieArgumentFactory AddArgument(FaerieArgument arg)
        {
            arguments.Add(arg);
            return this;
        }

        public string Build()
        {
            return string.Join(", ", arguments.Select(arg => arg.GetArgument()).ToArray());
        }
    }
}
