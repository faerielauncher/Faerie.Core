

using Microsoft.Extensions.Logging;

namespace Faerie.Core.Data
{
    public class FaerieDirectory : IDisposable
    {
        private readonly string path;
        private readonly string directoryName;
        private bool disposedValue;

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
            logger.LogInformation($"Creating directory: {GetPath()}");
            Directory.CreateDirectory(GetPath());
        }

        public string[] GetFiles() => Directory.GetFiles(GetPath());
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
            bool exists = Directory.Exists(GetPath());

            if (exists)
            {
                logger.LogInformation($"Directory exists! {GetPath()}");
            }

            return exists;
        }

        public bool IsEmpty()
        {
            try
            {
                bool empty = !Directory.EnumerateFileSystemEntries(GetPath()).Any();
                if (empty)
                {
                    logger.LogInformation($"Directory is empty! {GetPath()}");
                }

                return empty;
            }catch (Exception ex)
            {
                logger.LogWarning(ex.Message);
                return false;
            }

        }

        public void Delete()
        {
            logger.LogInformation($"Deleting! {GetPath()}");
            Directory.Delete(GetPath()); 
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~FaerieDirectory()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
