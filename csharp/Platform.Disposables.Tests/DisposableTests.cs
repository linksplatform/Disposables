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
        public static void AllowMultipleDisposeCallsConstructorTest()
        {
            var disposeCount = 0;
            Action incrementCount = () => disposeCount++;

            var disposable = new Disposable(incrementCount, allowMultipleDisposeCalls: true);
            
            disposable.Dispose();
            disposable.Dispose();
            disposable.Dispose();
            
            Assert.Equal(1, disposeCount);
        }

        [Fact]
        public static void AllowMultipleDisposeAttemptsConstructorTest()
        {
            var disposeCount = 0;
            Disposal incrementCount = (manual, wasDisposed) =>
            {
                if (!wasDisposed) disposeCount++;
            };

            var disposable = new Disposable(incrementCount, allowMultipleDisposeCalls: true, allowMultipleDisposeAttempts: true);
            
            disposable.Dispose();
            disposable.Dispose();
            
            Assert.Equal(1, disposeCount);
        }

        [Fact]
        public static void ParameterlessConstructorWithAllowMultipleDisposeCallsTest()
        {
            var disposable = new Disposable(allowMultipleDisposeCalls: true);
            
            disposable.Dispose();
            disposable.Dispose();
        }

        [Fact]
        public static void ParameterlessConstructorWithAllowMultipleDisposeAttemptsTest()
        {
            var disposable = new Disposable(allowMultipleDisposeCalls: true, allowMultipleDisposeAttempts: true);
            
            disposable.Dispose();
            disposable.Dispose();
        }

        [Fact]
        public static void DefaultParameterlessConstructorTest()
        {
            var disposable = new Disposable();
            
            disposable.Dispose();
            
            Assert.Throws<ObjectDisposedException>(() => disposable.Dispose());
        }

        [Fact]
        public static void DefaultActionConstructorTest()
        {
            var disposeCount = 0;
            Action incrementCount = () => disposeCount++;

            var disposable = new Disposable(incrementCount);
            
            disposable.Dispose();
            
            Assert.Equal(1, disposeCount);
            Assert.Throws<ObjectDisposedException>(() => disposable.Dispose());
        }

        [Fact]
        public static void DefaultDisposalConstructorTest()
        {
            var disposeCount = 0;
            Disposal incrementCount = (manual, wasDisposed) =>
            {
                if (!wasDisposed) disposeCount++;
            };

            var disposable = new Disposable(incrementCount);
            
            disposable.Dispose();
            
            Assert.Equal(1, disposeCount);
            Assert.Throws<ObjectDisposedException>(() => disposable.Dispose());
        }

        [Fact]
        public static void ReplacesWorkaroundFromIssueTest()
        {
            var disposeCount = 0;
            Disposal incrementCount = (manual, wasDisposed) =>
            {
                if (!wasDisposed) disposeCount++;
            };

            var disposableWithNewConstructor = new Disposable(incrementCount, allowMultipleDisposeCalls: true);
            
            disposableWithNewConstructor.Dispose();
            disposableWithNewConstructor.Dispose();
            disposableWithNewConstructor.Dispose();
            
            Assert.Equal(1, disposeCount);
        }
    }
}
