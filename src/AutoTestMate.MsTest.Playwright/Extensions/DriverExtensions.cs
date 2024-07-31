using System;
using System.Collections.ObjectModel;
using System.IO;

namespace AutoTestMate.MsTest.Playwright.Extensions
{
	public static class DriverExtensions
	{
		public static string ScreenShotSaveFile(this IWebDriver driver, string directory, string testName)
		{
			try
			{
				Directory.CreateDirectory(directory);
				string fileName = Path.Combine(directory, $"{testName}_{DateTime.Now:yyyy-MM-dd_HH.mm}.png");
				var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
				screenshot.SaveAsFile(fileName);
				return fileName;
			}
			catch
			{
				//cannot take screenshot, posssibly due to assert error
				return string.Empty;
			}
		}

		/// <summary>
		///     Waits for a specified function to be satisfied within the specified timeout.
		/// </summary>
		/// <param name="driver">The Web Driver</param>
		/// <param name="func">The function to be satisfied</param>
		/// <param name="limit">The maximum wait time in seconds</param>
		public static void Wait(this IWebDriver driver, Func<IWebDriver, IWebElement> func, int limit)
		{
			var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(limit));
			wait.Until(func);
		}

		public static void Wait(this IWebDriver driver, By by, int limit)
		{
			var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(limit));
			wait.Until(d => d.FindElement(by));
		}
		public static bool TryWait(this IWebDriver driver, By by, int limit)
		{
			try
			{
				var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(limit));
				wait.Until(d => d.FindElement(by));
				return true;
			}
			catch
			{
				return false;
			}
		}

		public static void Wait(this IWebDriver driver, Func<IWebDriver, bool> func, int limit)
		{
			var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(limit));
			wait.Until(func);
		}

		public static void Wait(this IWebDriver driver, Func<IWebDriver, ReadOnlyCollection<IWebElement>> func,
			int limit)
		{
			var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(limit));
			wait.Until(func);
		}

		public static IJavaScriptExecutor JavaScript(this IWebDriver driver)
		{
			return (driver as IJavaScriptExecutor);
		}

		/// <summary>
		/// 
		/// Selenium's default behavior for FindElement is to throw an exception if an element is not found. 
		/// HasElement catches this exception to allow us to test for elements without stopping execution of 
		/// the entire program.
		/// 
		/// Similary you can write an extension function for IWebElement to test of elements nested in another 
		/// element.
		/// 
		/// </summary>
		/// <param name="driver"></param>
		/// <param name="by"></param>
		/// <returns></returns>
		public static bool HasElement(this IWebDriver driver, By by)
		{
			try
			{
				driver.FindElement(by);
			}
			catch (NoSuchElementException)
			{
				return false;
			}

			return true;
		}

		public static IWebElement WaitUntilElementIsVisible(this IWebDriver driver, By by, int seconds = 60)
		{
			var wait = new WebDriverWait(driver, new TimeSpan(0, 0, seconds));
			var element = wait.Until(ExpectedConditions.ElementIsVisible(by));
			return element;
		}

		public static IWebElement WaitUntilElementIsNotVisible(this IWebDriver driver, By by, int seconds = 60, uint polling = 250 )
		{
			try
			{
				var wait = new WebDriverWait(driver, new TimeSpan(0, 0, seconds));

				wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(WebDriverException), typeof(StaleElementReferenceException));

				return wait.Until(ctx =>
					{
						var elems = ctx.FindElements(by);

						if (elems.Count == 0) return null;

						var elem = ctx.FindElement(by);

						if (elem == null) return null;

						if (!elem.Displayed || !elem.Enabled)
						{
							return elem;
						}

						return null;
					}
				);
			}
			catch
			{
				return null;
			}
		}
	}
}