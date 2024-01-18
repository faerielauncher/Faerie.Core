using Faerie.Core.Data;
using Faerie.Core.DataStore;
using Faerie.Core.Http;
using Faerie.Core.Templates;
using Microsoft.Extensions.Logging;
using System.IO.Compression;

namespace Faerie.Core.Game.Modloaders
{
    internal partial class Vanilla
    {
        List<string> jarPath = new List<string>();

        async Task<bool> DownloadLibraries(VersionTemplate? version)
        {
            if (version is null)
            {
                return false;
            }

            // download libraries
            foreach (var item in version.Libraries)
            {
                var artifact = item.Downloads?.Artifact;

                if (artifact?.Sha1 is not null && artifact?.Url is not null && artifact?.Path is not null)
                {

                    List<string> allowedPlatforms = new List<string>();

                    if (item.Rules is not null)
                    {
                        foreach (var rule in item.Rules)
                        {
                            var action = rule.Action;
                            if (action is not null)
                            {
                                if (action.Equals("allow"))
                                {

                                    if (rule.Os is not null)
                                    {
                                        allowedPlatforms.Add(rule.Os["name"]);
                                    }
                                    else
                                    {
                                        allowedPlatforms.Add("windows");
                                        allowedPlatforms.Add("linux");
                                        allowedPlatforms.Add("macos");
                                    }
                                }
                                if (action.Equals("disallow"))
                                {
                                    if (rule.Os is not null)
                                    {
                                        allowedPlatforms.Remove(rule.Os["name"]);
                                    }
                                    else
                                    {
                                        allowedPlatforms.Remove("windows");
                                        allowedPlatforms.Remove("linux");
                                        allowedPlatforms.Remove("macos");
                                    }
                                }
                            }

                        }
                        if (!allowedPlatforms.Contains(GetPlatform().Replace("mac", "osx")))
                        {
                            logger.LogWarning($"Skipping ({string.Join(",", allowedPlatforms.ToArray())}): {item.Name}");
                            continue;
                        }
                    }

                    var dir = new FaerieDirectory(Path.Combine(FaerieData.PATH, "libraries"), Path.Combine(artifact.Path, "../"));
                    if (!dir.Exists())
                    {
                        dir.CreateDirectory();
                    }

                    string? fileName = new Uri(artifact.Url).Segments.LastOrDefault();
                    string filePath = Path.Combine(Path.Combine(FaerieData.PATH, "libraries", artifact.Path));
                    bool skip = false;

                    if (fileName is null)
                    {
                        logger.LogWarning($"Couldn't fetch the file name, skipping {item.Name}");
                        continue;
                    }

                    jarPath.Add(filePath);

                    if (File.Exists(filePath))
                    {
                        using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                        {

                            string? checksum = GetChecksumSha1(stream);

                            if (checksum is not null && artifact.Sha1 is not null)
                            {
                                if (checksum.ToLower().Equals(artifact.Sha1.ToLower()))
                                {
                                    logger.LogInformation($"{fileName} exists! Skipping download.");
                                    skip = true;
                                }
                                else
                                {
                                    logger.LogInformation("Checksum mismatched, downloading.");
                                }
                            }
                        }

                    }

                    if (!skip)
                    {
                        await new FaerieHttpFactory(artifact.Url)
                            .CreateRequestDownload(dir, fileName);
                    }
                }

                // whoever thought this was a good idea lol
                var classifier = item?.Downloads?.Classifiers;
                if (classifier is not null && MinecraftVersion is not null)
                {
                    var nativesFolder = new FaerieDirectory(Path.Combine(FaerieData.PATH, "natives"), MinecraftVersion);

                    if (!nativesFolder.Exists())
                    {
                        nativesFolder.CreateDirectory();
                    }

                    string natives = $"natives-{GetPlatform().Replace("mac", "osx")}";
                    logger.LogInformation("Found natives in api, trying to download them.");
                    if (classifier.ContainsKey(natives))
                    {
                        var native = classifier[natives];
                        if(native.Url is null || native.Path is null)
                        {
                            logger.LogWarning($"Couldn't fetch the file name, skipping {item?.Name} as in natives.");
                            continue;
                        }

                        var dir = new FaerieDirectory(Path.Combine(FaerieData.PATH, "libraries"), Path.Combine(native.Path, "../"));
                        if (!dir.Exists())
                        {
                            dir.CreateDirectory();
                        }

                        var fileName = new Uri(native.Url).Segments.LastOrDefault();
                        var filePath = Path.Combine(Path.Combine(FaerieData.PATH, "libraries", native.Path));

                        Console.WriteLine(filePath);

                        if (fileName is null)
                        {
                            logger.LogWarning($"Couldn't fetch the file name, skipping {item?.Name} as in natives.");
                            continue;
                        }

                        jarPath.Add(filePath);
                        bool skip = false;

                        if (File.Exists(filePath))
                        {
                            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                            {

                                string? checksum = GetChecksumSha1(stream);

                                if (checksum is not null && native.Sha1 is not null)
                                {
                                    if (checksum.ToLower().Equals(native.Sha1.ToLower()))
                                    {
                                        logger.LogInformation($"{fileName} exists! Skipping download.");
                                        skip = true;
                                    }
                                    else
                                    {
                                        logger.LogInformation("Checksum mismatched, downloading.");
                                    }
                                }
                            }
                        }

                        if (!skip)
                        {
                            await new FaerieHttpFactory(native.Url)
                                .CreateRequestDownload(dir, fileName);

                        }

                        using (FileStream zipToOpen = new FileStream(filePath, FileMode.Open))
                        {
                            using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                            {
                                foreach (var entry in archive.Entries)
                                {
                                    if (entry.FullName.EndsWith("/"))
                                    {
                                        Directory.CreateDirectory(Path.Combine(nativesFolder.GetPath(), entry.FullName));
                                    }
                                    else
                                    {
                                        entry.ExtractToFile(Path.Join(nativesFolder.GetPath(), entry.FullName), true);
                                    }
                                }
                            }
                        }

                    }
                }

            }

            return true;
        }
    }
}
