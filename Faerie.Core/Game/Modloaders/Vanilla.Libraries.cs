using Faerie.Core.Data;
using Faerie.Core.DataStore;
using Faerie.Core.Http;
using Faerie.Core.Templates;
using Microsoft.Extensions.Logging;

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
                if (item.Downloads?.Artifact?.Url is null || item.Downloads?.Artifact?.Path is null)
                {
                    logger.LogWarning($"Skipping: {item.Name}");
                    continue;
                }

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

                var dir = new FaerieDirectory(Path.Combine(FaerieData.PATH, "libraries"), Path.Combine(item.Downloads.Artifact.Path, "../"));
                if (!dir.Exists())
                {
                    dir.CreateDirectory();
                }

                string? fileName = new Uri(item.Downloads.Artifact.Url).Segments.LastOrDefault();
                string filePath = Path.Combine(Path.Combine(FaerieData.PATH, "libraries", item.Downloads.Artifact.Path));
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

                        string? checksum = GetChecksumSHA1(stream);

                        if (checksum is not null && item.Downloads.Artifact.Sha1 is not null)
                        {
                            if (checksum.ToLower().Equals(item.Downloads.Artifact.Sha1.ToLower()))
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
                    await new FaerieHttpFactory(item.Downloads.Artifact.Url)
                        .CreateRequestDownload(dir, fileName);
                }


                // whoever thought this was a good idea lol
                if (item.Downloads.Classifiers is not null)
                {
                    string natives = $"natives-{GetPlatform().Replace("mac", "osx")}";
                    logger.LogInformation("Found natives in api, trying to download them.");
                    if (item.Downloads.Classifiers.ContainsKey(natives))
                    {
                        var native = item.Downloads.Classifiers[natives];
                        if(native.Url is null || native.Path is null)
                        {
                            logger.LogWarning($"Couldn't fetch the file name, skipping {item.Name} as in natives.");
                            continue;
                        }

                        fileName = new Uri(native.Url).Segments.LastOrDefault();
                        filePath = Path.Combine(Path.Combine(FaerieData.PATH, "libraries", native.Path));

                        if (fileName is null)
                        {
                            logger.LogWarning($"Couldn't fetch the file name, skipping {item.Name} as in natives.");
                            continue;
                        }

                        jarPath.Add(filePath);

                        if (File.Exists(filePath))
                        {
                            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                            {

                                string? checksum = GetChecksumSHA1(stream);

                                if (checksum is not null && native.Sha1 is not null)
                                {
                                    if (checksum.ToLower().Equals(native.Sha1.ToLower()))
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

                        await new FaerieHttpFactory(native.Url)
                            .CreateRequestDownload(dir, fileName);
                    }
                }

            }

            return true;
        }
    }
}
