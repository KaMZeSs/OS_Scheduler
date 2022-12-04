using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OS_Scheduler.HelperForms
{
    public partial class GetIntForm : Form
    {
        public int Value => Int32.Parse(this.textBox1.Text);

        int upperBound;
        int lowerBound;

        private GetIntForm()
        {
            InitializeComponent();
        }

        public GetIntForm(String str, int upperBound, int lowerBound = -1)
            : this()
        {
            this.label1.Text = str;
            this.upperBound = upperBound;
            this.lowerBound = lowerBound;
        }

        private void OK_Button_Click(object sender, EventArgs e)
        {
            int vs;
            if (!Int32.TryParse(this.textBox1.Text, out vs))
            {
                MessageBox.Show("Невозможно определить число");
                return;
            }

            if (vs < lowerBound || vs > upperBound)
            {
                MessageBox.Show("Число выходит за границы возможных значений");
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void Cancel_Button_Click(object sender, EventArgs e)
        {
            this.textBox1.Text = "1";
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
