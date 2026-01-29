using System;
using System.Windows;

namespace PandocBetterUse
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // 设置全局异常处理
            AppDomain.CurrentDomain.UnhandledException += (s, args) =>
            {
                MessageBox.Show($"未处理的异常：{args.ExceptionObject}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            };
        }
    }
}