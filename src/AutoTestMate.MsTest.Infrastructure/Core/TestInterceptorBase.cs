using System.Threading.Tasks;
using Castle.DynamicProxy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoTestMate.MsTest.Infrastructure.Core;

public abstract class TestInterceptorBase : IAsyncInterceptor
{
    public virtual void InterceptAsynchronous(IInvocation invocation) => invocation.ReturnValue = (object)this.InternalInterceptAsynchronous(invocation);
    
    public virtual void InterceptAsynchronous<TResult>(IInvocation invocation) => invocation.ReturnValue = (object)this.InternalInterceptAsynchronous<TResult>(invocation);
    public abstract void InterceptSynchronous(IInvocation invocation);
    public abstract Task InternalInterceptAsynchronous(IInvocation invocation);
    public abstract  Task<TResult> InternalInterceptAsynchronous<TResult>(IInvocation invocation);

}