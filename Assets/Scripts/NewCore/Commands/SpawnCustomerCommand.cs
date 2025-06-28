using UnityEngine;

namespace NewCore.Commands
{
    public sealed class SpawnCustomerCommand : ICommand
    {
        public string Type { get; }
        public Vector2 Position { get; }

        public SpawnCustomerCommand(string type, Vector2 position)
        {
            Type = type;
            Position = position;
        }
    }
}