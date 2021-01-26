using System;
using System.Linq;

namespace DiceGameClassLibrary
{
    public class Game
    {
        private readonly int _numberOfPlayers;
        private readonly int _winningScore;
        private Player[] _players;
        private int _activePlayers;
        private readonly Action<string> _outputCallback;
        private readonly Func<char> _inputCallback;
        private readonly Random _random;
        private readonly int[] _playerOrder;

        public Game(int numberOfPlayers, int winningScore, Action<string> outputCallback, Func<char> inputCallback)
        {
            _numberOfPlayers = numberOfPlayers;
            _winningScore = winningScore;
            _players = new Player[_numberOfPlayers];
            _activePlayers = _numberOfPlayers;
            _outputCallback = outputCallback;
            _inputCallback = inputCallback;

            _random = new Random();
            _playerOrder = Enumerable.Range(0, _numberOfPlayers).OrderBy(a => _random.Next()).ToArray();

            InitializePlayers();
        }

        public void Play()
        {
            int rank = 1;
            while (_activePlayers != 0)
            {
                for (int i = 0; i < _numberOfPlayers; i++)
                {
                    if(!_players[_playerOrder[i]].GameComplete)
                    {
                        (_players[_playerOrder[i]]) = RollTheDice(_players[_playerOrder[i]], TurnType.Normal);

                        _outputCallback("Results");
                        foreach (var player in _players)
                        {
                            _outputCallback($"{player.PlayerName} : {player.TotalScore}");
                        }

                        if (_players[_playerOrder[i]].GameComplete)
                        {
                            _players[_playerOrder[i]].Rank = rank++;
                            _activePlayers--;
                        }
                    }
                }

                if (_activePlayers !=0)_outputCallback("\n--------- Next Round ---------");
            }

            _players = _players.OrderBy(a => a.Rank).ToArray();

            _outputCallback("\nFinal Results");
            foreach (var player in _players)
            {
                _outputCallback($" Rank {player.Rank} : {player.PlayerName}");
            }
        }

        private Player RollTheDice(Player player, TurnType turnType)
        {
            if (!player.SkipNextChance)
            {
                if (turnType == TurnType.DiceValueSix)
                {
                    _outputCallback("You get another chance! Please press 'r' to roll the dice");
                }
                else if (turnType == TurnType.WrongInput)
                {
                    _outputCallback("Incorrect input, please press 'r' to roll the dice");
                }
                else
                {
                    _outputCallback($"\n{player.PlayerName}, it's your turn press 'r' to roll the dice");
                }

                var input = _inputCallback();
                if (input !='r' && input != 'R')
                {
                    return RollTheDice(player, TurnType.WrongInput);
                }

                var diceValue = GetDiceValue();
                _outputCallback($"You got a {diceValue}");
                player.TotalScore += diceValue;

                if (player.TotalScore >= _winningScore)
                {
                    player.GameComplete = true;
                    _outputCallback($"{player.PlayerName} game complete");
                    return (player);
                }

                switch (diceValue)
                {
                    case 1:
                        if(player.PreviousDiceValue ==1)
                            player.SkipNextChance = true;
                        break;
                    case 6:
                        if(turnType != TurnType.DiceValueSix)
                            return RollTheDice(player, TurnType.DiceValueSix);
                        break;
                }
                player.PreviousDiceValue = diceValue;
            }
            else
            {
                player.SkipNextChance = false;
                player.PreviousDiceValue = 0;
                _outputCallback($"\n{player.PlayerName} your turn is skipped because you got 1 twice!");
            }
            return (player);
        }

        private void InitializePlayers()
        {
            for (int i = 0; i < _numberOfPlayers; i++)
            {
                _players[i] = new Player(i + 1);
            }
        }

        private int GetDiceValue()
        {
            return _random.Next(1, 7);
        }

        private enum TurnType
        {
            Normal,
            DiceValueSix,
            WrongInput
        }
    }
}
