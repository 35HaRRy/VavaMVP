using VavaMVP.Entities;

#region Definitions
string exitKey = "bye";

string? folderPath = null;
#endregion

while (folderPath != exitKey)
{
    #region Get Matches - Files
    Console.WriteLine(@$"Please, enter folder path. To exit enter ""{exitKey}""");
    folderPath = Console.ReadLine();

    if (string.IsNullOrEmpty(folderPath))
    {
        Console.WriteLine("folder path must be entered.");
        continue;
    }

    if (Directory.Exists(folderPath))
    {
        Console.WriteLine("folder is not exists.");
        continue;
    }

    var matches = Directory.GetFiles(folderPath, "*.txt", SearchOption.AllDirectories);

    if (matches.Length == 0)
    {
        Console.WriteLine("folder doesn't contain any file.");
        continue;
    }
    #endregion

    foreach (var matchFilePath in matches)
    {
        var match = new Matches();
        var matchData = File.ReadAllLines(matchFilePath);

        #region Check & read match data
        if (matchData.Length > 1)
        {
            Console.WriteLine("Match data is invalid.");
            break;
        }

        Games game;
        bool isGameValid = Enum.TryParse(matchData[0], out game);

        if (isGameValid)
        {
            match.Game = game;
        }
        else
        {
            Console.WriteLine("Game data is invalid.");
            break;
        }

        for (int pIndex = 1; pIndex < matchData.Length; pIndex++)
        {
            // playername;nickname;number;team name;position;
            var player = new Players();
            var playerData = matchData[pIndex].Split(';');

            if (playerData.Length < 5)
            {
                Console.WriteLine("Player data is invalid.");
                break;
            }

            player.Name = playerData[0];
            player.NickName = playerData[1];

            int playerNumber = 0;
            bool isPlayerNumberValid = int.TryParse(matchData[2], out playerNumber);

            if (isPlayerNumberValid)
            {
                player.Number = playerNumber;
            }
            else
            {
                Console.WriteLine("Player data is invalid.");
                break;
            }

            
        }
        #endregion
    }
}
