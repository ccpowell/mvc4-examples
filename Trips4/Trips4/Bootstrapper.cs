using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc3;
using System.Web.Http;

using DRCOG.Common.Services.Interfaces;
using DRCOG.Data;
using DRCOG.Domain.Interfaces;
using DRCOG.Domain.ServiceInterfaces;
using DRCOG.TIP.Services;
using Trips4.Services;
using DRCOG.Common.Service.MemberShipServiceSupport.Interfaces;

namespace Trips4
{
    public static class Bootstrapper
    {
        public static void Initialise()
        {
            var container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

            // register dependency resolver for WebAPI RC
            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // e.g. container.RegisterType<ITestService, TestService>();     

            container.RegisterType<IAccountRepository, AccountRepository>();
            container.RegisterType<ITipRepository, TipRepository>();
            container.RegisterType<IRtpRepository, RtpRepository>();
            container.RegisterType<IEmailService, EmailService>();
            container.RegisterType<IProjectRepository, ProjectRepository>();
            container.RegisterType<IRtpProjectRepository, RtpProjectRepository>();
            container.RegisterType<ISurveyRepository, SurveyRepository>();
            container.RegisterType<IFileRepositoryExtender, FileRepository>();
            container.RegisterType<IUserRepositoryExtension, Trips4.Data.UserRepository>();

            return container;
        }
    }
}