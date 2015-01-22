using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaxprepAddinAPI;

namespace VSUnitTestAddin
{
    [TestClass]
    public class ClientLetters
    {
        IAppTaxApplicationService FApplication;
        IAppClientLetterManager FLetters;

        [TestInitialize]
        public void Init()
        {
            FApplication = (IAppTaxApplicationService)UnitTestHost.TestHost.CurrentInstance.AppInstance;
            FLetters = FApplication.ClientLetterManager;
        }
        
        [TestCleanup]
        public void CleanUp()
        {
            FLetters = null;
            FApplication = null;
        }

        [TestMethod]
        public void ClientLettersTest()
        {
            Assert.IsTrue(FLetters.Count > 0);
            for (int i = 0; i < FLetters.Count; ++i)
            {
                var LLetter = FLetters.ClientLetters[i];
                Assert.IsNotNull(LLetter);
                Assert.IsFalse(string.IsNullOrEmpty(LLetter.FilePath));
                Assert.IsFalse(string.IsNullOrEmpty(LLetter.FormNAme));
                Assert.IsTrue(LLetter.AddInsBitField >= 0);
            }
        }

    }
}
