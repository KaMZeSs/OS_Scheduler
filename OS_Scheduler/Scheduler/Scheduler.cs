using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Timer = System.Timers.Timer;

namespace OS_Scheduler.Scheduler
{
    internal class Scheduler
    {
        #region Fields

        Int32 quantOfTime;
        Timer mainTimer; // Таймер работы планировщика
        //Timer viewTimer; // Таймер обновления информации у кого-то (передавать метод обновления в конструкторе)
        Int32 ramSize;
        Int32 ramLeft;
        ProcessQueue queue;

        Int32 currentQuant;

        // Ниже - лучше (скок квантов на действие процесса)
        Int32 speedOfWork = 1;

        #endregion

        #region Events

        #region ViewerEvent

        public class SchedulerViewerEventArgs
        {
            public IReadOnlyCollection<Process> Processes { get; }

            public SchedulerViewerEventArgs(IReadOnlyCollection<Process> processes)
            {
                Processes = processes;
            }
        }
        public delegate void SchedulerViewerEventHandler(object sender, SchedulerViewerEventArgs e);
        public event SchedulerViewerEventHandler? SchedulerViewerEvent;

        #endregion

        #endregion

        #region Constructor

        private Scheduler()
        {
            this.mainTimer = new();
            //this.viewTimer = new();
        }
        public Scheduler(Int32 quantOfTime = 200, Int32 ramSize = 4096, 
            Int32 mainTimerInterval = 1, Int32 viewTimerInterval = 400) : this()
        {
            this.quantOfTime = quantOfTime;
            this.ramSize = this.ramLeft = ramSize;

            this.mainTimer.Interval = mainTimerInterval;
            //this.viewTimer.Interval = viewTimerInterval;

            this.mainTimer.Elapsed += this.MainTimer_Elapsed;
        }

        #endregion

        #region Methods

        #region MainWorker

        private void MainTimer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            currentQuant++;
            if (currentQuant < quantOfTime)
                return;
            currentQuant = 0;
            NextCircle();
        }

        private void NextCircle()
        {
            while (true)
            {
                if (this.queue.Processes.First().IsWorking)
                    this.queue.Processes.First().PauseWork();
                
                this.queue.NextMoveInCircle();

                var vs = this.queue.Processes.First();

                if (vs.State is Process.States.KILLED || vs.State is Process.States.COMPLETED) // Если процесс убит, или закончил работу, значит надо удалить из очереди
                {
                    this.queue.RemoveProcess(vs);
                }
                else
                {
                    vs.StartWork(speedOfWork);
                    break;
                }
            }

            SchedulerViewerEvent?.Invoke(this, new(this.queue.Processes));
        }

        #endregion

        #region Other

        public void CreateNewProcess(String Name, Int32 PPID, 
            Process.Priorities priority, Int32 RID, Int32 TimeToWork, Int32 Size)
        {
            var process = new Process(Name, PPID, priority, RID, TimeToWork, Size);
            process.ProcessStateChangeEvent += this.Process_ProcessStateChangeEvent;
            this.queue.AddProcess(process);
            process.SetReady();
        }

        private void Process_ProcessStateChangeEvent(object sender, Process.ProcessStateChangeEventArgs e)
        {
            var proc = sender as Process;
            if (proc?.PID == this.queue.Processes.First().PID)
            {
                if (proc?.State is Process.States.KILLED || proc?.State is Process.States.COMPLETED)
                {
                    this.NextCircle();
                }
            }
            SchedulerViewerEvent?.Invoke(this, new(this.queue.Processes));
        }

        public void KillProcess(Process process)
        {
            process.Kill();
        }

        #endregion

        #endregion
    }
}
