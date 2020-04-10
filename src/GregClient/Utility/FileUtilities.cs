using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.IO.Compression;


namespace Greg.Utility
{
    public class FileUtilities
    {

        /// <summary>
        /// Signs a hash with the RSA algorithm
        /// </summary>
        /// <param name="input"></param>
        public static void Sign(byte[] input)
        {
            using (var rsa = new RSACryptoServiceProvider(4096) )
            {

                var bytesToDecrypt = Convert.FromBase64String("la0Cz.....D43g=="); // string to decrypt, base64 encoded

                try
                {
                    var ps = new RSAParameters();
                    // ps.Modulus = my private key
                    // ps.Exponent = public key

                    rsa.ImportParameters(ps);

                    byte[] output = rsa.Encrypt(input, false);

                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
        }

        /// <summary>
        /// Compute MD5 checksum of file download
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static byte[] GetMD5Checksum(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    return md5.ComputeHash(stream);
                }
            }
        }

        /// <summary>
        /// Create a SHA-256 Secure Hash from a file
        /// </summary>
        /// <param name="fInfo"></param>
        /// <returns></returns>
        public static byte[] GetFileHash(FileInfo fInfo)
        {
            var mySHA256 = SHA256Managed.Create();

            byte[] hashValue;

            // Create a fileStream for the file.
            var fileStream = fInfo.Open(FileMode.Open);
            // Be sure it's positioned to the beginning of the stream.
            fileStream.Position = 0;
            // Compute the hash of the fileStream.
            hashValue = mySHA256.ComputeHash(fileStream);

            // Close the file.
            fileStream.Close();

            return hashValue;
        }

        /// <summary>
        /// Gets the full path of the user temporary folder
        /// </summary>
        /// <returns>String with the full path of the user temporary folder</returns>
        public static string GetTempFolder()
        {
            return Path.GetDirectoryName(Path.GetTempPath());
        }

        /// <summary>
        /// Generates a full path under the temporary folder with the prefix 'gregPkg'
        /// </summary>
        /// <returns>String with the full path</returns>
        public static string GetTempZipPath()
        {
            string tempZipPath = GetZipPath("gregPkg"); 

            return tempZipPath;
        }

        /// <summary>
        /// Generates a full output path under the temporary folder with the prefix 'gregPkgOutput'
        /// </summary>
        /// <returns>String with the full path</returns>
        public static string GetTempZipOutputPath()
        {
            string tempZipOutputPath = GetZipPath("gregPkgOutput");

            return tempZipOutputPath;
        }

        /// <summary>
        /// Generates a full path under the temporary folder of a zip file with a format like this prefix[GUID].zip
        /// </summary>
        /// <param name="prefixFileName">Prefix of the file</param>
        /// <returns>String with the full path</returns>
        private static string GetZipPath(string prefixFileName)
        {
            var tempFolder = GetTempFolder();
            string zipPath = Path.Combine(tempFolder, String.Format("{0}{1}.zip", prefixFileName, Guid.NewGuid().ToString()));
            
            return zipPath;
        }

        /// <summary>
        /// Make a zip file from a collection of files
        /// </summary>
        /// <param name="filePaths">A list of filepaths</param>
        public static string Zip(IEnumerable<string> filePaths)
        {
            var zipPath = GetTempZipPath();

            using (var zip = ZipFile.Open(zipPath, ZipArchiveMode.Create))
            {
                foreach (var filePath in filePaths)
                {   
                    zip.CreateEntryFromFile(filePath, filePath);
                }
            }

            return zipPath;
        }

        /// <summary>
        /// Make a zip file from a collection of files
        /// </summary>
        /// <param name="directory">A directory - should be the package root path</param>
        public static string Zip(string directory)
        {
            var zipPath = GetTempZipPath();
            ZipFile.CreateFromDirectory(directory, zipPath);
            return zipPath;
        }

        /// <summary>
        /// Given a path to a zip, extracts it and returns the 
        /// temp directory where it is located.
        /// </summary>
        /// <param name="zipFilePath"></param>
        /// <returns></returns>
        public static string UnZip(string zipFilePath)
        {
            var outputPath = GetTempZipOutputPath();
            var count = 0;
            while (Directory.Exists(outputPath + count))
            {
                count++;
            }
            outputPath = outputPath + count;

            UnZip(zipFilePath, outputPath);
            return outputPath;
        }

        /// <summary>
        /// Given a path to a zip and a destination directory.
        /// </summary>
        /// <param name="zipFilePath"></param>
        /// <param name="unzipDirectory"></param>
        /// <returns></returns>
        public static void UnZip(string zipFilePath, string unzipDirectory)
        {
            using (var source = ZipFile.Open(zipFilePath, ZipArchiveMode.Read))

            {
                // Implementation from System.IO.Compression.ZipFileExtensions.ExtractToDirectory
                // with modifications to not fail on malformed zips created by Ionic.Zip
                if (source == null)
                {
                    throw new IOException("Could not open archive at " + zipFilePath);
                }
                if (unzipDirectory == null)
                {
                    throw new ArgumentNullException("unzipDirectory");
                }
                DirectoryInfo directoryInfo = Directory.CreateDirectory(unzipDirectory);
                string fullName = directoryInfo.FullName;
                foreach (ZipArchiveEntry entry in source.Entries)
                {
                    string fullPath = Path.GetFullPath(Path.Combine(fullName, entry.FullName));
                    if (!fullPath.StartsWith(fullName, StringComparison.OrdinalIgnoreCase))
                    {
                        // entry.FullName begins with .. or / and would expand to a path outside 
                        // the extraction directory, so skip it. This is likely caused by the
                        // dummy empty files added by Ionic.Zip to support archive comments.
                        continue;
                    }
                    if (Path.GetFileName(fullPath).Length == 0)
                    {
                        Directory.CreateDirectory(fullPath);
                    }
                    else
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
                        entry.ExtractToFile(fullPath, false);
                    }
                }
            }
        }
    }
}
