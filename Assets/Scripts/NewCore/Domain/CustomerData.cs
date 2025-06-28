using System;
using UnityEngine;

namespace NewCore.Domain
{
    [Serializable]
    public class CustomerData : EntityData
    {
        public string Type;
        public Vector2 Position;
    }
}