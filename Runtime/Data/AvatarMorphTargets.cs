using System.Collections.Generic;

namespace PlayerZero.Data
{
    public class AvatarMorphTargets
    {
        /// <summary>
        /// A static list of morph targets (AKA blendshapes) and group names.
        /// </summary>
        private static readonly List<string> morphTargetGroupNames = new()
        {
            "Default",
            "Default + Oculus Visemes + eyeBlink",
            "Oculus Visemes",
            "ARKit",
            "Eyes Extra",
            "Basic Expressions"
        };

        /// <summary>
        /// A static list of morph targets (AKA blendshapes).
        /// </summary>
        private static readonly List<string> morphTargetNames = new()
        {
            "viseme_aa",
            "viseme_E",
            "viseme_I",
            "viseme_O",
            "viseme_U",
            "viseme_CH",
            "viseme_DD",
            "viseme_FF",
            "viseme_kk",
            "viseme_nn",
            "viseme_PP",
            "viseme_RR",
            "viseme_sil",
            "viseme_SS",
            "viseme_TH",
            "browDownLeft",
            "browDownRight",
            "browInnerUp",
            "browOuterUpLeft",
            "browOuterUpRight",
            "eyesClosed",
            "eyeBlinkLeft",
            "eyeBlinkRight",
            "eyeSquintLeft",
            "eyeSquintRight",
            "eyeWideLeft",
            "eyeWideRight",
            "eyesLookUp",
            "eyesLookDown",
            "eyeLookDownLeft",
            "eyeLookDownRight",
            "eyeLookUpLeft",
            "eyeLookUpRight",
            "eyeLookInLeft",
            "eyeLookInRight",
            "eyeLookOutLeft",
            "eyeLookOutRight",
            "jawOpen",
            "jawForward",
            "jawLeft",
            "jawRight",
            "noseSneerLeft",
            "noseSneerRight",
            "cheekPuff",
            "cheekSquintLeft",
            "cheekSquintRight",
            "mouthSmileLeft",
            "mouthSmileRight",
            "mouthOpen",
            "mouthSmile",
            "mouthLeft",
            "mouthRight",
            "mouthClose",
            "mouthFunnel",
            "mouthDimpleLeft",
            "mouthDimpleRight",
            "mouthStretchLeft",
            "mouthStretchRight",
            "mouthRollLower",
            "mouthRollUpper",
            "mouthPressLeft",
            "mouthPressRight",
            "mouthUpperUpLeft",
            "mouthUpperUpRight",
            "mouthFrownLeft",
            "mouthFrownRight",
            "mouthPucker",
            "mouthShrugLower",
            "mouthShrugUpper",
            "mouthLowerDownLeft",
            "mouthLowerDownRight",
            "tongueOut"
        };
        
        public static readonly string[] MorphTargetGroupNames = morphTargetGroupNames.ToArray();
        public static readonly string[] MorphTargetNames = morphTargetNames.ToArray();
    }
}
