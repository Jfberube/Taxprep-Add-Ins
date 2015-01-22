﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTestHost;
using TaxprepAddinAPI;

namespace VSUnitTestAddin
{
    [TestClass]
    public class DataBaseEnvEvents
    {
        IAppDatabaseEnvEventsService FEvents;

        [TestInitialize]
        public void Init()
        {
            FEvents = (IAppDatabaseEnvEventsService)UnitTestHost.TestHost.CurrentInstance.AppInstance;
        }

        [TestCleanup]
        public void CleanUp()
        {
            FEvents.AfterAcceptUserInput = null;
            FEvents.AfterAddRepeat = null;
            FEvents.AfterCalc = null;
            FEvents.AfterCalcGlobalRates = null;
            FEvents.AfterDeleteRepeat = null;
            FEvents.BeforeCalc = null;
            FEvents = null;
        }

        [TestMethod]
        public void AfterAcceptUserInput()
        { 
            Assert.IsNull(FEvents.AfterAcceptUserInput);
            FEvents.AfterAcceptUserInput = new TxpAddinLibrary.Handlers.DatabaseEnv.AfterAcceptUserInput(
                (aCell) => {});
            Assert.IsNotNull(FEvents.AfterAcceptUserInput);
            FEvents.AfterAcceptUserInput = null;
            Assert.IsNull(FEvents.AfterAcceptUserInput);
        }

        [TestMethod]
        public void AfterAddRepeat()
        {
            Assert.IsNull(FEvents.AfterAddRepeat);
            FEvents.AfterAddRepeat = new TxpAddinLibrary.Handlers.DatabaseEnv.NotifyWithGroupArray(
                (aGroup) => { });
            Assert.IsNotNull(FEvents.AfterAddRepeat);
            FEvents.AfterAddRepeat = null;
            Assert.IsNull(FEvents.AfterAddRepeat);
        }

        [TestMethod]
        public void AfterCalc()
        {
            Assert.IsNull(FEvents.AfterCalc);
            FEvents.AfterCalc = new TxpAddinLibrary.Handlers.DatabaseEnv.BeforeCalcHandler(
                (aReturn) => { });
            Assert.IsNotNull(FEvents.AfterCalc);
            FEvents.AfterCalc = null;
            Assert.IsNull(FEvents.AfterCalc);
        }

        [TestMethod]
        public void AfterCalcGlobalRates()
        {
            Assert.IsNull(FEvents.AfterCalcGlobalRates);
            FEvents.AfterCalcGlobalRates = new TxpAddinLibrary.Handlers.AppNotifyHandler(
                () => { });
            Assert.IsNotNull(FEvents.AfterCalcGlobalRates);
            FEvents.AfterCalcGlobalRates = null;
            Assert.IsNull(FEvents.AfterCalcGlobalRates);
        }

        [TestMethod]
        public void AfterDeleteRepeat()
        {
            Assert.IsNull(FEvents.AfterDeleteRepeat);
            FEvents.AfterDeleteRepeat = new TxpAddinLibrary.Handlers.DatabaseEnv.NotifyWithGroupArray(
                (aGroup) => { });
            Assert.IsNotNull(FEvents.AfterDeleteRepeat);
            FEvents.AfterDeleteRepeat = null;
            Assert.IsNull(FEvents.AfterDeleteRepeat);
        }

        [TestMethod]
        public void BeforeCalc()
        {
            Assert.IsNull(FEvents.BeforeCalc);
            FEvents.BeforeCalc = new TxpAddinLibrary.Handlers.DatabaseEnv.BeforeCalcHandler(
                (aReturn) => { });
            Assert.IsNotNull(FEvents.BeforeCalc);
            FEvents.BeforeCalc = null;
            Assert.IsNull(FEvents.BeforeCalc);
        }
    }
}
