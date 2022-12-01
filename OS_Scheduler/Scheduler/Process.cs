using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Timer = System.Timers.Timer;

namespace OS_Scheduler.Scheduler
{
    internal class Process : IDisposable
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
            private set
            {
                name = value;
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

        #endregion

        #region Methods

        #region Work

        /// <summary>
        /// Продолжить работу процесса.
        /// Запустить внутренний таймер процесса.
        /// </summary>
        /// <param name="quant">Скорость таймера (как часто уменьшается оставшееся время работы)</param>
        public void StartWork(Int32 quant)
        {
            if (this.State is States.COMPLETED)
            {

            }
            if (this.State is States.KILLED)
            {

            }
            if (this.State is States.RUNNING)
            {

            }
            if (this.State is States.UNBORN)
            {

            }
            this.State = States.RUNNING;
            this.timer.Interval = quant;
            this.timer.Start();
        }

        /// <summary>
        /// Приостановить работу процесса.
        /// PS: вызывается планировщиком.
        /// </summary>
        public void PauseWork()
        {
            this.timer.Stop();
            
            if (this.State is States.COMPLETED || this.State is States.KILLED)
                return;

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

            //TODO Пнуть планировщик
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
            if (this.TimeLeft is -1)
                return;
            this.TimeLeft--;
            if (this.TimeLeft is 0)
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
            if (!this.IsAlive)
            {
                throw new Exception();
            }
            if (this.State is States.RUNNING)
            {
                throw new Exception();
            }
            if (this.IsSwapped)
            {
                throw new Exception();
            }
            this.IsSwapped = true;
        }

        /// <summary>
        /// Получает информацию о процессе из SwapFile.
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void UnSwap()
        {
            if (!this.IsSwapped)
            {
                throw new Exception();
            }
            this.IsSwapped = false;
        }

        #endregion

        #region StateChange

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
