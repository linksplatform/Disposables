using Xunit;

namespace Platform.Disposables.Tests
{
    public static class SystemTests
    {
        [Fact]
        public static void UsingSupportsNullTest()
        {
            using var disposable = null as IDisposable;
            Assert.True(disposable == null);
        }
    }
}
