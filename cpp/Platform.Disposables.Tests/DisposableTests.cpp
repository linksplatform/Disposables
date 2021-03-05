namespace Platform::Disposables::Tests
{
    TEST_CLASS(DisposableTests)
    {
        public: TEST_METHOD(DisposalOrderTest)
        {
            auto logPath = Path.GetTempFileName();
            using (auto process = Process.Start(CreateProcessStartInfo(logPath, waitForCancellation: false)))
            {
                process.WaitForExit();
            }
            auto result = File.ReadAllText(logPath);
            Assert::AreEqual("21", result);
            File.Delete(logPath);
        }

        public: TEST_METHOD(DisposalAtProcessKillTest)
        {
            auto logPath = Path.GetTempFileName();
            using (auto process = Process.Start(CreateProcessStartInfo(logPath, waitForCancellation: true)))
            {
                Thread.Sleep(1000);
                process.Kill();
            }
            auto result = File.ReadAllText(logPath);
            Assert::AreEqual("", result);
            File.Delete(logPath);
        }

        private: static ProcessStartInfo CreateProcessStartInfo(std::string logPath, bool waitForCancellation)
        {
            auto projectPath = GetDisposalObjectTestProjectFilePath();
            return ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = std::string("run -p \"").append(Platform::Converters::To<std::string>(projectPath)).append("\" -f net5 \"").append(logPath).append("\" {Platform::Converters::To<std::string>(waitForCancellation).data()}"),
                UseShellExecute = false,
                CreateNoWindow = true
            };
        }

        private: static std::string GetDisposalObjectTestProjectFilePath()
        {
            inline static const std::string currentProjectName = "Platform" + "." + "Disposables" + "." + "Tests";
            inline static const std::string disposalOrderTestProjectName = currentProjectName + "." + "DisposalOrderTest";
            auto currentDirectory = Environment.CurrentDirectory;
            auto pathParts = currentDirectory.Split(Path.DirectorySeparatorChar);
            auto newPathParts = List<std::string>();
            for (auto i = 0; i < pathParts.Length; i++)
            {
                if (std::string.Equals(pathParts[i], currentProjectName))
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
            auto directory = std::string.Join(Path.Platform::Converters::To<std::string>(DirectorySeparatorChar).data(), pathParts.ToArray());
#else
            auto directory = Path.Combine(pathParts);
#endif
            auto path = Path.Combine(directory, std::string("").append(disposalOrderTestProjectName).append(".csproj"));
            if (!Path.IsPathRooted(path))
            {
                path = std::string("{Path.DirectorySeparatorChar}").append(Platform::Converters::To<std::string>(path)).append("");
            }
            return path;
        }
    };
}
