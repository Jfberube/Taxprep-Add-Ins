using TaxprepAddinAPI;

namespace TxpAddinLibrary.Handlers
{
    public class AppGetAdvertisingData : IAddinGetAdvertisingDataHandler
    {
        public delegate void ExecuteDelegate(out string AParameterName, out string AData);
        private readonly ExecuteDelegate _onExecute;

        #region IAddinGetAdvertisingDataHandler
        public void Execute(out string AParameterName, out string AData)
        {
            if (_onExecute != null)
            {
                _onExecute(out AParameterName, out AData);
            }
            else
            {
                AParameterName = "";
                AData = "";
            }
        }
        #endregion

        public AppGetAdvertisingData(ExecuteDelegate onExecute)
        {
            _onExecute = onExecute;
        }
    }
}
