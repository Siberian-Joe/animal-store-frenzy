using NewCore.Domain;

namespace NewCore.Data
{
    public class Customer : NavigableEntity<CustomerData>
    {
        public string Type { get; }

        public Customer(CustomerData model) : base(model) => Type = model.Type;

        protected override CustomerData CreateModel()
        {
            return new CustomerData
            {
                Id = Id,
                Type = Type,
                Position = Position.Value,
                TargetPosition = TargetPosition.Value,
            };
        }
    }
}