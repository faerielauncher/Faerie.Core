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
        private ModLoader modloader;

        public FaerieGameFactory()
        {

        }

        public FaerieGameFactory(FaerieConfig config, ModLoader modloader)
        {
            this.config = config;
            this.modloader = modloader;
        }

        public FaerieGameFactory SetModloader(ModLoader modloader)
        {
            this.modloader = modloader;
            return this;
        }

        public async Task Play()
        {
            await modloader.Download();
            await modloader.ConfigureJava();

            var args = modloader.Arguments();
            Console.WriteLine(args.Build());

        }
    }
}
