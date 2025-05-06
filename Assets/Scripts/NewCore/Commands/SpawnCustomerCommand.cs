using UnityEngine;

namespace NewCore.Commands
{
    public sealed class SpawnCustomerCommand : ICommand
    {
        public string Type { get; }
        public Vector3Int Position { get; }

        public SpawnCustomerCommand(string type, Vector3Int position)
        {
            Type = type;
            Position = position;
        }
    }
}