using System.Security.Cryptography.X509Certificates;

namespace OS_Scheduler
{
    public partial class MainForm : Form
    {
        Scheduler.Scheduler scheduler;

        public MainForm()
        {
            InitializeComponent();

            Scheduler.ProcessQueue processQueue = new();

            scheduler = new();
            //scheduler.SchedulerViewerEvent += this.Scheduler_SchedulerViewerEvent;

            scheduler.StartWork();
        }

        private void Scheduler_SchedulerViewerEvent(object sender, Scheduler.Scheduler.SchedulerViewerEventArgs e)
        {
            try
            {
                this.Invoke(() =>
                {
                    this.ViewProcesses(e.Processes.ToArray());
                });
            }
            catch
            {
                scheduler.StopWork();
            }
        }

        private void ViewProcesses(Scheduler.Process[] processes)
        {
            this.ProcessesDataGridView.Rows.Clear();
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

            var timeForm = new HelperForms.GetIntForm(Int32.MaxValue);
            if (timeForm.ShowDialog() is not DialogResult.OK)
            {
                return;
            }

            var sizeForm = new HelperForms.GetIntForm(300);
            if (sizeForm.ShowDialog() is not DialogResult.OK)
            {
                return;
            }

            var proc = scheduler.CreateNewProcess(nameForm.Value, 0, priorityForm.Value, 0, timeForm.Value, sizeForm.Value);
            proc.ProcessPropertyChangeEvent += this.Proc_ProcessPropertyChangeEvent;
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

        private void UpdateData(Scheduler.Process process, Scheduler.Process.ProcessPropertyChangeEventArgs e)
        {
            var index = this.FindRowByPID(process.PID);
            if (index is -1) // Добавить новую строку
            {
                index = this.ProcessesDataGridView.Rows.Add();

                this.ProcessesDataGridView["PID_Column", index].Value = process.PID;
                this.ProcessesDataGridView["PPID_Column", index].Value = process.PPID;
                this.ProcessesDataGridView["Priority_Column", index].Value = process.NN;
                this.ProcessesDataGridView["Name_Column", index].Value = process.Name;
                this.ProcessesDataGridView["RID_Column", index].Value = process.RID;
                this.ProcessesDataGridView["State_Column", index].Value = process.State;
                this.ProcessesDataGridView["TimeLeft_Column", index].Value = process.TimeLeft;
                this.ProcessesDataGridView["Size_Column", index].Value = process.Size;
                this.ProcessesDataGridView["IsSwappedColumn", index].Value = process.IsSwapped;

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
    }
}