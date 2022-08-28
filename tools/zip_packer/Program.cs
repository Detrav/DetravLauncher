
using System.IO.Compression;

var folder = args[0];
var target = args[1];

ZipFile.CreateFromDirectory(folder, target);
