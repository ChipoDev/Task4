namespace FigureReflectionApp
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

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
            this.btnLoadAssembly = new System.Windows.Forms.Button();
            this.lstShapes = new System.Windows.Forms.ListBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.pnlConstructor = new System.Windows.Forms.Panel();
            this.pnlMethods = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            
            this.btnLoadAssembly.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnLoadAssembly.Location = new System.Drawing.Point(0, 0);
            this.btnLoadAssembly.Name = "btnLoadAssembly";
            this.btnLoadAssembly.Size = new System.Drawing.Size(800, 40);
            this.btnLoadAssembly.TabIndex = 0;
            this.btnLoadAssembly.Text = "Load Shapes Assembly";
            this.btnLoadAssembly.UseVisualStyleBackColor = true;
            this.btnLoadAssembly.Click += new System.EventHandler(this.btnLoadAssembly_Click);
           
            this.lstShapes.Dock = System.Windows.Forms.DockStyle.Left;
            this.lstShapes.FormattingEnabled = true;
            this.lstShapes.ItemHeight = 16;
            this.lstShapes.Location = new System.Drawing.Point(0, 40);
            this.lstShapes.Name = "lstShapes";
            this.lstShapes.Size = new System.Drawing.Size(200, 410);
            this.lstShapes.TabIndex = 1;
            this.lstShapes.SelectedIndexChanged += new System.EventHandler(this.lstShapes_SelectedIndexChanged);
         
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(200, 40);
            this.splitContainer1.Name = "splitContainer1";
           
            this.splitContainer1.Panel1.Controls.Add(this.pnlConstructor);
          
            this.splitContainer1.Panel2.Controls.Add(this.pnlMethods);
            this.splitContainer1.Size = new System.Drawing.Size(600, 410);
            this.splitContainer1.SplitterDistance = 300;
            this.splitContainer1.TabIndex = 2;
          
            this.pnlConstructor.AutoScroll = true;
            this.pnlConstructor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlConstructor.Location = new System.Drawing.Point(0, 0);
            this.pnlConstructor.Name = "pnlConstructor";
            this.pnlConstructor.Size = new System.Drawing.Size(300, 410);
            this.pnlConstructor.TabIndex = 0;
         
            this.pnlMethods.AutoScroll = true;
            this.pnlMethods.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMethods.Location = new System.Drawing.Point(0, 0);
            this.pnlMethods.Name = "pnlMethods";
            this.pnlMethods.Size = new System.Drawing.Size(296, 410);
            this.pnlMethods.TabIndex = 0;
           
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.lstShapes);
            this.Controls.Add(this.btnLoadAssembly);
            this.Name = "MainForm";
            this.Text = "Shape Reflection App";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button btnLoadAssembly;
        private System.Windows.Forms.ListBox lstShapes;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel pnlConstructor;
        private System.Windows.Forms.Panel pnlMethods;
    }
}