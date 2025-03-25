using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace PlayerZero.Editor
{
    public class LinkXmlAutoGenerator : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;

        private const string FolderPath = "Assets/PlayerZero";
        
        private const string LinkXmlFile = "link.xml";

        public void OnPreprocessBuild(BuildReport report)
        {
            var targetGroup = BuildPipeline.GetBuildTargetGroup(report.summary.platform);
            
            if (!PlayerSettings.stripEngineCode && PlayerSettings.GetManagedStrippingLevel(targetGroup) == ManagedStrippingLevel.Disabled)
            {
                UnityEngine.Debug.LogWarning(
                    "[LinkXmlAutoGenerator] Managed Stripping Level is set to Disabled. link.xml is not required.");
                return;
            }

            if (!Directory.Exists(FolderPath))
            {
                UnityEngine.Debug.Log($"Creating folder: {FolderPath}");
                Directory.CreateDirectory(FolderPath);
            }

            var linkXmlPath = Path.Combine(FolderPath, LinkXmlFile);
            
            if (File.Exists(linkXmlPath))
            {
                UnityEngine.Debug.Log($"[LinkXmlAutoGenerator] link.xml already exists at: {linkXmlPath}. Be sure to preserve PlayerZero.Api* types.");
                return;
            }

            string xmlContent = @"<linker>
    <assembly fullname=""PlayerZero.Runtime"">
        <type fullname=""PlayerZero.Api*"" preserve=""all""/>
    </assembly>
</linker>";

            File.WriteAllText(linkXmlPath, xmlContent);
            UnityEngine.Debug.Log($"[LinkXmlAutoGenerator] Created link.xml to preserve PlayerZero.Api* types:\n{linkXmlPath}");
            AssetDatabase.Refresh();
        }
    }
}