using System.Threading.Tasks;
using AutoTestMate.MsTest.Infrastructure.Core;
using Castle.DynamicProxy;

namespace AutoTestMate.MsTest.Web.Core.Attributes;

public interface IRetryInterceptor
{
    ILoggingUtility LoggingUtility { get; set; }
    int RetryAmount { get; set; }
    int RetryInterval { get; set; }
    void Intercept(IInvocation invocation);
    void InterceptSynchronous(IInvocation invocation);
    Task<TResult> InternalInterceptAsynchronous<TResult>(IInvocation invocation);
    void InterceptAsynchronous(IInvocation invocation);
    void InterceptAsynchronous<TResult>(IInvocation invocation);
}