using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    class Player : Mob
    {
        // Member Variables
        public ClientHandler mClientHandler;

        public Player(ClientHandler ch) : base()
        {
            this.mClientHandler = ch;
        }// Constructor
    }// Class Player
}// Namespace _8th_Circle_Server
