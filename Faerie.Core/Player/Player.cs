using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faerie.Core.Player
{
    internal class Player
    {
        private string? accessToken { get; set; }
        public Player(string accessToken)
        {
            this.accessToken = accessToken;
        }

        public string getUUID()
        {
            return String.Empty;
        }

        public string getSkinURL()
        {
            return String.Empty;
        }

        public string getSkinFaceURL()
        {
            return String.Empty;
        }

        public string getSkinFaceANSI()
        {
            return String.Empty;
        }
    }
}
