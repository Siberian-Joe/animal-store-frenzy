using UnityEngine;

namespace Game.World.PlayerInteraction
{
    public sealed class DebugPlayerFeedback : IPlayerFeedback
    {
        public void ShowMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                return;

            Debug.Log(message.Trim());
        }
    }
}