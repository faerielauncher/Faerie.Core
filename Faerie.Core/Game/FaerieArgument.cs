using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faerie.Core.Game
{
    internal class FaerieArgument
    {
        private readonly string key;
        private readonly string splitter = " ";
        private readonly string value;

        public FaerieArgument(string key, string value)
        {
            this.key = key;
            this.value = value;
        }

        public FaerieArgument(string key, string splitter, string value)
        {
            this.key = key;
            this.splitter = splitter;
            this.value = value;
        }

        public string GetArgument()
        {
            if (value.Any(Char.IsWhiteSpace))
            {
                return $"\"{key}{splitter}{value}\"";
            }
            return $"{key}{splitter}{value}";
        }

        public string GetKey()
        {
            return key;
        }
        public string GetSplitter()
        {
            return splitter;
        }
        public string GetValue()
        {
            return value;
        }
    }
}
