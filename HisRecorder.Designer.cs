namespace Chessing_UI
{
    partial class HisRecorder
    {
        /// <summary> 
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 元件設計工具產生的程式碼

        /// <summary> 
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.WriteRegion = new System.Windows.Forms.Label();
            this.DrScroll = new System.Windows.Forms.VScrollBar();
            this.SuspendLayout();
            // 
            // WriteRegion
            // 
            this.WriteRegion.Cursor = System.Windows.Forms.Cursors.Hand;
            this.WriteRegion.Dock = System.Windows.Forms.DockStyle.Left;
            this.WriteRegion.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WriteRegion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(51)))), ((int)(((byte)(12)))));
            this.WriteRegion.Location = new System.Drawing.Point(0, 0);
            this.WriteRegion.Margin = new System.Windows.Forms.Padding(0);
            this.WriteRegion.Name = "WriteRegion";
            this.WriteRegion.Size = new System.Drawing.Size(300, 190);
            this.WriteRegion.TabIndex = 0;
            this.WriteRegion.MouseClick += new System.Windows.Forms.MouseEventHandler(this.WriteRegion_MouseClick);
            this.WriteRegion.MouseEnter += new System.EventHandler(this.WriteRegion_MouseEnter);
            // 
            // DrScroll
            // 
            this.DrScroll.Cursor = System.Windows.Forms.Cursors.Hand;
            this.DrScroll.Dock = System.Windows.Forms.DockStyle.Right;
            this.DrScroll.Enabled = false;
            this.DrScroll.LargeChange = 1;
            this.DrScroll.Location = new System.Drawing.Point(300, 0);
            this.DrScroll.Maximum = 10;
            this.DrScroll.Minimum = 10;
            this.DrScroll.Name = "DrScroll";
            this.DrScroll.Size = new System.Drawing.Size(22, 190);
            this.DrScroll.TabIndex = 1;
            this.DrScroll.Value = 10;
            this.DrScroll.ValueChanged += new System.EventHandler(this.DrScroll_ValueChanged);
            // 
            // HisRecorder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(17)))), ((int)(((byte)(108)))));
            this.Controls.Add(this.DrScroll);
            this.Controls.Add(this.WriteRegion);
            this.Font = new System.Drawing.Font("Segoe Marker", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "HisRecorder";
            this.Size = new System.Drawing.Size(322, 190);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label WriteRegion;
        private System.Windows.Forms.VScrollBar DrScroll;
    }
}
