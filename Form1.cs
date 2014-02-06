using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;

namespace DiningPhilosopherSimulation
{
    enum ForkType { left = 0, right }; 

    public partial class Form1 : Form
    {
		Thread monitorThread;

        public Form1()
        {
            InitializeComponent();
            solutionComboBox.SelectedIndex = 0;
        }
       
        private void startBtn_Click(object sender, EventArgs e)
        {
            if(logBox.Items.Count > 0) //if listbox already has thing in it, clear them.
                logBox.Items.Clear();
            Table.numOfPhilosophers = int.Parse(PhilosopherNumTextBox.Text); // get number of philosophers
            Table table = Table.getInstance();
            table.setOutput(ref logBox); // give table object listbox so philosopher threads can output to it
            table.setAttemptsBeforeStarving(int.Parse(starvationNumTextBox.Text)); // number of attempts philosopher make to eat before starving
            table.start(solutionComboBox.SelectedIndex); // start all philosopher threads
            monitorThread = new Thread(new ThreadStart(this.monitorPhilosophers)); // monitor philosopher for starvation
            monitorThread.Start();
        }

        private DataGridViewRow createRow()
        {
            Table table = Table.getInstance();

            DataGridViewRow row = new DataGridViewRow();
            DataGridViewTextBoxCell col1 = new DataGridViewTextBoxCell();
            col1.Value = Table.numOfPhilosophers.ToString();
            DataGridViewTextBoxCell col2 = new DataGridViewTextBoxCell();
            String col2Str = "{";
            String[] percentArray = new String[Table.numOfPhilosophers];
            for (int i = 0; i < Table.numOfPhilosophers; i++)
            {
                double percent = table.philosophers[i].mealsCount / (double)table.totalMeals;
                percentArray[i] = Math.Round((percent * 100), 2).ToString();
            }
            col2Str += String.Join(",", percentArray) + "}";
            col2.Value = col2Str;
            DataGridViewTextBoxCell col3 = new DataGridViewTextBoxCell();
            col3.Value = table.totalMeals.ToString();
            row.Cells.Add(col1);
            row.Cells.Add(col2);
            row.Cells.Add(col3);

            return row;
        }

        private void stopBtn_Click(object sender, EventArgs e)
        {
            Table table = Table.getInstance();
            table.stop(); // stop all threads

            DataGridViewRow row = this.createRow(); // create row
            dataGridView1.Rows.Add(row); // add row to data view
            table.close(); // reset table to default
			this.monitorThread.Abort();
			this.monitorThread.Join();
        }

        /*
         * Used to monitor philosopher thread to check when a philosopher has starved
         */
        private void monitorPhilosophers()
        {
            Table table = Table.getInstance();
            while (true)
            {
                if (table.isSomeoneStarving) 
                {
                    for (int i = 0; i < Table.numOfPhilosophers; i++) // wait for threads to finish
                        table.threads[i].Join();
                    DataGridViewRow row = this.createRow(); // create row for data view
                    dataGridView1.Invoke(new MethodInvoker(() => dataGridView1.Rows.Add(row))); // add row to data view
                    table.close(); // reset table
                    break; // leave loop
                }
                System.Threading.Thread.Sleep(100);
            }
        }
    }
}