using System;
using UnityEngine;

namespace NewCore.Domain
{
    [Serializable]
    public class Customer : Entity
    {
        public string Type;
        public Vector3Int Position;
    }
}