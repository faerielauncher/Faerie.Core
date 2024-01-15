using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faerie.Core.Game
{
    public class FaerieArgument
    {
        private string key;
        private string splitter;
        private string value;

        public FaerieArgument(string key = "", string splitter = " ", string value = "")
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
        public FaerieArgument SetKey(string key)
        {
            if(key is not null)
            {
                this.key = key;
            }
            return this;
        }
        public FaerieArgument SetSplitter(string splitter)
        {
            if(splitter is not null)
            {
                this.splitter = splitter;
            }
            return this;
        }
        public FaerieArgument SetValue(string value)
        {
            if (value is not null)
            {
                this.value = value;
            }
            return this;
        }
    }
}
