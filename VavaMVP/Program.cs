﻿using VavaMVP.Entities;

string exitKey = "bye";

string? folderPath = null;

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


        #endregion
    }
}