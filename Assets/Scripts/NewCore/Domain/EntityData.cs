using System;
using UnityEngine;

namespace NewCore.Domain
{
    [Serializable]
    public class EntityData : IModel
    {
        public string ID;
        public Vector3 Position;
    }
}