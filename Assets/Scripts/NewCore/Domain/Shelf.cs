using System;

namespace NewCore.Domain
{
    [Serializable]
    public class Shelf : Entity
    {
        public string Name;
        public int Capacity;
        public int Level;
    }
}