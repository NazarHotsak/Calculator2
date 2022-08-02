using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator2
{
    public partial class CalculatorForm : Form
    {
        private Calculator _calculate;

        public CalculatorForm()
        {
            InitializeComponent();
            _calculate = new Calculator();   
        }

        private void GetAnswer_Click(object sender, EventArgs e)
        {
            string errorMessange = "";

            Answer.Text = _calculate.Calculate(Expression.Text, ref errorMessange);

            if (errorMessange != "")
            {
                MessageBox.Show(errorMessange, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
