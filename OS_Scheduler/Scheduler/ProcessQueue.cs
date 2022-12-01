﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS_Scheduler.Scheduler
{
    internal class ProcessQueue
    {
        #region Fields

        private List<Process> list = new();

        #endregion

        #region Properties

        public Int32 RamUsage => list.Where(x => !x.IsSwapped).Sum(x => x.Size);
        public Int32 MaxPossibleRamUsage => list.Sum(x => x.Size);
        public IReadOnlyCollection<Process> Processes => list;

        #endregion

        #region Methods

        /// <summary>
        /// Добавить процесс в очередь.
        /// Процесс будет добавлен после процесса с таким же приоритетом, или ниже.
        /// Если процесс самый приоритетный из всех в очереди, то он будет добавлен после первого (т.к. он уже выполняется).
        /// </summary>
        /// <param name="process">Добавляемый процесс</param>
        public void AddProcess(Process process)
        {
            var index = list.FindLastIndex(x => x.NN <= process.NN);

            // Если нет индексов меньше или равных, значит он будет самым приоритетным
            if (index is -1)
            {
                // Если пуст - добавить в начало
                if (list.Count is 0)
                {
                    list.Add(process);
                }
                else // Если не пуст - добавить после начального (тк первый - сейчас выполняется)
                {
                    list.Insert(1, process);
                }
            }
            else
            {
                list.Insert(index + 1, process);
            }
        }

        public void RemoveProcess(Process process)
        {
            this.list.Remove(process);
        }

        public void NextMoveInCircle()
        {
            var vs = list.First();
            list.RemoveAt(0);

            this.AddProcess(vs);
        }

        #endregion
    }
}
