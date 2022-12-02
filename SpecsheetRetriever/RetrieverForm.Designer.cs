using System.Windows.Forms;

namespace SpecsheetRetriever
{
    partial class RetrieverForm
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
            this.lblItems = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.lblResults = new System.Windows.Forms.Label();
            this.btnStop = new System.Windows.Forms.Button();
            this.pbWorking = new System.Windows.Forms.ProgressBar();
            this.chbWait = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lblItems
            // 
            this.lblItems.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblItems.Location = new System.Drawing.Point(12, 9);
            this.lblItems.Name = "lblItems";
            this.lblItems.Size = new System.Drawing.Size(294, 204);
            this.lblItems.TabIndex = 0;
            this.lblItems.Text = "...";
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(178, 216);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(128, 23);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // lblResults
            // 
            this.lblResults.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblResults.Location = new System.Drawing.Point(312, 9);
            this.lblResults.Name = "lblResults";
            this.lblResults.Size = new System.Drawing.Size(449, 204);
            this.lblResults.TabIndex = 2;
            this.lblResults.Text = "...";
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(312, 216);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(128, 23);
            this.btnStop.TabIndex = 3;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // pbWorking
            // 
            this.pbWorking.Location = new System.Drawing.Point(12, 292);
            this.pbWorking.Name = "pbWorking";
            this.pbWorking.Size = new System.Drawing.Size(749, 23);
            this.pbWorking.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.pbWorking.TabIndex = 4;
            // 
            // chbWait
            // 
            this.chbWait.AutoSize = true;
            this.chbWait.Checked = true;
            this.chbWait.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbWait.Location = new System.Drawing.Point(12, 220);
            this.chbWait.Name = "chbWait";
            this.chbWait.Size = new System.Drawing.Size(148, 19);
            this.chbWait.TabIndex = 5;
            this.chbWait.Text = "Wait Between Requests";
            this.chbWait.UseVisualStyleBackColor = true;
            // 
            // RetrieverForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(773, 327);
            this.Controls.Add(this.chbWait);
            this.Controls.Add(this.pbWorking);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.lblResults);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.lblItems);
            this.Name = "RetrieverForm";
            this.Text = "RetrieverForm";
            this.Load += new System.EventHandler(this.RetvieverForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label lblItems;
        private Button btnStart;
        private Label lblResults;
        private Button btnStop;
        private ProgressBar pbWorking;
        private CheckBox chbWait;
    }
}