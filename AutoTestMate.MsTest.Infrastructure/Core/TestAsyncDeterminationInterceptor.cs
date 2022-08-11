using Castle.DynamicProxy;

namespace AutoTestMate.MsTest.Infrastructure.Core;

public class TestAsyncDeterminationInterceptor<TInterceptor> : AsyncDeterminationInterceptor where TInterceptor: IAsyncInterceptor
{
    public TestAsyncDeterminationInterceptor(TInterceptor asyncInterceptor) : base((IAsyncInterceptor)asyncInterceptor)
    {
    }
}