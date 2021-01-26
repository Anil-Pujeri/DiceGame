using System;

namespace DiceGameClassLibrary
{
    public class Player
    {
        public Player(int number)
        {
            PlayerName = $"Player-{number}";
            TotalScore = 0;
            SkipNextChance = false;
            PreviousDiceValue = 0;
            Rank = -1;
            GameComplete = false;   
        }

        public string PlayerName { get; set; }
        public int TotalScore { get; set; }
        public bool SkipNextChance { get; set; }
        public int PreviousDiceValue { get; set; }
        public int Rank { get; set; }
        public bool GameComplete { get; set; }
    }
}
