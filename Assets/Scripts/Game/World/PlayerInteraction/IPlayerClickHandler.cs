namespace Game.World.PlayerInteraction
{
    public interface IPlayerClickHandler
    {
        int Order { get; }
        PlayerClickHandlingResult Handle(PlayerWorldClick click);
    }
}