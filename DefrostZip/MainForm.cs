using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpCompress.Archives;
using SharpCompress.Archives.Zip;
using SharpCompress.Common;
using SharpCompress.Readers;

namespace DefrostZip
{
    public partial class MainForm : Form
    {
        private string selectedExtractPath = "";  // 解凍先パス

        public MainForm()
        {
            InitializeComponent();
            this.AllowDrop = true;
            this.DragEnter += new DragEventHandler(Form_DragEnter);
            this.DragDrop += new DragEventHandler(Form_DragDrop);
        }

        private void Form_DragEnter(object? sender, DragEventArgs e)
        {
            if (e.Data?.GetDataPresent(DataFormats.FileDrop) == true)
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void Form_DragDrop(object? sender, DragEventArgs e)
        {
            var data = e.Data?.GetData(DataFormats.FileDrop);

            if (data is string[] files)
            {
                foreach (string file in files)
                {
                    listBoxFiles.Items.Add(file);
                }
            }
        }

        private void btnSelectFolder_Click(object? sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "解凍先フォルダーを選択してください";
                folderDialog.ShowNewFolderButton = true;

                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    selectedExtractPath = folderDialog.SelectedPath;
                    labelExtractPath.Text = selectedExtractPath;
                }
            }
        }

        private async void btnUnzip_Click(object sender, EventArgs e)
        {
            if (listBoxFiles.Items.Count == 0)
            {
                MessageBox.Show("ファイルをドラッグ＆ドロップしてください。");
                return;
            }

            var password = textBoxPassword.Text;

            // 全ZIPファイルの合計エントリ数カウント
            var totalEntryCount = 0;
            foreach (string file in listBoxFiles.Items)
            {
                if (Path.GetExtension(file).Equals(".zip", StringComparison.OrdinalIgnoreCase))
                {
                    using (var archive = ZipArchive.Open(file, new ReaderOptions()
                    {
                        ArchiveEncoding = new ArchiveEncoding()
                        {
                            Default = System.Text.Encoding.GetEncoding("shift-jis")
                        },
                        Password = string.IsNullOrEmpty(password) ? null : password
                    }))
                    {
                        totalEntryCount += archive.Entries.Count(entry => !entry.IsDirectory);
                    }
                }
            }

            if (totalEntryCount == 0)
            {
                MessageBox.Show("ZIPファイルが見つからないか、空です。");
                return;
            }

            // ProgressBar 初期設定
            progressBarUnzip.Minimum = 0;
            progressBarUnzip.Maximum = totalEntryCount;
            progressBarUnzip.Value = 0;

            var processedEntries = 0;

            // 非同期実行
            await Task.Run(() =>
            {
                foreach (string file in listBoxFiles.Items)
                {
                    if (Path.GetExtension(file).Equals(".zip", StringComparison.OrdinalIgnoreCase))
                    {
                        try
                        {
                            using (var archive = ZipArchive.Open(file, new ReaderOptions()
                            {
                                ArchiveEncoding = new ArchiveEncoding()
                                {
                                    Default = System.Text.Encoding.GetEncoding("shift-jis")
                                },
                                Password = string.IsNullOrEmpty(password) ? null : password
                            }))
                            {
                                string extractPath;
                                string? directoryName = Path.GetDirectoryName(file) ?? "";
                                if (!string.IsNullOrEmpty(selectedExtractPath))
                                {
                                    extractPath = Path.Combine(selectedExtractPath, Path.GetFileNameWithoutExtension(file));
                                }
                                else
                                {
                                    extractPath = Path.Combine(directoryName, Path.GetFileNameWithoutExtension(file));
                                }

                                Directory.CreateDirectory(extractPath);

                                foreach (var entry in archive.Entries.Where(entry => !entry.IsDirectory))
                                {
                                    entry.WriteToDirectory(extractPath, new ExtractionOptions()
                                    {
                                        ExtractFullPath = true,
                                        Overwrite = true
                                    });

                                    processedEntries++;

                                    Invoke(new Action(() =>
                                    {
                                        progressBarUnzip.Value = processedEntries;
                                        progressBarUnzip.Refresh();
                                    }));
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Invoke(new Action(() =>
                            {
                                MessageBox.Show($"解凍に失敗しました: {file}\n{ex.Message}");
                            }));
                        }
                    }
                }
            });

            // 完了時
            progressBarUnzip.Value = progressBarUnzip.Maximum;
            MessageBox.Show("全ZIPファイルの解凍が完了しました！");
        }

        private void btnClear_Click(object? sender, EventArgs e)
        {
            listBoxFiles.Items.Clear();
            progressBarUnzip.Value = 0;
            selectedExtractPath = "";
            labelExtractPath.Text = "デフォルト：ZIPファイルと同じ場所";
        }
    }
}
