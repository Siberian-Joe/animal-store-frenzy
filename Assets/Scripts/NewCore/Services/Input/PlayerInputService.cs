using System;
using R3;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace NewCore.Services.Input
{
    public class PlayerInputService : IPlayerInputService, IDisposable
    {
        public Observable<Vector2> WorldClicked => _clicked;

        private readonly PlayerInput _playerInput;
        private readonly Camera _camera;
        private readonly Subject<Vector2> _clicked = new();

        [Inject]
        public PlayerInputService(ICameraProvider cameraProvider)
        {
            _playerInput = new PlayerInput();
            _camera = cameraProvider.Camera;

            _playerInput.Player.Click.performed += OnClick;
            _playerInput.Player.Enable();
        }

        private void OnClick(InputAction.CallbackContext context)
        {
            var worldPosition = _camera.ScreenToWorldPoint(context.ReadValue<Vector2>());
            _clicked.OnNext(worldPosition);
        }

        public void Dispose()
        {
            _playerInput.Player.Click.performed -= OnClick;
            _playerInput.Disable();
            _playerInput.Dispose();

            _clicked.OnCompleted();
            _clicked.Dispose();
        }
    }
}