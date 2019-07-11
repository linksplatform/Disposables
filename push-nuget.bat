dotnet pack -c Release
cd bin\Release
nuget push *.symbols.nupkg
del *.symbols.nupkg
nuget push *.nupkg
del *.nupkg
