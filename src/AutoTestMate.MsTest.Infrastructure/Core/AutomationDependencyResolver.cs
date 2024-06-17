using System;
using System.Reflection;
using Castle.MicroKernel.Context;
using Castle.MicroKernel.Resolvers;

namespace AutoTestMate.MsTest.Infrastructure.Core;

public class AutomationDependencyResolver : DefaultDependencyResolver
{
    protected override CreationContext RebuildContextForParameter(CreationContext current, Type parameterType)
    {
        return GetTypeInfo(parameterType).ContainsGenericParameters ? current : new CreationContext(parameterType, current, true);
    }

    public static TypeInfo GetTypeInfo(Type type)
    {
        if (type == (Type)null)
            throw new ArgumentNullException(nameof(type));

        return type is IReflectableType reflectableType ? reflectableType.GetTypeInfo() : (TypeInfo)new TypeDelegator(type);
    }
}