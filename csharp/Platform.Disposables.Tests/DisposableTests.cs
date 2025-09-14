using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                Arguments = $"run -p \"{projectPath}\" -f net7 \"{logPath}\" {waitForCancellation.ToString()}",
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
        public static void WrapSystemIDisposableInDisposableTest()
        {
            var disposed = false;
            var testDisposable = new TestDisposable(() => disposed = true);
            
            // Test creating wrapper from System.IDisposable using Create method
            var wrapper = Disposable.Create(testDisposable);
            Assert.NotNull(wrapper);
            Assert.False(disposed);
            
            wrapper.Dispose();
            Assert.True(disposed);
        }

        [Fact]
        public static void WrapSystemIDisposableInGenericDisposableTest()
        {
            var disposed = false;
            var testDisposable = new TestDisposable(() => disposed = true);
            
            // Test creating generic wrapper from System.IDisposable using Create method
            var wrapper = Disposable<System.IDisposable>.Create(testDisposable);
            Assert.NotNull(wrapper);
            Assert.Same(testDisposable, wrapper.Object);
            Assert.False(disposed);
            
            wrapper.Dispose();
            Assert.True(disposed);
        }

        [Fact]
        public static void WrapTwoSystemIDisposableInDisposableTest()
        {
            var disposed1 = false;
            var disposed2 = false;
            var testDisposable1 = new TestDisposable(() => disposed1 = true);
            var testDisposable2 = new TestDisposable(() => disposed2 = true);
            
            // Test creating wrapper for two System.IDisposable objects using Create method
            var wrapper = Disposable<System.IDisposable, System.IDisposable>.Create((testDisposable1, testDisposable2));
            Assert.NotNull(wrapper);
            Assert.Same(testDisposable1, wrapper.Object);
            Assert.Same(testDisposable2, wrapper.AuxiliaryObject);
            Assert.False(disposed1);
            Assert.False(disposed2);
            
            wrapper.Dispose();
            Assert.True(disposed1);
            Assert.True(disposed2);
        }

        [Fact]
        public static void AsDisposableExtensionMethodTest()
        {
            var disposed = false;
            var testDisposable = new TestDisposable(() => disposed = true);
            
            // Test extension method that wraps System.IDisposable in Disposable
            var wrapper = testDisposable.AsDisposable();
            Assert.NotNull(wrapper);
            Assert.False(disposed);
            
            wrapper.Dispose();
            Assert.True(disposed);
        }

        [Fact]
        public static void AsDisposableContainerExtensionMethodTest()
        {
            var disposed = false;
            var testDisposable = new TestDisposable(() => disposed = true);
            
            // Test extension method that wraps System.IDisposable in Disposable<System.IDisposable>
            var wrapper = testDisposable.AsDisposableContainer();
            Assert.NotNull(wrapper);
            Assert.Same(testDisposable, wrapper.Object);
            Assert.False(disposed);
            
            wrapper.Dispose();
            Assert.True(disposed);
        }

        private class TestDisposable : System.IDisposable
        {
            private readonly Action _disposeAction;
            private bool _disposed = false;

            public TestDisposable(Action disposeAction)
            {
                _disposeAction = disposeAction ?? throw new ArgumentNullException(nameof(disposeAction));
            }

            public void Dispose()
            {
                if (!_disposed)
                {
                    _disposed = true;
                    _disposeAction();
                }
            }
        }
    }
}
