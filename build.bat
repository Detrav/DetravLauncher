pushd src_v2
  dotnet restore
  dotnet publish Detrav.Launcher.Uploader/Detrav.Launcher.Uploader.csproj ^
                 --configuration Release ^
                 --self-contained ^
                 --runtime win-x64

dotnet run --project "../tools/zip_packer/zip_packer.csproj" "%~dp0src_v2/Detrav.Launcher.Uploader/bin/Release/net6.0/win-x64/publish" "%~dp0file.zip"
popd