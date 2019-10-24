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
            var path = GetDisposalObjectTestProjectFilePath();
            var logPath = Path.GetTempFileName();
            var processStartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"run -p \"{path}\" -f netcoreapp2.1 \"{logPath}\" false",
                UseShellExecute = false,
                //RedirectStandardOutput = true,
                CreateNoWindow = true
            };
            using (var process = Process.Start(processStartInfo))
            {
                //string line = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
            }
            var result = File.ReadAllText(logPath);
            Assert.Equal("21", result);
            File.Delete(logPath);
        }

        [Fact]
        public static void DisposalAtProcessKillTest()
        {
            var path = GetDisposalObjectTestProjectFilePath();
            var logPath = Path.GetTempFileName();
            var processStartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"run -p \"{path}\" -f netcoreapp2.1 \"{logPath}\" true",
                UseShellExecute = false,
                //RedirectStandardOutput = true,
                CreateNoWindow = true
            };
            using (var process = Process.Start(processStartInfo))
            {
                //string line = process.StandardOutput.ReadToEnd();
                Thread.Sleep(1000);
                process.Kill();
            }
            var result = File.ReadAllText(logPath);
            Assert.Equal("", result); // Currently, process termination will not release resources
            File.Delete(logPath);
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

#if NET471
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
    }
}
