namespace OWASP_INTEGRATOR
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            textBoxEngagementId = new TextBox();
            scanButton = new Button();
            SuspendLayout();
            // 
            // textBoxEngagementId
            // 
            textBoxEngagementId.Location = new Point(160, 27);
            textBoxEngagementId.Margin = new Padding(3, 4, 3, 4);
            textBoxEngagementId.Name = "textBoxEngagementId";
            textBoxEngagementId.Size = new Size(228, 27);
            textBoxEngagementId.TabIndex = 0;
            // 
            // scanButton
            // 
            scanButton.Location = new Point(141, 138);
            scanButton.Margin = new Padding(3, 4, 3, 4);
            scanButton.Name = "scanButton";
            scanButton.Size = new Size(171, 31);
            scanButton.TabIndex = 2;
            scanButton.Text = "Skanuj i wyślij";
            scanButton.UseVisualStyleBackColor = true;
            scanButton.Click += scanButton_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(439, 215);
            Controls.Add(scanButton);
            Controls.Add(textBoxEngagementId);
            Margin = new Padding(3, 4, 3, 4);
            Name = "Form1";
            Text = "OWASP Integrator";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox textBoxEngagementId;
        private Button scanButton;
    }
}
