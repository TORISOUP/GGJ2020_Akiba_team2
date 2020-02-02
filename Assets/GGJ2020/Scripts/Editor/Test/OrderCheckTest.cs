using GGJ2020.Managers.Scores;
using GGJ2020.Parts;
using NUnit.Framework;

namespace GGJ2020.Scripts.Editor.Test
{
    public class OrderCheckTest
    {
        [Test]
        public void Orderを満たすとtrue()
        {
            var order = new Order
            (
                name: OrderName.Car,
                100,
                new[]
                {
                    new OrderElement(Type.Wheel, 3),
                    new OrderElement(Type.CarBody, 1),
                }
            );

            var checker = new OrderCalculator(new Order[] {order});

            // Just ok
            Assert.True(checker.Check(new[]
            {
                new Part(Type.Wheel, Quality.High, 0),
                new Part(Type.Wheel, Quality.High, 0),
                new Part(Type.Wheel, Quality.High, 0),
                new Part(Type.CarBody, Quality.High, 0),
            }, order));

            // Over, it is ok
            Assert.True(checker.Check(new[]
            {
                new Part(Type.Wheel, Quality.High, 0),
                new Part(Type.Wheel, Quality.Normal, 0),
                new Part(Type.Wheel, Quality.Normal, 0),
                new Part(Type.Wheel, Quality.High, 0),
                new Part(Type.CarBody, Quality.High, 0),
                new Part(Type.CarBody, Quality.High, 0),
            }, order));


            // NG
            Assert.False(checker.Check(new[]
            {
                new Part(Type.Wheel, Quality.High, 0),
                new Part(Type.Wheel, Quality.High, 0),
                new Part(Type.CarBody, Quality.High, 0),
            }, order));
        }

        [Test]
        public void OrderNameを指定してコンボボーナスが得られる()
        {
            var order = new Order
            (
                name: OrderName.Car,
                100,
                new[]
                {
                    new OrderElement(Type.Wheel, 4),
                    new OrderElement(Type.CarBody, 1),
                }
            );
            var checker = new OrderCalculator(new[] {order});

            var parts = new[]
            {
                new Part(Type.Wheel, Quality.Normal, 0),
                new Part(Type.Wheel, Quality.Normal, 0),
                new Part(Type.Wheel, Quality.Normal, 0),
                new Part(Type.Wheel, Quality.Normal, 0),
                new Part(Type.CarBody, Quality.Normal, 0),
            };

            // Car指定
            var shipment1 = new ShippingParts(parts, OrderName.Car);
            var result = checker.Calculate(shipment1);

            // Car指定なのでCarのコンボボーナスが得られる
            Assert.AreEqual(100, result);

            // Car指定ではない
            var shipment2 = new ShippingParts(parts, OrderName.Doll);
            var result2 = checker.Calculate(shipment2);

            // Car指定ではないのコンボボーナスは得られない
            Assert.AreEqual(0, result2);
        }

        [Test]
        public void グレードがいいやつはボーナスがつく()
        {
            var order = new Order
            (
                name: OrderName.Car,
                100,
                new[]
                {
                    new OrderElement(Type.Wheel, 4),
                    new OrderElement(Type.CarBody, 1),
                }
            );
            var checker = new OrderCalculator(new[] {order});

            var parts = new[]
            {
                new Part(Type.Wheel, Quality.Normal, 0),
                new Part(Type.Wheel, Quality.High, 0),
                new Part(Type.Wheel, Quality.High, 0),
                new Part(Type.Wheel, Quality.Normal, 0),
                new Part(Type.CarBody, Quality.Normal, 0),
            };

            // Car指定
            var shipment1 = new ShippingParts(parts, OrderName.Car);
            var result = checker.Calculate(shipment1);

            // 倍率で点数が上がるはず
            Assert.AreEqual((int) (100 * 1.2f * 1.2f), (int) result);
        }
        
        [Test]
        public void グレードが悪いやつは点数は減る()
        {
            var order = new Order
            (
                name: OrderName.Car,
                100,
                new[]
                {
                    new OrderElement(Type.Wheel, 4),
                    new OrderElement(Type.CarBody, 1),
                }
            );
            var checker = new OrderCalculator(new[] {order});

            var parts = new[]
            {
                new Part(Type.Wheel, Quality.Normal, 0),
                new Part(Type.Wheel, Quality.Normal, 0),
                new Part(Type.Wheel, Quality.Normal, 0),
                new Part(Type.Wheel, Quality.Low, 0),
                new Part(Type.CarBody, Quality.Low, 0),
            };

            // Car指定
            var shipment1 = new ShippingParts(parts, OrderName.Car);
            var result = checker.Calculate(shipment1);

            // 倍率で点数が下がる
            Assert.AreEqual((int) (100 * 0.8f * 0.8f), (int) result);
        }
    }
}