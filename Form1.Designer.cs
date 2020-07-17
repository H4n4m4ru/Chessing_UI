namespace Chessing_UI
{
    partial class Form1
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

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.gameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.L_Welcome_to_Heaven = new System.Windows.Forms.Label();
            this.L_Welcome_to_Hell = new System.Windows.Forms.Label();
            this.Valhalla_Holy_BGI = new System.Windows.Forms.PictureBox();
            this.Calumn_Hint_Right = new System.Windows.Forms.PictureBox();
            this.Calumn_Hint_Left = new System.Windows.Forms.PictureBox();
            this.Row_Hint_Top = new System.Windows.Forms.PictureBox();
            this.Row_Hint_Bottom = new System.Windows.Forms.PictureBox();
            this.Board = new System.Windows.Forms.PictureBox();
            this.Valhalla_Inferno_BGI = new System.Windows.Forms.PictureBox();
            this.Valhalla_Hell = new Chessing_UI.Valhalla();
            this.Valhalla_Heaven = new Chessing_UI.Valhalla();
            this.hisRecorder1 = new Chessing_UI.HisRecorder();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Valhalla_Holy_BGI)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Calumn_Hint_Right)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Calumn_Hint_Left)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Row_Hint_Top)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Row_Hint_Bottom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Board)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Valhalla_Inferno_BGI)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.AutoSize = false;
            this.menuStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(137)))), ((int)(((byte)(69)))));
            this.menuStrip1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.menuStrip1.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gameToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1344, 23);
            this.menuStrip1.TabIndex = 1;
            // 
            // gameToolStripMenuItem
            // 
            this.gameToolStripMenuItem.AutoSize = false;
            this.gameToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.restartToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.gameToolStripMenuItem.Font = new System.Drawing.Font("Buxton Sketch", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gameToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(53)))), ((int)(((byte)(72)))));
            this.gameToolStripMenuItem.Name = "gameToolStripMenuItem";
            this.gameToolStripMenuItem.Padding = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.gameToolStripMenuItem.Size = new System.Drawing.Size(56, 30);
            this.gameToolStripMenuItem.Text = "Game";
            this.gameToolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // restartToolStripMenuItem
            // 
            this.restartToolStripMenuItem.AutoSize = false;
            this.restartToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(137)))), ((int)(((byte)(69)))));
            this.restartToolStripMenuItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.restartToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.restartToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(53)))), ((int)(((byte)(72)))));
            this.restartToolStripMenuItem.Name = "restartToolStripMenuItem";
            this.restartToolStripMenuItem.Size = new System.Drawing.Size(152, 28);
            this.restartToolStripMenuItem.Text = "Restart";
            this.restartToolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.restartToolStripMenuItem.Click += new System.EventHandler(this.restartToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.AutoSize = false;
            this.exitToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(137)))), ((int)(((byte)(69)))));
            this.exitToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(53)))), ((int)(((byte)(72)))));
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 28);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // L_Welcome_to_Heaven
            // 
            this.L_Welcome_to_Heaven.Font = new System.Drawing.Font("Segoe Marker", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.L_Welcome_to_Heaven.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(240)))), ((int)(((byte)(162)))));
            this.L_Welcome_to_Heaven.Location = new System.Drawing.Point(826, 62);
            this.L_Welcome_to_Heaven.Name = "L_Welcome_to_Heaven";
            this.L_Welcome_to_Heaven.Size = new System.Drawing.Size(223, 38);
            this.L_Welcome_to_Heaven.TabIndex = 8;
            this.L_Welcome_to_Heaven.Text = "Welcome to Heaven";
            this.L_Welcome_to_Heaven.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // L_Welcome_to_Hell
            // 
            this.L_Welcome_to_Hell.Font = new System.Drawing.Font("Segoe Marker", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.L_Welcome_to_Hell.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(43)))), ((int)(((byte)(41)))));
            this.L_Welcome_to_Hell.Location = new System.Drawing.Point(846, 652);
            this.L_Welcome_to_Hell.Name = "L_Welcome_to_Hell";
            this.L_Welcome_to_Hell.Size = new System.Drawing.Size(215, 38);
            this.L_Welcome_to_Hell.TabIndex = 9;
            this.L_Welcome_to_Hell.Text = "Welcome to ... Hell";
            this.L_Welcome_to_Hell.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Valhalla_Holy_BGI
            // 
            this.Valhalla_Holy_BGI.Image = global::Chessing_UI.Properties.Resources.Valhalla_Holy_Pic01;
            this.Valhalla_Holy_BGI.Location = new System.Drawing.Point(687, 49);
            this.Valhalla_Holy_BGI.Name = "Valhalla_Holy_BGI";
            this.Valhalla_Holy_BGI.Size = new System.Drawing.Size(512, 64);
            this.Valhalla_Holy_BGI.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Valhalla_Holy_BGI.TabIndex = 15;
            this.Valhalla_Holy_BGI.TabStop = false;
            // 
            // Calumn_Hint_Right
            // 
            this.Calumn_Hint_Right.Image = global::Chessing_UI.Properties.Resources.Calumn_Hint;
            this.Calumn_Hint_Right.Location = new System.Drawing.Point(600, 120);
            this.Calumn_Hint_Right.Margin = new System.Windows.Forms.Padding(0);
            this.Calumn_Hint_Right.Name = "Calumn_Hint_Right";
            this.Calumn_Hint_Right.Size = new System.Drawing.Size(64, 512);
            this.Calumn_Hint_Right.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Calumn_Hint_Right.TabIndex = 13;
            this.Calumn_Hint_Right.TabStop = false;
            // 
            // Calumn_Hint_Left
            // 
            this.Calumn_Hint_Left.Image = global::Chessing_UI.Properties.Resources.Calumn_Hint;
            this.Calumn_Hint_Left.Location = new System.Drawing.Point(8, 120);
            this.Calumn_Hint_Left.Margin = new System.Windows.Forms.Padding(0);
            this.Calumn_Hint_Left.Name = "Calumn_Hint_Left";
            this.Calumn_Hint_Left.Size = new System.Drawing.Size(64, 512);
            this.Calumn_Hint_Left.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Calumn_Hint_Left.TabIndex = 12;
            this.Calumn_Hint_Left.TabStop = false;
            // 
            // Row_Hint_Top
            // 
            this.Row_Hint_Top.Image = global::Chessing_UI.Properties.Resources.Row_Hint_2;
            this.Row_Hint_Top.Location = new System.Drawing.Point(80, 48);
            this.Row_Hint_Top.Margin = new System.Windows.Forms.Padding(0);
            this.Row_Hint_Top.Name = "Row_Hint_Top";
            this.Row_Hint_Top.Size = new System.Drawing.Size(512, 64);
            this.Row_Hint_Top.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Row_Hint_Top.TabIndex = 11;
            this.Row_Hint_Top.TabStop = false;
            // 
            // Row_Hint_Bottom
            // 
            this.Row_Hint_Bottom.Image = global::Chessing_UI.Properties.Resources.Row_Hint_2;
            this.Row_Hint_Bottom.Location = new System.Drawing.Point(80, 640);
            this.Row_Hint_Bottom.Margin = new System.Windows.Forms.Padding(0);
            this.Row_Hint_Bottom.Name = "Row_Hint_Bottom";
            this.Row_Hint_Bottom.Size = new System.Drawing.Size(512, 64);
            this.Row_Hint_Bottom.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Row_Hint_Bottom.TabIndex = 10;
            this.Row_Hint_Bottom.TabStop = false;
            // 
            // Board
            // 
            this.Board.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Board.Location = new System.Drawing.Point(80, 120);
            this.Board.Name = "Board";
            this.Board.Size = new System.Drawing.Size(512, 512);
            this.Board.TabIndex = 2;
            this.Board.TabStop = false;
            // 
            // Valhalla_Inferno_BGI
            // 
            this.Valhalla_Inferno_BGI.Image = global::Chessing_UI.Properties.Resources.Valhalla_Inferno_Pic01;
            this.Valhalla_Inferno_BGI.Location = new System.Drawing.Point(687, 639);
            this.Valhalla_Inferno_BGI.Name = "Valhalla_Inferno_BGI";
            this.Valhalla_Inferno_BGI.Size = new System.Drawing.Size(512, 64);
            this.Valhalla_Inferno_BGI.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Valhalla_Inferno_BGI.TabIndex = 14;
            this.Valhalla_Inferno_BGI.TabStop = false;
            // 
            // Valhalla_Hell
            // 
            this.Valhalla_Hell.LightColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(36)))), ((int)(((byte)(33)))));
            this.Valhalla_Hell.Location = new System.Drawing.Point(687, 504);
            this.Valhalla_Hell.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Valhalla_Hell.Name = "Valhalla_Hell";
            this.Valhalla_Hell.ShadeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(22)))), ((int)(((byte)(13)))));
            this.Valhalla_Hell.Size = new System.Drawing.Size(512, 128);
            this.Valhalla_Hell.TabIndex = 7;
            // 
            // Valhalla_Heaven
            // 
            this.Valhalla_Heaven.LightColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(240)))), ((int)(((byte)(162)))));
            this.Valhalla_Heaven.Location = new System.Drawing.Point(687, 120);
            this.Valhalla_Heaven.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Valhalla_Heaven.Name = "Valhalla_Heaven";
            this.Valhalla_Heaven.ShadeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(226)))), ((int)(((byte)(135)))));
            this.Valhalla_Heaven.Size = new System.Drawing.Size(512, 128);
            this.Valhalla_Heaven.TabIndex = 6;
            // 
            // hisRecorder1
            // 
            this.hisRecorder1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(17)))), ((int)(((byte)(108)))));
            this.hisRecorder1.Font = new System.Drawing.Font("Segoe Marker", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hisRecorder1.Location = new System.Drawing.Point(782, 279);
            this.hisRecorder1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.hisRecorder1.Name = "hisRecorder1";
            this.hisRecorder1.Size = new System.Drawing.Size(322, 190);
            this.hisRecorder1.TabIndex = 5;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(53)))), ((int)(((byte)(72)))));
            this.ClientSize = new System.Drawing.Size(1344, 729);
            this.Controls.Add(this.Calumn_Hint_Right);
            this.Controls.Add(this.Calumn_Hint_Left);
            this.Controls.Add(this.Row_Hint_Top);
            this.Controls.Add(this.Row_Hint_Bottom);
            this.Controls.Add(this.L_Welcome_to_Hell);
            this.Controls.Add(this.L_Welcome_to_Heaven);
            this.Controls.Add(this.Valhalla_Hell);
            this.Controls.Add(this.Valhalla_Heaven);
            this.Controls.Add(this.hisRecorder1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.Board);
            this.Controls.Add(this.Valhalla_Inferno_BGI);
            this.Controls.Add(this.Valhalla_Holy_BGI);
            this.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Form1";
            this.Text = "King Crimson ";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Valhalla_Holy_BGI)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Calumn_Hint_Right)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Calumn_Hint_Left)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Row_Hint_Top)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Row_Hint_Bottom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Board)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Valhalla_Inferno_BGI)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem gameToolStripMenuItem;
        private System.Windows.Forms.PictureBox Board;
        private System.Windows.Forms.ToolStripMenuItem restartToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private Valhalla Valhalla_Heaven;
        private Valhalla Valhalla_Hell;
        public HisRecorder hisRecorder1;
        private System.Windows.Forms.Label L_Welcome_to_Heaven;
        private System.Windows.Forms.Label L_Welcome_to_Hell;
        private System.Windows.Forms.PictureBox Row_Hint_Bottom;
        private System.Windows.Forms.PictureBox Row_Hint_Top;
        private System.Windows.Forms.PictureBox Calumn_Hint_Left;
        private System.Windows.Forms.PictureBox Calumn_Hint_Right;
        private System.Windows.Forms.PictureBox Valhalla_Inferno_BGI;
        private System.Windows.Forms.PictureBox Valhalla_Holy_BGI;


    }
}

