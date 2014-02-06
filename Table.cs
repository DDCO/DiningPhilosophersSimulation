using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace DiningPhilosopherSimulation
{
    class Table
    {
        private static Table instance = null;
        public Philosopher[] philosophers;
        public static int numOfPhilosophers;
        public ListBox log;
        public Thread[] threads;
        public int totalMeals;
        private int starvationNum;
        public bool isSomeoneStarving;

        private Table()
        {
            Philosopher.philosopherCount = 0; //set philosopher count to zero
            philosophers = new Philosopher[numOfPhilosophers];
            threads = new Thread[numOfPhilosophers];
            for (int i = 0; i < numOfPhilosophers; i++) // create instance of philosophers
                this.philosophers[i] = new Philosopher();
            this.isSomeoneStarving = false; // knowone is starving by default
        }

        public static Table getInstance()
        {
            if (instance == null)
                instance = new Table();
            return instance;
        }

        /*
         * start all philosopher threads 
         */
        public void start(int selectedIndex)
        {
            for (int i = 0; i < numOfPhilosophers; i++)
            {
				switch(selectedIndex)
				{
				case 0:
					threads[i] = new Thread(new ThreadStart(this.philosophers[i].mySolution));
					break;
				case 1:
					threads[i] = new Thread(new ThreadStart(this.philosophers[i].dijkstraSolution));
					break;
				default:
					threads[i] = new Thread(new ThreadStart(this.philosophers[i].mySolution));
					break;
				}
                threads[i].Start();
            }
        }

        /*
         * Stop all philosopher threads 
         */
        public void stop()
        {
            for (int i = 0; i < numOfPhilosophers; i++)
            {
                if (threads[i] != null)
				{
                    threads[i].Abort();
					threads[i].Join();
				}
            }
        }

        public void setAttemptsBeforeStarving(int num)
        {
            this.starvationNum = num;
        }

        public int getAttemptsBeforeStarving()
        {
            return this.starvationNum;
        }

        public void setOutput(ref ListBox listBox)
        {
            this.log = listBox;
        }

        public void close()
        {
            instance = null;
        }
    }
}
