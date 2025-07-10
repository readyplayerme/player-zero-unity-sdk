using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerZero.Data
{
    [Serializable, CreateAssetMenu(fileName = "Avatar Render Config", menuName = "Player Zero/Avatar Render Config", order = 2)]
    public class AvatarRenderConfig : ScriptableObject
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
                SceneType.Portrait => "default-Nova-render-scene-portrait-closeup",
                _ => throw new ArgumentOutOfRangeException(nameof(sceneType), sceneType, null)
            };
        }
    }
}
