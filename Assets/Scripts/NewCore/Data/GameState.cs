using System;
using System.Collections.Generic;
using NewCore.Domain;

namespace NewCore.Data
{
    [Serializable]
    public class GameState
    {
        public List<Shelf> Shelves;
        public List<Domain.Customer> Customers;
    }
}