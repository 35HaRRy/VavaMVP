
namespace VavaMVP
{
    public static class Constants
    {
        #region System
        public static char Delimeter = ';';
        public static string SetupFilePathTemplate = Environment.CurrentDirectory + @"\SetupFiles\{0}";

        public static int DefaultPlayerDataCount = 5;
        public static int WinnerPlayerBonusRating = 10;
        #endregion

        #region Messages
        public static string ExitKey = "bye";
        public static string InputMessage = @$"Please, enter folder path. To exit enter ""{ExitKey}""";
        public static string FolderPathIsEmpty = "Folder path must be entered.";
        public static string FolderPathIsNotExists = "Folder is not exists.";
        public static string EmptyFolder = "Folder doesn't contain any file.";
        public static string MatchDataIsInvalid = "Match data should contains at least two player (row) data.";
        public static string GameNotFound = "Game is not found.";
        public static string PlayerDataIsInvalid = "Please check player data. There is missing value.";
        public static string PlayerNumberIsInvalid = "Player number value is invalid";
        public static string MatchTeamDataIsInvalid = "Please check teams. There is 3. one team for same match.";
        public static string PositionIsNotFound = "Position is not found.";
        public static string PlayerTeamPositionIsInvalid = "One player must not play in different teams and positions in the same match.";
        public static string PlayerRatingDataIsInvalid = "Player rating data is invalid.";
        public static string RatingValueIsInvalid = "Rating value is invalid.";
        public static string MatchMustHasWinner = "Match must has a winner.";
        public static string WinnerText = "MVP: {0} - {1} ({2})\n\tTotal rating: {3}\n\tPlayed match count: {4}";
        #endregion
    }
}
