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
    public partial class GetElFromEnumForm<Type> : Form
    {
        public Type Value => 
            (Type)Enum.Parse(typeof(Type), this.Priority_ComboBox.SelectedItem.ToString());

        public GetElFromEnumForm()
        {
            InitializeComponent();

            var vs = typeof(Type);

            if (!vs.IsEnum)
            {
                throw new Exception();
            }

            Priority_ComboBox.Items.AddRange(Enum.GetNames(vs));
        }

        private void OK_Button_Click(object sender, EventArgs e)
        {
            if (this.Priority_ComboBox.SelectedItem is null)
                return;

            this.DialogResult = DialogResult.OK;

            this.Close();
        }

        private void Cancel_Button_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
