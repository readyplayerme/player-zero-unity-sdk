using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerZero
{
    public enum AvatarQuality
    {
        Low,
        Medium,
        High,
    }
    
    public enum CameraType
    {
        Portrait,
        Fullbody,
        Fit,
    }

    public enum ExpressionType
    {
        None,
        Scared,
        Happy,
        Lol,
        Rage,
        Sad,
    }
    
    public enum RenderSizeLimitType
    {
        [InspectorName("64px")]
        Size64 = 64,
        [InspectorName("128px")]
        Size128 = 128,
        [InspectorName("256px")]
        Size256 = 256,
        [InspectorName("512px")]
        Size512 = 512,
        [InspectorName("1024px")]
        Size1024 = 1024
    }
    
    public enum SceneType {
        Fullbody,
        Portrait,
        Custom,
    }
    
    public enum PoseType
    {
        Default,
        Dancing,
        Athlete,
        Defending,
        Fighting,
        Shrugging,
        Standing,
        Standing2,
        Standing3,
        Sitting
    }

    [Serializable, CreateAssetMenu(fileName = "Avatar Image Config", menuName = "Player Zero/Avatar Image Config", order = 2)]
    public class AvatarImageConfig : ScriptableObject
    {
        public RenderSizeLimitType Size = RenderSizeLimitType.Size64;
        [Tooltip("Choose image quality: Low, Medium, or High")]
        public AvatarQuality Quality;
        
        [Tooltip("Background color, opacity needs to be set to 1 for it to be visible")]
        public Color Background = Color.clear;
        
        [Tooltip("Expression type: Scared, Happy, Lol, Rage, or Sad")]
        public ExpressionType Expression;
        public PoseType Pose;
        public SceneType Scene;
        [Tooltip("Contact Player Zero support if you want to use a custom scene")]
        public string CustomScene;
        
        public string GetParams()
        {
            var parameters = new List<string>();
            parameters.Add($"size={(int) Size}");
            parameters.Add($"quality={Quality.ToString().ToLower()}");
            parameters.Add($"pose={Pose.ToString().ToLower()}");
            parameters.Add($"scene={((Scene != SceneType.Custom) ? FriendlySceneToString(Scene) : CustomScene)}");
            
            if (Background.a >= 1)
            {
                parameters.Add($"background={GetColorString(Background)}");    
            }
       
            if (Expression != ExpressionType.None)
                parameters.Add($"expression={Expression.ToString().ToLower()}");

            return string.Join("&", parameters);
        }
        
        private string GetColorString(Color color)
        {
            return $"{Math.Round(color.r * 255)},{Math.Round(color.g * 255)},{Math.Round(color.b * 255)}";
        }

        private string FriendlySceneToString(SceneType sceneType)
        {
            return sceneType switch
            {
                SceneType.Fullbody => "default-Nova-render-scene-fullbody",
                SceneType.Portrait => "default-Nova-render-scene-portrait",
                _ => throw new ArgumentOutOfRangeException(nameof(sceneType), sceneType, null)
            };
        }
    }
}
