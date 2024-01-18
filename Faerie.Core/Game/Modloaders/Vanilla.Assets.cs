using Faerie.Core.Data;
using Faerie.Core.DataStore;
using Faerie.Core.Http;
using Faerie.Core.Templates;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faerie.Core.Game.Modloaders
{
    internal partial class Vanilla
    {
        /// <summary>
        /// https://resources.download.minecraft.net/b6/b62ca8ec10d07e6bf5ac8dae0c8c1d2e6a1e3356
        /// https://resources.download.minecraft.net/<first 2 characters>/<whole hash>
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        async Task<bool> DownloadAssets(VersionTemplate? version)
        {
            if (version is null)
            {
                return false;
            }
            var assetDir = new FaerieDirectory(FaerieData.PATH, "assets");
            var indexesDir = new FaerieDirectory(assetDir.GetPath(), "indexes");
            var objectsDir = new FaerieDirectory(assetDir.GetPath(), "objects");
            var indexJson = Path.Combine(indexesDir.GetPath(), $"{version.AssetIndex.Id}.json");

            if (!indexesDir.Exists())
            {
                indexesDir.CreateDirectory();
            }
            if (!objectsDir.Exists())
            {
                objectsDir.CreateDirectory();
            }

            if (!File.Exists(indexJson))
            {
                await new FaerieHttpFactory(version.AssetIndex.Url)
                    .CreateRequestDownload(indexesDir);
            }

            var assets = JsonDeserialize<ObjectsTemplate>(indexJson);

            if (assets?.Objects is null)
            {
                return false;
            }

            foreach (var asset in assets.Objects)
            {
                var hash = asset.Value.Hash;

                if (hash is null)
                {
                    logger.LogWarning($"Couldn't download {hash}");
                    continue;
                }

                var dir = new FaerieDirectory(objectsDir.GetPath(), hash.Substring(0,2));
                if (!dir.Exists())
                {
                    dir.CreateDirectory();
                }

                string filePath = Path.Join(dir.GetPath(), hash);

                if (File.Exists(filePath))
                {
                    using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {

                        string? checksum = GetChecksumSha1(stream);

                        if (checksum is not null && hash is not null)
                        {
                            if (checksum.ToLower().Equals(hash.ToLower()))
                            {
                                logger.LogInformation($"{hash} exists! Skipping download.");
                                continue;
                            }
                            else
                            {
                                logger.LogInformation("Checksum mismatched, downloading.");
                            }
                        }
                    }
                }


                if (hash is null)
                {
                    logger.LogWarning($"Couldn't download {hash}");
                    continue;
                }

                await new FaerieHttpFactory($"https://resources.download.minecraft.net/{hash.Substring(0, 2)}/{hash}")
                    .CreateRequestDownload(dir);
            }

            return true;

        }
    }
}
