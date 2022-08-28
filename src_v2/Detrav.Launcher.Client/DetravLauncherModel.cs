using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Detrav.Launcher.Client
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ComVisible(true)]
    public class DetravLauncherModel
    {
        public void TestMethod(object value)
        {
            MessageBox.Show("TryToInstall " + (value?.ToString() ?? "----"));
        }
    }
}
