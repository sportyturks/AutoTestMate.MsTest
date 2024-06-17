using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public virtual ITestManager TestManager { get; set; }
		public virtual TestContext TestContext { get; set; }

		public virtual void CustomAttributesInitialise(string testMethod)
        {
            // Get instance of the attribute.
            IEnumerable<IAuthenticationAttribute> classAttributes = Attribute.GetCustomAttributes(GetType(), true).OfType<IAuthenticationAttribute>().ToList();
            var method = GetType().GetMethod(TestContext.TestName);

            _actionTestDataAttributes = method.GetCustomAttributes(typeof(ITestDataAttribute), true).OfType<ITestDataAttribute>();
            foreach (var testDataAttribute in _actionTestDataAttributes)
            {
                testDataAttribute.BeforeTest(testMethod, TestContext, TestManager);
            }

            if (classAttributes.Any())
            {
                foreach (var authenticationAttribute in classAttributes)
                {
                    if (!AttributeLifeCycleHelper.ClassAttributeInitialized(GetType(), authenticationAttribute.GetType()))
                    {
                        authenticationAttribute.BeforeTest(testMethod, TestContext, TestManager);
                        AttributeLifeCycleHelper.InitializeClassAttribute(GetType(), authenticationAttribute.GetType());
                    }
                }
            }
            else
            {
                _actionAuthenticationAttributes = method.GetCustomAttributes(typeof(IAuthenticationAttribute), true).OfType<IAuthenticationAttribute>();
                foreach (var authenticationAttribute in _actionAuthenticationAttributes)
                {
                    authenticationAttribute.BeforeTest(testMethod, TestContext, TestManager);
                }
            }
        }

        public virtual void CustomAttributesCleanup(string testMethod)
        { 
            foreach (var authenticationAttribute in _actionAuthenticationAttributes)
            {
                authenticationAttribute.AfterTest(testMethod, TestManager);
            }

            foreach (var tesdataAttribute in _actionTestDataAttributes)
            {
                tesdataAttribute.AfterTest(testMethod, TestManager);
            }
        }
    }
}
