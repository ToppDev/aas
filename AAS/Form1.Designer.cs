namespace AAS
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.scintilla = new ScintillaNET.Scintilla();
            this.autocompleteMenu1 = new AutocompleteMenuNS.AutocompleteMenu();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.pibo_wait_key = new System.Windows.Forms.PictureBox();
            this.pibo_wait_window = new System.Windows.Forms.PictureBox();
            this.pibo_wait_color = new System.Windows.Forms.PictureBox();
            this.pibo_wait_time = new System.Windows.Forms.PictureBox();
            this.pibo_select_window = new System.Windows.Forms.PictureBox();
            this.pibo_select_color = new System.Windows.Forms.PictureBox();
            this.pibo_select_coordinate = new System.Windows.Forms.PictureBox();
            this.pibo_record = new System.Windows.Forms.PictureBox();
            this.pibo_open = new System.Windows.Forms.PictureBox();
            this.pibo_save = new System.Windows.Forms.PictureBox();
            this.pibo_compile = new System.Windows.Forms.PictureBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.tabControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pibo_wait_key)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pibo_wait_window)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pibo_wait_color)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pibo_wait_time)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pibo_select_window)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pibo_select_color)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pibo_select_coordinate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pibo_record)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pibo_open)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pibo_save)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pibo_compile)).BeginInit();
            this.SuspendLayout();
            // 
            // scintilla
            // 
            this.scintilla.AutoCSeparator = '|';
            this.scintilla.Lexer = ScintillaNET.Lexer.Cpp;
            this.scintilla.Location = new System.Drawing.Point(0, 19);
            this.scintilla.Margin = new System.Windows.Forms.Padding(2);
            this.scintilla.Name = "scintilla";
            this.scintilla.ScrollWidth = 200;
            this.scintilla.Size = new System.Drawing.Size(439, 430);
            this.scintilla.TabIndex = 0;
            this.scintilla.Text = "namespace AAS\r\n{\r\n    public class Class\r\n    {\r\n        public void Main()\r\n    " +
    "    {\r\n            \r\n        }\r\n    }\r\n}";
            // 
            // autocompleteMenu1
            // 
            this.autocompleteMenu1.Colors = ((AutocompleteMenuNS.Colors)(resources.GetObject("autocompleteMenu1.Colors")));
            this.autocompleteMenu1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.autocompleteMenu1.ImageList = null;
            this.autocompleteMenu1.Items = new string[0];
            this.autocompleteMenu1.TargetControlWrapper = null;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Location = new System.Drawing.Point(0, -1);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(419, 32);
            this.tabControl1.TabIndex = 1;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage1.Size = new System.Drawing.Size(411, 6);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Main";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage2.Size = new System.Drawing.Size(411, 6);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Reference";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(411, 6);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "HookManager";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(411, 6);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "AssemblyNames";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tabPage5
            // 
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(411, 6);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Credits";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // pibo_wait_key
            // 
            this.pibo_wait_key.Image = global::AAS.Properties.Resources.wait_key;
            this.pibo_wait_key.Location = new System.Drawing.Point(442, 409);
            this.pibo_wait_key.Margin = new System.Windows.Forms.Padding(2);
            this.pibo_wait_key.Name = "pibo_wait_key";
            this.pibo_wait_key.Size = new System.Drawing.Size(33, 32);
            this.pibo_wait_key.TabIndex = 12;
            this.pibo_wait_key.TabStop = false;
            this.toolTip1.SetToolTip(this.pibo_wait_key, "Wait for key");
            this.pibo_wait_key.Click += new System.EventHandler(this.pibo_wait_key_Click);
            // 
            // pibo_wait_window
            // 
            this.pibo_wait_window.Image = global::AAS.Properties.Resources.wait_window;
            this.pibo_wait_window.Location = new System.Drawing.Point(442, 370);
            this.pibo_wait_window.Margin = new System.Windows.Forms.Padding(2);
            this.pibo_wait_window.Name = "pibo_wait_window";
            this.pibo_wait_window.Size = new System.Drawing.Size(33, 32);
            this.pibo_wait_window.TabIndex = 11;
            this.pibo_wait_window.TabStop = false;
            this.toolTip1.SetToolTip(this.pibo_wait_window, "Wait for active window");
            this.pibo_wait_window.Click += new System.EventHandler(this.pibo_wait_window_Click);
            // 
            // pibo_wait_color
            // 
            this.pibo_wait_color.Image = global::AAS.Properties.Resources.wait_color;
            this.pibo_wait_color.Location = new System.Drawing.Point(442, 331);
            this.pibo_wait_color.Margin = new System.Windows.Forms.Padding(2);
            this.pibo_wait_color.Name = "pibo_wait_color";
            this.pibo_wait_color.Size = new System.Drawing.Size(33, 32);
            this.pibo_wait_color.TabIndex = 10;
            this.pibo_wait_color.TabStop = false;
            this.toolTip1.SetToolTip(this.pibo_wait_color, "Wait for color");
            this.pibo_wait_color.Click += new System.EventHandler(this.pibo_wait_color_Click);
            // 
            // pibo_wait_time
            // 
            this.pibo_wait_time.Image = global::AAS.Properties.Resources.Hourglass_32;
            this.pibo_wait_time.Location = new System.Drawing.Point(442, 292);
            this.pibo_wait_time.Margin = new System.Windows.Forms.Padding(2);
            this.pibo_wait_time.Name = "pibo_wait_time";
            this.pibo_wait_time.Size = new System.Drawing.Size(33, 32);
            this.pibo_wait_time.TabIndex = 9;
            this.pibo_wait_time.TabStop = false;
            this.toolTip1.SetToolTip(this.pibo_wait_time, "Wait time");
            this.pibo_wait_time.Click += new System.EventHandler(this.pibo_wait_time_Click);
            // 
            // pibo_select_window
            // 
            this.pibo_select_window.Image = global::AAS.Properties.Resources.Coherence;
            this.pibo_select_window.Location = new System.Drawing.Point(442, 253);
            this.pibo_select_window.Margin = new System.Windows.Forms.Padding(2);
            this.pibo_select_window.Name = "pibo_select_window";
            this.pibo_select_window.Size = new System.Drawing.Size(33, 32);
            this.pibo_select_window.TabIndex = 8;
            this.pibo_select_window.TabStop = false;
            this.toolTip1.SetToolTip(this.pibo_select_window, "Select window");
            this.pibo_select_window.Click += new System.EventHandler(this.pibo_select_window_Click);
            // 
            // pibo_select_color
            // 
            this.pibo_select_color.Image = global::AAS.Properties.Resources.pipette_2;
            this.pibo_select_color.Location = new System.Drawing.Point(442, 214);
            this.pibo_select_color.Margin = new System.Windows.Forms.Padding(2);
            this.pibo_select_color.Name = "pibo_select_color";
            this.pibo_select_color.Size = new System.Drawing.Size(33, 32);
            this.pibo_select_color.TabIndex = 7;
            this.pibo_select_color.TabStop = false;
            this.toolTip1.SetToolTip(this.pibo_select_color, "Select color");
            this.pibo_select_color.Click += new System.EventHandler(this.pibo_select_color_Click);
            // 
            // pibo_select_coordinate
            // 
            this.pibo_select_coordinate.Image = global::AAS.Properties.Resources.crosshairs;
            this.pibo_select_coordinate.Location = new System.Drawing.Point(442, 175);
            this.pibo_select_coordinate.Margin = new System.Windows.Forms.Padding(2);
            this.pibo_select_coordinate.Name = "pibo_select_coordinate";
            this.pibo_select_coordinate.Size = new System.Drawing.Size(33, 32);
            this.pibo_select_coordinate.TabIndex = 6;
            this.pibo_select_coordinate.TabStop = false;
            this.toolTip1.SetToolTip(this.pibo_select_coordinate, "Select coordinate");
            this.pibo_select_coordinate.Click += new System.EventHandler(this.pibo_select_coordinate_Click);
            // 
            // pibo_record
            // 
            this.pibo_record.Image = global::AAS.Properties.Resources.Record_Button;
            this.pibo_record.Location = new System.Drawing.Point(442, 136);
            this.pibo_record.Margin = new System.Windows.Forms.Padding(2);
            this.pibo_record.Name = "pibo_record";
            this.pibo_record.Size = new System.Drawing.Size(33, 32);
            this.pibo_record.TabIndex = 5;
            this.pibo_record.TabStop = false;
            this.toolTip1.SetToolTip(this.pibo_record, "Start recording");
            this.pibo_record.Click += new System.EventHandler(this.pibo_record_Click);
            // 
            // pibo_open
            // 
            this.pibo_open.Image = global::AAS.Properties.Resources.open_alt;
            this.pibo_open.Location = new System.Drawing.Point(442, 97);
            this.pibo_open.Margin = new System.Windows.Forms.Padding(2);
            this.pibo_open.Name = "pibo_open";
            this.pibo_open.Size = new System.Drawing.Size(33, 32);
            this.pibo_open.TabIndex = 4;
            this.pibo_open.TabStop = false;
            this.toolTip1.SetToolTip(this.pibo_open, "Open");
            this.pibo_open.Click += new System.EventHandler(this.pibo_open_Click);
            // 
            // pibo_save
            // 
            this.pibo_save.Image = global::AAS.Properties.Resources.save_32;
            this.pibo_save.Location = new System.Drawing.Point(442, 58);
            this.pibo_save.Margin = new System.Windows.Forms.Padding(2);
            this.pibo_save.Name = "pibo_save";
            this.pibo_save.Size = new System.Drawing.Size(33, 32);
            this.pibo_save.TabIndex = 3;
            this.pibo_save.TabStop = false;
            this.toolTip1.SetToolTip(this.pibo_save, "Save");
            this.pibo_save.Click += new System.EventHandler(this.pibo_save_Click);
            // 
            // pibo_compile
            // 
            this.pibo_compile.Image = global::AAS.Properties.Resources.StepForwardNormalBlue;
            this.pibo_compile.Location = new System.Drawing.Point(442, 19);
            this.pibo_compile.Margin = new System.Windows.Forms.Padding(2);
            this.pibo_compile.Name = "pibo_compile";
            this.pibo_compile.Size = new System.Drawing.Size(33, 32);
            this.pibo_compile.TabIndex = 2;
            this.pibo_compile.TabStop = false;
            this.toolTip1.SetToolTip(this.pibo_compile, "Compile");
            this.pibo_compile.Click += new System.EventHandler(this.pibo_compile_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "AAS-Files|*.aas|All Files|*.*";
            this.openFileDialog1.Title = "Open AAS-File";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "AAS-Files|*.aas|All Files|*.*";
            this.saveFileDialog1.Title = "Save AAS-File";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(483, 448);
            this.Controls.Add(this.pibo_wait_key);
            this.Controls.Add(this.pibo_wait_window);
            this.Controls.Add(this.pibo_wait_color);
            this.Controls.Add(this.pibo_wait_time);
            this.Controls.Add(this.pibo_select_window);
            this.Controls.Add(this.pibo_select_color);
            this.Controls.Add(this.pibo_select_coordinate);
            this.Controls.Add(this.pibo_record);
            this.Controls.Add(this.pibo_open);
            this.Controls.Add(this.pibo_save);
            this.Controls.Add(this.pibo_compile);
            this.Controls.Add(this.scintilla);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "AAS";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            this.tabControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pibo_wait_key)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pibo_wait_window)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pibo_wait_color)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pibo_wait_time)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pibo_select_window)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pibo_select_color)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pibo_select_coordinate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pibo_record)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pibo_open)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pibo_save)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pibo_compile)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.PictureBox pibo_compile;
        private System.Windows.Forms.PictureBox pibo_save;
        private System.Windows.Forms.PictureBox pibo_open;
        private System.Windows.Forms.PictureBox pibo_record;
        private System.Windows.Forms.PictureBox pibo_select_coordinate;
        private System.Windows.Forms.PictureBox pibo_select_color;
        private System.Windows.Forms.PictureBox pibo_select_window;
        private System.Windows.Forms.PictureBox pibo_wait_time;
        private System.Windows.Forms.PictureBox pibo_wait_color;
        private System.Windows.Forms.PictureBox pibo_wait_window;
        private System.Windows.Forms.PictureBox pibo_wait_key;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.ToolTip toolTip1;
        public ScintillaNET.Scintilla scintilla;
        public AutocompleteMenuNS.AutocompleteMenu autocompleteMenu1;
    }
}

