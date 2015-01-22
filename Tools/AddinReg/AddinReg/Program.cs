using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.IO;
using System.Xml;

namespace AddinReg
{
    class Program
    {
        static void Main(string[] args)
        {
            bool clear, register = false, unregister = false, proxy = false;
            string appName = "", addinShortName = "", addinDllPath = "", 
                proxyDllPath = "", proxyGuid = "", proxyName = "", proxyVersion = "";

            try
            {
                if (args.Count() < 2 || args.Count() > 10)
                    throw new InvalidOperationException();
                appName = args[0];
                clear = args[1].Equals("-c", StringComparison.InvariantCultureIgnoreCase);
                int paramIndex = clear ? 2 : 1;
                if (!clear && args.Count() == 2)
                    throw new InvalidOperationException();
                if (args.Count() > 2)
                {
                    if (args[paramIndex].Equals("-register", StringComparison.InvariantCultureIgnoreCase))
                        register = true;
                    else if (args[paramIndex].Equals("-unregister", StringComparison.InvariantCultureIgnoreCase))
                        unregister = true;
                    else
                        throw new InvalidOperationException();
                    paramIndex++;
                    proxy = args[paramIndex].Equals("-p", StringComparison.InvariantCultureIgnoreCase);
                    if (proxy)
                    {
                        ++paramIndex;
                        if (args.Count() != paramIndex + 6)
                            throw new InvalidOperationException();
                        addinShortName = args[paramIndex];
                        proxyName = args[paramIndex + 1];
                        proxyGuid = args[paramIndex + 2];
                        proxyVersion = args[paramIndex + 3];
                        addinDllPath = args[paramIndex + 4];
                        proxyDllPath = args[paramIndex + 5];
                    }
                    else
                    {
                        if (args.Count() != paramIndex + 2)
                            throw new InvalidOperationException();
                        addinShortName = args[paramIndex];
                        addinDllPath = args[paramIndex + 1];
                    }
                }
            }
            catch(InvalidOperationException)
            {
                PrintUsage();
                Environment.Exit(-1);
                return;
            }
            try
            {
                if (clear)
                    ClearRegisteredAddIns(appName);
                if (unregister)
                    UnregisterAddin(appName, addinShortName);
                if (register && !proxy)
                    RegisterSignedAddin(appName, addinShortName, addinDllPath);
                if (register && proxy)
                    RegisterProxyAddin(appName, addinShortName, addinDllPath, proxyDllPath, proxyName, proxyGuid, proxyVersion);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.ToString());
                Environment.Exit(-2);
            }
        }

        private static void RegisterProxyAddin(string appName, string addinShortName, string addinDllPath, string proxyDllPath, 
            string proxyName, string proxyGuid, string proxyVersion)
        {
            //change the config file
            if (!File.Exists(proxyDllPath))
                throw new Exception("Proxy add-in is not existed");
            string configFilePath = proxyDllPath + ".config";
            if (!File.Exists(configFilePath))
                throw new Exception("Proxy add-in config file is not existed");
            var xml = new XmlDocument();
            xml.Load(configFilePath);
            var nodes = xml.DocumentElement.SelectSingleNode("/configuration/appSettings");
            foreach (XmlNode node in nodes)
            {
                if (node.Attributes["key"].Value.Equals("AddinName", StringComparison.InvariantCultureIgnoreCase))
                    node.Attributes["value"].Value = proxyName;
                else if (node.Attributes["key"].Value.Equals("AddinShortName", StringComparison.InvariantCultureIgnoreCase))
                    node.Attributes["value"].Value = addinShortName;
                else if (node.Attributes["key"].Value.Equals("AddinGuid", StringComparison.InvariantCultureIgnoreCase))
                    node.Attributes["value"].Value = proxyGuid.ToUpper();
                else if (node.Attributes["key"].Value.Equals("AddinVersion", StringComparison.InvariantCultureIgnoreCase))
                    node.Attributes["value"].Value = proxyVersion;
                else if (node.Attributes["key"].Value.Equals("AddinDLL", StringComparison.InvariantCultureIgnoreCase))
                    node.Attributes["value"].Value = addinDllPath;
            }
            //File.Delete(configFilePath);
            xml.Save(configFilePath);
            RegisterSignedAddin(appName, addinShortName, proxyDllPath);
        }

        private static void RegisterSignedAddin(string appName, string addinName, string addinDllPath)
        {
            var key = Registry.CurrentUser.CreateSubKey(string.Format("Software\\CCH\\{0}\\Addins\\{1}", appName, addinName));
            key.SetValue("FileName", addinDllPath);
        }

        private static void UnregisterAddin(string appName, string addinName)
        {
            var key = Registry.CurrentUser.OpenSubKey(string.Format("Software\\CCH\\{0}\\Addins", appName), true);
            if ( key==null || key.OpenSubKey(addinName) == null )
            {
                Console.WriteLine("Add-in is not registered");
            }
            else
            {
                key.DeleteSubKeyTree(addinName);
                Console.WriteLine("Add-in has been unregistered successfully");
            }
        }

        private static void ClearRegisteredAddIns(string appName)
        {
            var key = Registry.CurrentUser.OpenSubKey(string.Format("Software\\CCH\\{0}\\Addins", appName), true);
            if (key != null)
            {
                foreach (var addinkey in key.GetSubKeyNames())
                {
                    key.DeleteSubKeyTree(addinkey);
                }
            }
            Console.WriteLine("Cleaned up successully");
        }

        private static void PrintUsage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("  For singed add-ins:");
            Console.WriteLine("    AddinReg.exe <Application> [-c] -register|-unregister AddinShortName AddinDllPath");
            Console.WriteLine("  For proxy add-ins:");
            Console.WriteLine("    AddinReg.exe <Application> [-c] -register|-unregister -p AddinShortName AddinName AddinGuid AddinVersion AddinDllPath AddinProxyDllPath");
            Console.WriteLine("  -c : clear the list of registered add-ins");
            Console.WriteLine("  Application:");
            Console.WriteLine("    \"T1 Taxprep 2014\"");
            Console.WriteLine("    \"T2 Taxprep 2014-1\"");
            Console.WriteLine("    ...");
            Console.WriteLine("    \"Taxprep Forms 2014\"");
        }
    }
}
