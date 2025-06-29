using UnityEngine;

namespace NewCore.Services.Input
{
    [RequireComponent(typeof(Camera))]
    public class CameraProvider : MonoBehaviour, ICameraProvider
    {
        private Camera _camera;
 
        public Camera Camera => _camera ??= GetComponent<Camera>();
    }
}