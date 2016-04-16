using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Greg.Requests;
using Greg.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Greg
{
    /// <summary>
    /// The Whitelist class contains static methods for updating the whitelist,
    /// and determining whether assemblies are located in white-listed packages.
    /// </summary>
    public static class Whitelist
    {

        private static string whitelistPackageDirectory;
        private static IEnumerable<AssemblyName> whiteListedAssemblyNames = new List<AssemblyName>();

        public static IEnumerable<AssemblyName> WhiteListedAssemblyNames
        {
            get { return whiteListedAssemblyNames;}
        } 

        /// <summary>
        /// The directory in which white-listed packages are stored.
        /// This will default to the 'packages' folder in the directory of the executing assembly.
        /// </summary>
        public static string WhitelistPackageDirectory
        {
            get
            {
                if (!string.IsNullOrEmpty(whitelistPackageDirectory)) return whitelistPackageDirectory;

                var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                whitelistPackageDirectory = Path.Combine(directoryName, "packages");

                return whitelistPackageDirectory;
            }
            set { whitelistPackageDirectory = value; }
        }

        /// <summary>
        /// Clears the whitelisted node collection and does
        /// a new fetch of the whitelisted assembly.
        /// </summary>
        public static void UpdateWhitelist(string packageManagerUrl)
        {
            var gregClient = new GregClient(null, packageManagerUrl);

            whiteListedAssemblyNames = null;
            whiteListedAssemblyNames = GetWhitelistedAssemblyNames(gregClient);
        }

        /// <summary>
        /// Is the provided assembly included in a whitelisted packaged?
        /// </summary>
        /// <param name="existingAssemblyName">An <seealso cref="AssemblyName"/> object representing the existing assembly.</param>
        /// <param name="whiteListedAssemblies">A collection of <seealso cref="AssemblyName"/> objects.</param>
        /// <returns>True, if a white-listed package contains this assembly, false if no
        /// white-listed packages contains this assembly, or the 'node_libraries' key is not
        /// found in the package.json.</returns>
        public static bool IsAssemblyInWhitelistedPackage(AssemblyName existingAssemblyName, IEnumerable<AssemblyName> whiteListedAssemblies)
        {
            // Match against the full name of the assembly, including the version, culture, etc.
            return whiteListedAssemblies.Any(n => n.FullName == existingAssemblyName.FullName);
        }

        /// <summary>
        /// Get a unique list of <seealso cref="AssemblyName"/> objects representing
        /// the assemblies that are in all the white listed packages.
        ///
        /// This method uses the 'node_libraries' value in the package.json to
        /// determine whether the provided assembly is included in a white-listed package.
        /// </summary>
        /// <param name="whitelistPackageDirectory">The directory containing the white listed packages.</param>
        /// <returns></returns>
        internal static IEnumerable<AssemblyName> GetWhiteListedAssemblyNames(string whitelistPackageDirectory)
        {
            var assemblyNames = new List<AssemblyName>();

            if (!Directory.Exists(WhitelistPackageDirectory))
            {
                return assemblyNames;
            }

            // Check the node_libraries collection in all package headers in the white list directory.
            // Any node that comes from one of those assemblies should be allowed.
            var di = new DirectoryInfo(whitelistPackageDirectory);

            foreach (var dinfo in di.GetDirectories())
            {
                var pkgFiles = dinfo.GetFiles("*.json");
                if (!pkgFiles.Any()) continue;

                var pkgFile = pkgFiles.First();
                using (var sr = new StreamReader(pkgFile.FullName))
                {
                    var json = sr.ReadToEnd();
                    var pkgData = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                    if (!pkgData.ContainsKey("node_libraries")) continue;

                    var nodeLibsArr = (JArray)pkgData["node_libraries"];
                    if (nodeLibsArr == null || !nodeLibsArr.Any()) continue;

                    var asmNames = nodeLibsArr.Select(nl => new AssemblyName((string)nl));
                    assemblyNames.AddRange(asmNames);
                }
            }

            return assemblyNames.Distinct();
        }

        /// <summary>
        /// Get a unique list of <seealso cref="AssemblyName"/> objects representing
        /// the assemblies that are in all the white listed packages.
        ///
        /// This method requests the white-listed package headers from the package manager.
        /// It then aggregates all of the values stored in the node_libraries fields.
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<AssemblyName> GetWhitelistedAssemblyNames(IGregClient gregClient)
        {
            var assemblyNames = new List<AssemblyName>();

            try
            {
                var nv = WhitelistHeaderCollectionDownload.All();
                var pkgResponse = gregClient.ExecuteAndDeserializeWithContent<List<PackageHeader>>(nv);
                if (pkgResponse == null || pkgResponse.content == null)
                {
                    return assemblyNames;
                }

                // Get all the node_libraries for the latest version of each package
                var nodeLibraries = pkgResponse.content.ToList().
                    Where(content => content.versions.Any() && content.versions.Last().node_libraries != null).
                    SelectMany(content => content.versions.Last().node_libraries).Distinct();

                foreach (var asmName in nodeLibraries)
                {
                    try
                    {
                        assemblyNames.Add(new AssemblyName(asmName));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("The referenced assembly name in the package, {0}, could not be converted to an AssemblyName", asmName);
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to get whitelist assemblies from {0}", gregClient.BaseUrl);
                Console.WriteLine(ex.Message);
            }
            // convert values to AssemblyName
            return assemblyNames;
        }
    }
}
