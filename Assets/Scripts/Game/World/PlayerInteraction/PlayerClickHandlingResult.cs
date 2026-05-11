namespace Game.World.PlayerInteraction
{
    public readonly struct PlayerClickHandlingResult
    {
        public bool IsConsumed { get; }
        public string FeedbackMessage { get; }
        public bool HasFeedback => string.IsNullOrWhiteSpace(FeedbackMessage) == false;

        private PlayerClickHandlingResult(bool isConsumed, string feedbackMessage)
        {
            IsConsumed = isConsumed;
            FeedbackMessage = feedbackMessage;
        }

        public static PlayerClickHandlingResult Pass() => new(false, null);
        public static PlayerClickHandlingResult Consume(string feedbackMessage = null) => new(true, feedbackMessage);
    }
}