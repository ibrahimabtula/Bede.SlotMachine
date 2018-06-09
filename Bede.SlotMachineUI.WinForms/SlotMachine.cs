using Bede.SlotMachine.Engine;
using System.Linq;
using System.Windows.Forms;

namespace Bede.SlotMachineUI.WinForms
{
    public partial class SlotMachine : Form
    {
        private SlotEngine slotEngine;

        public SlotMachine()
        {
            InitializeComponent();

            slotEngine = new Bede.SlotMachine.Engine.SlotEngine();
            slotEngine.EnterDeposit(100);
            slotEngine.EnterStake(10);

            //tbBalance.DataBindings.Add(nameof(TextBox.Text), slotEngine, nameof(SlotEngine.Balance));

        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            slotEngine.Spin();
            if (slotEngine.SpinHistory.Count() > 0)
            {
                var lbl = new Label()
                {
                    Visible = true,
                    Name = "lbl",
                    Text = "test"
                };

                this.Controls.Add(lbl);

                //dataGridView1.DataSource = slotEngine.SpinHistory
                //    .OrderByDescending(o => o.Date)
                //    .First()
                //    .Dimensions
                //    .SelectMany(s => s.Symbols)
                //    .Select(s => new { s.Coefficient, s.Probability, Name = s.Type.ToString()  })
                //    .ToList();
            }
        }
    }
}
