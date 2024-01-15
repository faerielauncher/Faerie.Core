using Faerie.Core.Data;
using Faerie.Core.DataStore;
using Faerie.Core.Http;
using Faerie.Core.Templates;
using Microsoft.Extensions.Logging;

namespace Faerie.Core.Game.Modloaders
{
    internal partial class Vanilla
    {
        async Task<bool> DownloadLibraries(VersionTemplate? version)
        {
            if (version is null)
            {
                return false;
            }

            // download libraries
            foreach (var item in version.Libraries)
            {
                if (item.Downloads?.Artifact?.Url is null || item.Downloads?.Artifact?.Path is null)
                {
                    logger.LogWarning($"Skipping: {item.Name}");
                    continue;
                }

                var dir = new FaerieDirectory(Path.Combine(FaerieData.PATH, "libraries"), Path.Combine(item.Downloads.Artifact.Path, "../"));
                if (!dir.Exists())
                {
                    dir.CreateDirectory();
                }

                string? fileName = new Uri(item.Downloads.Artifact.Url).Segments.LastOrDefault();
                string filePath = Path.Combine(Path.Combine(FaerieData.PATH, "libraries", item.Downloads.Artifact.Path));

                if (File.Exists(filePath))
                {
                    using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {

                        string? checksum = GetChecksumSHA1(stream);

                        if (checksum is not null && item.Downloads.Artifact.Sha1 is not null)
                        {
                            if (checksum.ToLower().Equals(item.Downloads.Artifact.Sha1.ToLower()))
                            {
                                logger.LogInformation($"{fileName} exists! Skipping download.");
                                continue;
                            }
                            else
                            {
                                logger.LogInformation("Checksum mismatched, downloading.");
                            }
                        }
                    }

                }

                await new FaerieHttpFactory(item.Downloads.Artifact.Url)
                    .CreateRequestDownload(dir, fileName);

            }

            return true;
        }
    }
}
