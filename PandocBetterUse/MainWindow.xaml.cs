using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace PandocBetterUse
{
    public partial class MainWindow : Window
    {
        private Config config;
        private string configPath = "pandoc_config.json";
        private List<string> inputFormats = new List<string>();
        private List<string> outputFormats = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
            LoadFormatLists();
            LoadConfig();
            UpdateStatusUI();
            Loaded += MainWindow_Loaded;
        }

        private void LoadFormatLists()
        {
            try
            {
                // 加载输入格式
                if (File.Exists("input.ini"))
                {
                    string[] lines = File.ReadAllLines("input.ini");
                    foreach (string line in lines)
                    {
                        string format = line.Trim();
                        if (!string.IsNullOrEmpty(format) && !format.StartsWith("#"))
                        {
                            inputFormats.Add(format);
                        }
                    }
                }
                else
                {
                    // 创建默认输入格式列表
                    inputFormats = new List<string>
                    {
                        "markdown", "html", "latex", "docx", "pptx",
                        "epub", "odt", "rtf", "txt", "xml"
                    };
                    SaveInputFormatsToFile();
                }

                // 加载输出格式
                if (File.Exists("output.ini"))
                {
                    string[] lines = File.ReadAllLines("output.ini");
                    foreach (string line in lines)
                    {
                        string format = line.Trim();
                        if (!string.IsNullOrEmpty(format) && !format.StartsWith("#"))
                        {
                            outputFormats.Add(format);
                        }
                    }
                }
                else
                {
                    // 创建默认输出格式列表
                    outputFormats = new List<string>
                    {
                        "docx", "html", "pdf", "latex", "markdown",
                        "pptx", "epub", "odt", "rtf", "txt"
                    };
                    SaveOutputFormatsToFile();
                }

                // 更新下拉框
                UpdateComboBoxes();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载格式列表失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                // 使用默认格式
                inputFormats = new List<string> { "markdown", "html", "latex", "docx", "pptx" };
                outputFormats = new List<string> { "docx", "html", "pdf", "latex", "markdown", "pptx" };
                UpdateComboBoxes();
            }
        }

        private void UpdateComboBoxes()
        {
            InputFormatComboBox.Items.Clear();
            OutputFormatComboBox.Items.Clear();

            foreach (string format in inputFormats)
            {
                InputFormatComboBox.Items.Add(format);
            }

            foreach (string format in outputFormats)
            {
                OutputFormatComboBox.Items.Add(format);
            }

            // 设置默认选择
            if (InputFormatComboBox.Items.Count > 0)
                InputFormatComboBox.SelectedIndex = 0;
            if (OutputFormatComboBox.Items.Count > 0)
                OutputFormatComboBox.SelectedIndex = 0;
        }

        private void SaveInputFormatsToFile()
        {
            try
            {
                File.WriteAllLines("input.ini", inputFormats);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存输入格式列表失败: {ex.Message}", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void SaveOutputFormatsToFile()
        {
            try
            {
                File.WriteAllLines("output.ini", outputFormats);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存输出格式列表失败: {ex.Message}", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // 设置默认输出目录
            if (string.IsNullOrEmpty(OutputFolderTextBox.Text))
            {
                OutputFolderTextBox.Text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }
        }

        private void LoadConfig()
        {
            try
            {
                if (File.Exists(configPath))
                {
                    string json = File.ReadAllText(configPath);
                    config = JsonSerializer.Deserialize<Config>(json) ?? new Config();
                }
                else
                {
                    config = new Config
                    {
                        InputFile = "",
                        InputFormat = "markdown",
                        OutputFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                        OutputFormat = "docx"
                    };
                    SaveConfig();
                }

                // 更新UI
                InputFileTextBox.Text = config.InputFile;
                OutputFolderTextBox.Text = config.OutputFolder;

                // 设置下拉框选中项
                if (!string.IsNullOrEmpty(config.InputFormat))
                {
                    InputFormatComboBox.Text = config.InputFormat;
                }
                else if (InputFormatComboBox.Items.Count > 0)
                {
                    InputFormatComboBox.SelectedIndex = 0;
                }

                if (!string.IsNullOrEmpty(config.OutputFormat))
                {
                    OutputFormatComboBox.Text = config.OutputFormat;
                }
                else if (OutputFormatComboBox.Items.Count > 0)
                {
                    OutputFormatComboBox.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载配置失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                config = new Config();
            }
        }

        private void SaveConfig()
        {
            UpdateConfigFromUI();
            try
            {
                string json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(configPath, json);
                UpdateStatus("配置已保存", StatusType.Success);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存配置失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateConfigFromUI()
        {
            config.InputFile = InputFileTextBox.Text;
            config.InputFormat = InputFormatComboBox.Text;
            config.OutputFolder = OutputFolderTextBox.Text;
            config.OutputFormat = OutputFormatComboBox.Text;
        }

        private enum StatusType
        {
            Normal,
            Success,
            Error,
            Processing
        }

        private void UpdateStatus(string message, StatusType type = StatusType.Normal)
        {
            StatusText.Text = message;

            switch (type)
            {
                case StatusType.Success:
                    StatusIndicator.Fill = (SolidColorBrush)FindResource("SuccessBrush");
                    break;
                case StatusType.Error:
                    StatusIndicator.Fill = (SolidColorBrush)FindResource("ErrorBrush");
                    break;
                case StatusType.Processing:
                    StatusIndicator.Fill = (SolidColorBrush)FindResource("WarningBrush");
                    break;
                default:
                    StatusIndicator.Fill = (SolidColorBrush)FindResource("PrimaryBrush");
                    break;
            }
        }

        private void UpdateStatusUI()
        {
            StatusInputFileText.Text = $"📄 输入文件: {(string.IsNullOrEmpty(config.InputFile) ? "未选择" : Path.GetFileName(config.InputFile))}";
            StatusInputFormatText.Text = $"📝 输入格式: {config.InputFormat}";
            StatusOutputFolderText.Text = $"📁 输出目录: {Path.GetFileName(config.OutputFolder)}";
            StatusOutputFormatText.Text = $"🎯 输出格式: {config.OutputFormat}";
        }

        private void BrowseInputButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "所有文件 (*.*)|*.*",
                Title = "选择输入文件",
                CheckFileExists = true
            };

            if (openFileDialog.ShowDialog() == true)
            {
                InputFileTextBox.Text = openFileDialog.FileName;

                // 尝试自动检测格式
                string ext = Path.GetExtension(openFileDialog.FileName).ToLower().TrimStart('.');
                if (!string.IsNullOrEmpty(ext))
                {
                    // 检查扩展名是否在格式列表中
                    foreach (string format in inputFormats)
                    {
                        if (format.ToLower() == ext)
                        {
                            InputFormatComboBox.Text = format;
                            break;
                        }
                    }
                }

                UpdateConfigFromUI();
                UpdateStatusUI();
                UpdateStatus("文件已选择", StatusType.Success);
            }
        }

        private void BrowseOutputButton_Click(object sender, RoutedEventArgs e)
        {
            // 使用 WPF 的 OpenFileDialog 来"模拟"选择文件夹
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Title = "选择输出文件夹",
                CheckFileExists = false,  // 允许选择不存在的文件（用于选择文件夹）
                FileName = "选择文件夹",  // 显示提示文字
                Filter = "文件夹|*."  // 设置一个特殊的过滤器
            };

            if (openFileDialog.ShowDialog() == true)
            {
                // 获取选择的文件夹路径
                string selectedPath = System.IO.Path.GetDirectoryName(openFileDialog.FileName);
                OutputFolderTextBox.Text = selectedPath;
                UpdateConfigFromUI();
                UpdateStatusUI();
                UpdateStatus("输出目录已设置", StatusType.Success);
            }
        }

        private void SaveConfigButton_Click(object sender, RoutedEventArgs e)
        {
            SaveConfig();
            UpdateStatusUI();
        }

        private void PreviewButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateConfigFromUI();

            string previewText = $"转换预览：\n\n" +
                               $"📄 输入文件：{config.InputFile}\n" +
                               $"📝 输入格式：{config.InputFormat}\n" +
                               $"🎯 输出格式：{config.OutputFormat}\n" +
                               $"📁 输出目录：{config.OutputFolder}";

            if (!string.IsNullOrWhiteSpace(config.InputFile) && File.Exists(config.InputFile))
            {
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string inputName = Path.GetFileNameWithoutExtension(config.InputFile);
                string outputFile = Path.Combine(
                    config.OutputFolder,
                    $"{inputName}_{timestamp}.{config.OutputFormat}");

                previewText += $"\n\n💾 输出文件：{Path.GetFileName(outputFile)}\n" +
                              $"📅 时间戳：{timestamp}";
            }

            MessageBox.Show(previewText, "转换预览", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void RefreshFormatsButton_Click(object sender, RoutedEventArgs e)
        {
            // 重新加载格式列表
            LoadFormatLists();
            UpdateStatus("格式列表已刷新", StatusType.Success);
        }

        private async void ConvertButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateConfigFromUI();

            // 验证输入
            if (string.IsNullOrWhiteSpace(config.InputFile))
            {
                MessageBox.Show("请选择输入文件", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!File.Exists(config.InputFile))
            {
                MessageBox.Show($"输入文件不存在：\n{config.InputFile}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // 检查格式是否有效
            if (!inputFormats.Contains(config.InputFormat))
            {
                MessageBox.Show($"输入格式 '{config.InputFormat}' 不在支持的格式列表中。\n请检查 input.ini 文件。",
                    "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!outputFormats.Contains(config.OutputFormat))
            {
                MessageBox.Show($"输出格式 '{config.OutputFormat}' 不在支持的格式列表中。\n请检查 output.ini 文件。",
                    "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 检查Pandoc
            if (!await CheckPandocAsync())
            {
                var result = MessageBox.Show(
                    "未检测到 Pandoc，是否要下载安装？\n\nPandoc 是一个免费的文档转换工具。",
                    "Pandoc 未安装",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "https://pandoc.org/installing.html",
                        UseShellExecute = true
                    });
                }
                return;
            }

            // 执行转换
            await ConvertFileAsync();
        }

        private async Task<bool> CheckPandocAsync()
        {
            try
            {
                using (Process process = new Process())
                {
                    process.StartInfo = new ProcessStartInfo
                    {
                        FileName = "pandoc",
                        Arguments = "--version",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };

                    process.Start();
                    await Task.Run(() => process.WaitForExit(2000));
                    return process.ExitCode == 0;
                }
            }
            catch
            {
                return false;
            }
        }

        private async Task ConvertFileAsync()
        {
            // 生成输出文件名
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string inputName = Path.GetFileNameWithoutExtension(config.InputFile);

            // 根据输出格式确定文件扩展名
            string extension = GetFileExtension(config.OutputFormat);
            string outputFile = Path.Combine(
                config.OutputFolder,
                $"{inputName}_{timestamp}.{extension}");

            // 确保输出目录存在
            if (!Directory.Exists(config.OutputFolder))
            {
                try
                {
                    Directory.CreateDirectory(config.OutputFolder);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"无法创建输出文件夹：{ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            // 构建命令
            string arguments = $"\"{config.InputFile}\" -f {config.InputFormat} -t {config.OutputFormat} -o \"{outputFile}\"";

            UpdateStatus("正在转换...", StatusType.Processing);
            ConvertButton.IsEnabled = false;

            try
            {
                using (Process process = new Process())
                {
                    process.StartInfo = new ProcessStartInfo
                    {
                        FileName = "pandoc",
                        Arguments = arguments,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        StandardOutputEncoding = System.Text.Encoding.UTF8,
                        StandardErrorEncoding = System.Text.Encoding.UTF8
                    };

                    List<string> output = new List<string>();
                    List<string> error = new List<string>();

                    process.OutputDataReceived += (s, e) => { if (e.Data != null) output.Add(e.Data); };
                    process.ErrorDataReceived += (s, e) => { if (e.Data != null) error.Add(e.Data); };

                    process.Start();
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    await Task.Run(() => process.WaitForExit());

                    if (process.ExitCode == 0 && File.Exists(outputFile))
                    {
                        UpdateStatus("转换成功！", StatusType.Success);

                        var openResult = MessageBox.Show(
                            $"🎉 转换完成！\n\n📄 文件已保存到：\n{outputFile}\n\n📂 是否打开所在文件夹？",
                            "转换成功",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Information);

                        if (openResult == MessageBoxResult.Yes)
                        {
                            Process.Start(new ProcessStartInfo
                            {
                                FileName = "explorer.exe",
                                Arguments = $"/select,\"{outputFile}\"",
                                UseShellExecute = true
                            });
                        }
                    }
                    else
                    {
                        UpdateStatus("转换失败", StatusType.Error);
                        string errorMsg = string.Join("\n", error);

                        if (string.IsNullOrWhiteSpace(errorMsg))
                            errorMsg = string.Join("\n", output);

                        if (string.IsNullOrWhiteSpace(errorMsg))
                            errorMsg = "未知错误";

                        MessageBox.Show($"❌ 转换失败：\n{errorMsg}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                UpdateStatus("转换异常", StatusType.Error);
                MessageBox.Show($"转换过程中发生异常：{ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                ConvertButton.IsEnabled = true;
            }
        }

        private string GetFileExtension(string format)
        {
            // 常见格式的扩展名映射
            var extensionMap = new Dictionary<string, string>
            {
                { "docx", "docx" },
                { "html", "html" },
                { "pdf", "pdf" },
                { "latex", "tex" },
                { "markdown", "md" },
                { "pptx", "pptx" },
                { "epub", "epub" },
                { "odt", "odt" },
                { "rtf", "rtf" },
                { "txt", "txt" },
                { "xml", "xml" },
                { "json", "json" },
                { "tex", "tex" },
                { "texinfo", "texi" },
                { "man", "1" },
                { "rst", "rst" },
                { "textile", "textile" },
                { "org", "org" },
                { "ipynb", "ipynb" },
                { "csv", "csv" },
                { "tsv", "tsv" }
            };

            return extensionMap.ContainsKey(format) ? extensionMap[format] : format;
        }

        // 配置文件类
        public class Config
        {
            public string InputFile { get; set; } = "";
            public string InputFormat { get; set; } = "markdown";
            public string OutputFolder { get; set; } = "";
            public string OutputFormat { get; set; } = "docx";
        }
    }
}