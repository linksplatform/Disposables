namespace Platform::Disposables::Tests::DisposalOrderTest
{
    class Program
    {
        static void Main(std::string args[])
        {
            if (args.Length == 0)
            {
                args = new[] { "the.log", "false" };
            }
            if (args.Length == 1)
            {
                args = new[] { args[0], "false" };
            }
            auto logPath = args[0];
            auto waitForCancellation = bool.Parse(args[1]);
            using auto consoleCancellationHandler = ConsoleCancellation();
            auto disposable1 = Disposable([&]()-> auto { return { File.AppendAllText(logPath, "1"); } });
            auto disposable2 = Disposable([&]()-> auto { return { File.AppendAllText(logPath, "2"); } });
            Console.WriteLine(disposable1.IsDisposed && disposable2.IsDisposed);
            if (waitForCancellation)
            {
                consoleCancellationHandler.Wait();
            }
        }
    };
}
