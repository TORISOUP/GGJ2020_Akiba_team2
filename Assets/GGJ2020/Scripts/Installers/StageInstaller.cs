using GGJ2020.Managers.Scores;
using UnityEngine;
using Zenject;

public class StageInstaller : MonoInstaller
{
    [SerializeField] private OrderConfig _orderConfig;

    public override void InstallBindings()
    {
        Container.Bind<Order[]>().FromInstance(_orderConfig.Orders).AsCached();
        Container.Bind<OrderCalculator>().AsCached();
    }
}