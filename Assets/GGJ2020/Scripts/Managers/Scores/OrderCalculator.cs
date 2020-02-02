using System;
using System.Collections.Generic;
using System.Linq;
using GGJ2020.Parts;
using UniRx.Async;

namespace GGJ2020.Managers.Scores
{
    /// <summary>
    /// PartのリストがOrderを満たすかどうか
    /// </summary>
    public sealed class OrderCalculator
    {
        private readonly Order[] _orders;

        public OrderCalculator(Order[] orders)
        {
            _orders = orders;
        }

        /// <summary>
        /// PartリストがOrderを満たしているか？
        /// </summary>
        public bool Check(IReadOnlyList<Part> parts, Order order)
        {
            foreach (var element in order.Elements)
            {
                if (parts.Count(x => x.Type == element.PartType)
                    >= element.NeedCount)
                {
                    continue;
                }

                return false;
            }

            return true;
        }

        public int Calculate(ShippingParts parts)
        {
            var gotScore = 0;

            var targetParts = parts.Parts
                .Where(x => x.Type.IsOrderType(parts.OrderName));

            // パーツ素体の点数の合計
            var unitScores = targetParts.Select(x => x.UnitScore)
                .DefaultIfEmpty(0)
                .Sum();

            gotScore += unitScores;

            // ComboBonus
            var targetOrder = _orders.FirstOrDefault(x => x.Name == parts.OrderName);
            if (targetOrder.Elements == null) return gotScore;
            if (Check(parts.Parts, targetOrder))
            {
                var combo = targetOrder.ComboBonus;
                var upRate = targetParts
                    .Select(x =>
                    {
                        switch (x.Quality)
                        {
                            case Quality.High:
                                return 1.2f;
                            case Quality.Normal:
                                return 1.0f;
                            case Quality.Low:
                                return 0.8f;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }).DefaultIfEmpty(1.0f)
                    .Aggregate((p, c) => (p * c));

                gotScore += (int) (combo * upRate);
            }

            return gotScore;
        }
    }
}