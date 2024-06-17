namespace AutoTestMate.MsTest.Infrastructure.Core
{
    public interface IFactory<T>
    {
        T Create();
    }
}