using System;
using System.Collections.Generic;
using NewCore.Domain;

namespace NewCore.Data
{
    [Serializable]
    public class GameState : IModel
    {
        public List<Shelf> Shelves;
        public List<Customer> Customers;
    }
}