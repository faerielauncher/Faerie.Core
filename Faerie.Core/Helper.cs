using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Faerie.Core
{
    internal class Helper
    {
        private static Random random = new Random(Guid.NewGuid().GetHashCode());
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static string? GetChecksumSHA1(Stream file)
        {
            using (var algorithm = SHA1.Create())
            {
                try
                {
                    StringBuilder sb = new();

                    file.Position = 0;
                    byte[] hashValue = algorithm.ComputeHash(file);

                    foreach (byte b in hashValue)
                    {
                        sb.AppendFormat("{0:X2}", b);
                    }

                    return sb.ToString();
                }
                catch (IOException e)
                {
                    logger.LogWarning($"I/O Exception: {e.Message}");
                }
                catch (UnauthorizedAccessException e)
                {
                    logger.LogWarning($"Access Exception: {e.Message}");
                }
            }
            return null;
        }
        public static bool IsValidFilename(string? testName)
        {
            Regex containsABadCharacter = new Regex("["
                  + Regex.Escape(new string(System.IO.Path.GetInvalidPathChars())) + "]");

            if (testName is null)
            {
                return false;
            }

            if (containsABadCharacter.IsMatch(testName)) { return false; };

            return true;
        }

        public static T? JsonDeserialize<T>(string path)
        {
            using StreamReader stream = new StreamReader(path);

            return JsonSerializer.Deserialize<T>(stream.ReadToEnd());
        }

        public static string GetArchitecture()
        {
            return System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture.ToString().ToLower();
        }

        public static string GetPlatform()
        {
            string platform = String.Empty;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                platform = "windows";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                platform = "linux";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                platform = "mac";
            }
            else
            {
                throw new Exception("Unknown platform!");
            }

            return platform;
        }
    }
}
