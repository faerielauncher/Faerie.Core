using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faerie.Core.Game.Modloaders
{
    /// <summary>
    /// https://meta.fabricmc.net/v2/versions/game
    /// https://meta.fabricmc.net/v2/versions/loader
    /// </summary>
    internal class Fabric : ModLoader
    {
        public override FaerieArgumentFactory Arguments()
        {
            throw new NotImplementedException();
        }

        public override Task<bool> Download()
        {
            throw new NotImplementedException();
        }
    }
}
