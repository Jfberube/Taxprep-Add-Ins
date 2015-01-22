﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaxprepAddinAPI;


namespace VSUnitTestAddin
{
    [TestClass]
    public class TaxReturn
    {
        IAppDocReturn FDocument;
        IAppTaxApplicationService FApplication;

        [TestInitialize]
        public void Init()
        {
            FApplication = (IAppTaxApplicationService)UnitTestHost.TestHost.CurrentInstance.AppInstance;
            FDocument = FApplication.GetCurrentDocReturn();
        }

        [TestCleanup]
        public void Cleanup()
        {
            FApplication = null;
            FDocument = null;
        }

        private readonly string Alias = "$PersonalizedID";

        [TestMethod]
        public void CheckAlias()
        {
            FDocument.CheckAlias(Alias);
        }

        [TestMethod]
        public void GetAliasString()
        {
            FDocument.GetAliasString(Alias);
        }

        [TestMethod]
        public void SetAliasString()
        {
            var lValue = FDocument.GetAliasString(Alias);
            FDocument.SetAliasString(Alias, lValue);
            Assert.IsTrue(string.Equals(lValue, FDocument.GetAliasString(Alias)));
        }

        [TestMethod]
        public void ChangeAliasValue()
        {
            var lValue = FDocument.GetAliasString(Alias);
            FDocument.SetAliasString(Alias, lValue);
            Assert.IsTrue(string.Equals(lValue, FDocument.GetAliasString(Alias)));
            FDocument.SetAliasString(Alias, "test");
            Assert.IsTrue(string.Equals("test", FDocument.GetAliasString(Alias)));
            FDocument.SetAliasString(Alias, lValue);
            Assert.IsTrue(string.Equals(lValue, FDocument.GetAliasString(Alias)));
        }
    }
}
