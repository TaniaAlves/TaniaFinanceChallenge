using System;

namespace BankRecord.Communication.Options
{
    public class BankRecordOptions
    {
        private string _baseAddress;
        public string BaseAddress
        {
            get { return _baseAddress ?? throw new InvalidOperationException("Base address Financial API must be setted."); }
            set { _baseAddress = value; }
        }

        private string _endPoint;
        public string EndPoint
        {
            get { return _endPoint ?? throw new InvalidOperationException("Bank Record EndPoint must be setted."); }
            set { _endPoint = value; }
        }

        public string GetBankRecordEndPoint() => $"{BaseAddress}/{EndPoint}";
    }
}
