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

        public Player()
        {
            this.mClientHandler = null;
        }// Constructor

        public override string destroy()
        {
            return "You can't destroy a player!";
        }// destroy

    }// Class Player

}// Namespace _8th_Circle_Server
