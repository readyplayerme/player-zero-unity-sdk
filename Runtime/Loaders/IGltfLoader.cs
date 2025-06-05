using System.Threading.Tasks;
using UnityEngine;

namespace PlayerZero.Runtime.Sdk
{
    public interface IGltfLoader
    {
        Task<GameObject> LoadModelAsync(string url);
    }
}
