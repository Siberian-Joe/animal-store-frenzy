using NewCore.Domain;

namespace NewCore.Data
{
    public class Customer : NavigableEntity<CustomerData>
    {
        public string Type;

        public override void Initialize(CustomerData data)
        {
            base.Initialize(data);
            Type = data.Type;
        }

        public override CustomerData ToModel()
        {
            return new CustomerData
            {
                ID = ID,
                Type = Type,
                Position = Position.Value,
                TargetPosition = TargetPosition.Value,
            };
        }
    }
}