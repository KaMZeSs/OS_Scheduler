namespace OS_Scheduler
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.SchedulerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.QuantSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RAMSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ProcessesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CreateProcessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ChangeSelectedProcessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ChangePriorityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ChangeNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.KillSelectedProcessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.RAMLeftToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ProcessesDataGridView = new System.Windows.Forms.DataGridView();
            this.PID_Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PPID_Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Priority_Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Name_Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RID_Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.State_Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TimeLeft_Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Size_Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsSwappedColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.EmptyColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GenerateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ProcessesDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SchedulerToolStripMenuItem,
            this.GenerateToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1011, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // SchedulerToolStripMenuItem
            // 
            this.SchedulerToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SettingsToolStripMenuItem,
            this.ProcessesToolStripMenuItem});
            this.SchedulerToolStripMenuItem.Name = "SchedulerToolStripMenuItem";
            this.SchedulerToolStripMenuItem.Size = new System.Drawing.Size(99, 20);
            this.SchedulerToolStripMenuItem.Text = "Планировщик";
            // 
            // SettingsToolStripMenuItem
            // 
            this.SettingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.QuantSizeToolStripMenuItem,
            this.RAMSizeToolStripMenuItem});
            this.SettingsToolStripMenuItem.Name = "SettingsToolStripMenuItem";
            this.SettingsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.SettingsToolStripMenuItem.Text = "Параметры";
            // 
            // QuantSizeToolStripMenuItem
            // 
            this.QuantSizeToolStripMenuItem.Name = "QuantSizeToolStripMenuItem";
            this.QuantSizeToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.QuantSizeToolStripMenuItem.Text = "Размер кванта";
            this.QuantSizeToolStripMenuItem.Click += new System.EventHandler(this.QuantSizeToolStripMenuItem_Click);
            // 
            // RAMSizeToolStripMenuItem
            // 
            this.RAMSizeToolStripMenuItem.Name = "RAMSizeToolStripMenuItem";
            this.RAMSizeToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.RAMSizeToolStripMenuItem.Text = "Количество ОЗУ";
            this.RAMSizeToolStripMenuItem.Click += new System.EventHandler(this.RAMSizeToolStripMenuItem_Click);
            // 
            // ProcessesToolStripMenuItem
            // 
            this.ProcessesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CreateProcessToolStripMenuItem,
            this.ChangeSelectedProcessToolStripMenuItem,
            this.KillSelectedProcessToolStripMenuItem});
            this.ProcessesToolStripMenuItem.Name = "ProcessesToolStripMenuItem";
            this.ProcessesToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.ProcessesToolStripMenuItem.Text = "Процессы";
            // 
            // CreateProcessToolStripMenuItem
            // 
            this.CreateProcessToolStripMenuItem.Name = "CreateProcessToolStripMenuItem";
            this.CreateProcessToolStripMenuItem.Size = new System.Drawing.Size(250, 22);
            this.CreateProcessToolStripMenuItem.Text = "Создать процесесс";
            this.CreateProcessToolStripMenuItem.Click += new System.EventHandler(this.CreateProcessToolStripMenuItem_Click);
            // 
            // ChangeSelectedProcessToolStripMenuItem
            // 
            this.ChangeSelectedProcessToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ChangePriorityToolStripMenuItem,
            this.ChangeNameToolStripMenuItem});
            this.ChangeSelectedProcessToolStripMenuItem.Name = "ChangeSelectedProcessToolStripMenuItem";
            this.ChangeSelectedProcessToolStripMenuItem.Size = new System.Drawing.Size(250, 22);
            this.ChangeSelectedProcessToolStripMenuItem.Text = "Изменить выделенный процесс";
            // 
            // ChangePriorityToolStripMenuItem
            // 
            this.ChangePriorityToolStripMenuItem.Name = "ChangePriorityToolStripMenuItem";
            this.ChangePriorityToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.ChangePriorityToolStripMenuItem.Text = "Изменить приоритет";
            this.ChangePriorityToolStripMenuItem.Click += new System.EventHandler(this.ChangePriorityToolStripMenuItem_Click);
            // 
            // ChangeNameToolStripMenuItem
            // 
            this.ChangeNameToolStripMenuItem.Name = "ChangeNameToolStripMenuItem";
            this.ChangeNameToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.ChangeNameToolStripMenuItem.Text = "Изменить имя";
            this.ChangeNameToolStripMenuItem.Click += new System.EventHandler(this.ChangeNameToolStripMenuItem_Click);
            // 
            // KillSelectedProcessToolStripMenuItem
            // 
            this.KillSelectedProcessToolStripMenuItem.Name = "KillSelectedProcessToolStripMenuItem";
            this.KillSelectedProcessToolStripMenuItem.Size = new System.Drawing.Size(250, 22);
            this.KillSelectedProcessToolStripMenuItem.Text = "Удалить выделенный процесс";
            this.KillSelectedProcessToolStripMenuItem.Click += new System.EventHandler(this.KillSelectedProcessToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RAMLeftToolStripStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 428);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1011, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // RAMLeftToolStripStatusLabel
            // 
            this.RAMLeftToolStripStatusLabel.Name = "RAMLeftToolStripStatusLabel";
            this.RAMLeftToolStripStatusLabel.Size = new System.Drawing.Size(118, 17);
            this.RAMLeftToolStripStatusLabel.Text = "toolStripStatusLabel1";
            // 
            // ProcessesDataGridView
            // 
            this.ProcessesDataGridView.AllowUserToAddRows = false;
            this.ProcessesDataGridView.AllowUserToDeleteRows = false;
            this.ProcessesDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ProcessesDataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.ProcessesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ProcessesDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PID_Column,
            this.PPID_Column,
            this.Priority_Column,
            this.Name_Column,
            this.RID_Column,
            this.State_Column,
            this.TimeLeft_Column,
            this.Size_Column,
            this.IsSwappedColumn,
            this.EmptyColumn});
            this.ProcessesDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ProcessesDataGridView.Location = new System.Drawing.Point(0, 24);
            this.ProcessesDataGridView.MultiSelect = false;
            this.ProcessesDataGridView.Name = "ProcessesDataGridView";
            this.ProcessesDataGridView.ReadOnly = true;
            this.ProcessesDataGridView.RowTemplate.Height = 25;
            this.ProcessesDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.ProcessesDataGridView.Size = new System.Drawing.Size(1011, 404);
            this.ProcessesDataGridView.TabIndex = 3;
            // 
            // PID_Column
            // 
            this.PID_Column.HeaderText = "PID";
            this.PID_Column.Name = "PID_Column";
            this.PID_Column.ReadOnly = true;
            // 
            // PPID_Column
            // 
            this.PPID_Column.HeaderText = "PPID";
            this.PPID_Column.Name = "PPID_Column";
            this.PPID_Column.ReadOnly = true;
            // 
            // Priority_Column
            // 
            this.Priority_Column.HeaderText = "Priority";
            this.Priority_Column.Name = "Priority_Column";
            this.Priority_Column.ReadOnly = true;
            // 
            // Name_Column
            // 
            this.Name_Column.HeaderText = "Name";
            this.Name_Column.Name = "Name_Column";
            this.Name_Column.ReadOnly = true;
            // 
            // RID_Column
            // 
            this.RID_Column.HeaderText = "RID";
            this.RID_Column.Name = "RID_Column";
            this.RID_Column.ReadOnly = true;
            // 
            // State_Column
            // 
            this.State_Column.HeaderText = "State";
            this.State_Column.Name = "State_Column";
            this.State_Column.ReadOnly = true;
            // 
            // TimeLeft_Column
            // 
            this.TimeLeft_Column.HeaderText = "TimeLeft";
            this.TimeLeft_Column.Name = "TimeLeft_Column";
            this.TimeLeft_Column.ReadOnly = true;
            // 
            // Size_Column
            // 
            this.Size_Column.HeaderText = "Size";
            this.Size_Column.Name = "Size_Column";
            this.Size_Column.ReadOnly = true;
            // 
            // IsSwappedColumn
            // 
            this.IsSwappedColumn.HeaderText = "Swapped";
            this.IsSwappedColumn.Name = "IsSwappedColumn";
            this.IsSwappedColumn.ReadOnly = true;
            this.IsSwappedColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.IsSwappedColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // EmptyColumn
            // 
            this.EmptyColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.EmptyColumn.HeaderText = "";
            this.EmptyColumn.Name = "EmptyColumn";
            this.EmptyColumn.ReadOnly = true;
            // 
            // GenerateToolStripMenuItem
            // 
            this.GenerateToolStripMenuItem.Name = "GenerateToolStripMenuItem";
            this.GenerateToolStripMenuItem.Size = new System.Drawing.Size(77, 20);
            this.GenerateToolStripMenuItem.Text = "Генерация";
            this.GenerateToolStripMenuItem.Click += new System.EventHandler(this.GenerateToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1011, 450);
            this.Controls.Add(this.ProcessesDataGridView);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ProcessesDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem SchedulerToolStripMenuItem;
        private ToolStripMenuItem SettingsToolStripMenuItem;
        private ToolStripMenuItem ProcessesToolStripMenuItem;
        private ToolStripMenuItem CreateProcessToolStripMenuItem;
        private ToolStripMenuItem ChangeSelectedProcessToolStripMenuItem;
        private ToolStripMenuItem KillSelectedProcessToolStripMenuItem;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel RAMLeftToolStripStatusLabel;
        private DataGridView ProcessesDataGridView;
        private DataGridViewTextBoxColumn PID_Column;
        private DataGridViewTextBoxColumn PPID_Column;
        private DataGridViewTextBoxColumn Priority_Column;
        private DataGridViewTextBoxColumn Name_Column;
        private DataGridViewTextBoxColumn RID_Column;
        private DataGridViewTextBoxColumn State_Column;
        private DataGridViewTextBoxColumn TimeLeft_Column;
        private DataGridViewTextBoxColumn Size_Column;
        private DataGridViewCheckBoxColumn IsSwappedColumn;
        private DataGridViewTextBoxColumn EmptyColumn;
        private ToolStripMenuItem ChangePriorityToolStripMenuItem;
        private ToolStripMenuItem ChangeNameToolStripMenuItem;
        private ToolStripMenuItem QuantSizeToolStripMenuItem;
        private ToolStripMenuItem RAMSizeToolStripMenuItem;
        private ToolStripMenuItem GenerateToolStripMenuItem;
    }
}