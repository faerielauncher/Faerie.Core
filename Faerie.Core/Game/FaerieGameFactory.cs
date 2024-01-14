using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faerie.Core.Game
{
    internal class FaerieGameFactory
    {
        private readonly FaerieConfig? config;
        private IModloader modloader;

        public FaerieGameFactory()
        {

        }

        public FaerieGameFactory(FaerieConfig config)
        {
            this.config = config;
        }

        public FaerieGameFactory SetModloader(IModloader modloader)
        {
            this.modloader = modloader;
            return this;
        }

        public void Play()
        {

        }
    }
}
