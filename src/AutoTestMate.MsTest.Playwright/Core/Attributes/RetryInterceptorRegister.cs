using System;
using System.Linq;
using System.Reflection;
using AutoTestMate.MsTest.Infrastructure.Core;
using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace AutoTestMate.MsTest.Playwright.Core.Attributes;

public static class RetryInterceptorRegister
{
    public static void Initialize(WindsorContainer container)
    {
        container.Register(Component.For<RetryInterceptor>().LifestyleTransient());
        container.Register(Component.For<TestAsyncDeterminationInterceptor<RetryInterceptor>>().LifestyleTransient());
        container.Kernel.ComponentRegistered += Kernel_ComponentRegistered;
    }

    private static void Kernel_ComponentRegistered(string key, IHandler handler)
    {
        if (ShouldIntercept(handler.ComponentModel.Implementation))
        {
            handler.ComponentModel.Interceptors.Add(
                new InterceptorReference(typeof(TestAsyncDeterminationInterceptor<RetryInterceptor>)));
        }
    }

    private static bool ShouldIntercept(Type type)
    {
        if (SelfOrMethodsDefinesAttribute<RetryAttribute>(type))
        {
            return true;
        }

        return false;
    }

    private static bool SelfOrMethodsDefinesAttribute<TAttr>(Type type)
    {
        if (type.GetTypeInfo().IsDefined(typeof(TAttr), true))
        {
            return true;
        }

        return type
            .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Any(m => m.IsDefined(typeof(TAttr), true));
    }
}
