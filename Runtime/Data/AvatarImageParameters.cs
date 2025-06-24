using System;
using System.Collections.Generic;

namespace PlayerZero
{
    [Serializable]
    public class AvatarImageParameters
    {
        public Dictionary<string, float> blendShapes = new Dictionary<string, float>();
        public int size = 512;
        public string quality = "high";
        public string camera = "portrait";
        /// <summary>Background color as comma separated RGB values e.g. "255,255,255"</summary>
        public string background;
        public string expression;
        public string pose = "default";
        public string scene = "default-Nova-render-scene-portrait-closeup";
    }
}
