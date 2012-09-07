using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Castle.Windsor;
using Castle.MicroKernel;
using System.Reflection;
using Castle.Core.Logging;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor.Installer;
using ConRepo;
using Castle.Facilities.Logging;

namespace ConMvc4Site
{
    /// <summary>
    /// Inject dependencies into the event filters prior to invoking an action.
    /// </summary>
    /// <remarks>http://weblogs.asp.net/psteele/archive/2009/11/04/using-windsor-to-inject-dependencies-into-asp-net-mvc-actionfilters.aspx
    /// </remarks>
    public class WindsorActionInvoker : ControllerActionInvoker
    {
        readonly IWindsorContainer container;

        public WindsorActionInvoker(IWindsorContainer container)
        {
            this.container = container;
        }

        protected override ActionExecutedContext InvokeActionMethodWithFilters(
            ControllerContext controllerContext,
            IList<IActionFilter> filters,
            ActionDescriptor actionDescriptor,
            IDictionary<string, object> parameters)
        {
            foreach (var filter in filters.Where(f => !(f is Controller)))
            {
                InjectProperties(container.Kernel, filter);
            }
            return base.InvokeActionMethodWithFilters(controllerContext, filters, actionDescriptor, parameters);
        }


        protected override ExceptionContext InvokeExceptionFilters(
            ControllerContext controllerContext,
            IList<IExceptionFilter> filters,
            Exception exception)
        {
            foreach (var filter in filters.Where(f => !(f is Controller)))
            {
                InjectProperties(container.Kernel, filter);
            }
            return base.InvokeExceptionFilters(controllerContext, filters, exception);
        }

        private void InjectProperties(IKernel kernel, object target)
        {
            // the controllers are handled by the ControllerFactory, so we ignore them here
            if (target is Controller)
            {
                return;
            }
            var type = target.GetType();
            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (property.CanWrite && kernel.HasComponent(property.PropertyType))
                {
                    var value = kernel.Resolve(property.PropertyType);
                    try
                    {
                        property.SetValue(target, value, null);
                    }
                    catch (Exception ex)
                    {
                        var message = string.Format("Error setting property {0} on type {1}, See inner exception for more information.", property.Name, type.FullName);
                        throw new Exception(message, ex);
                    }
                }
            }
        }
    }


    /// <summary>
    /// This controller factory resolves controllers and arranges for dependency injection for the actions before
    /// they are invoked.
    /// </summary>
    public class WindsorControllerFactory : DefaultControllerFactory
    {
        private readonly IKernel kernel;
        private readonly ILogger logger;

        public WindsorControllerFactory(IKernel kernel, ILogger logger)
        {
            this.kernel = kernel;
            this.logger = logger;
        }

        public override void ReleaseController(IController controller)
        {
            kernel.ReleaseComponent(controller);
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
            {
                throw new HttpException(404, string.Format("The controller for path '{0}' could not be found.", requestContext.HttpContext.Request.Path));
            }

            Controller controller = null;
            try
            {
                controller = (Controller)kernel.Resolve(controllerType);

                // connect action invoker
                if (controller != null)
                {
                    controller.ActionInvoker = kernel.Resolve<IActionInvoker>();
                }
            }
            catch (Exception ex)
            {
                logger.WarnFormat(ex, "Could not instantiate/connect controller for path '{0}'", requestContext.HttpContext.Request.Path);
            }

            if (controller == null)
            {
                throw new HttpException(404, string.Format("The controller for path '{0}' could not be instantiated.", requestContext.HttpContext.Request.Path));
            }

            return (IController)controller;
        }
    }

    public class ControllersInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            // add ActionInvoker - it can be a singleton
            container.Register(Component.For<IActionInvoker>().Instance(new WindsorActionInvoker(container)));

            // register controllers
            container.Register(Classes.FromThisAssembly()
                                .BasedOn<IController>()
                                .LifestyleTransient());
        }
    }

    public class ResourceInstaller : IWindsorInstaller
    {
        // other components can be registered here
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<ContactsRepository>()
                .ImplementedBy<ContactsRepository>());
        }
    }

    public class LoggerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            IFacility logging = new LoggingFacility(LoggerImplementation.NLog, "NLog.config");
            container.AddFacility(logging);
        }
    }

    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        private IWindsorContainer container;

        private void BootstrapContainer()
        {
            container = new WindsorContainer()
                .Install(FromAssembly.This());

            // singleton cache because it is shared and readonly
            var logger = container.Resolve<ILogger>();
            var repo = container.Resolve<ContactsRepository>();
            var users = new Parts.UserCache(repo, logger);
            container.Register(Component.For<Parts.UserCache>().Instance(users));

            var controllerFactory = new WindsorControllerFactory(container.Kernel, logger);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);
            logger.Debug("Contacts (MVC4) started");
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            BootstrapContainer();
        }
    }
}