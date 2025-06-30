using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager;

namespace PlayerZero.Editor
{
    /// <summary>
    ///     Class <c>ModuleList</c> is a static class that can be referenced to get the latest module version.
    /// </summary>
    public static class PackageList
    {

        /// <summary>
        ///     Unity Module that adds support for gltf files that use DracoCompression.
        /// </summary>
        public static GitPackageInfo DracoCompression = new GitPackageInfo
        {
            name = "com.unity.cloud.draco",
            gitUrl = "",
            branch = "",
            version = "5.0.0"
        };
        
        
        /// <summary>
        ///     Unity Module that adds support for gltf files that use DracoCompression.
        /// </summary>
        public static GitPackageInfo MeshOptCompression = new GitPackageInfo
        {
            name = "com.unity.meshopt.decompress",
            gitUrl = "",
            branch = "",
            version = "0.1.0"
        };
        
        

        /// <summary>
        ///     Get installed modules from Modules list.
        /// </summary>
        /// <returns>A <see cref="Dictionary"/> of installed Unity Module information in the format of <c>Dictionary&lt;string: name, string: version&gt;</c>. </returns>
        public static Dictionary<string, string> GetInstalledModuleVersionDictionary()
        {
            PackageInfo[] packageList = GitPackageInstaller.GetPackageList();

            var installedModules = new Dictionary<string, string>();
   
            if (packageList.Any(x => x.name == DracoCompression.name))
            {
                installedModules.Add(DracoCompression.name, DracoCompression.version);
            }

            return installedModules;
        }
    }
}
