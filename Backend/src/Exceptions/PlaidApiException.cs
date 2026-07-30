namespace DoughBro.src.Exceptions
{
    public class PlaidApiException : Exception
    {
        public string ErrorType { get; }
        public string ErrorCode { get; }
        public string ErrorMessage { get; }

        public PlaidApiException(string errorType, string errorCode, string errorMessage)
            : base($"Plaid API Error [{errorCode}]: {errorMessage}")
        {
            ErrorType = errorType;
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }
    }
}
