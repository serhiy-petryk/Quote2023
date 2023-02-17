namespace DGWnd
{
    public static class Settings
    {
        internal const string DbConnectionString = "Data Source=localhost;Initial Catalog=dbQuote2022;Integrated Security=True;Connect Timeout=150;";

        private const string BaseFolder = @"E:\Quote\";

        internal const string MinuteYahooDataFolder = BaseFolder + @"WebData\Minute\Yahoo\Data\";
        internal const string MinuteAlphaVantageDataFolder = BaseFolder + @"WebData\Minute\AlphaVantage\Data\";
    }
}
