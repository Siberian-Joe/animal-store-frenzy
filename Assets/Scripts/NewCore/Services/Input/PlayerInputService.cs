using System;
using System.Collections.Generic;
using R3;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace NewCore.Services.Input
{
    public class PlayerInputService : IPlayerInputService, IDisposable
    {
        public Observable<ClickContext> Clicked => _worldClicked;

        private readonly PlayerInput _playerInput;
        private readonly Camera _camera;
        private readonly Subject<ClickContext> _worldClicked = new();
        private readonly List<RaycastResult> _uiHits = new(16);

        public PlayerInputService(ICameraProvider cameraProvider)
        {
            _playerInput = new PlayerInput();
            _camera = cameraProvider.Camera;

            _playerInput.Player.Click.performed += OnClick;
            _playerInput.Player.Enable();
        }

        private void OnClick(InputAction.CallbackContext context)
        {
            var screenPos = context.ReadValue<Vector2>();
            if (screenPos == Vector2.zero && Mouse.current != null)
                screenPos = Mouse.current.position.ReadValue();

            if (IsPointerOverUI(screenPos))
                return;

            var ray = _camera.ScreenPointToRay(screenPos);
            var hit = Physics2D.GetRayIntersection(ray);

            var worldPos = _camera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, _camera.nearClipPlane));
            _worldClicked.OnNext(new ClickContext(worldPos, hit.collider));
        }

        private bool IsPointerOverUI(Vector2 screenPos)
        {
            var eventSystem = EventSystem.current;
            if (eventSystem == null) return false;

            var pointerEventData = new PointerEventData(eventSystem) { position = screenPos };

            _uiHits.Clear();
            eventSystem.RaycastAll(pointerEventData, _uiHits);

            for (var i = 0; i < _uiHits.Count; i++)
            {
                if (_uiHits[i].module is GraphicRaycaster)
                    return true;
            }

            return false;
        }

        public void Dispose()
        {
            _playerInput.Player.Click.performed -= OnClick;
            _playerInput.Disable();
            _playerInput.Dispose();

            _worldClicked.OnCompleted();
            _worldClicked.Dispose();

            _uiHits.Clear();
        }
    }
}