using System;
using R3;
using UnityEngine;

namespace Game.World.PlayerInteraction
{
    public sealed class PlayerFeedbackService : IPlayerFeedback, IPlayerFeedbackReader, IDisposable
    {
        public string CurrentMessage { get; private set; } = string.Empty;
        public Observable<string> MessageShown => _messageShown;

        private readonly Subject<string> _messageShown = new();

        public void ShowMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                return;

            CurrentMessage = message.Trim();
            Debug.Log(CurrentMessage);
            _messageShown.OnNext(CurrentMessage);
        }

        public void Dispose() => _messageShown.Dispose();
    }
}