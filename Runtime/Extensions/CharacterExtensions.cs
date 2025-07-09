using PlayerZero.Api.V1;
using UnityEngine;

namespace PlayerZero
{
    public static class CharacterExtensions
    {
        private const string DEFAULT_FEMALE_BLUEPRINT_ID = "65e0a59ebba2f3e2a45841ff";
        
        public static bool IsMale(this Character character)
        {
            if (character != null) return character.BlueprintId != DEFAULT_FEMALE_BLUEPRINT_ID;
            Debug.LogError("Character is null.");
            return true;
        }
    }
}
