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

        #region Properties

        public Int32 QuantOfTime
        {
            get
            {
                return quantOfTime;
            }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException();
                quantOfTime = value;
            }
        }
        public Int32 RamSize
        {
            get
            {
                return ramSize;
            }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException();
                ramSize = value;
            }
        }
        public Int32 RamLeft
        {
            get
            {
                return ramLeft;
            }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException();
                ramLeft = value;
            }
        }
        public Int32 CurrentQuant
        {
            get
            {
                return currentQuant;
            }
        }
        public Int32 SpeedOfWork
        {
            get
            {
                return speedOfWork;
            }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException();
                speedOfWork = value;
            }
        }
        public bool IsWorking => this.mainTimer.Enabled;

        public IReadOnlyCollection<Process> Processes => queue.Processes;
        public int RamUsage => queue.RamUsage;
        public int MaxPossibleRamUsage => queue.MaxPossibleRamUsage;

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
            this.queue = new();
        }
        public Scheduler(Int32 quantOfTime = 200, Int32 ramSize = 4096, Int32 mainTimerInterval = 1) : this()
        {
            this.QuantOfTime = quantOfTime;
            this.RamSize = this.RamLeft = ramSize;

            this.mainTimer.Interval = mainTimerInterval;

            this.mainTimer.Elapsed += this.MainTimer_Elapsed;
        }

        #endregion

        #region Methods

        #region MainWorker

        private void MainTimer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            SchedulerViewerEvent?.Invoke(this, new(this.queue.Processes));
            currentQuant++;
            if (currentQuant < quantOfTime)
                return;
            currentQuant = 0;
            NextCircle();
        }

        private void NextCircle()
        {
            if (this.queue.Processes.Count is 0)
            {
                SchedulerViewerEvent?.Invoke(this, new(this.queue.Processes));
                return;
            }    

            if (this.queue.Processes.First().IsWorking)
                this.queue.Processes.First().PauseWork();

            while (true)
            {
                try
                {
                    if (this.queue.Processes.First().IsWorking)
                        this.queue.Processes.First().PauseWork();
                }
                catch
                {
                    break;
                }
                

                this.queue.NextMoveInCircle();

                var vs = this.queue.Processes.First();

                if (vs.State is Process.States.KILLED || vs.State is Process.States.COMPLETED) // Если процесс убит, или закончил работу, значит надо удалить из очереди
                {
                    vs.SetUnborn();
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

        #region WorkState

        public void StartWork()
        {
            if (!this.mainTimer.Enabled)
                this.mainTimer.Start();
        }

        public void StopWork()
        {
            try
            {
                this.queue.Processes.First(x => x.IsWorking).PauseWork();
            }
            catch
            {

            }
            
            if (this.mainTimer.Enabled)
                this.mainTimer.Stop();
        }

        #endregion

        #region Other

        public Process CreateNewProcess(String Name, Int32 PPID,
            Process.Priorities priority, Int32 RID, Int32 TimeToWork, Int32 Size)
        {
            var process = new Process(Name, PPID, priority, RID, TimeToWork, Size);
            process.ProcessStateChangeEvent += this.Process_ProcessStateChangeEvent;
            this.queue.AddProcess(process);
            process.SetReady();

            if (!this.IsWorking)
            {
                this.StartWork();
            }

            return process;
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

        public void KillProcess(Int32 PID)
        {
            try
            {
                this.queue.Processes.First(x => x.PID.Equals(PID)).Kill();
            }
            catch
            {
                throw new Exception();
            }
        }

        public void ChangeProcess(Process process, 
            Process.Priorities? priority = null, String? name = null)
        {
            if (process is null)
                throw new ArgumentNullException("process");

            process.NN = priority ?? process.NN;
            process.Name = name ?? process.Name;
        }

        #endregion

        #endregion
    }
}
