using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootShapesUp
{
    static class PlayerStatus
    {
     
       

#if WINDOWS_UAP
            public static string highScoreFilename = string.Format("{0}\\{1}", ApplicationData.Current.LocalFolder.Path, "highscore.txt");
#else
        public static string highScoreFilename = "highscore.txt";
#endif

        private static int _lives;
        private static int _score;
 
   

        // for storing how many lives the player has 
        public static int Lives
        {
            get
            {
                return _lives;
            }
            private set
            {
                _lives = value;
            }
        }

        // score stage facility
        public static int Score
        {
            get
            {
                return _score;
            }
            private set
            {
                _score = value;
            }
        }



        // the the reset method on construction
        static PlayerStatus()
        {
            
            Reset();
        }

        // the reset method sets all the values back to the start
        public static void Reset()
        {
 

            Score = 0;
            Lives = 3;
            
        }

        

        public static void AddPoints(int basePoints)
        {
            // first check if the player is alive or not, if he is dead then just exit
            if (PlayerShip.Instance.IsDead)
            {
                return;
            }

            // add to the score the number of points and apply the multiplier
            Score += basePoints;

            
        }

        

       
        // method for subtracting lives from the player
        public static void RemoveLife()
        {
            Lives -= 1;
        }
    }
}
