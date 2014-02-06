using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiningPhilosopherSimulation
{
    class Fork
    {
        public bool isBeingUsed;
        public int ownerID;
        public int userID;

        public Fork(int owner)
        {
            this.isBeingUsed = false;
            this.ownerID = owner;
            this.userID = -1;
        }

        public void useFork(int user)
        {
            this.isBeingUsed = true;
            this.userID = user;
        }
    }
}
