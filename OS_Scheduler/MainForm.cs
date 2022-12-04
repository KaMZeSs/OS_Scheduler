namespace OS_Scheduler
{
    public partial class MainForm : Form
    {
        Scheduler.Scheduler scheduler;

        public MainForm()
        {
            InitializeComponent();
            this.ProcessesDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            Scheduler.ProcessQueue processQueue = new();

            scheduler = new();

            scheduler.StartWork();

            this.UpdateRAMView();
        }

        private void AddProcessRangeToTable(Scheduler.Process[] processes)
        {
            foreach (Scheduler.Process process in processes)
            {
                var index = this.ProcessesDataGridView.Rows.Add();

                this.ProcessesDataGridView["PID_Column", index].Value = process.PID;
                this.ProcessesDataGridView["PPID_Column", index].Value = process.PPID;
                this.ProcessesDataGridView["Priority_Column", index].Value = process.NN;
                this.ProcessesDataGridView["Name_Column", index].Value = process.Name;
                this.ProcessesDataGridView["RID_Column", index].Value = process.RID;
                this.ProcessesDataGridView["State_Column", index].Value = process.State;
                this.ProcessesDataGridView["TimeLeft_Column", index].Value = process.TimeLeft;
                this.ProcessesDataGridView["Size_Column", index].Value = process.Size;
                this.ProcessesDataGridView["IsSwappedColumn", index].Value = process.IsSwapped;
            }

            this.UpdateRAMView();
        }

        private void CreateProcessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var nameForm = new HelperForms.GetStringForm();
            if (nameForm.ShowDialog() is not DialogResult.OK)
            {
                return;
            }

            var priorityForm = new HelperForms.GetElFromEnumForm<Scheduler.Process.Priorities>();
            if (priorityForm.ShowDialog() is not DialogResult.OK)
            {
                return;
            }

            var timeForm = new HelperForms.GetIntForm("¬ведите количество работы", Int32.MaxValue);
            if (timeForm.ShowDialog() is not DialogResult.OK)
            {
                return;
            }

            var sizeForm = new HelperForms.GetIntForm("¬ведите размер процесса", this.scheduler.RamSize);
            if (sizeForm.ShowDialog() is not DialogResult.OK)
            {
                return;
            }

            var proc = scheduler.CreateNewProcess(nameForm.Value, 0, priorityForm.Value, 0, timeForm.Value, sizeForm.Value);
            proc.ProcessPropertyChangeEvent += this.Proc_ProcessPropertyChangeEvent;
            this.AddProcessRangeToTable(new[] { proc });
        }

        private void Proc_ProcessPropertyChangeEvent(object sender, Scheduler.Process.ProcessPropertyChangeEventArgs e)
        {
            try
            {
                this.Invoke(() =>
                {
                    this.UpdateData(sender as Scheduler.Process, e);
                });
            }
            catch
            {
                scheduler.StopWork();
            }
        }

        private async void UpdateData(Scheduler.Process process, Scheduler.Process.ProcessPropertyChangeEventArgs e)
        {
            if (process.IsDisposed)
            {
                return;
            }

            var index = this.FindRowByPID(process.PID);

            if (index is -1)
            {
                return;
            }

            switch (e.PropertyName)
            {
                case "PPID":
                {
                    this.ProcessesDataGridView["PPID_Column", index].Value = process.PPID;
                    break;
                }
                case "NN":
                {
                    this.ProcessesDataGridView["Priority_Column", index].Value = process.NN;
                    break;
                }
                case "RID":
                {
                    this.ProcessesDataGridView["RID_Column", index].Value = process.RID;
                    break;
                }
                case "State": 
                {
                    if (process.State is Scheduler.Process.States.UNBORN) // «начит уже не в очереди
                    {
                        this.ProcessesDataGridView.Rows.RemoveAt(index);
                        await Task.Delay(50);
                        this.UpdateRAMView();
                        return;
                    }
                    this.ProcessesDataGridView["State_Column", index].Value = process.State;
                    break;
                }
                case "TimeLeft":
                {
                    this.ProcessesDataGridView["TimeLeft_Column", index].Value = process.TimeLeft;
                    break;
                }
                case "Size":
                {
                    this.ProcessesDataGridView["Size_Column", index].Value = process.Size;
                    break;
                }
                case "IsSwapped":
                {
                    this.ProcessesDataGridView["IsSwappedColumn", index].Value = process.IsSwapped;
                    this.UpdateRAMView();
                    break;
                }
                case "Name":
                {
                    this.ProcessesDataGridView["Name_Column", index].Value = process.Name;
                    break;
                }
                default:
                {
                    throw new Exception(e.PropertyName);
                }
            }

        }

        private int FindRowByPID(int pid)
        {
            if (this.ProcessesDataGridView.Rows.Count is 0)
                return -1;

            for (int i = 0; i < this.ProcessesDataGridView.Rows.Count; i++)
            {
                if (this.ProcessesDataGridView["PID_Column", i].Value.ToString().Equals(pid.ToString()))
                {
                    return i;
                }
            }

            return -1;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.scheduler.StopWork();
        }

        private void KillSelectedProcessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.ProcessesDataGridView.SelectedRows.Count is 0)
                return;
            int pid = (int)this.ProcessesDataGridView.SelectedRows[0].Cells["PID_Column"].Value;
            this.scheduler.KillProcess(pid);

            this.UpdateRAMView();
        }

        private void ChangeNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.ProcessesDataGridView.SelectedRows.Count is 0)
                return;

            Scheduler.Process process;

            try
            {
                int pid = (int)this.ProcessesDataGridView.SelectedRows[0].Cells["PID_Column"].Value;
                process = this.scheduler.Processes.First(x => x.PID.Equals(pid));
            }
            catch
            {
                return;
            }

            var nameForm = new HelperForms.GetStringForm();
            if (nameForm.ShowDialog() is not DialogResult.OK)
            {
                return;
            }

            if (process.IsDisposed)
                return;
            
            process.Name = nameForm.Value;
        }

        private void ChangePriorityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.ProcessesDataGridView.SelectedRows.Count is 0)
                return;

            Scheduler.Process process;

            try
            {
                int pid = (int)this.ProcessesDataGridView.SelectedRows[0].Cells["PID_Column"].Value;
                process = this.scheduler.Processes.First(x => x.PID.Equals(pid));
            }
            catch
            {
                return;
            }

            var priorityForm = new HelperForms.GetElFromEnumForm<Scheduler.Process.Priorities>();
            if (priorityForm.ShowDialog() is not DialogResult.OK)
            {
                return;
            }

            if (process.IsDisposed)
                return;

            process.NN = priorityForm.Value;
        }

        private void UpdateRAMView()
        {
            this.RAMLeftToolStripStatusLabel.Text = $"—вободно {this.scheduler.RamSize - this.scheduler.RamUsage}";
        }

        private void QuantSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sizeForm = new HelperForms.GetIntForm("¬ведите размер кванта", Int32.MaxValue, 1);
            if (sizeForm.ShowDialog() is not DialogResult.OK)
            {
                return;
            }

            this.scheduler.QuantOfTime = sizeForm.Value;
        }

        private void RAMSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sizeForm = new HelperForms.GetIntForm("¬ведите размер ќ«”", Int32.MaxValue, 
                this.scheduler.MinimumPossibleRamUsage);
            if (sizeForm.ShowDialog() is not DialogResult.OK)
            {
                return;
            }

            this.scheduler.RamSize = sizeForm.Value;

            this.UpdateRAMView();
        }

        private void GenerateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sizeForm = new HelperForms.GetIntForm("¬ведите количество генерируемых процессов", 
                Int32.MaxValue, 1);
            if (sizeForm.ShowDialog() is not DialogResult.OK)
            {
                return;
            }

            var num = sizeForm.Value;

            Random rnd = new Random();

            for (int i = 0; i < num; i++)
            {
                var proc = this.scheduler.CreateNewProcess(
                    $"Proc {rnd.Next()}", rnd.Next(),
                    (Scheduler.Process.Priorities)
                    rnd.Next(0, Enum.GetNames(typeof(Scheduler.Process.Priorities))
                    .Length),
                    rnd.Next(), rnd.Next(1000, 1000000), 
                    rnd.Next(10, this.scheduler.RamSize));
                proc.ProcessPropertyChangeEvent += this.Proc_ProcessPropertyChangeEvent;
                this.AddProcessRangeToTable(new[] { proc });
            }
        }
    }
}