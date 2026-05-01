using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Modules.Presentation.Runtime.Panels;
using UnityEngine;

namespace Game.Presentation.Transitions.Loading
{
    public sealed class LoadingOverlay : SystemOverlay
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField, Min(0f)] private float _fadeInDuration = 0.25f;
        [SerializeField, Min(0f)] private float _fadeOutDuration = 0.25f;

        private Tween _fadeTween;

        protected override void OnOpened()
        {
            EnsureCanvasGroup();
            KillFade();

            _canvasGroup.alpha = 0f;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }

        protected override void OnClosed()
        {
            KillFade();

            if (_canvasGroup == false)
                return;

            _canvasGroup.alpha = 0f;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }

        protected override void OnReleased() => KillFade();

        public void ShowImmediate()
        {
            EnsureCanvasGroup();
            KillFade();

            _canvasGroup.alpha = 1f;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }

        public UniTask FadeInAsync(CancellationToken token) => FadeAsync(1f, _fadeInDuration, token);
        public UniTask FadeOutAsync(CancellationToken token) => FadeAsync(0f, _fadeOutDuration, token);

        private async UniTask FadeAsync(
            float targetAlpha,
            float duration,
            CancellationToken token)
        {
            EnsureCanvasGroup();
            KillFade();

            if (duration <= 0f)
            {
                _canvasGroup.alpha = targetAlpha;
                return;
            }

            var tween = _canvasGroup
                .DOFade(targetAlpha, duration)
                .SetEase(Ease.OutQuad)
                .SetUpdate(true)
                .SetLink(gameObject);

            _fadeTween = tween;

            try
            {
                await tween.ToUniTask(cancellationToken: token);
            }
            finally
            {
                if (_fadeTween == tween)
                    _fadeTween = null;
            }
        }

        private void KillFade()
        {
            if (_fadeTween == null)
                return;

            _fadeTween.Kill();
            _fadeTween = null;
        }

        private void EnsureCanvasGroup()
        {
            if (_canvasGroup == false)
                throw new InvalidOperationException(
                    $"{nameof(LoadingOverlay)} requires assigned {nameof(CanvasGroup)}.");
        }
    }
}