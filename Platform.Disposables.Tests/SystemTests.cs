using Xunit;

namespace Platform.Disposables.Tests
{
    /// <summary>
    /// <para>Contains tests for features of .NET Framework itself, that are required by current implementations.</para>
    /// <para>Содержит тесты для функций самой .NET Framework, которые требуются для текущих реализаций.</para>
    /// </summary>
    public static class SystemTests
    {
        [Fact]
        public static void UsingSupportsNullTest()
        {
            using (var disposable = null as IDisposable)
            {
                Assert.True(disposable == null);
            }
        }
    }
}
