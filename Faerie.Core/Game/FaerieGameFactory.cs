using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

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

            
            var args = modloader.Arguments()
                .SetMainClass("net.minecraft.client.main.Main")
                .BuildNoEmpty();

            var java = modloader.GetJavaExecutable();

            using(StreamWriter sw = new StreamWriter(Path.Combine(Environment.CurrentDirectory, "args.txt")))
            {
                sw.Write(args);
            }

            if (java is not null)
            {
                Process.Start(java, args);
            } else
            {
                logger.LogWarning("Couldn't find Java, exitting.");
            }

        }
    }
}
