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
        Scared,
        Happy,
        Lol,
        Rage,
        Sad,
    }
    
    [Serializable]
    public class AvatarImageParameters
    {
        public Dictionary<string, float> blendShapes = new Dictionary<string, float>();
        public int size = 512;
        [Tooltip("Choose image quality: Low, Medium, or High")]
        public AvatarQuality quality;

        [Tooltip("Choose camera type: Portrait, Fullbody, or Fit")]
        public CameraType camera;
        
        [Tooltip("Background color as comma separated RGB values e.g. '255,255,255'")]
        public string background;
        
        [Tooltip("Expression type: Scared, Happy, Lol, Rage, or Sad")]
        public ExpressionType expression;
        public string pose;
        public string scene;
    }
}
