using System;
using System.Collections.Generic;
using System.Linq;
using AutoTestMate.MsTest.Infrastructure.Core;

namespace AutoTestMate.MsTest.Web.Core
{
	public class SeleniumGridDriverCleanupFactory : IFactory<IDriverCleanup>
	{
		private readonly IConfigurationReader _configurationReader;
		private readonly ILoggingUtility _loggingUtility;
		private readonly IProcess _osProcess;
		public SeleniumGridDriverCleanupFactory(IConfigurationReader configurationReader, ILoggingUtility loggingUtility, IProcess osProcess)
		{
			_configurationReader = configurationReader;
			_loggingUtility = loggingUtility;
			_osProcess = osProcess;
		}

		public virtual IDriverCleanup Create()
		{
			return new SeleniumGridDriverCleanup(_osProcess, _configurationReader);
		}
	}

	public class SeleniumGridDriverCleanup : IDriverCleanup
	{
		public const int MaxKillAttemps = 100;
		private readonly IConfigurationReader _configurationReader;
		
		public SeleniumGridDriverCleanup(IProcess process, IConfigurationReader configurationReader)
		{
			_configurationReader = configurationReader;
			Process = process;
		}

		public void Dispose()
		{
		}

		public void Initialise()
		{
		}

		public IProcess Process { get; }

	}
}