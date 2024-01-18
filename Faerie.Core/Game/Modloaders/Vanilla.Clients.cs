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
        async Task<bool> DownloadClient(VersionTemplate? version)
        {
            if(version is null)
            {
                return false;
            }

            var versionsDir = new FaerieDirectory(FaerieData.PATH, "versions");
            if(MinecraftVersion is null)
            {
                return false;
            }

            var minecraftDir = new FaerieDirectory(versionsDir.GetPath(), MinecraftVersion);
            if (!minecraftDir.Exists())
            {
                minecraftDir.CreateDirectory();
            }

            if(version.Downloads.Client is null)
            {
                return false;
            }

            var clientUrl = version.Downloads.Client.Url;

            if (clientUrl is null)
            {
                return false;
            }

            string clientPath = Path.Join(minecraftDir.GetPath(), $"{MinecraftVersion}.jar");
            jarPath.Add( clientPath );

            if (File.Exists(clientPath))
            {
                using (FileStream stream = new FileStream(clientPath, FileMode.Open, FileAccess.Read))
                {

                    string? checksum = GetChecksumSha1(stream);

                    if (checksum is not null && version.Downloads.Client.Sha1 is not null)
                    {
                        if (checksum.ToLower().Equals(version.Downloads.Client.Sha1.ToLower()))
                        {
                            logger.LogInformation($"{MinecraftVersion}.jar exists! Skipping download.");
                            return true;
                        }
                        else
                        {
                            logger.LogInformation("Checksum mismatched, downloading.");
                        }
                    }
                }
            }

            var client = await new FaerieHttpFactory(clientUrl)
                .CreateRequestDownload(minecraftDir, $"{MinecraftVersion}.jar");

            return client.Item1;
        }
    }
}
