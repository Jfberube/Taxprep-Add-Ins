using System;
using System.Reflection;
using System.Runtime.InteropServices;
using RGiesecke.DllExport;
using TaxprepAddinAPI;

namespace VSUnitTestAddin
{
    [AddinInstance]
    public class AddinInstance : TxpAddinLibrary.AddinInstanceBase
    {
        public override void ReleaseApp()
        {
            base.ReleaseApp();
            // do nothing
        }

        public AddinInstance()
            : base(new Guid("EEAC0C48-F6B1-4865-B60A-8304F6F10ABF"), "C# Unit Test Add-in", "1.0.0.0")
        {
            
        }

        public override void InitializeTaxPrepAddin()
        {
            try
            {
                var lMenuService = (IAppMenuService)_appInstance;

                var lRootMenu = lMenuService.AddRootMenu("Add-in Unit Test");
                lRootMenu.Visible = true;
                lRootMenu.Enabled = true;

                MenuTests.InitTestEvents(lRootMenu, true);
                ModuleManagerTests.InitTestEvents(_appInstance);
                TaxApplicationTests.InitTestEvents(_appInstance);

                UnitTestHost.TestHost.AddTestMenu(_appInstance, lRootMenu, true);
            }
            catch (Exception)
            {
                ReleaseApp();
                throw;
            }
        }
    }
}
