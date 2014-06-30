using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc4;
using OnlineStore.Common;
using OnlineStoreProvider;
using OnlineStore.Controllers;

namespace OnlineStore
{
    public static class Bootstrapper
    {
        public static IUnityContainer Initialise()
        {
            var container = BuildUnityContainer();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            return container;
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        }

        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<IBasketManager, BasketManager>();
            container.RegisterType<IItemManager, ItemManager>();
            container.RegisterType<IController, StoreController>("Store");
            container.RegisterType<ICheckOut, CheckOutManager>();
            container.RegisterType<IController, PaymentController>("Payment");
            container.RegisterType<IController, BasketController>("Basket");
            container.RegisterType<IAccountManager, AccountManager>();
            container.RegisterType<IController, AccountController>("Account");

        }
    }
}