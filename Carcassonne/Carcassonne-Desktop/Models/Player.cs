using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using Carcassonne_Desktop.Models.NetModels.GameModels;

namespace Carcassonne_Desktop.Models
{
    public class Player : ObservableObject
    {
        private List<Pawn> pawns;
        private int score;
        private int pawnCount;
        public bool IsCpuPlayer { get; set; }
        public string ID;
        public string Username { get; set; }
        public string Avatar { get; set; }

        public PlayerGameData GameData { get; set; }



        public Player(string name)
        {
            Username = name;
            score = 0;
            pawns = new List<Pawn>();
            PawnCount = 7;
            for (int i = 0; i < 7; i++)
            {
                Pawn temp = new Pawn {Player = this};
                pawns.Add(temp);
            }
        }
        public int Score
        {
            get { return score; }
            set { Set(() => Score, ref score, value); }
        }

        internal List<Pawn> Pawns
        {
            get { return pawns; }
            set { pawns = value; }
        }

        public int PawnCount
        {
            get { return pawnCount; }
            set { Set(() => PawnCount, ref pawnCount, value); }
        }
        public Pawn get_pawn()
        {
            if (pawns.Count > 0)
            {
                Pawn temp = pawns[pawns.Count - 1];
                pawns.RemoveAt(pawns.Count - 1);
                PawnCount--;
                return temp;
            }
            throw new IndexOutOfRangeException("There are no more pawns left");
        }

        public void add_pawn(Pawn p)
        {
            PawnCount++;
            pawns.Add(p);
            p.remove_pawn();
        
        }
      

    }
}