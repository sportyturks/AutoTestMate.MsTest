using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTestMate.MsTest.Infrastructure.Core;
using Castle.DynamicProxy;

namespace AutoTestMate.MsTest.Playwright.Core.Attributes
{
    /// <summary>
    /// Retry actions on a POM with a base class of 'BasePage'.
    /// </summary>
    public class RetryInterceptor : TestInterceptorBase, IRetryInterceptor
    {
        public ILoggingUtility LoggingUtility { get; set; }
        public int RetryAmount { get; set; }
        public int RetryInterval { get; set; }
        public RetryInterceptor(ILoggingUtility loggingUtility)
        {
            LoggingUtility = loggingUtility;
        }
        public void Intercept(IInvocation invocation)
        {
            var attributes = Attribute.GetCustomAttributes(invocation.Method, typeof(RetryAttribute), true).ToList();
            if (attributes.Count <= 0)
            {
                invocation.Proceed();
                return;
            }

            var retryAttribute = ((IRetryAttribute)attributes.FirstOrDefault());
            var amount = retryAttribute == null || retryAttribute.Amount <= 0 ? 3 : retryAttribute.Amount;
            var interval = retryAttribute == null || retryAttribute.Interval <= 0 ? 100 : retryAttribute.Interval;

            var exp = new System.Exception();
            var totalRetries = 0;

            RetryAmount = Convert.ToInt32(amount);
            RetryInterval = Convert.ToInt32(interval);
            LoggingUtility.Info($"Retry interceptor: {invocation.MethodInvocationTarget.Name} starting...");
            for (var i = 0; i < RetryAmount; i++)
            {
                try
                {
                    invocation.Proceed();
                    LoggingUtility.Info($"Retry interceptor: {invocation.MethodInvocationTarget.Name}, passed.");
                    break;
                }
                catch (System.Exception e)
                {
                    exp = e;
                    totalRetries++;
                    LoggingUtility.Info($"Retry interceptor: {invocation.MethodInvocationTarget.Name}, progressing...");
                    Thread.Sleep(interval);
                }
            }
            LoggingUtility.Info($"Retry interceptor: {invocation.MethodInvocationTarget.Name}, completed with total retries {totalRetries}.");

            if (totalRetries < RetryAmount) return;
            LoggingUtility.Info($"Retry interceptor: {invocation.MethodInvocationTarget.Name}, failed with total retries: {totalRetries}.");
            throw exp;
        }
        public override void InterceptSynchronous(IInvocation invocation)
        {
            var attributes = Attribute.GetCustomAttributes(invocation.Method, typeof(RetryAttribute), true).ToList();
            if (attributes.Count <= 0)
            {
                invocation.Proceed();
                return;
            }

            var retryAttribute = ((IRetryAttribute)attributes.FirstOrDefault());
            var amount = retryAttribute == null || retryAttribute.Amount <= 0 ? 3 : retryAttribute.Amount;
            var interval = retryAttribute == null || retryAttribute.Interval <= 0 ? 100 : retryAttribute.Interval;

            var exp = new System.Exception();
            var totalRetries = 0;

            RetryAmount = Convert.ToInt32(amount);
            RetryInterval = Convert.ToInt32(interval);
            LoggingUtility.Info($"Retry interceptor: {invocation.MethodInvocationTarget.Name} starting...");

            for (var i = 0; i < RetryAmount; i++)
            {
                try
                {
                    invocation.Proceed();
                    LoggingUtility.Info($"Retry interceptor: {invocation.MethodInvocationTarget.Name}, passed.");
                    break;
                }
                catch (System.Exception e)
                {
                    exp = e;
                    totalRetries++;
                    LoggingUtility.Info($"Retry interceptor: {invocation.MethodInvocationTarget.Name}, progressing...");
                    Thread.Sleep(interval);
                }
            }
            LoggingUtility.Info($"Retry interceptor: {invocation.MethodInvocationTarget.Name}, completed with total retries {totalRetries}.");

            if (totalRetries < RetryAmount) return;
            LoggingUtility.Info($"Retry interceptor: {invocation.MethodInvocationTarget.Name}, failed with total retries: {totalRetries}.");
            throw exp;
        }

        public override async Task InternalInterceptAsynchronous(IInvocation invocation)
        {
            var proceedInfo = invocation.CaptureProceedInfo();
            var attributes = Attribute.GetCustomAttributes(invocation.Method, typeof(RetryAttribute), true).ToList();
            if (attributes.Count <= 0)
            {
                proceedInfo.Invoke();
                var task = (Task)invocation.ReturnValue;
                await task.ConfigureAwait(false);
            }

            var retryAttribute = ((IRetryAttribute)attributes.FirstOrDefault());
            var amount = retryAttribute == null || retryAttribute.Amount <= 0 ? 3 : retryAttribute.Amount;
            var interval = retryAttribute == null || retryAttribute.Interval <= 0 ? 100 : retryAttribute.Interval;

            var exp = new System.Exception();
            var totalRetries = 0;

            RetryAmount = Convert.ToInt32(amount);
            RetryInterval = Convert.ToInt32(interval);
            LoggingUtility.Info($"Retry interceptor: {invocation.MethodInvocationTarget.Name} starting...");
            for (var i = 0; i < RetryAmount; i++)
            {
                try
                {
                    invocation.Proceed();
                    LoggingUtility.Info($"Retry interceptor: {invocation.MethodInvocationTarget.Name}, passed.");
                    break;
                }
                catch (System.Exception e)
                {
                    exp = e;
                    totalRetries++;
                    LoggingUtility.Info($"Retry interceptor: {invocation.MethodInvocationTarget.Name}, progressing...");
                    await Task.Delay(interval).ConfigureAwait(false);
                }
            }
            LoggingUtility.Info($"Retry interceptor: {invocation.MethodInvocationTarget.Name}, completed with total retries {totalRetries}.");

            if (totalRetries < RetryAmount) return;
            LoggingUtility.Info($"Retry interceptor: {invocation.MethodInvocationTarget.Name}, failed with total retries: {totalRetries}.");
            throw exp;
        }

        public override async Task<TResult> InternalInterceptAsynchronous<TResult>(IInvocation invocation)
        {
            Task<TResult> taskResult = null;
            var proceedInfo = invocation.CaptureProceedInfo();
            var attributes = Attribute.GetCustomAttributes(invocation.Method, typeof(RetryAttribute), true).ToList();
            if (attributes.Count <= 0)
            {
                proceedInfo.Invoke();
                taskResult = (Task<TResult>)invocation.ReturnValue;
                return await taskResult.ConfigureAwait(false);
            }

            var retryAttribute = ((IRetryAttribute)attributes.FirstOrDefault());
            var amount = retryAttribute == null || retryAttribute.Amount <= 0 ? 3 : retryAttribute.Amount;
            var interval = retryAttribute == null || retryAttribute.Interval <= 0 ? 100 : retryAttribute.Interval;

            var exp = new System.Exception();
            var totalRetries = 0;

            RetryAmount = Convert.ToInt32(amount);
            RetryInterval = Convert.ToInt32(interval);
            LoggingUtility.Info($"Retry interceptor: {invocation.MethodInvocationTarget.Name} starting...");
            for (var i = 0; i < RetryAmount; i++)
            {
                try
                {
                    proceedInfo.Invoke();
                    taskResult = (Task<TResult>)invocation.ReturnValue;
                    await taskResult.ConfigureAwait(false);
                    LoggingUtility.Info($"Retry interceptor: {invocation.MethodInvocationTarget.Name}, passed.");
                    break;
                }
                catch (System.Exception e)
                {
                    exp = e;
                    totalRetries++;
                    LoggingUtility.Info($"Retry interceptor: {invocation.MethodInvocationTarget.Name}, progressing...");
                    await Task.Delay(interval).ConfigureAwait(false);
                }
            }
            LoggingUtility.Info($"Retry interceptor: {invocation.MethodInvocationTarget.Name}, completed with total retries {totalRetries}.");


            // ReSharper disable once PossibleNullReferenceException
            if (totalRetries < RetryAmount) return await taskResult.ConfigureAwait(false); 
            LoggingUtility.Info($"Retry interceptor: {invocation.MethodInvocationTarget.Name}, failed with total retries: {totalRetries}.");
            throw exp;
        }
    }
}