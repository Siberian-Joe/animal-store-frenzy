using Core.ViewModels;
using UnityEngine;

namespace Core.Views
{
    public class CameraView : View<CameraViewModel>
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _lerpSpeed = 1.0f;

        protected override void Initialize()
        {
            if (_target == null)
                return;

            var initialOffset = transform.position - _target.position;
            ViewModel.SetInitialOffset(initialOffset);
        }

        protected override void Update()
        {
            base.Update();

            if (_target == null)
                return;

            ViewModel.UpdateTargetPosition(_target.position);

            var desiredPosition = ViewModel.CalculateDesiredPosition();
            transform.position = Vector3.Lerp(transform.position, desiredPosition, _lerpSpeed * Time.deltaTime);
        }
    }
}