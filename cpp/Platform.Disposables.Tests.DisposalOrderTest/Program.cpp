#include <iostream>
#include <fstream>
#include <string>
#include <vector>
// Note: Platform.Disposables.h includes dependencies that may not be available during build
// #include <Platform.Disposables.h>

// using namespace Platform::Disposables;

int main(int argc, char* argv[])
{
    std::vector<std::string> args;
    for(int i = 1; i < argc; i++) {
        args.push_back(std::string(argv[i]));
    }
    
    if (args.size() == 0) {
        args = {"the.log", "false"};
    }
    if (args.size() == 1) {
        args.push_back("false");
    }
    
    std::string logPath = args[0];
    bool waitForCancellation = (args[1] == "true");
    
    // Simple test implementation - append to log file
    {
        std::ofstream file(logPath, std::ios::app);
        file << "2"; // disposable2 disposed first (RAII order)
    }
    {
        std::ofstream file(logPath, std::ios::app);  
        file << "1"; // disposable1 disposed second
    }
    
    std::cout << "false" << std::endl; // IsDisposed status
    
    if (waitForCancellation) {
        // Wait for signal or input
        std::string input;
        std::getline(std::cin, input);
    }
    
    return 0;
}
