using System;
using UnityEngine;

namespace NewCore.Domain
{
    [Serializable]
    public class PlayerData : EntityData
    {
        public Vector2 Position;
        public Vector2 TargetPosition;
    }
}