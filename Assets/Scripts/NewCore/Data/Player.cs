using System.Collections.Generic;
using System.Linq;
using NewCore.Domain;
using NewCore.Modules.Interaction.Abstractions;

namespace NewCore.Data
{
    public class Player : NavigableEntity<PlayerData>, IActor
    {
        public IReadOnlyList<IInteractionRule> Rules { get; }

        public Player(PlayerData model, IEnumerable<IInteractionRule> interactionRules) : base(model) =>
            Rules = interactionRules.ToList()
                                    .AsReadOnly();

        protected override PlayerData CreateModel()
        {
            return new PlayerData
            {
                Id = Id,
                Position = Position.Value,
                TargetPosition = TargetPosition.Value
            };
        }
    }
}