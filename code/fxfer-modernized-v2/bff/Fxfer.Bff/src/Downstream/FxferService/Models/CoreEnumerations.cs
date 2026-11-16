namespace Fxfer.Bff.Downstream.FxferService.Models;

public static class CoreEnumerations
{
    public static class Messaging
    {
        public enum StatusCodeType
        {
            OK,
            Error,
            NotFound,
        }

        public enum SortDirectionType
        {
            Ascending,
            Descending,
        }

        public enum SearchModeType
        {
            EqualTo,
            Contains,
            StartsWith,
            EndsWith,
        }

        public enum MessageType
        {
            Error,
            Warning,
            Information,
        }
    }

    public static class Views
    {
        public enum DataListDisplayFormatType
        {
            Name,
        }
    }
}

