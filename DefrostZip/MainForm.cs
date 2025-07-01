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
        private string selectedExtractPath = "";  // �𓀐�p�X

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
                folderDialog.Description = "�𓀐�t�H���_�[��I�����Ă�������";
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
                MessageBox.Show("�t�@�C�����h���b�O���h���b�v���Ă��������B");
                return;
            }

            var password = textBoxPassword.Text;

            // �SZIP�t�@�C���̍��v�G���g�����J�E���g
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
                MessageBox.Show("ZIP�t�@�C����������Ȃ����A��ł��B");
                return;
            }

            // ProgressBar �����ݒ�
            progressBarUnzip.Minimum = 0;
            progressBarUnzip.Maximum = totalEntryCount;
            progressBarUnzip.Value = 0;

            var processedEntries = 0;

            // �񓯊����s
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
                                MessageBox.Show($"�𓀂Ɏ��s���܂���: {file}\n{ex.Message}");
                            }));
                        }
                    }
                }
            });

            // ������
            progressBarUnzip.Value = progressBarUnzip.Maximum;
            MessageBox.Show("�SZIP�t�@�C���̉𓀂��������܂����I");
        }

        private void btnClear_Click(object? sender, EventArgs e)
        {
            listBoxFiles.Items.Clear();
            progressBarUnzip.Value = 0;
            selectedExtractPath = "";
            labelExtractPath.Text = "�f�t�H���g�FZIP�t�@�C���Ɠ����ꏊ";
        }
    }
}
