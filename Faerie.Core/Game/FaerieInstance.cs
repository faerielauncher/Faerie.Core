using Faerie.Core.Data;

namespace Faerie.Core.Game
{
    internal class FaerieInstance
    {
        public static List<FaerieInstance> Loaded = new();

        private string name { get; set; }
        private string description { get; set; }
        private string iconPath { get; set; }
        private ModLoader modloader { get; set; }
        private IProvider providers { get; set; }
        private FaerieDirectory faerieDirectory { get; set; }
        public FaerieInstance(FaerieDirectory dir)
        {
            this.faerieDirectory = dir;
        }

        public async Task Play()
        {

        }

        public async Task Delete()
        {

        }

        public async Task Edit()
        {

        }

        public async Task Export()
        {

        }

        public async Task Import()
        {

        }
    }
}
