using System;

namespace NewCore.Domain
{
    [Serializable]
    public class ShelfData : EntityData
    {
        public string Name;
        public int Capacity;
        public int Level;
    }
}