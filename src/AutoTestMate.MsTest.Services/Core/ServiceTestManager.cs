using System;
using System.Net;
using System.Net.Http;
using AutoTestMate.MsTest.Infrastructure.Constants;
using AutoTestMate.MsTest.Infrastructure.Core;
using Castle.MicroKernel.Registration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoTestMate.MsTest.Services.Core
{
    public class ServiceTestManager : TestManager
    {
        
        private static readonly Lazy<ServiceTestManager> Singleton = new(() => new ServiceTestManager());
        public new static ServiceTestManager Instance => Singleton.Value;
        
        #region Properties

        public virtual bool UseWcfServices
        {
            get
            {
                if (ConfigurationReader == null)

                {
                    return false;
                }

                var useWcfServicesConfigValue = ConfigurationReader.GetConfigurationValue(Constants.Configuration.UseWcfServiceConfig);

                return !string.IsNullOrWhiteSpace(useWcfServicesConfigValue) && string.Equals(useWcfServicesConfigValue.ToLower(), Generic.TrueValue);
            }
        }

        public virtual bool UseHttpClient
        { 
            get
            {
                if (ConfigurationReader == null)

                {
                    return false;
                }

                var useHttpClientConfigValue = ConfigurationReader.GetConfigurationValue(Constants.Configuration.UseHttpClientConfig);

                return !string.IsNullOrWhiteSpace(useHttpClientConfigValue) && string.Equals(useHttpClientConfigValue.ToLower(), Generic.TrueValue);
            }
        }
        public bool UseMockedAuthorisation
        {
            get
            {
                if (ConfigurationReader == null)

                {
                    return false;
                }

                var useMockedAuthorisationConfigValue = ConfigurationReader.GetConfigurationValue(Constants.Configuration.UseMockedAuthorisationConfig);

                return !string.IsNullOrWhiteSpace(useMockedAuthorisationConfigValue) && string.Equals(useMockedAuthorisationConfigValue.ToLower(), Generic.TrueValue);
            }
        }

        public virtual HttpClient HttpClient
        {
            get
            {
                if (UseHttpClient)
                {
                    return Container.Resolve<HttpClient>();
                }

                throw new ApplicationException(Constants.Configuration.HttpClientSettingExceptionMsg);
            }
        }
        
        //public IAuthorisationService AuthorisationService => Container.Resolve<IAuthorisationService>();

        //public IAuthorisationSecurityRoles AuthorisationSecurityRoles => Container.Resolve<IAuthorisationSecurityRoles>();

        #endregion

        #region Constructor

        protected ServiceTestManager()
        {
        }

        #endregion

        #region Public Methods

        public override void OnInitialiseAssemblyDependencies(TestContext testContext = null)
        {
            base.OnInitialiseAssemblyDependencies(testContext);

            InitialiseHttpClient();
        }

        public override void OnDisposeAssemblyDependencies()
        {
            if (UseHttpClient)
            {
                HttpClient?.Dispose();
            }

            base.OnDisposeAssemblyDependencies();
        }

        public virtual void InitialiseHttpClient()
        {
            var useHttpClient = ConfigurationReader.GetConfigurationValue(Constants.Configuration.UseHttpClientConfig);

            if (string.IsNullOrWhiteSpace(useHttpClient) || !string.Equals(useHttpClient.ToLower(), Generic.TrueValue)) return;
            
            var cookieContainer = new CookieContainer {PerDomainCapacity = 5};
            var httpClientHandler = new HttpClientHandler
            {
                CookieContainer = cookieContainer,
                UseDefaultCredentials = true,
                AllowAutoRedirect = false
            };

            var httpClient = new HttpClient(httpClientHandler);
            Container.Register(Component.For<HttpClient>().Instance(httpClient).OverridesExistingRegistration().LifestyleSingleton())
                .Register(Component.For<HttpClientHandler>().Instance(httpClientHandler).OverridesExistingRegistration().LifestyleSingleton())
                .Register(Component.For<CookieContainer>().Instance(cookieContainer).OverridesExistingRegistration().LifestyleSingleton());
        }

        #endregion
    }
}
