using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Timer = System.Timers.Timer;

namespace OS_Scheduler.Scheduler
{
    public class Process : IDisposable
    {
        #region Static

        readonly static SortedSet<Int32> _userIds = new();

        private static Int32 GenerateNewId()
        {
            if (_userIds.Count is Int32.MaxValue)
            {
                throw new Exception();
            }

            int newId = 0;
            while (_userIds.Contains(newId))
                newId++;

            _userIds.Add(newId);
            return newId;
        }

        #endregion

        #region Enums

        public enum Priorities { ABSOLUTE, HIGH, NORMAL, LOW }
        public enum States { UNBORN, BORN, READY, RUNNING, WAITING, COMPLETED, KILLED }

        #endregion

        #region Fields

        Int32 processId;
        Int32 parentProcessId;
        Priorities niceNumber; // Относительный приоритет (лекция 11)
        Int32 realId; // Идентификатор пользователя, запустившего процесс
        States state;
        DateTime bornTime; // Дата создания
        Int32 timeLeft; // Оставшееся время работы
        Int32 size; // Размер (для своппинга и "ОЗУ")
        bool isSwapped;
        String name;
        Timer timer;
        bool isInfinite;

        Int32 quant;
        Int32 previousTimeLeft;

        #endregion

        #region Properties

        /// <summary>
        /// Идентификатор процесса.
        /// </summary>
        public Int32 PID
        {
            get
            {
                return processId;
            }
            private set
            {
                if (processId.Equals(value))
                    return;
                if (Process._userIds.Contains(value))
                {
                    throw new Exception();
                }
                processId = value;
            }
        }

        /// <summary>
        /// Идентификатор родителя процесса.
        /// </summary>
        public Int32 PPID
        {
            get
            {
                return parentProcessId;
            }
            private set
            {
                parentProcessId = value;
                this.ProcessPropertyChangeEvent?.Invoke(this, new("PPID"));
            }
        }

        /// <summary>
        /// Относительный приоритет.
        /// </summary>
        public Priorities NN
        {
            get
            {
                return niceNumber;
            }
            set
            {
                niceNumber = value;
                this.ProcessPropertyChangeEvent?.Invoke(this, new("NN"));
            }
        }

        /// <summary>
        /// Идентификатор пользователя, запустившего процесс.
        /// </summary>
        public Int32 RID
        {
            get
            {
                return realId;
            }
            private set
            {
                realId = value;
                this.ProcessPropertyChangeEvent?.Invoke(this, new("RID"));
            }
        }

        /// <summary>
        /// Текущее состояние процесса.
        /// </summary>
        public States State
        {
            get
            {
                return state;
            }
            private set
            {
                state = value;
                ProcessStateChangeEvent?.Invoke(this, new(this.state));
                this.ProcessPropertyChangeEvent?.Invoke(this, new("State"));
            }
        }

        /// <summary>
        /// Дата рождения процесса.
        /// </summary>
        public DateTime BornTime
        {
            get
            {
                return bornTime;
            }
        }

        /// <summary>
        /// Оставшееся время работы процесса.
        /// </summary>
        public Int32 TimeLeft
        {
            get
            {
                return timeLeft;
            }
            private set
            {
                timeLeft = value;
                this.ProcessPropertyChangeEvent?.Invoke(this, new("TimeLeft"));
            }
        }

        /// <summary>
        /// Размер процесса.
        /// </summary>
        public Int32 Size
        {
            get
            {
                return size;
            }
            private set
            {
                size = value;
                this.ProcessPropertyChangeEvent?.Invoke(this, new("Size"));
            }
        }

        /// <summary>
        /// Свопнут ли процесс.
        /// </summary>
        public bool IsSwapped
        {
            get
            {
                return isSwapped;
            }
            private set
            {
                isSwapped = value;
                this.ProcessPropertyChangeEvent?.Invoke(this, new("IsSwapped"));
            }
        }

        /// <summary>
        /// Имя процесса.
        /// </summary>
        public String Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                this.ProcessPropertyChangeEvent?.Invoke(this, new("Name"));
            }
        }

        public bool IsAlive =>
            state == States.BORN || state == States.READY
            || state == States.RUNNING || state == States.WAITING;

        public bool IsWorking 
        { 
            get 
            { 
                return this.timer.Enabled; 
            } 
        }

        public bool IsDisposed => isDisposed;

        #endregion

        #region Dispose

        private bool isDisposed = false;

        public void Dispose()
        {
            if (isDisposed) return;

            Process._userIds.Remove(this.processId);
            this.timer.Close();
            isDisposed = true;
        }

        #endregion

        #region Constructor

        private Process()
        {
            this.processId = Process.GenerateNewId();
            this.name = string.Empty;
            this.state = States.BORN;
            this.isSwapped = false;
            this.timer = new();
            this.timer.Elapsed += this.Timer_Elapsed;
            this.isSwapped = false;
        }

        public Process(String name, int parentProcessId,
            Priorities niceNumber, int realId, int timeLeft, int size)
            : this()
        {
            this.name = name;
            this.parentProcessId = parentProcessId;
            this.niceNumber = niceNumber;
            this.realId = realId;
            this.timeLeft = timeLeft;
            this.size = size;
            this.isInfinite = timeLeft is -1;
        }

        #endregion

        #region Events

        public class ProcessStateChangeEventArgs
        {
            public Process.States CurrentState { get; }

            public ProcessStateChangeEventArgs(Process.States CurrentState)
            {
                this.CurrentState = CurrentState;
            }
        }
        public delegate void ProcessStateChangeEventHandler(object sender, ProcessStateChangeEventArgs e);
        public event ProcessStateChangeEventHandler? ProcessStateChangeEvent;

        public class ProcessPropertyChangeEventArgs
        {
            public String PropertyName { get; }

            public ProcessPropertyChangeEventArgs(String PropertyName)
            {
                this.PropertyName = PropertyName;
            }
        }
        public delegate void ProcessPropertyChangeEventHandler(object sender, ProcessPropertyChangeEventArgs e);
        public event ProcessPropertyChangeEventHandler? ProcessPropertyChangeEvent;

        #endregion

        #region Methods

        #region Work

        /// <summary>
        /// Продолжить работу процесса.
        /// Запустить внутренний таймер процесса.
        /// </summary>
        /// <param name="quant">Скорость таймера (как часто уменьшается оставшееся время работы)</param>
        public void StartWork(Int32 speedOfWork, Int32 quant)
        {
            if (this.State is States.COMPLETED)
            {
                throw new Exception();
            }
            if (this.State is States.KILLED)
            {
                throw new Exception();
            }
            if (this.State is States.RUNNING)
            {
                throw new Exception();
            }
            if (this.State is States.UNBORN)
            {
                throw new Exception();
            }
            this.State = States.RUNNING;
            this.timer.Interval = speedOfWork;
            this.quant = quant;
            this.previousTimeLeft = this.TimeLeft;
            this.timer.Start();
        }

        /// <summary>
        /// Приостановить работу процесса.
        /// PS: вызывается планировщиком.
        /// </summary>
        public void PauseWork()
        {
            if (this.IsWorking)
                this.timer.Stop();
            
            if (this.State is States.COMPLETED || this.State is States.KILLED)
                return;

            this.timeLeft = this.previousTimeLeft - quant;
            this.State = States.READY;
        }

        /// <summary>
        /// Завершает работу процесса.
        /// Переводит процесс в состояние COMPLETED.
        /// Планировщик удалит его из очереди при следующей "встрече".
        /// </summary>
        private void CompleteWork()
        {
            this.State = States.COMPLETED;
            this.timer.Stop();
        }

        /// <summary>
        /// Работа процесса.
        /// PS: Вызывается внутренним таймером процесса.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            if (this.isInfinite)
                return;
            this.TimeLeft--;
            if (this.TimeLeft <= 0)
            {
                this.CompleteWork();
            }
        }

        #endregion

        #region Swap

        /// <summary>
        /// Записывает информацию о процессе в SwapFile.
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void Swap()
        {
            this.IsSwapped = true;
        }

        /// <summary>
        /// Получает информацию о процессе из SwapFile.
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void UnSwap()
        {
            this.IsSwapped = false;
        }

        #endregion

        #region StateChange

        public void SetUnborn()
        {
            this.State = States.UNBORN;
            this.Dispose();
        }

        /// <summary>
        /// Установить состояние READY.
        /// PS: устанавливается ProcessQueue при добавлении.
        /// </summary>
        public void SetReady()
        {
            this.State = States.READY;
            this.timer.Stop();
        }

        /// <summary>
        /// Убить процесс.
        /// Процесс останавливает работу.
        /// Процесс будет удален из очереди при следующей "встрече" планировщиком.
        /// Вызывается планировщиком.
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void Kill()
        {
            if (this.State is States.KILLED)
            {
                throw new Exception();
            }
            this.State = States.KILLED;
            this.timer.Stop();
        }

        #endregion

        #endregion
    }
}
