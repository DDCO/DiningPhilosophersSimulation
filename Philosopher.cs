using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiningPhilosopherSimulation
{
    class Philosopher
    {
        public Fork myFork;
        public static int philosopherCount;
        public int philosopherID;
        public int forksUsing;
        public int mealsCount;
        public int attempts;

        public Philosopher()
        {
            philosopherID = philosopherCount;
            myFork = new Fork(philosopherCount);
            philosopherCount++;
        }

        public void eat()
        {
            Table table = Table.getInstance();
            waitRandomTime(); 
            this.mealsCount++;
            this.attempts = 0; // reset attempts
            table.totalMeals++;
            table.log.Invoke(new MethodInvoker(() => table.log.Items.Add("Philosopher " + this.philosopherID + " finished eating")));
        }

        public void think(int time)
        {
            Table table = Table.getInstance();
            System.Threading.Thread.Sleep(time);
            table.log.Invoke(new MethodInvoker(() => table.log.Items.Add("Philosopher " + this.philosopherID + " finished thinking")));
        }

		public void eat(int time)
        {
            Table table = Table.getInstance();
            System.Threading.Thread.Sleep(time);
            this.mealsCount++;
            this.attempts = 0; // reset attempts
            table.totalMeals++;
            table.log.Invoke(new MethodInvoker(() => table.log.Items.Add("Philosopher " + this.philosopherID + " finished eating")));
        }

        public void think()
        {
            Table table = Table.getInstance();
            waitRandomTime();
            table.log.Invoke(new MethodInvoker(() => table.log.Items.Add("Philosopher " + this.philosopherID + " finished thinking")));
        }

		public int takeMinFork()
		{
			Table table = Table.getInstance ();

			int min = Table.numOfPhilosophers;
			int max = -1;
			for (int i = 0; i < Table.numOfPhilosophers; i++) {
				if (!table.philosophers[i].myFork.isBeingUsed && i < min)
					min = i;
				if (!table.philosophers[i].myFork.isBeingUsed && i > max)
					max = i;
			}
			if (min != max) 
			{
				table.philosophers[min].myFork.isBeingUsed = true; // im not using my fork
                this.forksUsing += 1; // I have +1 forks
                table.philosophers[min].myFork.userID = this.philosopherID; // im the philosopher who is using the fork
                table.log.Invoke(new MethodInvoker(() => table.log.Items.Add("Philosopher " + this.philosopherID + " is taking Min fork")));
				return min;
			}

			return -1;
        }

		public int takeMaxFork()
		{
			Table table = Table.getInstance ();

			int min = Table.numOfPhilosophers;
			int max = -1;
			for (int i = 0; i < Table.numOfPhilosophers; i++) {
				if (!table.philosophers[i].myFork.isBeingUsed && i < min)
					min = i;
				if (!table.philosophers[i].myFork.isBeingUsed && i > max)
					max = i;
			}
			if (min != max) 
			{
				table.philosophers[max].myFork.isBeingUsed = true; // im not using my fork
                this.forksUsing += 1; // I have +1 forks
                table.philosophers[max].myFork.userID = this.philosopherID; // im the philosopher who is using the fork
                table.log.Invoke(new MethodInvoker(() => table.log.Items.Add("Philosopher " + this.philosopherID + " is taking Max fork")));
				return max;
			}

			return -1;
        }

		public bool dropFork(int index)
		{
			Table table = Table.getInstance();

			if(!(index > -1))
				return false;

			if(table.philosophers[index].myFork.isBeingUsed)
			{
                this.forksUsing -= 1; // I have +1 forks
                table.philosophers[index].myFork.userID = -1; // im the philosopher who is using the fork
				table.philosophers[index].myFork.isBeingUsed = false; // im not using my fork
                table.log.Invoke(new MethodInvoker(() => table.log.Items.Add("Philosopher " + this.philosopherID + " is dropping fork")));
				return true;
			}
			return false;
        }

        public bool takeFork(ForkType fork)
        {
            waitRandomTime();
            Table table = Table.getInstance();

            if (fork == ForkType.right)
            {
                if (this.myFork.isBeingUsed)
                    return false;

                this.myFork.isBeingUsed = true; // im not using my fork
                this.forksUsing += 1; // I have +1 forks
                this.myFork.userID = this.philosopherID; // im the philosopher who is using the fork
                table.log.Invoke(new MethodInvoker(() => table.log.Items.Add("Philosopher " + this.philosopherID + " is taking right fork")));
            }
            else
            {
                int userID = (this.philosopherID - 1) < 0 ? (philosopherCount - 1) : (this.philosopherID - 1); // philosopher to the left of self
                if (table.philosophers[userID].myFork.isBeingUsed)
                    return false;

                table.philosophers[userID].myFork.isBeingUsed = true; // using fork of the philosopher to the left of me
                this.forksUsing += 1; // I have +1 forks
                table.philosophers[userID].myFork.userID = this.philosopherID; // im the philosopher who is using the fork
                table.log.Invoke(new MethodInvoker(() => table.log.Items.Add("Philosopher " + this.philosopherID + " is taking left fork")));
            }
            return true;
        }

        public bool dropFork(ForkType fork)
        {
            waitRandomTime();
            Table table = Table.getInstance();

            if (fork == ForkType.right)
            {
                if (this.myFork.isBeingUsed && this.myFork.userID == this.philosopherID) // fork is being used and im the user of the fork
                {
                    this.forksUsing -= 1; // using one less fork
                    this.myFork.userID = -1; // -1 means nobody is using the fork
                    this.myFork.isBeingUsed = false; // im no longer using the fork
                    table.log.Invoke(new MethodInvoker(() => table.log.Items.Add("Philosopher " + this.philosopherID + " is dropping right fork")));
                }
                else // I am not currently using the right fork
                    return false;
            }
            else
            {
                int userID = (this.philosopherID - 1) < 0 ? (philosopherCount - 1) : (this.philosopherID - 1); // philosopher to the left of self

                if (table.philosophers[userID].myFork.isBeingUsed && table.philosophers[userID].myFork.userID == this.philosopherID) // fork is being used and im the user of the fork
                {
                    this.forksUsing -= 1; // using one less fork
                    table.philosophers[userID].myFork.userID = -1; // -1 means nobody is using the fork
                    table.philosophers[userID].myFork.isBeingUsed = false; // im no longer using the fork
                    table.log.Invoke(new MethodInvoker(() => table.log.Items.Add("Philosopher " + this.philosopherID + " is dropping left fork")));
                }
                else // I am not currently using the left fork
                    return false;
            }
            return true;
        }

        private void waitRandomTime()
        {
            Random rand = new Random();
            System.Threading.Thread.Sleep(rand.Next(100));
        }

        public void mySolution()
        {
            Table table = Table.getInstance();
            while (!table.isSomeoneStarving) // is anyone starving?
            {
                this.think(); 
                if(this.takeFork(ForkType.right)) 
                    this.takeFork(ForkType.left);
                if (this.forksUsing == 2)
                    this.eat();
                else
                {
                    this.attempts++;
                    if (table.getAttemptsBeforeStarving() <= this.attempts) // is philosopher starving
                        table.isSomeoneStarving = true;
                }
                this.dropFork(ForkType.left);
                this.dropFork(ForkType.right);
            }
        }

		public void dijkstraSolution()
        {
            Table table = Table.getInstance();
            while (!table.isSomeoneStarving) // is anyone starving?
            {
				this.think(100); 
				int min = this.takeMinFork();
				int max = -1;
                if(min > -1) 
                    max = this.takeMaxFork();
                if (this.forksUsing == 2)
                    this.eat(100);
                else
                {
                    this.attempts++;
                    if (table.getAttemptsBeforeStarving() <= this.attempts) // is philosopher starving
                        table.isSomeoneStarving = true;
                }
				this.dropFork(max);
                this.dropFork(min);
            }
        }
    }
}
