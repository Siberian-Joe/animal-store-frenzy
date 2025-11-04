using System.Collections.Generic;
using NewCore.Data;

namespace NewCore.Modules.Interaction.Abstractions
{
    public interface IActor : IEntityProxy
    {
        IReadOnlyList<IInteractionRule> Rules { get; }
    }
}