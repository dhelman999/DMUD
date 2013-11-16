using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    class Player : Mob
    {
        // Member Variables
        ClientHandler mCh;

        public Player(ClientHandler ch) : base()
        {
            this.mCh = ch;
        }
    }
}
