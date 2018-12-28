namespace WillFromAfarBot
{
    partial class LogIn
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.ChannelName = new System.Windows.Forms.TextBox();
            this.BotName = new System.Windows.Forms.TextBox();
            this.BotId = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.button1.Location = new System.Drawing.Point(232, 158);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Connect";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ChannelName
            // 
            this.ChannelName.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ChannelName.Location = new System.Drawing.Point(147, 27);
            this.ChannelName.Name = "ChannelName";
            this.ChannelName.Size = new System.Drawing.Size(160, 20);
            this.ChannelName.TabIndex = 1;
            // 
            // BotName
            // 
            this.BotName.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BotName.Location = new System.Drawing.Point(147, 72);
            this.BotName.Name = "BotName";
            this.BotName.Size = new System.Drawing.Size(160, 20);
            this.BotName.TabIndex = 1;
            // 
            // BotId
            // 
            this.BotId.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BotId.Location = new System.Drawing.Point(147, 117);
            this.BotId.Name = "BotId";
            this.BotId.Size = new System.Drawing.Size(160, 20);
            this.BotId.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Channel Name";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(37, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Bot Name";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(59, 120);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "BotId";
            // 
            // LogIn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BotId);
            this.Controls.Add(this.BotName);
            this.Controls.Add(this.ChannelName);
            this.Controls.Add(this.button1);
            this.Name = "LogIn";
            this.Size = new System.Drawing.Size(324, 199);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        //
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox ChannelName;
        private System.Windows.Forms.TextBox BotName;
        private System.Windows.Forms.TextBox BotId;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}
