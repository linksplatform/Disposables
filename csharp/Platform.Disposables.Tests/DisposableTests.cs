using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using Xunit;

namespace Platform.Disposables.Tests
{
    public static class DisposableTests
    {
        [Fact]
        public static void DisposalOrderTest()
        {
            var logPath = Path.GetTempFileName();
            using (var process = Process.Start(CreateProcessStartInfo(logPath, waitForCancellation: false)))
            {
                process.WaitForExit();
            }
            var result = File.ReadAllText(logPath);
            Assert.Equal("21", result);
            File.Delete(logPath);
        }

        [Fact]
        public static void DisposalAtProcessKillTest()
        {
            var logPath = Path.GetTempFileName();
            using (var process = Process.Start(CreateProcessStartInfo(logPath, waitForCancellation: true)))
            {
                Thread.Sleep(1000);
                process.Kill();
            }
            var result = File.ReadAllText(logPath);
            Assert.Equal("", result); // Currently, process termination will not release resources
            File.Delete(logPath);
        }
        private static ProcessStartInfo CreateProcessStartInfo(string logPath, bool waitForCancellation)
        {
            var projectPath = GetDisposalObjectTestProjectFilePath();
            return new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"run -p \"{projectPath}\" -f net8 \"{logPath}\" {waitForCancellation.ToString()}",
                UseShellExecute = false,
                CreateNoWindow = true
            };
        }
        private static string GetDisposalObjectTestProjectFilePath()
        {
            const string currentProjectName = nameof(Platform) + "." + nameof(Disposables) + "." + nameof(Tests);
            const string disposalOrderTestProjectName = currentProjectName + "." + nameof(DisposalOrderTest);
            var currentDirectory = Environment.CurrentDirectory;
            var pathParts = currentDirectory.Split(Path.DirectorySeparatorChar);
            var newPathParts = new List<string>();
            for (var i = 0; i < pathParts.Length; i++)
            {
                if (string.Equals(pathParts[i], currentProjectName))
                {
                    newPathParts.Add(disposalOrderTestProjectName);
                    break;
                }
                else
                {
                    newPathParts.Add(pathParts[i]);
                }
            }
            pathParts = newPathParts.ToArray();
#if NET472
            var directory = string.Join(Path.DirectorySeparatorChar.ToString(), pathParts.ToArray());
#else
            var directory = Path.Combine(pathParts);
#endif
            var path = Path.Combine(directory, $"{disposalOrderTestProjectName}.csproj");
            if (!Path.IsPathRooted(path))
            {
                path = $"{Path.DirectorySeparatorChar}{path}";
            }
            return path;
        }

        [Fact]
        public static void MultipleDisposeExceptionMessageInternationalizationTest()
        {
            var testDisposable = new TestDisposable();
            testDisposable.Dispose();

            // Test English (default) culture
            var originalCulture = CultureInfo.CurrentUICulture;
            try
            {
                CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");
                var englishException = Assert.Throws<ObjectDisposedException>(() => testDisposable.Dispose());
                Assert.Contains("Multiple dispose calls are not allowed", englishException.Message);

                // Test Russian culture
                CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("ru-RU");
                var russianException = Assert.Throws<ObjectDisposedException>(() => testDisposable.Dispose());
                Assert.Contains("Множественные вызовы Dispose не разрешены", russianException.Message);
            }
            finally
            {
                CultureInfo.CurrentUICulture = originalCulture;
            }
        }

        private class TestDisposable : DisposableBase
        {
            protected override void Dispose(bool manual, bool wasDisposed)
            {
                // Empty implementation for testing
            }
        }
    }
}
