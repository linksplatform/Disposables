using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Xunit;

namespace Platform.Disposables.Tests
{
    public static class DisposableTests
    {
        [Fact]
        public static void DisposalOrderTest()
        {
            string path = GetDisposalObjectTestProjectFilePath();
            var logPath = Path.GetTempFileName();
            using (var process = Process.Start("dotnet", $"run -p \"{path}\" \"{logPath}\" false"))
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
            string path = GetDisposalObjectTestProjectFilePath();
            var logPath = Path.GetTempFileName();
            using (var process = Process.Start("dotnet", $"run -p \"{path}\" \"{logPath}\" true"))
            {
                Thread.Sleep(1000);
                process.Kill();
            }
            var result = File.ReadAllText(logPath);
            Assert.Equal("", result); // Currently process termination will not release resources
            File.Delete(logPath);
        }

        private static string GetDisposalObjectTestProjectFilePath()
        {
            const string currentProjectName = "Platform.Disposables.Tests";
            const string disposalOrderTestProjectName = "Platform.Disposables.Tests.DisposalOrderTest";
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
            var path = Path.Combine(Path.Combine(pathParts), $"{disposalOrderTestProjectName}.csproj");
            if (!Path.IsPathRooted(path))
            {
                path = $"{Path.PathSeparator}{path}";
            }
            return path;
        }
    }
}
