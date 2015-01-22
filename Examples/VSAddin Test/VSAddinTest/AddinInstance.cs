// uncomment this to check the drag and drop exceptions handling
//#define EmulateDragAndDropError

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using RGiesecke.DllExport;
using TaxprepAddinAPI;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;
using TxpAddinLibrary.Handlers;
using TxpAddinLibrary;

namespace VSAddinTest
{

    [AddinInstance]
    public class AddinInstance : TxpAddinLibrary.AddinInstanceBase
    {
        private IAppMenuItem _testMenuItem;
        private IAppSubMenu _testSubMenu;
        private IAppMenuItem _debugOutputMenuItem;
        private bool _showDebugOutput;

        public override void ReleaseApp()
        {
            base.ReleaseApp();
            // do nothing
        }

        #region Event handlers

        private void ShowDebugMessage(string aMessage)
        {
            Debug.WriteLine(aMessage);
            if (_showDebugOutput)
                MessageBox.Show(aMessage);
        }

        private void ShowDebugMessage(string aFormat, params object[] aArgs) { ShowDebugMessage(string.Format(aFormat, aArgs)); }

        private void DoHelloWorld()
        {
            MessageBox.Show("Sub menu!");            
        }

        private void DoMenuItemClick()
        {
            IAppClientFileManagerService aManager = (IAppClientFileManagerService)_appInstance;
            IAppClientFile aFile = aManager.GetCurrentClientFile();
            MessageBox.Show(aFile.GetCurrentReturn().GetReturnPath());
        }

        private void DoAppProperties()
        {
            IAppTaxApplicationService aApplication = (IAppTaxApplicationService)_appInstance;
            AppProperiesForm.ShowProperties(aApplication);
        }

        private void DoClientFileProperties()
        {
            IAppClientFileManagerService aManager = (IAppClientFileManagerService)_appInstance;
            IAppClientFile aFile = aManager.GetCurrentClientFile();
            ClientFileProperties.ShowProperties(aFile);
        }

        private void DoAfterClientFileSave(string aFileName)
        {
            Debug.WriteLine("ClientFileSaveHandler {0}", aFileName);
        }

        private void DoAfterClientFileNameChange(string aFileName)
        {
            Debug.WriteLine("AfterClientFileNameChange {0}", aFileName);
        }

        private void DoAfterChangeClientFileHeaderProperty(string aClientFileHeaderKey)
        {
            Debug.WriteLine("AfterChangeClientFileHeaderProperty {0}", aClientFileHeaderKey);
        }

        private void DoAfterCurrentDocumentChange(IAppTaxDocument aDocument)
        {
            Debug.WriteLine("AfterCurrentDocumentChange");
        }

        private void DoAfterDocumentAdd(IAppTaxDocument aDocument)
        {
            Debug.WriteLine("AfterDocumentAdd");
        }

        private void DoAfterDocumentRemove(IAppTaxDocument aDocument)
        {
            Debug.WriteLine("AfterDocumentRemove");
        }

        private void DoBeforeClientFileNameChange(string aOldFileName, string aNewFileName)
        {
            Debug.WriteLine("BeforeClientFileNameChange AOldFilename = {0}, ANewFileName = {1}",
                 aOldFileName, aNewFileName);
        }

        private void DoBeforeClientFileSave(string aFileName, out bool aAccept)
        {
            Debug.WriteLine("BeforeClientFileSave {0}", aFileName);
            aAccept = true;
        }

        private void DoCurrentCell()
        {
            var LTaxpApp = (IAppTaxApplicationService)_appInstance;
            var LCell = LTaxpApp.GetCurrentTaxCell();
            if (LCell == null)
            {
                MessageBox.Show("Current cell is not assigned");
                return;
            }
            CellPropertieForm.ShowCellProperties(LCell, LTaxpApp);
        }

        private void DoReturnStatuses()
        {
            var LTaxApp = (IAppTaxApplicationService)_appInstance;
            var LReturn = LTaxApp.GetCurrentDocReturn();
            ReturnProperties.ShowReturnproperties(LReturn);
        }

        private void DoBeforeCurrentDocumentChange(IAppTaxDocument OldDocument, IAppTaxDocument NewDocument)
        {
            Debug.WriteLine("BeforeCurrentDocumentChange");
        }

        private void DoBeforeDocumentAdd(IAppTaxDocument aDocument)
        {
            Debug.WriteLine("BeforeDocumentAdd");
        }

        private void DoBeforeDocumentRemove(IAppTaxDocument aDocument)
        {
            Debug.WriteLine("BeforeDocumentRemove");
        }

        private void DoBeforeReturnStatusChange(IAppDocReturn aDocReturn, string AStatusName, int aOldValue, int AValue)
        {
           Debug.WriteLine("Change return status {0} from {1} to {2}", AStatusName, aOldValue, AValue);
        }

        private void DoAfterAcceptUserInput(IAppTaxCell aCell)
        {
            Debug.WriteLine(aCell.GetCellNameWithGroup());
            //CellPropertieForm.ShowCellProperties(aCell);
        }

        private void DoAfterAddRepeat(IAppGroupArray aGroup)
        {
            Debug.WriteLine("DbEnv.AfterAddRepeat");
        }

        private void DoAfterCalc(IAppDocReturn aReturn)
        {
            Debug.WriteLine("DbEnv.AfterCalc");
        }

        private void DoAfterCalcGlobalRates()
        {
            Debug.WriteLine("DbEnv.AfterCalcGlobalRates");
        }

        private void DoAfterDeleteRepeat(IAppGroupArray aGroup)
        {
            Debug.WriteLine("DbEnv.AfterDeleteRepeat");
        }

        private void DoBeforeCalc(IAppDocReturn aReturn)
        {
            Debug.WriteLine("DbEnv.BeforeCalc");
        }

        private void DoTriggerAddinException()
        {
            throw new Exception("Addin-triggered exception");
        }

        private void DoTriggerAppException()
        {
            var lTesting = (IAppTesting)_appInstance;
            lTesting.TestRaiseError("Test message 1");
        }

#if (EmulateDragAndDropError)
        string sOld;
#endif
        private dynamic DoGetUnicodeTextDragDropData(IAppCellSelectionIter aSelection)
        {
            var lText = "[";
            aSelection.First();
            while (!aSelection.IsDone())
            {
#if (EmulateDragAndDropError)
                if (!string.IsNullOrEmpty(sOld) && sOld != aSelection.Current().GetFieldCellPath(false))
                {
                    throw new Exception("bad field");
                }
                sOld = aSelection.Current().GetFieldCellPath(false);
#endif
                lText += '"' + aSelection.Current().GetFieldCellPath(false) +  '"';
                aSelection.Next();
                if (!aSelection.IsDone())
                    lText += ',';
            }
            lText += ']';

            lText = '{' + string.Format("name:\"VSAddinTest\",cells:{0}", lText) + "}\0";

            return System.Text.Encoding.Unicode.GetBytes(lText);
        }

        private void DoQueryComplexData()
        {
            var LApplication = (IAppTaxApplicationService)_appInstance;
            using (var form = new VSAddinTest.UI.QueryComplexDataForm())
            {
                form.LoadData(LApplication.GetCurrentTaxData());
                form.ShowDialog();
            }
        }

        private void DoCurrentModuleInfo()
        {
            var lMsg = "(none)";
            var lModule = ((IAppModuleManager) _appInstance).CurrentModule;
            if (lModule != null)
                lMsg = string.Format("Name: '{0}'\r\nTitleName: '{1}'\r\nTitleButtonName: '{2}'", 
                    lModule.Name, lModule.TitleName, lModule.TitleButtonName);

            MessageBox.Show(lMsg);
        }

        private void DoShowDebugOutputClick()
        {
            _showDebugOutput = !_showDebugOutput;
            _debugOutputMenuItem.Caption = _showDebugOutput ? "Hide Debug Output" : "Show Debug Output";
        }

        private void DoShowUnlockCodeEfile()
        {
            var LApp = (IAppTaxApplicationService)_appInstance;
            var LConfig = LApp.Configuration;
            int LEfileType = LConfig.AsInteger("Application", "EFILEType", 0);
            var LEFileUnlock = LApp.GetStringWithLanguage("\\Unlock\\EFILE\\", LEfileType.ToString(), LApp.GetDefaultLanguage());
            MessageBox.Show(string.IsNullOrEmpty(LEFileUnlock) ? "No efile unlock code assigned" : LEFileUnlock);
        }

        private void DoShowUnlockKey()
        {
            var LApp = (IAppTaxApplicationService)_appInstance;
            var LConfig = LApp.Configuration;
            var LKey = LConfig.AsString("Application", "UnlockKey", "");
            MessageBox.Show(string.Format("{0}\n The code is copied to clipboard", string.IsNullOrEmpty(LKey) ? "No unlock key assigned" : LKey));
            Clipboard.SetText(LKey);
        }

        private void DoShowUniqueID()
        {
            var LApp = (IAppTaxApplicationService)_appInstance;
            var LKey = LApp.GetMachineIdentifier();
            MessageBox.Show(string.Format("{0}\n The Unique ID is copied to clipboard", string.IsNullOrEmpty(LKey) ? "No unlock key assigned" : LKey));
            Clipboard.SetText(LKey);
        }

        private void DoShowUnlockCodeNetwork()
        {
            var LApp = (IAppTaxApplicationService)_appInstance;
            var LConfig = LApp.Configuration;
            int LNetworkType = LConfig.AsInteger("Application", "NetworkType", 0);
            string networkOption = LApp.GetStringWithLanguage("\\Unlock\\Network", LNetworkType.ToString(), LApp.GetDefaultLanguage());
            MessageBox.Show(string.IsNullOrEmpty(networkOption) ? "No network unlock code assigned" : networkOption);
        }

        private void DoAddConfigurationSection(string aSectionName)
        {
            ShowDebugMessage("Add the configuration section {0}", aSectionName);
        }

        private void DoRemoveConfigurationSection(string aSectionName)
        {
            ShowDebugMessage("Remove the configuration section {0}", aSectionName);
        }

        private void DoModifyKey(string aLevel, string aSection, string aKey, string AValue)
        {
            ShowDebugMessage("Modify the configuration key Level: {3}, Secion: {0}, Key: {1}, NewValue: {2}", 
                aSection, aKey, AValue, aLevel);
        }

        private void DoConfigurationDemo()
        {
            var LApp = (IAppTaxApplicationService)_appInstance;
            var LConfig = LApp.Configuration;
            VSAddinTest.UI.ConfigurationDemo.ShowConfiguration(LConfig);
        }

        private void DoDiagDemo()
        {
            var LApp = (IAppTaxApplicationService)_appInstance;
            var LDiag = LApp.GetClientFileManager().GetCurrentClientFile().GetCurrentReturn().GetDiagnostic();
            VSAddinTest.UI.Diagnotics.ShowDiagnostic(LDiag);
        }

        private void DoUFLDemo()
        {
            var LApp = (IAppTaxApplicationService)_appInstance;
            var LUFL = LApp.UFL;
            var LCurrentReturn = LApp.GetCurrentDocReturn();
            VSAddinTest.UI.UFLDemo.ShowUFL(LUFL, LCurrentReturn);
        }

        private void DoClientLettersDemo()
        {
            var LApp = (IAppTaxApplicationService)_appInstance;
            var LCLManager = LApp.ClientLetterManager;
            var LUFL = LApp.UFL;
            var LCurrentReturn = LApp.GetCurrentDocReturn();
            VSAddinTest.UI.ClientLettersDemo.ShowDemo(LCLManager, LCurrentReturn, LUFL);
        }

        private void DoCursorDemo()
        {
            var LApp = (IAppTaxApplicationService)_appInstance;
            VSAddinTest.UI.CursorDemo.Display(LApp);
        }

        private void DoBaundsDemo()
        {
            int x, y, width, heigh;
            uint handle;
            var app = (IAppTaxApplicationService) _appInstance;
            handle = app.getMainFormHandle();
            app.getMainMainFormsBaunds(out x, out y, out width, out heigh);
            MessageBox.Show(string.Format("Baunds x:{0}, y:{1}, width{2}, height{3}\nHandle {4}", x, y, width, heigh, handle));
        }

        private void DoSendToSignSafe()
        {
            MessageBox.Show("Hello world!");
        }

        private void DoToggleVsAddinAdvertisementOnStartup()
        {
            var lApp = (IAppTaxApplicationService)_appInstance;
            var lValue = lApp.Configuration.AsBoolean("Application", "AdvertisingShowAtStartUp_VsAddinTest", true);
            lApp.Configuration.SetBoolean("Application", "AdvertisingShowAtStartUp_VsAddinTest", !lValue);
            lApp.RefreshAdvertisingMenu();
        }

        private void DoToggleVsAddinAdvertisement()
        {
            var lApp = (IAppTaxApplicationService)_appInstance;
            var lValue = lApp.Configuration.AsBoolean("Application", "AdvertisingActivated_VsAddinTest", true);
            lApp.Configuration.SetBoolean("Application", "AdvertisingActivated_VsAddinTest", !lValue);
            lApp.RefreshAdvertisingMenu();
        }

        private void DoToggleCurrentAdvertisementOnStartup()
        {
            var lApp = (IAppTaxApplicationService)_appInstance;
            var lIdent = lApp.Configuration.AsString("Application", "AdvertisingIdent", "");
            if (string.Equals(lIdent, ""))
            {
                MessageBox.Show("No active ident selected");
            }
            var lValue = lApp.Configuration.AsBoolean("Application", "AdvertisingShowAtStartUp_" + lIdent, true);
            lApp.Configuration.SetBoolean("Application", "AdvertisingShowAtStartUp_" + lIdent, !lValue);
            lApp.RefreshAdvertisingMenu();
        }

        private void DoToggleCurrentAdvertisement()
        {
            var lApp = (IAppTaxApplicationService)_appInstance;
            var lIdent = lApp.Configuration.AsString("Application", "AdvertisingIdent", "");
            if (string.Equals(lIdent, ""))
            {
                MessageBox.Show("No active ident selected");
            }
            var lValue = lApp.Configuration.AsBoolean("Application", "AdvertisingActivated_" + lIdent, true);
            lApp.Configuration.SetBoolean("Application", "AdvertisingActivated_" + lIdent, !lValue);
            lApp.RefreshAdvertisingMenu();
        }

        private void DoToggleSignSafeAdv()
        {
            var lApp = (IAppTaxApplicationService)_appInstance;
            var lValue = lApp.Configuration.AsBoolean("Application", "AdvertisingActivated_SignSafe", true);
            lApp.Configuration.SetBoolean("Application", "AdvertisingActivated_SignSafe", !lValue);
            lApp.RefreshAdvertisingMenu();
        }

        #endregion

        #region Initialization

        private IAppMenuItem CreateMenuItem(IAppSubMenu aParent, string aCaption,
            TxpAddinLibrary.Handlers.AppNotifyHandler.ExecuteDelegate aOnExecute, bool aNewGroup = false)
        {
            var item = aParent.AddItem(aCaption, aNewGroup);
            item.Enabled = true;

            if (aOnExecute != null)
                item.ClickHandler = new AppNotifyHandler(aOnExecute);

            return item;
        }

        private void InitClientFileEvents()
        {
            var ClientFileEvents = (IAppClientFileEventsService)_appInstance;
            if (ClientFileEvents != null)
            {
                ClientFileEvents.AfterClientFileSave = new TxpAddinLibrary.Handlers.ClientFile.AfterSaveHandler(DoAfterClientFileSave);
                ClientFileEvents.AfterClientFileNameChange = new TxpAddinLibrary.Handlers.ClientFile.AfterChangeNameHandler(DoAfterClientFileNameChange);
                ClientFileEvents.AfterChangeClientFileHeaderProperty = new TxpAddinLibrary.Handlers.ClientFile.AfterChangeHeaderPropertyHandler(DoAfterChangeClientFileHeaderProperty);
                ClientFileEvents.AfterCurrentDocumentChange = new TxpAddinLibrary.Handlers.ClientFile.ClientFileNotifyWithDocumentHandler(DoAfterCurrentDocumentChange);
                ClientFileEvents.AfterDocumentAdd = new TxpAddinLibrary.Handlers.ClientFile.ClientFileNotifyWithDocumentHandler(DoAfterDocumentAdd);
                ClientFileEvents.AfterDocumentRemove = new TxpAddinLibrary.Handlers.ClientFile.ClientFileNotifyWithDocumentHandler(DoAfterDocumentRemove);
                ClientFileEvents.BeforeClientFileNameChange = new TxpAddinLibrary.Handlers.ClientFile.BeforeChangeNameHandler(DoBeforeClientFileNameChange);
                ClientFileEvents.BeforeClientFileSave = new TxpAddinLibrary.Handlers.ClientFile.BeforeSaveHandler(DoBeforeClientFileSave);
                ClientFileEvents.BeforeCurrentDocumentChange = new TxpAddinLibrary.Handlers.ClientFile.BeforeCurrentDocumentChangeHandler(DoBeforeCurrentDocumentChange);
                ClientFileEvents.BeforeDocumentAdd = new TxpAddinLibrary.Handlers.ClientFile.ClientFileNotifyWithDocumentHandler(DoBeforeDocumentAdd);
                ClientFileEvents.BeforeDocumentRemove = new TxpAddinLibrary.Handlers.ClientFile.ClientFileNotifyWithDocumentHandler(DoBeforeDocumentRemove);
                ClientFileEvents.BeforeReturnStatusChange = new TxpAddinLibrary.Handlers.ClientFile.BeforeReturnStatusChangeHandler(DoBeforeReturnStatusChange);
            }
            var LApplication = (IAppTaxApplicationService)_appInstance;
            var LConfiguration = LApplication.Configuration;
            LConfiguration.AfterAddSection = new TxpAddinLibrary.Handlers.Configuration.SectionAddRemove(DoAddConfigurationSection);
            LConfiguration.AfterRemoveSection = new TxpAddinLibrary.Handlers.Configuration.SectionAddRemove(DoRemoveConfigurationSection);
            LConfiguration.AfterModifyKey = new TxpAddinLibrary.Handlers.Configuration.KeyModify(DoModifyKey);
        }

        private void InitDatabaseEnvEvents()
        {
            var DBEnv = (IAppDatabaseEnvEventsService)_appInstance;
            if (DBEnv != null)
            {
                DBEnv.AfterAcceptUserInput = new TxpAddinLibrary.Handlers.DatabaseEnv.AfterAcceptUserInput(DoAfterAcceptUserInput);
                DBEnv.AfterAddRepeat = new TxpAddinLibrary.Handlers.DatabaseEnv.NotifyWithGroupArray(DoAfterAddRepeat);
                DBEnv.AfterCalc = new TxpAddinLibrary.Handlers.DatabaseEnv.BeforeCalcHandler(DoAfterCalc);
                DBEnv.AfterCalcGlobalRates = new TxpAddinLibrary.Handlers.AppNotifyHandler(DoAfterCalcGlobalRates);
                DBEnv.AfterDeleteRepeat = new TxpAddinLibrary.Handlers.DatabaseEnv.NotifyWithGroupArray(DoAfterDeleteRepeat);
                DBEnv.BeforeCalc = new TxpAddinLibrary.Handlers.DatabaseEnv.BeforeCalcHandler(DoBeforeCalc);
            }
        }

        private void InitMenu()
        {
            var appMenuService = (IAppMenuService)_appInstance;
            if (appMenuService != null)
            {
                var subMenu = appMenuService.AddRootMenu("Addin Test");
                subMenu.Visible = true;
                //subMenu.PopupHandler = new AppNotifyHandler(DoHelloWorld);

                var item = subMenu.AddItem("Hello world", false);
                item.ClickHandler = new TxpAddinLibrary.Handlers.AppNotifyHandler(DoHelloWorld);
                item.Visible = true;
                item.Enabled = true;

                var item2 = subMenu.AddItem("Current return path", true);
                item2.ClickHandler = new TxpAddinLibrary.Handlers.AppNotifyHandler(DoMenuItemClick);
                item2.Visible = true;
                item2.Enabled = true;

                var item3 = subMenu.AddItem("Application properties", false);
                item3.ClickHandler = new TxpAddinLibrary.Handlers.AppNotifyHandler(DoAppProperties);
                item3.Visible = true;
                item3.Enabled = true;

                var item4 = subMenu.AddItem("Current client file info", false);
                item4.ClickHandler = new TxpAddinLibrary.Handlers.AppNotifyHandler(DoClientFileProperties);
                item4.Enabled = true;
                item4.Visible = true;

                var item5 = subMenu.AddItem("Current cell", false);
                item5.ClickHandler = new TxpAddinLibrary.Handlers.AppNotifyHandler(DoCurrentCell);
                item5.Enabled = true;
                item5.Visible = true;

                CreateMenuItem(subMenu, "Current Module", DoCurrentModuleInfo, false);

                var item6 = subMenu.AddItem("Current return statuses", false);
                item6.ClickHandler = new TxpAddinLibrary.Handlers.AppNotifyHandler(DoReturnStatuses);
                item6.Enabled = true;
                item6.Visible = true;

                var item7 = subMenu.AddItem("Trigger Addin exception", true);
                item7.ClickHandler = new TxpAddinLibrary.Handlers.AppNotifyHandler(DoTriggerAddinException);
                item7.Enabled = true;
                item7.Visible = true;

                var item8 = subMenu.AddItem("Trigger Application exception", false);
                item8.ClickHandler = new TxpAddinLibrary.Handlers.AppNotifyHandler(DoTriggerAppException);
                item8.Enabled = true;
                item8.Visible = true;

                var item9 = subMenu.AddItem("Query complex data", false);
                item9.ClickHandler = new TxpAddinLibrary.Handlers.AppNotifyHandler(DoQueryComplexData);
                item9.Enabled = true;
                item9.Visible = true;

                var itemConfig = subMenu.AddItem("Configuration read demo", false);
                itemConfig.ClickHandler = new TxpAddinLibrary.Handlers.AppNotifyHandler(DoConfigurationDemo);
                itemConfig.Enabled = true;
                itemConfig.Visible = true;

                var itemUnlockCodeNetwork = subMenu.AddItem("Unlock code network", false);
                itemUnlockCodeNetwork.ClickHandler = new TxpAddinLibrary.Handlers.AppNotifyHandler(DoShowUnlockCodeNetwork);
                itemUnlockCodeNetwork.Enabled = true;
                itemUnlockCodeNetwork.Visible = true;

                var itemUnlockCodeEfile = subMenu.AddItem("Unlock code efile", false);
                itemUnlockCodeEfile.ClickHandler = new TxpAddinLibrary.Handlers.AppNotifyHandler(DoShowUnlockCodeEfile);
                itemUnlockCodeEfile.Enabled = true;
                itemUnlockCodeEfile.Visible = true;

                var itemUnlockKey = subMenu.AddItem("Unlock key", false);
                itemUnlockKey.ClickHandler = new TxpAddinLibrary.Handlers.AppNotifyHandler(DoShowUnlockKey);
                itemUnlockKey.Enabled = true;
                itemUnlockKey.Visible = true;

                var itemUniqueId = subMenu.AddItem("Unique ID", false);
                itemUniqueId.ClickHandler = new TxpAddinLibrary.Handlers.AppNotifyHandler(DoShowUniqueID);
                itemUniqueId.Enabled = true;
                itemUniqueId.Visible = true;

                var itemDiag = subMenu.AddItem("Diagnostic demo", false);
                itemDiag.ClickHandler = new TxpAddinLibrary.Handlers.AppNotifyHandler(DoDiagDemo);
                itemDiag.Enabled = true;
                itemDiag.Visible = true;

                var itemUFL = subMenu.AddItem("UFL demo", false);
                itemUFL.ClickHandler = new TxpAddinLibrary.Handlers.AppNotifyHandler(DoUFLDemo);
                itemUFL.Enabled = true;
                itemUFL.Visible = true;

                var itemClientLetters = subMenu.AddItem("Client Letters demo", false);
                itemClientLetters.ClickHandler = new TxpAddinLibrary.Handlers.AppNotifyHandler(DoClientLettersDemo);
                itemClientLetters.Enabled = true;
                itemClientLetters.Visible = true;

                var itemCursorDemo = subMenu.AddItem("Hours glass cursor demo", false);
                itemCursorDemo.ClickHandler = new TxpAddinLibrary.Handlers.AppNotifyHandler(DoCursorDemo);
                itemCursorDemo.Enabled = true;
                itemCursorDemo.Visible = true;

                var itemBaunds = subMenu.AddItem("Baunds and handle", false);
                itemBaunds.ClickHandler = new TxpAddinLibrary.Handlers.AppNotifyHandler(DoBaundsDemo);
                itemBaunds.Enabled = true;
                itemBaunds.Visible = true;

                var itemAdvertising = subMenu.AddSubMenu("Advertising", false);

                var itemToggleVsAddinAdvertisement = itemAdvertising.AddItem("Toggle VsAddinAdvertising", false);
                itemToggleVsAddinAdvertisement.ClickHandler = new TxpAddinLibrary.Handlers.AppNotifyHandler(DoToggleVsAddinAdvertisement);
                itemToggleVsAddinAdvertisement.Enabled = true;
                itemToggleVsAddinAdvertisement.Visible = true;

                var itemToggleVsAddinAdvertisementOnStartup = itemAdvertising.AddItem("Toggle VsAddinAdvertising at startup", false);
                itemToggleVsAddinAdvertisementOnStartup.ClickHandler = new TxpAddinLibrary.Handlers.AppNotifyHandler(DoToggleVsAddinAdvertisementOnStartup);
                itemToggleVsAddinAdvertisementOnStartup.Enabled = true;
                itemToggleVsAddinAdvertisementOnStartup.Visible = true;

                var itemToggleCurrentAdvertisement = itemAdvertising.AddItem("Toggle current Advertising", false);
                itemToggleCurrentAdvertisement.ClickHandler = new TxpAddinLibrary.Handlers.AppNotifyHandler(DoToggleCurrentAdvertisement);
                itemToggleCurrentAdvertisement.Enabled = true;
                itemToggleCurrentAdvertisement.Visible = true;

                var itemToggleCurrentAdvertisementOnStartup = itemAdvertising.AddItem("Toggle current Advertising at startup", false);
                itemToggleCurrentAdvertisementOnStartup.ClickHandler = new TxpAddinLibrary.Handlers.AppNotifyHandler(DoToggleCurrentAdvertisementOnStartup);
                itemToggleCurrentAdvertisementOnStartup.Enabled = true;
                itemToggleCurrentAdvertisementOnStartup.Visible = true;

                var itemToggleSignSafeAdv = itemAdvertising.AddItem("Toggle SignSafeAdvertising", false);
                itemToggleSignSafeAdv.ClickHandler = new TxpAddinLibrary.Handlers.AppNotifyHandler(DoToggleSignSafeAdv);
                itemToggleSignSafeAdv.Enabled = true;
                itemToggleSignSafeAdv.Visible = true;

                var testSubMenu = subMenu.AddSubMenu("Test Sub Menu", true);
                _testSubMenu = testSubMenu.AddSubMenu("SubMenu", false);
                _testSubMenu.AddItem("(empty)", false).Enabled = false;
                _testMenuItem = testSubMenu.AddItem("Menu Item", false);

                var predefinedServices = (IAppPredefinedService)_appInstance;

                IAppMenuItem sendToSignSafeMenu;
                if (predefinedServices.GetPredefinedMenu("SendToSignSafe", out sendToSignSafeMenu))
                {
                    sendToSignSafeMenu.Caption = "Send to sign safe";
                    sendToSignSafeMenu.ClickHandler = new TxpAddinLibrary.Handlers.AppNotifyHandler(DoSendToSignSafe);
                    var app = (IAppTaxApplicationService)_appInstance;
                }

                CreateMenuItem(testSubMenu, "Change Submenu Enabled", () => _testSubMenu.Enabled = !_testSubMenu.Enabled, true);
                CreateMenuItem(testSubMenu, "Change Submenu Visible", () => _testSubMenu.Visible = !_testSubMenu.Visible, false);

                CreateMenuItem(testSubMenu, "Change MenuItem Enabled", () => _testMenuItem.Enabled = !_testMenuItem.Enabled, true);
                CreateMenuItem(testSubMenu, "Change MenuItem Visible", () => _testMenuItem.Visible = !_testMenuItem.Visible, false);

                _debugOutputMenuItem = CreateMenuItem(testSubMenu, "Show Debug Output", DoShowDebugOutputClick, true);
                }
        }

        private void InitDragDrop()
        {
            const uint cfUnicodetext = 13; //CF_UNICODETEXT

            var appDragDrop = (IAppDragDropService)_appInstance;
            if (appDragDrop != null)
            {
                appDragDrop.RegisterDataFormat(cfUnicodetext, new TxpAddinLibrary.Handlers.DragDrop.DataFormatHandler(DoGetUnicodeTextDragDropData));
            }
        }

        private void DoGetAdvertisingData(out string AParameterName, out string AData)
        {
            AParameterName = "VsAddinTest";
            AData = "SomeData";
        }

        private void InitAdvertisingData()
        {
            var LApp = (IAppTaxApplicationService)_appInstance;
            LApp.OnGetAdvertisingData = new TxpAddinLibrary.Handlers.AppGetAdvertisingData(DoGetAdvertisingData);
        }

        private delegate string GetModuleName(IAppModule aModule);

        private void InitModuleManager()
        {
            var mn = (GetModuleName)(m => (m == null) ? "(none)" : m.Name);

            var lManager = (IAppModuleManager)_appInstance;

            lManager.BeforeCurrentModuleChange = new TxpAddinLibrary.Handlers.ModuleManager.NotifyHandler(
                aModule => ShowDebugMessage("Changing '{0}' module to '{1}'", mn(lManager.CurrentModule), mn(aModule)));

            lManager.AfterCurrentModuleChange = new TxpAddinLibrary.Handlers.ModuleManager.NotifyHandler(
                aModule => ShowDebugMessage("Module '{0}' is active now.", mn(aModule)));
        }

        #endregion

        public AddinInstance()
            : base(new Guid("8C6E1349-8287-42C7-83ED-6AAE51DE11A5"), "C# Addin Test", "1.0.0.0")
        {
        }

        public override void InitializeTaxPrepAddin()
        {
            //if (_appInstance.AppVersion != "1.0.0.0")
            //    throw new Exception("Incorrect Application version.");

            Debug.WriteLine("Initializing addin instance");

            InitMenu();
            InitClientFileEvents();
            InitDatabaseEnvEvents();
            InitDragDrop();
            InitAdvertisingData();
            InitModuleManager();

        }
    }
}
