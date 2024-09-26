using System.Drawing.Imaging;
using SD = System.Drawing; // 为 System.Drawing 命名空间创建别名
using SixLabors.ImageSharp; // 使用 SixLabors.ImageSharp 作为默认图像处理库
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing; // 引入处理命名空间
using System.Threading;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Png;

namespace ImageCompress
{
    partial class Form1 : Form
    {
        private System.ComponentModel.IContainer components = null;

        // 控制任务的相关变量
        private CancellationTokenSource cancellationTokenSource;
        private ManualResetEventSlim pauseEvent;

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
            infoLabel = new Label();
            sizeTextBox = new TextBox();
            scaleComboBox = new ComboBox();
            SuspendLayout();
            // 
            // inputDirTextBox
            // 
            inputDirTextBox.Location = new SD.Point(118, 53);
            inputDirTextBox.Name = "inputDirTextBox";
            inputDirTextBox.Size = new SD.Size(446, 23);
            inputDirTextBox.TabIndex = 0;
            // 
            // outputDirTextBox
            // 
            outputDirTextBox.Location = new SD.Point(118, 152);
            outputDirTextBox.Name = "outputDirTextBox";
            outputDirTextBox.Size = new SD.Size(446, 23);
            outputDirTextBox.TabIndex = 1;
            // 
            // SelectInputDirButton
            // 
            SelectInputDirButton.Location = new SD.Point(604, 53);
            SelectInputDirButton.Name = "SelectInputDirButton";
            SelectInputDirButton.Size = new SD.Size(91, 23);
            SelectInputDirButton.TabIndex = 2;
            SelectInputDirButton.Text = "选择输入目录";
            SelectInputDirButton.UseVisualStyleBackColor = true;
            SelectInputDirButton.Click += SelectInputDirButton_Click;
            // 
            // SelectOutputDirButton
            // 
            SelectOutputDirButton.Location = new SD.Point(604, 152);
            SelectOutputDirButton.Name = "SelectOutputDirButton";
            SelectOutputDirButton.Size = new SD.Size(91, 23);
            SelectOutputDirButton.TabIndex = 3;
            SelectOutputDirButton.Text = "选择输出目录";
            SelectOutputDirButton.UseVisualStyleBackColor = true;
            SelectOutputDirButton.Click += SelectOutputDirButton_Click;
            // 
            // StartCompressionButton
            // 
            StartCompressionButton.Location = new SD.Point(118, 389);
            StartCompressionButton.Name = "StartCompressionButton";
            StartCompressionButton.Size = new SD.Size(91, 23);
            StartCompressionButton.TabIndex = 4;
            StartCompressionButton.Text = "开始压缩";
            StartCompressionButton.UseVisualStyleBackColor = true;
            StartCompressionButton.Click += StartCompressionButton_Click;
            // 
            // PauseCompressionButton
            // 
            PauseCompressionButton.Location = new SD.Point(241, 389);
            PauseCompressionButton.Name = "PauseCompressionButton";
            PauseCompressionButton.Size = new SD.Size(91, 23);
            PauseCompressionButton.TabIndex = 5;
            PauseCompressionButton.Text = "暂停压缩";
            PauseCompressionButton.UseVisualStyleBackColor = true;
            PauseCompressionButton.Click += PauseCompressionButton_Click;
            // 
            // ResumeCompressionButton
            // 
            ResumeCompressionButton.Location = new SD.Point(356, 389);
            ResumeCompressionButton.Name = "ResumeCompressionButton";
            ResumeCompressionButton.Size = new SD.Size(91, 23);
            ResumeCompressionButton.TabIndex = 6;
            ResumeCompressionButton.Text = "继续压缩";
            ResumeCompressionButton.UseVisualStyleBackColor = true;
            ResumeCompressionButton.Click += ResumeCompressionButton_Click;
            // 
            // StopCompressionButton
            // 
            StopCompressionButton.Location = new SD.Point(473, 389);
            StopCompressionButton.Name = "StopCompressionButton";
            StopCompressionButton.Size = new SD.Size(91, 23);
            StopCompressionButton.TabIndex = 7;
            StopCompressionButton.Text = "结束压缩";
            StopCompressionButton.UseVisualStyleBackColor = true;
            StopCompressionButton.Click += StopCompressionButton_Click;
            // 
            // progressBar
            // 
            progressBar.Location = new SD.Point(118, 293);
            progressBar.Name = "progressBar";
            progressBar.Size = new SD.Size(446, 23);
            progressBar.TabIndex = 8;
            // 
            // infoLabel
            // 
            infoLabel.AutoSize = true;
            infoLabel.Location = new SD.Point(118, 355);
            infoLabel.Name = "infoLabel";
            infoLabel.Size = new SD.Size(0, 17);
            infoLabel.TabIndex = 9;
            // 
            // sizeTextBox
            // 
            sizeTextBox.Location = new SD.Point(118, 238);
            sizeTextBox.Name = "sizeTextBox";
            sizeTextBox.PlaceholderText = "目标大小 (MB)";
            sizeTextBox.Size = new SD.Size(150, 23);
            sizeTextBox.TabIndex = 10;
            // 
            // scaleComboBox
            // 
            scaleComboBox.Items.AddRange(new object[] { "1", "1/2", "1/4", "1/8" });
            scaleComboBox.Location = new SD.Point(371, 238);
            scaleComboBox.Name = "scaleComboBox";
            scaleComboBox.Size = new SD.Size(76, 25);
            scaleComboBox.TabIndex = 11;
            scaleComboBox.Text = "压缩倍率";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SD.SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new SD.Size(800, 500);
            Controls.Add(infoLabel);
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
            Text = "图像压缩工具";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
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
        private Label infoLabel;
        private TextBox sizeTextBox; // 目标大小文本框
        private ComboBox scaleComboBox; // 缩放比例下拉框

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
            cancellationTokenSource = new CancellationTokenSource();
            pauseEvent = new ManualResetEventSlim(true); // 初始状态不暂停

            string inputDir = inputDirTextBox.Text;
            string outputDir = outputDirTextBox.Text;
            float targetSizeMB = 0;
            bool compressToSize = false;

            if (!string.IsNullOrEmpty(sizeTextBox.Text))
            {
                if (float.TryParse(sizeTextBox.Text, out targetSizeMB))
                {
                    compressToSize = true;
                }
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

            var files = Directory.GetFiles(inputDir, "*.*", SearchOption.AllDirectories)
                                 .Where(file => file.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                                                file.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                                                file.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase)).ToArray();

            int totalFiles = files.Length;
            progressBar.Maximum = totalFiles;
            progressBar.Value = 0;

            DateTime startTime = DateTime.Now;
            object lockObj = new object();

            await Task.Run(() =>
            {
                Parallel.ForEach(files, new ParallelOptions { CancellationToken = cancellationTokenSource.Token }, file =>
                {
                    try
                    {
                        pauseEvent.Wait(); // 检查是否暂停

                        using (var image = SixLabors.ImageSharp.Image.Load(file))
                        {
                            // 删除图片元数据
                            image.Metadata.ExifProfile = null;

                            // 删除多余的通道，如Alpha通道
                            if (image.PixelType.BitsPerPixel > 24) // 超过24位通常包含Alpha通道
                            {
                                image.Mutate(x => x.BackgroundColor(SixLabors.ImageSharp.Color.White)); // 使用白色背景去除透明通道
                            }

                            // 获取用户选择的缩放比例
                            string selectedScale = scaleComboBox.SelectedItem.ToString();
                            float scale = selectedScale switch
                            {
                                "1" => 1f,
                                "1/2" => 0.5f,
                                "1/4" => 0.25f,
                                "1/8" => 0.125f,
                                _ => 1f // 默认不缩放
                            };

                            if (scale < 1f)
                            {
                                // 按比例缩放
                                image.Mutate(x => x.Resize((int)(image.Width * scale), (int)(image.Height * scale)));
                            }

                            var relativePath = Path.GetRelativePath(inputDir, file);
                            var outputFilePath = Path.Combine(outputDir, relativePath);
                            var outputDirectory = Path.GetDirectoryName(outputFilePath);

                            if (!Directory.Exists(outputDirectory))
                            {
                                Directory.CreateDirectory(outputDirectory);
                            }

                            if (file.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase))
                            {
                                if (compressToSize)
                                {
                                    int quality = 90;
                                    while (true)
                                    {
                                        using (var ms = new MemoryStream())
                                        {
                                            var encoder = new JpegEncoder { Quality = quality };
                                            image.Save(ms, encoder);

                                            if (ms.Length / 1024f / 1024f <= targetSizeMB || quality <= 10)
                                            {
                                                using (var outputStream = File.Create(outputFilePath))
                                                {
                                                    ms.Seek(0, SeekOrigin.Begin);
                                                    ms.CopyTo(outputStream);
                                                }
                                                break;
                                            }

                                            quality -= 10;
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
                            else if (file.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                            {
                                // 对于PNG，只支持缩放和删除元数据、Alpha通道
                                var encoder = new PngEncoder
                                {
                                    CompressionLevel = PngCompressionLevel.BestCompression
                                };
                                using (var outputStream = File.Create(outputFilePath))
                                {
                                    image.Save(outputStream, encoder);
                                }
                            }
                            else if (file.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase))
                            {
                                // 对于BMP，只支持缩放和删除元数据、Alpha通道
                                var encoder = new BmpEncoder();
                                using (var outputStream = File.Create(outputFilePath))
                                {
                                    image.Save(outputStream, encoder);
                                }
                            }
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        return; // 任务被取消时，退出循环
                    }
                    catch (Exception ex)
                    {
                        Invoke(new Action(() =>
                        {
                            MessageBox.Show($"压缩文件 {file} 时出错: {ex.Message}\n{ex.StackTrace}");
                        }));
                    }

                    // 更新进度条和处理信息
                    lock (lockObj)
                    {
                        progressBar.Invoke(new Action(() =>
                        {
                            progressBar.Value += 1;

                            double progressPercentage = (double)progressBar.Value / totalFiles * 100;
                            TimeSpan elapsed = DateTime.Now - startTime;
                            TimeSpan estimatedTotalTime = TimeSpan.FromTicks(elapsed.Ticks * totalFiles / progressBar.Value);
                            TimeSpan remainingTime = estimatedTotalTime - elapsed;

                            infoLabel.Text = $"正在压缩: {Path.GetFileName(file)} - {progressPercentage:F2}% 完成 - 预计剩余时间: {remainingTime:mm\\:ss}";
                        }));
                    }
                });
            });

            MessageBox.Show("压缩完成！");
            this.Invoke(new Action(() => infoLabel.Text = "压缩完成！"));
        }



        // 暂停任务
        private void PauseCompressionButton_Click(object sender, EventArgs e)
        {
            pauseEvent.Reset(); // 设置暂停状态
            infoLabel.Text = "任务已暂停";
        }

        // 继续任务
        private void ResumeCompressionButton_Click(object sender, EventArgs e)
        {
            pauseEvent.Set(); // 恢复任务
            infoLabel.Text = "任务继续进行";
        }

        // 结束任务
        private void StopCompressionButton_Click(object sender, EventArgs e)
        {
            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel(); // 取消任务
                infoLabel.Text = "任务已取消";
            }
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            var codecs = ImageCodecInfo.GetImageDecoders();
            return codecs.FirstOrDefault(c => c.FormatID == format.Guid);
        }
    }
}
