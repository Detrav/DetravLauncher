pushd src_v2
  dotnet restore
  dotnet publish Detrav.Launcher.Uploader/Detrav.Launcher.Uploader.csproj ^
                 --configuration Release ^
                 --self-contained ^
                 --runtime win-x64
  dotnet run --project "../tools/zip_packer/zip_packer.csproj" ^
             -- ^
             "%~dp0src_v2/Detrav.Launcher.Uploader/bin/Release/net6.0/win-x64/publish" ^
             "%~dp0src_v2/Detrav.Launcher.Server/wwwroot/files/uploader.zip"
  dotnet publish Detrav.Launcher.Client.Launhcer/Detrav.Launcher.Client.Launhcer.csproj ^
                 --configuration Release ^
                 --self-contained ^
                 --runtime win-x64
  dotnet run --project "../tools/zip_packer/zip_packer.csproj" ^
             -- ^
             "%~dp0src_v2/Detrav.Launcher.Client.Launhcer/bin/Release/net6.0-windows/win-x64/publish" ^
             "%~dp0src_v2/Detrav.Launcher.Server/wwwroot/files/launcher.zip"
  dotnet publish Detrav.Launcher.Client/Detrav.Launcher.Client.csproj ^
                 --configuration Release ^
                 --self-contained ^
                 --runtime win-x64
  dotnet run --project "../tools/zip_packer/zip_packer.csproj" ^
             -- ^
             "%~dp0src_v2/Detrav.Launcher.Client/bin/Release/net6.0-windows/win-x64/publish" ^
             "%~dp0src_v2/Detrav.Launcher.Server/wwwroot/files/app.zip"
popd