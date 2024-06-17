using Castle.DynamicProxy;

namespace AutoTestMate.MsTest.Infrastructure.Core;

public class TestAsyncDeterminationInterceptor<TInterceptor>(TInterceptor asyncInterceptor)
    : AsyncDeterminationInterceptor(asyncInterceptor)
    where TInterceptor : IAsyncInterceptor;