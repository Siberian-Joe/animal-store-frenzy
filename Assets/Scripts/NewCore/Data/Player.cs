using NewCore.Domain;

namespace NewCore.Data
{
    public class Player : NavigableEntity<PlayerData>
    {
        public override PlayerData ToModel()
        {
            return new PlayerData
            {
                ID = ID,
                Position = Position.Value,
                TargetPosition = TargetPosition.Value
            };
        }
    }
}