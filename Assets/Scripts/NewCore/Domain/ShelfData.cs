using System;

namespace NewCore.Domain
{
    [Serializable]
    public class ShelfData : EntityData
    {
        public string Name;
        public int MaxCapacity = 5;
        public int Capacity;
    }
}