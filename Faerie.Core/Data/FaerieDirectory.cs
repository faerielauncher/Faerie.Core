

using Microsoft.Extensions.Logging;

namespace Faerie.Core.Data
{
    internal class FaerieDirectory
    {
        private readonly string path;
        private readonly string directoryName;

        public FaerieDirectory(string path, string directoryName)
        {
            if (path is null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (directoryName is null)
            {
                throw new ArgumentNullException(nameof(directoryName));
            }

            this.path = path;
            this.directoryName = directoryName;
        }

        public void CreateDirectory()
        {
            logger.LogInformation($"Creating directory: {Path.Combine(path, directoryName)}");
            Directory.CreateDirectory(Path.Combine(path, directoryName));
        }

        public string[] GetFiles() => Directory.GetFiles(Path.Combine(path, directoryName));
        public string GetPath() => Path.Combine(path, directoryName);
        public bool DeleteFile()
        {
            return false;
        }

        public bool DeleteFiles()
        {
            return false;
        }

        public bool Exists()
        {
            logger.LogInformation($"Directory exists! {Path.Combine(path, directoryName)}");
            return Directory.Exists(Path.Combine(path, directoryName));
        }
    }
}
