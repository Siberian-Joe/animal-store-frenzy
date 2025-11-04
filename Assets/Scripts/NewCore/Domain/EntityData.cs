using System;
using UnityEngine;

namespace NewCore.Domain
{
    [Serializable]
    public class EntityData : IModel
    {
        public string Id;
        public Vector3 Position;
    }
}