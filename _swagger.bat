pushd tools
  dotnet tool restore
  dotnet build "../src/DetravLauncher.Server/DetravLauncher.Server.csproj" -c Release
  dotnet swagger tofile --output "swagger.json" "../src/DetravLauncher.Server/bin/Release/net6.0/DetravLauncher.Server.dll" v1
  dotnet nswag swagger2csclient /input:swagger.json  /classname:DetravLauncherClient /namespace:DetravLauncher /output:../src/DetravLauncher/DetravLauncherClient.cs
  dotnet nswag swagger2csclient /input:swagger.json  /classname:DetravLauncherClient /namespace:DetravLauncher /output:../src/DetravLauncher.Uploader/DetravLauncherClient.cs
popd