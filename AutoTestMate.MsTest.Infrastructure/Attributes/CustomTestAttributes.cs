using System;
using System.Collections.Generic;
using System.Linq;
using AutoTestMate.MsTest.Infrastructure.Core;
using AutoTestMate.MsTest.Infrastructure.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoTestMate.MsTest.Infrastructure.Attributes
{
    public abstract class CustomTestAttributes
    {
        private IEnumerable<IAuthenticationAttribute> _actionAuthenticationAttributes = new List<IAuthenticationAttribute>();
        private IEnumerable<ITestDataAttribute> _actionTestDataAttributes = new List<ITestDataAttribute>();
        public ITestManager TestManager { get; set; }
		public TestContext TestContext { get; set; }

		public virtual void CustomAttributesInitialise()
        {
            // Get instance of the attribute.
            IEnumerable<IAuthenticationAttribute> classAttributes = Attribute.GetCustomAttributes(GetType(), true).OfType<IAuthenticationAttribute>().ToList();
            var method = GetType().GetMethod(TestContext.TestName);

            _actionTestDataAttributes = method.GetCustomAttributes(typeof(ITestDataAttribute), true).OfType<ITestDataAttribute>();
            foreach (var tesdataAttribute in _actionTestDataAttributes)
            {
                tesdataAttribute.BeforeTest(TestContext, TestManager);
            }

            if (classAttributes.Any())
            {
                foreach (var authenticationAttribute in classAttributes)
                {
                    if (!AttributeLifeCycleHelper.ClassAttributeInitialized(GetType(), authenticationAttribute.GetType()))
                    {
                        authenticationAttribute.BeforeTest(TestContext, TestManager);
                        AttributeLifeCycleHelper.InitializeClassAttribute(GetType(), authenticationAttribute.GetType());
                    }
                }
            }
            else
            {
                _actionAuthenticationAttributes = method.GetCustomAttributes(typeof(IAuthenticationAttribute), true).OfType<IAuthenticationAttribute>();
                foreach (var authenticationAttribute in _actionAuthenticationAttributes)
                {
                    authenticationAttribute.BeforeTest(TestContext, TestManager);
                }
            }
        }

        public virtual void CustomAttributesCleanup()
        { 
            foreach (var authenticationAttribute in _actionAuthenticationAttributes)
            {
                authenticationAttribute.AfterTest(TestManager);
            }

            foreach (var tesdataAttribute in _actionTestDataAttributes)
            {
                tesdataAttribute.AfterTest(TestManager);
            }
        }
    }
}
