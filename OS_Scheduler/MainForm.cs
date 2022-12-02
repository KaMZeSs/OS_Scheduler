namespace OS_Scheduler
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            Scheduler.ProcessQueue processQueue = new();

            processQueue.AddProcess(new("", 0, Scheduler.Process.Priorities.NORMAL, 1, 1, 15));
            processQueue.AddProcess(new("", 0, Scheduler.Process.Priorities.NORMAL, 1, 1, 15));
            processQueue.AddProcess(new("", 0, Scheduler.Process.Priorities.HIGH, 1, 1, 15));
            processQueue.AddProcess(new("", 0, Scheduler.Process.Priorities.LOW, 1, 1, 15));

            Scheduler.Scheduler scheduler = new();
            
        }
    }
}