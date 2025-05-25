using System;
using System.Collections.Generic;
using NewCore.Domain;

namespace NewCore.Data
{
    [Serializable]
    public class GameStateData : IModel
    {
        public List<ShelfData> Shelves;
        public List<CustomerData> Customers;
    }
}