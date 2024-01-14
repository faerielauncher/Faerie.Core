using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faerie.Core.Game
{
    internal interface IModloader
    {
        string Endpoint();
        string MinecraftVersion();
        string LoaderVersion();
    }
}
