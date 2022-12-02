using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SpecsheetRetriever.Models;
using SpecsheetRetriever.Workers;

namespace SpecsheetRetriever
{
    public partial class RetrieverForm : Form
    {
        public RetrieverForm()
        {
            InitializeComponent();
        }

        private RetrieverFactory _factory = new RetrieverFactory();
        private List<SearchItem> _searchList = new List<SearchItem>();

        private void RetvieverForm_Load(object sender, EventArgs e)
        {
            this.lblResults.Visible = false;
            this.pbWorking.Visible = false;
            this.btnStop.Enabled = false;

            _searchList.Add(new SearchItem() { Brand = "BMS", Model = "12N620" });
            _searchList.Add(new SearchItem() { Brand = "Precision Devices", Model = "PD.103NR1" });
            _searchList.Add(new SearchItem() { Brand = "Eighteen Sound", Model = "15ND930" });
            _searchList.Add(new SearchItem() { Brand = "FaitalPRO", Model = "10FH520", OhmVersion = 8 });
            _searchList.Add(new SearchItem() { Brand = "FaitalPRO", Model = "10FH520", OhmVersion = 16 });

            this.lblItems.Text = string.Join("\n", _searchList.Select(x => x.ToListItemString()));
        }

        CancellationTokenSource? _cTokenSource;
        List<SpecsheetItem>? _lastResult;

        private async void btnStart_Click(object sender, EventArgs e)
        {
            this.btnStart.Enabled = false;
            this.btnStop.Enabled = true;
            this.pbWorking.Visible = true;
            _cTokenSource = new CancellationTokenSource();
            var cToken = _cTokenSource.Token;

            var list = _searchList; // Capture list only.
            var retriever = _factory.CreateRetriever();
            var worker = new RetrieverWorker() { WaitBetweenDrivers = this.chbWait.Checked };

            try
            {
                await Task.Run(() => worker.Start(retriever, list, cToken));

                this.lblResults.Text = $"Found {worker.SuccessCount} items."
                    + ((worker.FailedCount > 0) ? $" Failed {worker.FailedCount} items." : null)
                    + (cToken.IsCancellationRequested ? " Stopped by user request." : null)
                    + ((worker.SuccessfulResults.Count > 0) ? "\n" + string.Join("\n", worker.SuccessfulResults.Select(x => x.ToListItemString())) : null)
                    ;

                // TODO: Save payloads on disk or something.
                if (worker.SuccessCount > 0)
                {
                    _lastResult = worker.SuccessfulResults;
                }
            }
            catch (Exception ex)
            {
                this.lblResults.Text = ex.ToString();
            }

            this.lblResults.Visible = true;
            this.pbWorking.Visible = false;
            this.btnStart.Enabled = true;
            this.btnStop.Enabled = false;
            _cTokenSource.Dispose();
            _cTokenSource = null;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            _cTokenSource?.Cancel();
        }

    }
}