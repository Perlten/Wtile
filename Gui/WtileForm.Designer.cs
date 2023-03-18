namespace Wtile.Gui
{
    partial class WtileForm
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
            leftLabel = new Label();
            SuspendLayout();
            // 
            // leftLabel
            // 
            leftLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            leftLabel.AutoSize = true;
            leftLabel.Font = new Font("Segoe UI", 13F, FontStyle.Regular, GraphicsUnit.Point);
            leftLabel.ForeColor = SystemColors.ButtonFace;
            leftLabel.Location = new Point(12, 5);
            leftLabel.Name = "leftLabel";
            leftLabel.Size = new Size(143, 30);
            leftLabel.TabIndex = 0;
            leftLabel.Text = "Workspace: 0";
            leftLabel.Click += label1_Click;
            // 
            // WtileForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaptionText;
            ClientSize = new Size(1087, 44);
            Controls.Add(leftLabel);
            FormBorderStyle = FormBorderStyle.None;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "WtileForm";
            ShowIcon = false;
            ShowInTaskbar = false;
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Wtile";
            TopMost = true;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label leftLabel;
    }
}