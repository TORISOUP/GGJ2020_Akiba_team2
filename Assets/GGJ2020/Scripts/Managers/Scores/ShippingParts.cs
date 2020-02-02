using System.Collections.Generic;
using GGJ2020.Parts;

namespace GGJ2020.Managers.Scores
{
    public struct ShippingParts
    {
        public IReadOnlyList<Part> Parts { get; }
        public OrderName OrderName { get; }

        public ShippingParts(IReadOnlyList<Part> parts, OrderName orderName)
        {
            Parts = parts;
            OrderName = orderName;
        }
    }
}