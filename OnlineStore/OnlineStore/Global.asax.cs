using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.Practices.Unity;

namespace OnlineStore
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            var container = Bootstrapper.Initialise();
            var factory = new UnityControllerFactory(container);
            ControllerBuilder.Current.SetControllerFactory(factory);
        }
    }

    public class UnityControllerFactory : IControllerFactory
    {
        private readonly IUnityContainer container;
        private readonly IControllerFactory factory;

        public UnityControllerFactory(IUnityContainer container)
            : this(container, new DefaultControllerFactory())
        {

        }

        protected UnityControllerFactory(IUnityContainer container, DefaultControllerFactory factory)
        {
            this.container = container;
            this.factory = factory;
        }

        public IController CreateController(RequestContext requestContext, string controllerName)
        {
            try
            {
                return container.Resolve<IController>(controllerName);
            }
            catch
            {
                return factory.CreateController(requestContext, controllerName);
            }
        }

        public System.Web.SessionState.SessionStateBehavior GetControllerSessionBehavior(RequestContext requestContext, string controllerName)
        {
            return System.Web.SessionState.SessionStateBehavior.Default;
        }

        public void ReleaseController(IController controller)
        {
            container.Teardown(controller);
        }
    }
}