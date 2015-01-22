using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTestHost;
using TaxprepAddinAPI;

namespace VSUnitTestAddin
{
    [TestClass]
    public class ClientFilEvents
    {
        IAppClientFileEventsService FEvents;
        
        [TestInitialize]
        public void Init()
        {
            FEvents = (IAppClientFileEventsService)UnitTestHost.TestHost.CurrentInstance.AppInstance;
        }

        [TestCleanup]
        public void CleanUp()
        {
            FEvents.AfterChangeClientFileHeaderProperty = null;
            FEvents.AfterClientFileNameChange = null;
            FEvents.AfterClientFileSave = null;
            FEvents.AfterCurrentDocumentChange = null;
            FEvents.AfterDocumentAdd = null;
            FEvents.AfterDocumentRemove = null;
            FEvents.BeforeClientFileNameChange = null;
            FEvents.BeforeClientFileSave = null;
            FEvents.BeforeCurrentDocumentChange = null;
            FEvents.BeforeDocumentAdd = null;
            FEvents.BeforeDocumentRemove = null;
            FEvents.BeforeReturnStatusChange = null;
            FEvents = null;
        }

        [TestMethod]
        public void AfterChangeClientFileHeaderProperty()
        {
            Assert.IsNull(FEvents.AfterChangeClientFileHeaderProperty);
            FEvents.AfterChangeClientFileHeaderProperty = new TxpAddinLibrary.Handlers.ClientFile.AfterChangeHeaderPropertyHandler(
                (aFilename) => {});
            Assert.IsNotNull(FEvents.AfterChangeClientFileHeaderProperty);
            FEvents.AfterChangeClientFileHeaderProperty = null;
            Assert.IsNull(FEvents.AfterChangeClientFileHeaderProperty);
        }

        [TestMethod]
        public void AfterClientFileNameChange()
        {
            Assert.IsNull(FEvents.AfterClientFileNameChange);
            FEvents.AfterClientFileNameChange = new TxpAddinLibrary.Handlers.ClientFile.AfterChangeNameHandler(
                (aFilename) => { });
            Assert.IsNotNull(FEvents.AfterClientFileNameChange);
            FEvents.AfterClientFileNameChange = null;
            Assert.IsNull(FEvents.AfterClientFileNameChange);
        }

        [TestMethod]
        public void AfterClientFileSave()
        {
            Assert.IsNull(FEvents.AfterClientFileSave);
            FEvents.AfterClientFileSave = new TxpAddinLibrary.Handlers.ClientFile.AfterSaveHandler(
                (aFilename) => { });
            Assert.IsNotNull(FEvents.AfterClientFileSave);
            FEvents.AfterClientFileSave = null;
            Assert.IsNull(FEvents.AfterClientFileSave);
        }

        [TestMethod]
        public void AfterCurrentDocumentChange()
        {
            Assert.IsNull(FEvents.AfterCurrentDocumentChange);
            FEvents.AfterCurrentDocumentChange = new TxpAddinLibrary.Handlers.ClientFile.ClientFileNotifyWithDocumentHandler(
                (Document) => { });
            Assert.IsNotNull(FEvents.AfterCurrentDocumentChange);
            FEvents.AfterCurrentDocumentChange = null;
            Assert.IsNull(FEvents.AfterCurrentDocumentChange);
        }

        [TestMethod]
        public void AfterDocumentAdd()
        {
            Assert.IsNull(FEvents.AfterDocumentAdd);
            FEvents.AfterDocumentAdd = new TxpAddinLibrary.Handlers.ClientFile.ClientFileNotifyWithDocumentHandler(
                (Document) => { });
            Assert.IsNotNull(FEvents.AfterDocumentAdd);
            FEvents.AfterDocumentAdd = null;
            Assert.IsNull(FEvents.AfterDocumentAdd);
        }

        [TestMethod]
        public void AfterDocumentRemove()
        {
            Assert.IsNull(FEvents.AfterDocumentRemove);
            FEvents.AfterDocumentRemove = new TxpAddinLibrary.Handlers.ClientFile.ClientFileNotifyWithDocumentHandler(
                (Document) => { });
            Assert.IsNotNull(FEvents.AfterDocumentRemove);
            FEvents.AfterDocumentRemove = null;
            Assert.IsNull(FEvents.AfterDocumentRemove);
        }

        [TestMethod]
        public void BeforeClientFileNameChange()
        {
            Assert.IsNull(FEvents.BeforeClientFileNameChange);
            FEvents.BeforeClientFileNameChange = new TxpAddinLibrary.Handlers.ClientFile.BeforeChangeNameHandler(
                (aOldFilename, aNewFileName) => { });
            Assert.IsNotNull(FEvents.BeforeClientFileNameChange);
            FEvents.BeforeClientFileNameChange = null;
            Assert.IsNull(FEvents.BeforeClientFileNameChange);
        }

        private void ExecuteDelegate(string aFileName, out bool aAccept)
        {
             aAccept = true;
        }

        [TestMethod]
        public void BeforeClientFileSave()
        {
            Assert.IsNull(FEvents.BeforeClientFileSave);
            FEvents.BeforeClientFileSave = new TxpAddinLibrary.Handlers.ClientFile.BeforeSaveHandler(ExecuteDelegate);
            Assert.IsNotNull(FEvents.BeforeClientFileSave);
            FEvents.BeforeClientFileSave = null;
            Assert.IsNull(FEvents.BeforeClientFileSave);
        }

        [TestMethod]
        public void BeforeCurrentDocumentChange()
        {
            Assert.IsNull(FEvents.BeforeCurrentDocumentChange);
            FEvents.BeforeCurrentDocumentChange = new TxpAddinLibrary.Handlers.ClientFile.BeforeCurrentDocumentChangeHandler(
                (OldDocument, NewDocument) => { });
            Assert.IsNotNull(FEvents.BeforeCurrentDocumentChange);
            FEvents.BeforeCurrentDocumentChange = null;
            Assert.IsNull(FEvents.BeforeCurrentDocumentChange);
        }

        [TestMethod]
        public void BeforeDocumentAdd()
        {
            Assert.IsNull(FEvents.BeforeDocumentAdd);
            FEvents.BeforeDocumentAdd = new TxpAddinLibrary.Handlers.ClientFile.ClientFileNotifyWithDocumentHandler(
                (Document) => { });
            Assert.IsNotNull(FEvents.BeforeDocumentAdd);
            FEvents.BeforeDocumentAdd = null;
            Assert.IsNull(FEvents.BeforeDocumentAdd);
        }

        [TestMethod]
        public void BeforeDocumentRemove()
        {
            Assert.IsNull(FEvents.BeforeDocumentRemove);
            FEvents.BeforeDocumentRemove = new TxpAddinLibrary.Handlers.ClientFile.ClientFileNotifyWithDocumentHandler(
                (Document) => { });
            Assert.IsNotNull(FEvents.BeforeDocumentRemove);
            FEvents.BeforeDocumentRemove = null;
            Assert.IsNull(FEvents.BeforeDocumentRemove);
        }

        [TestMethod]
        public void BeforeReturnStatusChange()
        {
            Assert.IsNull(FEvents.BeforeReturnStatusChange);
            FEvents.BeforeReturnStatusChange = new TxpAddinLibrary.Handlers.ClientFile.BeforeReturnStatusChangeHandler(
                (aDocReturn, AStatusName, aOldValue, AValue) => { });
            Assert.IsNotNull(FEvents.BeforeReturnStatusChange);
            FEvents.BeforeReturnStatusChange = null;
            Assert.IsNull(FEvents.BeforeReturnStatusChange);
        }
    }
}
