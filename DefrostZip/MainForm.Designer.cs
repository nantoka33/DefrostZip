namespace DefrostZip
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
            listBoxFiles = new ListBox();
            checkBoxUsePassword = new CheckBox();
            textBoxPassword = new TextBox();
            labelExtractPath = new Label();
            btnSelectFolder = new Button();
            btnUnzip = new Button();
            progressBarUnzip = new ProgressBar();
            btnClear = new Button();
            SuspendLayout();
            // 
            // listBoxFiles
            // 
            listBoxFiles.FormattingEnabled = true;
            listBoxFiles.ItemHeight = 15;
            listBoxFiles.Location = new Point(12, 12);
            listBoxFiles.Name = "listBoxFiles";
            listBoxFiles.Size = new Size(460, 169);
            listBoxFiles.TabIndex = 0;
            // 
            // checkBoxUsePassword
            // 
            checkBoxUsePassword.AutoSize = true;
            checkBoxUsePassword.Location = new Point(12, 195);
            checkBoxUsePassword.Name = "checkBoxUsePassword";
            checkBoxUsePassword.Size = new Size(103, 19);
            checkBoxUsePassword.TabIndex = 1;
            checkBoxUsePassword.Text = "パスワードを使用";
            checkBoxUsePassword.UseVisualStyleBackColor = true;
            // 
            // textBoxPassword
            // 
            textBoxPassword.Location = new Point(138, 193);
            textBoxPassword.Name = "textBoxPassword";
            textBoxPassword.Size = new Size(334, 23);
            textBoxPassword.TabIndex = 2;
            // 
            // labelExtractPath
            // 
            labelExtractPath.AutoSize = true;
            labelExtractPath.Location = new Point(170, 273);
            labelExtractPath.Name = "labelExtractPath";
            labelExtractPath.Size = new Size(167, 15);
            labelExtractPath.TabIndex = 3;
            labelExtractPath.Text = "デフォルト：ZIPファイルと同じ場所";
            // 
            // btnSelectFolder
            // 
            btnSelectFolder.Location = new Point(12, 265);
            btnSelectFolder.Name = "btnSelectFolder";
            btnSelectFolder.Size = new Size(150, 30);
            btnSelectFolder.TabIndex = 4;
            btnSelectFolder.Text = "解凍先フォルダー選択";
            btnSelectFolder.UseVisualStyleBackColor = true;
            btnSelectFolder.Click += btnSelectFolder_Click;
            // 
            // btnUnzip
            // 
            btnUnzip.Location = new Point(12, 304);
            btnUnzip.Name = "btnUnzip";
            btnUnzip.Size = new Size(460, 35);
            btnUnzip.TabIndex = 5;
            btnUnzip.Text = "解凍開始";
            btnUnzip.UseVisualStyleBackColor = true;
            btnUnzip.Click += btnUnzip_Click;
            // 
            // progressBarUnzip
            // 
            progressBarUnzip.Location = new Point(12, 349);
            progressBarUnzip.Name = "progressBarUnzip";
            progressBarUnzip.Size = new Size(460, 23);
            progressBarUnzip.TabIndex = 6;
            // 
            // btnClear
            // 
            btnClear.Location = new Point(322, 230);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(150, 30);
            btnClear.TabIndex = 7;
            btnClear.Text = "ファイルリストクリア";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += btnClear_Click;
            // 
            // MainForm
            // 
            ClientSize = new Size(484, 382);
            Controls.Add(progressBarUnzip);
            Controls.Add(btnUnzip);
            Controls.Add(btnSelectFolder);
            Controls.Add(labelExtractPath);
            Controls.Add(textBoxPassword);
            Controls.Add(checkBoxUsePassword);
            Controls.Add(btnClear);
            Controls.Add(listBoxFiles);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "MainForm";
            Text = "自動解凍ツール";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ListBox listBoxFiles;
        private System.Windows.Forms.CheckBox checkBoxUsePassword;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.Label labelExtractPath;
        private System.Windows.Forms.Button btnSelectFolder;
        private System.Windows.Forms.Button btnUnzip;
        private System.Windows.Forms.ProgressBar progressBarUnzip;
        private System.Windows.Forms.Button btnClear;
    }
}
