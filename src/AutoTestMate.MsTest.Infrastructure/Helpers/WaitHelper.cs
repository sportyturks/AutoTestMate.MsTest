using System.Threading.Tasks;

namespace AutoTestMate.MsTest.Infrastructure.Helpers;

public class WaitHelper
{
    public static void Wait(int milliseconds)
    {
        Task.Delay(milliseconds).Wait();
    }
}