
using System.IO.Compression;

var folder = args[0];
var target = args[1];

if (File.Exists(target))
    File.Delete(target);

var fullpath = Path.GetFullPath(target);
var dir = Path.GetDirectoryName(fullpath);
if (!Directory.Exists(dir))
    Directory.CreateDirectory(dir!);

ZipFile.CreateFromDirectory(folder, target);
