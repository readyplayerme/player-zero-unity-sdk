using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerZero.Data
{
    /// <summary>
    /// Defines configuration options for avatar rendering, including size, quality, background,
    /// expression, pose, and scene. Provides a method to generate query parameters for rendering requests.
    /// </summary>
    [Serializable, CreateAssetMenu(fileName = "Avatar Render Config", menuName = "Player Zero/Avatar Render Config", order = 2)]
    public class AvatarRenderConfig : ScriptableObject
    {
        /// <summary>
        /// The size limit for avatar rendering.
        /// </summary>
        public RenderSizeLimitType Size = RenderSizeLimitType.Size64;
        
        /// <summary>
        /// The image quality for avatar rendering.
        /// </summary>
        [Tooltip("Choose image quality: Low, Medium, or High")]
        public AvatarQuality Quality;
        
        /// <summary>
        /// The background color for the avatar. Opacity must be 1 to be visible.
        /// </summary>
        [Tooltip("Background color, opacity needs to be set to 1 for it to be visible")]
        public Color Background = Color.clear;
        
        /// <summary>
        /// The expression type for the avatar.
        /// </summary>
        [Tooltip("Expression type: Scared, Happy, Lol, Rage, or Sad")]
        public ExpressionType Expression;
        
        /// <summary>
        /// The pose type for the avatar.
        /// </summary>
        public PoseType Pose;
        
        /// <summary>
        /// The scene type for the avatar.
        /// </summary>
        public SceneType Scene;
        
        /// <summary>
        /// Custom scene name, used if <see cref="SceneType.Custom"/> is selected.
        /// </summary>
        [Tooltip("Contact Player Zero support if you want to use a custom scene")]
        public string CustomScene;
        
        /// <summary>
        /// Generates query parameters for the avatar rendering request based on the current configuration.
        /// </summary>
        /// <returns>A query string representing the configuration.</returns>
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
