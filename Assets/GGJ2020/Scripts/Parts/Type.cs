using System;
using GGJ2020.Managers.Scores;

namespace GGJ2020.Parts
{
    public enum Type
    {
        CarBody,
        Wheel,

        RobotCore,
        RobotLeg,
        RobotArms,
        RobotHead,
        Weapon,

        DollHead,
        DollBody,
        Ribbon
    }

    public static class TypeExtension
    {
        public static bool IsOrderType(this Type type, OrderName orderName)
        {
            switch (orderName)
            {
                case OrderName.Car:
                    return type == Type.CarBody || type == Type.Wheel;
                    break;
                case OrderName.Robot:
                    return
                        type == Type.RobotArms
                        || type == Type.RobotLeg
                        || type == Type.RobotCore
                        || type == Type.RobotHead
                        || type == Type.Weapon;

                    break;
                case OrderName.Doll:
                    return type == Type.DollBody ||
                           type == Type.DollHead ||
                           type == Type.Ribbon;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(orderName), orderName, null);
            }
        }
    }
}