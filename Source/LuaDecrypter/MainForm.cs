using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace LuaDecrypter
{
    public class MainForm : Form
    {
        private Decrypter decrypter;
        private IContainer components;
        private Button selectButton;
        private Button destinationButton;
        private ProgressBar progressBar;
        private TextBox statusBox;
        private Button decryptButton;
        private FolderBrowserDialog selectDestinationDialog;
        private OpenFileDialog selectFileDialog;

        public MainForm()
        {
            this.InitializeComponent();
            this.decrypter = new Decrypter();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            return;
        }

        private void selectButton_Click(object sender, EventArgs e)
        {
            int num = (int)this.selectFileDialog.ShowDialog((IWin32Window)this);
        }

        private void destinationButton_Click(object sender, EventArgs e)
        {
            if (this.selectFileDialog.FileNames.Length == 0)
            {
                this.statusBox.Text = "Please select some files";
            }
            else
            {
                int num = (int)this.selectDestinationDialog.ShowDialog((IWin32Window)this);
            }
        }

        private void decryptButton_Click(object sender, EventArgs e)
        {
            if (this.selectFileDialog.FileNames.Length == 0)
                this.statusBox.Text = "Please select some files";
            else if (this.selectDestinationDialog.SelectedPath.Length == 0)
            {
                this.statusBox.Text = "Please select a destination";
            }
            else
            {
                string[] fileNames = this.selectFileDialog.FileNames;
                string selectedPath = this.selectDestinationDialog.SelectedPath;
                this.progressBar.Value = 0;
                this.progressBar.Maximum = fileNames.Length * 10;
                this.statusBox.Text = "Decrypting files...";
                foreach (string path1 in fileNames)
                {
                    string path2 = selectedPath + "\\" + Path.GetFileName(path1);
                    if (File.Exists(path1))
                    {
                        Stream inStream = (Stream)new FileStream(path1, FileMode.Open, FileAccess.Read);
                        Stream outStream = (Stream)new FileStream(path2, FileMode.Create, FileAccess.Write);
                        if (this.decrypter.Decrypt(inStream, outStream, true, 4))
                            outStream.SetLength(outStream.Length - 1L);
                        inStream.Close();
                        outStream.Close();
                        this.progressBar.Value += 10;
                    }
                }
                this.statusBox.Text = "Decryption complete!";
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(MainForm));
            this.selectButton = new Button();
            this.destinationButton = new Button();
            this.progressBar = new ProgressBar();
            this.statusBox = new TextBox();
            this.decryptButton = new Button();
            this.selectDestinationDialog = new FolderBrowserDialog();
            this.selectFileDialog = new OpenFileDialog();
            this.SuspendLayout();
            this.selectButton.Location = new Point(12, 13);
            this.selectButton.Name = "selectButton";
            this.selectButton.Size = new Size(260, 23);
            this.selectButton.TabIndex = 0;
            this.selectButton.Text = "Select Files";
            this.selectButton.UseVisualStyleBackColor = true;
            this.selectButton.Click += new EventHandler(this.selectButton_Click);
            this.destinationButton.Location = new Point(12, 42);
            this.destinationButton.Name = "destinationButton";
            this.destinationButton.Size = new Size(260, 23);
            this.destinationButton.TabIndex = 1;
            this.destinationButton.Text = "Select Destination";
            this.destinationButton.UseVisualStyleBackColor = true;
            this.destinationButton.Click += new EventHandler(this.destinationButton_Click);
            this.progressBar.Location = new Point(13, 102);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new Size(258, 21);
            this.progressBar.Style = ProgressBarStyle.Continuous;
            this.progressBar.TabIndex = 2;
            this.statusBox.Enabled = false;
            this.statusBox.Location = new Point(13, 131);
            this.statusBox.Name = "statusBox";
            this.statusBox.Size = new Size(258, 20);
            this.statusBox.TabIndex = 3;
            this.statusBox.TabStop = false;
            this.statusBox.TextAlign = HorizontalAlignment.Center;
            this.decryptButton.Location = new Point(12, 71);
            this.decryptButton.Name = "decryptButton";
            this.decryptButton.Size = new Size(260, 23);
            this.decryptButton.TabIndex = 4;
            this.decryptButton.Text = "Decrypt";
            this.decryptButton.UseVisualStyleBackColor = true;
            this.decryptButton.Click += new EventHandler(this.decryptButton_Click);
            this.selectFileDialog.Filter = "Lua files (.lua)|*.lua";
            this.selectFileDialog.Multiselect = true;
            this.AutoScaleDimensions = new SizeF(6f, 13f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(284, 163);
            this.Controls.Add((Control)this.decryptButton);
            this.Controls.Add((Control)this.statusBox);
            this.Controls.Add((Control)this.progressBar);
            this.Controls.Add((Control)this.destinationButton);
            this.Controls.Add((Control)this.selectButton);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            //this.Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Text = "LuaDecrypter";
            this.Load += new EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}