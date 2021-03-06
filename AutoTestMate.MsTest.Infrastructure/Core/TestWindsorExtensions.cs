﻿using System;
using Castle.MicroKernel.Registration;

namespace AutoTestMate.MsTest.Infrastructure.Core
{
	public static class TestWindsorExtensions
	{
		public static ComponentRegistration<T> OverridesExistingRegistration<T>(this ComponentRegistration<T> componentRegistration) where T : class
		{
			return componentRegistration
				.Named(Guid.NewGuid().ToString())
				.IsDefault();
		}
	}
}