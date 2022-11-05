using VavaMVP;
using VavaMVP.Entities;

#region Setup
var games = File
    .ReadAllLines(string.Format(Constants.SetupFilePathTemplate, "games.csv"))
    .Select(game =>
    {
        var gameData = game.Split(Constants.Delimeter);

        return new Games()
        {
            Name = gameData[0],
        };
    });

var positions = File
    .ReadAllLines(string.Format(Constants.SetupFilePathTemplate, "positions.csv"))
    .Select(position =>
    {
        var positionData = position.Split(Constants.Delimeter);

        return new Positions()
        {
            Game = games.First(game => game.Name == positionData[0]),
            Name = positionData[1],
            Abbreviation = positionData[2],
        };
    });

var ratings = File
    .ReadAllLines(string.Format(Constants.SetupFilePathTemplate, "ratings.csv"))
    .Select(rating =>
    {
        var ratingData = rating.Split(Constants.Delimeter);

        return new Ratings()
        {
            Game = games.First(game => game.Name == ratingData[0]),
            Name = ratingData[1],
            Position = positions.First(postion => postion.Game.Name == ratingData[0] && postion.Name == ratingData[2]),
            Ratio = int.Parse(ratingData[3]),
            IsInitial = bool.Parse(ratingData[4]),
            IsForTeam = bool.Parse(ratingData[5]),
        };
    });
#endregion

while (true)
{
    try
    {
        var matches = new List<Matches>();
        var teams = new List<Teams>();
        var players = new List<Players>();

        #region Get matches - files
        Console.WriteLine(Constants.InputMessage);
        var folderPath = Console.ReadLine();

        if (string.IsNullOrEmpty(folderPath))
        {
            Console.WriteLine(Constants.FolderPathIsEmpty);
            continue;
        }

        if (folderPath.Equals(Constants.ExitKey))
        {
            Environment.Exit(0);
        }

        if (!Directory.Exists(folderPath))
        {
            Console.WriteLine(Constants.FolderPathIsNotExists);
            continue;
        }

        var matchFiles = Directory.GetFiles(folderPath, "*.csv", SearchOption.AllDirectories);

        if (matchFiles.Length == 0)
        {
            Console.WriteLine(Constants.EmptyFolder);
            continue;
        }
        #endregion

        foreach (var matchFilePath in matchFiles)
        {
            var match = new Matches()
            {
                FilePath = matchFilePath,
                RatedPlayers = new List<RatedPlayers>(),
            };
            var matchData = File.ReadAllLines(matchFilePath);

            #region Check & read match data
            if (matchData.Length < 2)
            {
                Utils.ThrowControlledError(Constants.MatchDataIsInvalid);
            }

            Games? game = games
                .Where(game => game.Name == matchData[0])
                .FirstOrDefault();

            if (game == null)
            {
                Utils.ThrowControlledError(Constants.GameNotFound);
            }
            else
            {
                match.Game = game;
            }

            var matchRatings = ratings.Where(rating => rating.Game.Name == match.Game.Name);
            #endregion

            for (int pIndex = 1; pIndex < matchData.Length; pIndex++)
            {
                #region Player data
                // playername;nickname;number;team name;position;
                var ratedPlayer = new RatedPlayers()
                {
                    UnPivotRatings = new List<Ratings>(),
                };
                var playerData = matchData[pIndex].Split(Constants.Delimeter);

                if (playerData.Length <= Constants.DefaultPlayerDataCount)
                {
                    Utils.ThrowControlledError(Constants.PlayerDataIsInvalid);
                }

                var player = players
                    .Where(player => player.NickName == playerData[1])
                    .FirstOrDefault();

                if (player == null)
                {
                    player = new Players()
                    {
                        Name = playerData[0],
                        NickName = playerData[1],
                        PlayedMatchCount = 1,
                        TotalRating = 0,
                    };

                    int playerNumber = 0;
                    bool isPlayerNumberValid = int.TryParse(playerData[2], out playerNumber);

                    if (isPlayerNumberValid)
                    {
                        player.Number = playerNumber;
                    }
                    else
                    {
                        Utils.ThrowControlledError(Constants.PlayerNumberIsInvalid);
                    }

                    players.Add(player);
                }
                else
                {
                    player.PlayedMatchCount++;
                }

                ratedPlayer.Player = player;
                #endregion

                #region Team
                Teams? team = teams
                    .Where(team => team.Name == playerData[3])
                    .FirstOrDefault();
                if (team == null)
                {
                    team = new Teams()
                    {
                        Name = matchData[3],
                    };
                    teams.Add(team);
                }

                ratedPlayer.Team = team;

                if (match.Home == null)
                {
                    match.Home = team.Shallowcopy();
                }
                else if (match.Away == null)
                {
                    match.Away = team.Shallowcopy();
                }
                else if (match.Home.Name != team.Name && match.Away.Name != team.Name)
                {
                    Utils.ThrowControlledError(Constants.MatchTeamDataIsInvalid);
                }
                #endregion

                #region Position
                Positions? position = positions
                    .Where(positionItem => positionItem.Game.Name == match.Game.Name && positionItem.Abbreviation == playerData[4])
                    .FirstOrDefault();

                if (position == null)
                {
                    Utils.ThrowControlledError(Constants.PositionIsNotFound);
                }
                else
                {
                    ratedPlayer.Position = position;
                }
                #endregion

                var isRatedPlayerValid = match.RatedPlayers.Any(alreadyRatedPlayer => alreadyRatedPlayer.Player.NickName == ratedPlayer.Player.NickName && (alreadyRatedPlayer.Team.Name != ratedPlayer.Team.Name || alreadyRatedPlayer.Position.Name != ratedPlayer.Position.Name));
                if (!isRatedPlayerValid)
                {
                    Utils.ThrowControlledError(Constants.PlayerTeamPositionIsInvalid);
                    break;
                }

                #region Rating
                var playerRatings = matchRatings.Where(rating => rating.Position.Name == ratedPlayer.Position.Name && !rating.IsInitial);
                var playerInitialRatings = matchRatings.Where(rating => rating.Position.Name == ratedPlayer.Position.Name && rating.IsInitial);

                if (playerData.Length != Constants.DefaultPlayerDataCount + playerRatings.Count())
                {
                    Utils.ThrowControlledError(Constants.PlayerRatingDataIsInvalid);
                }

                for (int rIndex = 0; rIndex < playerRatings.Count(); rIndex++)
                {
                    var playerRating = playerRatings
                        .ElementAt(rIndex)
                        .Shallowcopy();
                    ratedPlayer.UnPivotRatings.Add(playerRating);

                    int rating = 0;
                    bool isRatingValid = int.TryParse(playerData[Constants.DefaultPlayerDataCount + rIndex], out rating);

                    if (isRatingValid)
                    {
                        ratedPlayer.UnPivotRatings[rIndex].Ratio *= rating;
                        player.TotalRating += ratedPlayer.UnPivotRatings[rIndex].Ratio;

                        if (playerRating.IsForTeam)
                        {
                            if (match.Home.Name == team.Name)
                            {
                                match.Home.Score += rating;
                            }
                            else
                            {
                                match.Away.Score += rating;
                            }
                        }
                    }
                    else
                    {
                        Utils.ThrowControlledError(Constants.RatingValueIsInvalid);
                    }
                }

                ratedPlayer.UnPivotRatings.AddRange(playerInitialRatings);

                match.RatedPlayers.Add(ratedPlayer);
                #endregion
            }

            #region Rules
            if (match.Home.Score == match.Away.Score)
            {
                Utils.ThrowControlledError(Constants.MatchMustHasWinner);
            }
            #endregion

            #region Winner team
            var winnerTeam = "";
            if (match.Home.Score > match.Away.Score)
            {
                match.Home.IsWinner = true;
                winnerTeam = match.Home.Name;
            }
            {
                match.Away.IsWinner = true;
                winnerTeam = match.Away.Name;
            }

            var winnerTeamPlayers = match.RatedPlayers.Where(player => player.Team.Name == winnerTeam);
            foreach (var winnerRatedPlayer in winnerTeamPlayers)
            {
                var winnerPlayer = players.Find(player => player.NickName == winnerRatedPlayer.Player.NickName)!;
                winnerPlayer.TotalRating += Constants.WinnerPlayerBonusRating;
            }
            #endregion

            matches.Add(match);
        }

        var mvp = players
            .OrderByDescending(player => player.TotalRating)
            .First();

        Console.WriteLine(Constants.WinnerText, mvp.Number, mvp.Name, mvp.NickName, mvp.TotalRating, mvp.PlayedMatchCount);
    }
    catch (Exception ex)
    {
        if (ex.Source != "ControlledError")
        {
            Console.WriteLine("Unhandled exception: " + ex.Message);
        }
    }
}
