using UnityEngine;

namespace NewCore.Services.Input
{
    public interface ICameraProvider
    {
        Camera Camera { get; }
    }
}