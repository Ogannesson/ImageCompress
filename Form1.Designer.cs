using System.Drawing.Imaging;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Png;
using System.Threading.Tasks;
using System.Threading;

namespace ImageCompress
{
    partial class Form1 : Form
    {
        private System.ComponentModel.IContainer components = null;

        // 控制任务的相关变量
        private CancellationTokenSource cancellationTokenSource;
        private ManualResetEventSlim pauseEvent;
        private bool isCompressing = false; // 标记当前是否正在压缩
        private bool isPaused = false; // 标记当前是否处于暂停状态

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            inputDirTextBox = new TextBox();
            outputDirTextBox = new TextBox();
            SelectInputDirButton = new Button();
            SelectOutputDirButton = new Button();
            StartCompressionButton = new Button();
            PauseCompressionButton = new Button();
            ResumeCompressionButton = new Button();
            StopCompressionButton = new Button();
            progressBar = new ProgressBar();
            sizeTextBox = new TextBox();
            scaleComboBox = new ComboBox();
            fileNameLabel = new Label();
            progressInfoLabel = new Label();
            SuspendLayout();
            // 
            // inputDirTextBox
            // 
            inputDirTextBox.Location = new Point(118, 53);
            inputDirTextBox.Name = "inputDirTextBox";
            inputDirTextBox.Size = new Size(446, 23);
            inputDirTextBox.TabIndex = 0;
            // 
            // outputDirTextBox
            // 
            outputDirTextBox.Location = new Point(118, 152);
            outputDirTextBox.Name = "outputDirTextBox";
            outputDirTextBox.Size = new Size(446, 23);
            outputDirTextBox.TabIndex = 1;
            // 
            // SelectInputDirButton
            // 
            SelectInputDirButton.Location = new Point(604, 53);
            SelectInputDirButton.Name = "SelectInputDirButton";
            SelectInputDirButton.Size = new Size(91, 23);
            SelectInputDirButton.TabIndex = 2;
            SelectInputDirButton.Text = "选择输入目录";
            SelectInputDirButton.UseVisualStyleBackColor = true;
            SelectInputDirButton.Click += SelectInputDirButton_Click;
            // 
            // SelectOutputDirButton
            // 
            SelectOutputDirButton.Location = new Point(604, 152);
            SelectOutputDirButton.Name = "SelectOutputDirButton";
            SelectOutputDirButton.Size = new Size(91, 23);
            SelectOutputDirButton.TabIndex = 3;
            SelectOutputDirButton.Text = "选择输出目录";
            SelectOutputDirButton.UseVisualStyleBackColor = true;
            SelectOutputDirButton.Click += SelectOutputDirButton_Click;
            // 
            // StartCompressionButton
            // 
            StartCompressionButton.Location = new Point(118, 389);
            StartCompressionButton.Name = "StartCompressionButton";
            StartCompressionButton.Size = new Size(91, 23);
            StartCompressionButton.TabIndex = 4;
            StartCompressionButton.Text = "开始压缩";
            StartCompressionButton.UseVisualStyleBackColor = true;
            StartCompressionButton.Click += StartCompressionButton_Click;
            // 
            // PauseCompressionButton
            // 
            PauseCompressionButton.Location = new Point(241, 389);
            PauseCompressionButton.Name = "PauseCompressionButton";
            PauseCompressionButton.Size = new Size(91, 23);
            PauseCompressionButton.TabIndex = 5;
            PauseCompressionButton.Text = "暂停压缩";
            PauseCompressionButton.UseVisualStyleBackColor = true;
            PauseCompressionButton.Click += PauseCompressionButton_Click;
            // 
            // ResumeCompressionButton
            // 
            ResumeCompressionButton.Location = new Point(356, 389);
            ResumeCompressionButton.Name = "ResumeCompressionButton";
            ResumeCompressionButton.Size = new Size(91, 23);
            ResumeCompressionButton.TabIndex = 6;
            ResumeCompressionButton.Text = "继续压缩";
            ResumeCompressionButton.UseVisualStyleBackColor = true;
            ResumeCompressionButton.Click += ResumeCompressionButton_Click;
            // 
            // StopCompressionButton
            // 
            StopCompressionButton.Location = new Point(473, 389);
            StopCompressionButton.Name = "StopCompressionButton";
            StopCompressionButton.Size = new Size(91, 23);
            StopCompressionButton.TabIndex = 7;
            StopCompressionButton.Text = "结束压缩";
            StopCompressionButton.UseVisualStyleBackColor = true;
            StopCompressionButton.Click += StopCompressionButton_Click;
            // 
            // progressBar
            // 
            progressBar.Location = new Point(118, 293);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(446, 23);
            progressBar.TabIndex = 8;
            // 
            // sizeTextBox
            // 
            sizeTextBox.Location = new Point(118, 238);
            sizeTextBox.Name = "sizeTextBox";
            sizeTextBox.PlaceholderText = "目标大小 (MB)";
            sizeTextBox.Size = new Size(150, 23);
            sizeTextBox.TabIndex = 10;
            // 
            // scaleComboBox
            // 
            scaleComboBox.Items.AddRange(new object[] { "1", "1/2", "1/4", "1/8" });
            scaleComboBox.Location = new Point(371, 238);
            scaleComboBox.Name = "scaleComboBox";
            scaleComboBox.Size = new Size(76, 25);
            scaleComboBox.TabIndex = 11;
            scaleComboBox.Text = "压缩倍率";
            // 
            // fileNameLabel
            // 
            fileNameLabel.AutoSize = true;
            fileNameLabel.Location = new Point(118, 325);
            fileNameLabel.Name = "fileNameLabel";
            fileNameLabel.Size = new Size(65, 17);
            fileNameLabel.TabIndex = 12;
            fileNameLabel.Text = "等待开始...";
            // 
            // progressInfoLabel
            // 
            progressInfoLabel.AutoSize = true;
            progressInfoLabel.Location = new Point(371, 325);
            progressInfoLabel.Name = "progressInfoLabel";
            progressInfoLabel.Size = new Size(56, 17);
            progressInfoLabel.TabIndex = 13;
            progressInfoLabel.Text = "进度信息";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 500);
            Controls.Add(fileNameLabel);
            Controls.Add(progressInfoLabel);
            Controls.Add(progressBar);
            Controls.Add(StartCompressionButton);
            Controls.Add(PauseCompressionButton);
            Controls.Add(ResumeCompressionButton);
            Controls.Add(StopCompressionButton);
            Controls.Add(SelectOutputDirButton);
            Controls.Add(SelectInputDirButton);
            Controls.Add(outputDirTextBox);
            Controls.Add(inputDirTextBox);
            Controls.Add(sizeTextBox);
            Controls.Add(scaleComboBox);
            Name = "Form1";
            Text = "图像压缩";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        // 更新按钮的启用/禁用状态
        private void UpdateButtonStates()
        {
            StartCompressionButton.Enabled = !isCompressing;
            PauseCompressionButton.Enabled = isCompressing && !isPaused;
            ResumeCompressionButton.Enabled = isPaused;
            StopCompressionButton.Enabled = isCompressing;
        }

        private TextBox inputDirTextBox;
        private TextBox outputDirTextBox;
        private Button SelectInputDirButton;
        private Button SelectOutputDirButton;
        private Button StartCompressionButton;
        private Button PauseCompressionButton;
        private Button ResumeCompressionButton;
        private Button StopCompressionButton;
        private ProgressBar progressBar;
        private Label fileNameLabel;
        private Label progressInfoLabel;
        private TextBox sizeTextBox;
        private ComboBox scaleComboBox;

        private void SelectInputDirButton_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    inputDirTextBox.Text = fbd.SelectedPath;
                }
            }
        }

        private void SelectOutputDirButton_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    outputDirTextBox.Text = fbd.SelectedPath;
                }
            }
        }

        // 点击开始压缩按钮
        private async void StartCompressionButton_Click(object sender, EventArgs e)
        {
            isCompressing = true;
            isPaused = false;
            UpdateButtonStates();
            cancellationTokenSource = new CancellationTokenSource();
            pauseEvent = new ManualResetEventSlim(true); // 初始状态不暂停

            string inputDir = inputDirTextBox.Text;
            string outputDir = outputDirTextBox.Text;
            float targetSizeMB = 0;
            bool compressToSize = false;

            if (!string.IsNullOrEmpty(sizeTextBox.Text) && float.TryParse(sizeTextBox.Text, out targetSizeMB))
            {
                compressToSize = true;
            }

            if (string.IsNullOrEmpty(inputDir) || string.IsNullOrEmpty(outputDir))
            {
                MessageBox.Show("请确保输入和输出目录都已选择。");
                return;
            }

            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            var files = Directory.EnumerateFiles(inputDir, "*.jpg", SearchOption.AllDirectories)
                .Concat(Directory.EnumerateFiles(inputDir, "*.png", SearchOption.AllDirectories))
                .Concat(Directory.EnumerateFiles(inputDir, "*.bmp", SearchOption.AllDirectories))
                .ToArray();

            int totalFiles = files.Length;
            progressBar.Maximum = totalFiles;
            progressBar.Value = 0;

            DateTime startTime = DateTime.Now;
            object lockObj = new object();

            var parallelOptions = new ParallelOptions
            {
                CancellationToken = cancellationTokenSource.Token,
                MaxDegreeOfParallelism = -1 // 不限制并发
            };

            try
            {
                await Task.Run(() =>
                {
                    Parallel.ForEach(files, parallelOptions, file =>
                    {
                        try
                        {
                            // 检查是否被请求取消
                            parallelOptions.CancellationToken.ThrowIfCancellationRequested();

                            pauseEvent.Wait(parallelOptions.CancellationToken); // 支持暂停

                            using (var image = SixLabors.ImageSharp.Image.Load(file))
                            {
                                // 删除元数据和多余通道
                                image.Metadata.ExifProfile = null;
                                if (image.PixelType.BitsPerPixel > 24)
                                {
                                    image.Mutate(x => x.BackgroundColor(SixLabors.ImageSharp.Color.White));
                                }

                                string selectedScale = scaleComboBox.SelectedItem?.ToString() ?? "1";
                                float scale = selectedScale switch
                                {
                                    "1" => 1f,
                                    "1/2" => 0.5f,
                                    "1/4" => 0.25f,
                                    "1/8" => 0.125f,
                                    _ => 1f
                                };

                                if (scale < 1f)
                                {
                                    image.Mutate(x => x.Resize((int)(image.Width * scale), (int)(image.Height * scale)));
                                }

                                var relativePath = Path.GetRelativePath(inputDir, file);
                                var outputFilePath = Path.Combine(outputDir, relativePath);
                                var outputDirectory = Path.GetDirectoryName(outputFilePath);

                                if (!Directory.Exists(outputDirectory))
                                {
                                    Directory.CreateDirectory(outputDirectory);
                                }

                                // 保存文件
                                if (file.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase))
                                {
                                    SaveJpegWithSize(image, outputFilePath, targetSizeMB, compressToSize);
                                }
                                else if (file.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                                {
                                    SavePng(image, outputFilePath);
                                }
                                else if (file.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase))
                                {
                                    SaveBmp(image, outputFilePath);
                                }
                            }
                        }
                        catch (OperationCanceledException)
                        {
                            // 任务被取消，优雅退出
                            return;
                        }
                        catch (Exception ex)
                        {
                            Invoke(new Action(() =>
                            {
                                MessageBox.Show($"压缩文件 {file} 时出错: {ex.Message}\n{ex.StackTrace}");
                                isPaused = true;
                                pauseEvent.Reset();
                                progressInfoLabel.Text = "出现错误，任务已暂停";
                            }));
                        }

                        lock (lockObj)
                        {
                            progressBar.Invoke(new Action(() =>
                            {
                                progressBar.Value += 1;

                                if (progressBar.Value % 10 == 0) // 每处理10个文件更新一次UI
                                {
                                    double progressPercentage = (double)progressBar.Value / totalFiles * 100;
                                    TimeSpan elapsed = DateTime.Now - startTime;
                                    TimeSpan estimatedTotalTime = TimeSpan.FromTicks(elapsed.Ticks * totalFiles / progressBar.Value);
                                    TimeSpan remainingTime = estimatedTotalTime - elapsed;

                                    fileNameLabel.Text = $"正在压缩: {Path.GetFileName(file)}";
                                    progressInfoLabel.Text = $"{progressPercentage:F2}% 完成 - 预计剩余时间: {remainingTime:mm\\:ss}";
                                }
                            }));
                        }
                    });
                });
            }
            catch (OperationCanceledException)
            {
                // 正常处理任务取消后的逻辑
                MessageBox.Show("压缩任务已取消。");
            }

            isCompressing = false;
            isPaused = false;
            UpdateButtonStates();
            this.Invoke(new Action(() => progressInfoLabel.Text = "压缩完成！"));
        }


        // 保存JPEG并尝试达到目标大小
        private void SaveJpegWithSize(SixLabors.ImageSharp.Image image, string outputFilePath, float targetSizeMB, bool compressToSize)
        {
            if (compressToSize)
            {
                int low = 10, high = 90;
                while (low <= high)
                {
                    int quality = (low + high) / 2;
                    using (var ms = new MemoryStream())
                    {
                        var encoder = new JpegEncoder { Quality = quality };
                        image.Save(ms, encoder);

                        if (ms.Length / 1024f / 1024f <= targetSizeMB || quality <= 10)
                        {
                            File.WriteAllBytes(outputFilePath, ms.ToArray());
                            break;
                        }

                        if (ms.Length / 1024f / 1024f > targetSizeMB)
                        {
                            high = quality - 1;
                        }
                        else
                        {
                            low = quality + 1;
                        }
                    }
                }
            }
            else
            {
                var encoder = new JpegEncoder { Quality = 90 };
                using (var outputStream = File.Create(outputFilePath))
                {
                    image.Save(outputStream, encoder);
                }
            }
        }

        private void SavePng(SixLabors.ImageSharp.Image image, string outputFilePath)
        {
            var encoder = new PngEncoder
            {
                CompressionLevel = PngCompressionLevel.BestCompression
            };
            using (var outputStream = File.Create(outputFilePath))
            {
                image.Save(outputStream, encoder);
            }
        }

        private void SaveBmp(SixLabors.ImageSharp.Image image, string outputFilePath)
        {
            var encoder = new BmpEncoder();
            using (var outputStream = File.Create(outputFilePath))
            {
                image.Save(outputStream, encoder);
            }
        }

        // 暂停任务
        private void PauseCompressionButton_Click(object sender, EventArgs e)
        {
            isPaused = true;
            pauseEvent.Reset(); // 设置暂停状态
            progressInfoLabel.Text = "任务已暂停";
            UpdateButtonStates();
        }

        // 继续任务
        private void ResumeCompressionButton_Click(object sender, EventArgs e)
        {
            isPaused = false;
            pauseEvent.Set(); // 恢复任务
            progressInfoLabel.Text = "任务继续进行";
            UpdateButtonStates();
        }

        // 结束任务
        private void StopCompressionButton_Click(object sender, EventArgs e)
        {
            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel(); // 取消任务
                isCompressing = false;
                isPaused = false;
                UpdateButtonStates();
                progressInfoLabel.Text = "任务已取消";
                fileNameLabel.Text = "等待开始...";
            }
        }
    }
}
