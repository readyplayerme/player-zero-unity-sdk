using PlayerZero.Api.V1;
using UnityEngine;

namespace PlayerZero
{
    /// <summary>
    /// Extension methods for <see cref="Character"/>.
    /// </summary>
    public static class CharacterExtensions
    {
        private const string DEFAULT_FEMALE_BLUEPRINT_ID = "65e0a59ebba2f3e2a45841ff";

        /// <summary>
        /// Determines if the character is male by comparing its blueprint ID.
        /// Returns <c>true</c> if the blueprint ID does not match the default female blueprint ID.
        /// Logs an error and returns <c>true</c> if the character is <c>null</c>.
        /// </summary>
        /// <param name="character">The character to check.</param>
        /// <returns><c>true</c> if male; otherwise, <c>false</c>.</returns>
        public static bool IsMale(this Character character)
        {
            if (character != null) return character.BlueprintId != DEFAULT_FEMALE_BLUEPRINT_ID;
            Debug.LogError("Character is null.");
            return true;
        }
    }
}
