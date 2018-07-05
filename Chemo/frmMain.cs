using Chemo.Treatment;
using Microsoft.Dism;
using System;
using System.Linq;
using System.Windows.Forms;
using System.Threading;

namespace Chemo
{
    public partial class frmMain : Form
    {
        private static readonly Logger logger = Logger.Instance;

        public frmMain()
        {
            InitializeComponent();

            // Other init stuff here
            logger.SetTarget(txtResults);
            DismApi.Initialize(DismLogLevel.LogErrors);
        }

        private void BtnInitiateTreatment_Click(object sender, EventArgs e)
        {
            txtResults.Clear();
            txtResults.Refresh();

            foreach (CheckBox c in grpTreatments.Controls.OfType<CheckBox>())
            {
                if (c.Checked && c.Tag != null)
                {
                    logger.Log("\r");
                    logger.Log("== Applying Treatment: {0} ==", c.Text);

                    // Create treatment instance based on checkbox tag
                    Type componentType = Type.GetType("Chemo.Treatment." + c.Tag.ToString());
                    ITreatment tr = (ITreatment)Activator.CreateInstance(componentType);

                    // Perform work in the background to not lock up the UI
                    Thread treatmentThread = new Thread(tr.PerformTreatment);
                    treatmentThread.Start();
                }
            }
        }
    }
}
