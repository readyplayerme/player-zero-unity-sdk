using System.Threading.Tasks;
using UnityEngine;

namespace PlayerZero.Runtime.Sdk
{
    /// <summary>
    /// Defines an interface for asynchronously loading GLTF models and returning the root GameObject.
    /// </summary>
    public interface IGltfLoader
    {
        /// <summary>
        /// Asynchronously loads a GLTF model from the specified URL.
        /// Returns the root GameObject of the imported model.
        /// </summary>
        /// <param name="url">The URL of the GLTF model to load.</param>
        /// <returns>A task that resolves to the root GameObject of the imported model.</returns>
        Task<GameObject> LoadModelAsync(string url);
    }
}
