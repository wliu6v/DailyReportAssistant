using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Configuration;

namespace DailyReportAssistant
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    /// 
    

    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            Transcation.Initialize();

            // 主界面初始化
            String[] texts = Transcation.Read();
            textBox1.Text = GlobalVar.dailyReport[0];
            textBox2.Text = GlobalVar.dailyReport[1];
            textBox3.Text = GlobalVar.dailyReport[2];
            textBox4.Text = GlobalVar.dailyReport[3];
            textBox5.Text = GlobalVar.dailyReport[4];
            textBox1.Focus();

        }

        private void btnSetting_Click(object sender, RoutedEventArgs e)
        {
            if (TabMain.IsSelected == true)
            {
                TabSetting.IsSelected = true;
            }
            else if (TabSetting.IsSelected == true)
            {
                // 丢弃更改
                TabMain.IsSelected = true;
            }

        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TabMain.IsSelected == true)
            {
                btnSetting.Content = "设置";
            }
            else if (TabSetting.IsSelected == true)
            {
                settingViewInitial();
                btnSetting.Content = "取消";
            }
        }

        private void MetroWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.Enter)
            {
                if (TabMain.IsSelected)
                {
                    btnOK_Click(sender, e);
                }
                else if (TabSetting.IsSelected)
                {
                    btnSOK_Click(sender, e);
                }
            }

            if (e.Key == Key.Escape)
            {
                if (TabMain.IsSelected)
                {
                    btnCancel_Click(sender, e);
                }
                else if (TabSetting.IsSelected)
                {
                    btnSCancel_Click(sender, e);
                }   
            }

            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.O)
            {
                btnSetting_Click(sender, e);
            }
        }

        //---- 主页面

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            String[] texts = {textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text};
            Transcation.Write(texts);
			this.WindowState = WindowState.Minimized;
            String output = Transcation.SVNCommit();
            Environment.Exit(Environment.ExitCode);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }

        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            process.StandardInput.WriteLine("start " + GlobalVar.filePath);
        }

        private void textBox_GotFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).Select(0, ((TextBox)sender).Text.Length);
        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.Enter)
            {
                if (TabMain.IsSelected)
                {
                    btnOK_Click(sender, e);
                }
                else if (TabSetting.IsSelected)
                {
                    btnSOK_Click(sender, e);
                }
            }


            if ((e.Key == Key.Enter) || (e.Key == Key.Down))
            {
                var uie = e.OriginalSource as UIElement;
                e.Handled = true;
                uie.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }

            if (e.Key == Key.Up)
            {
                var uie = e.OriginalSource as UIElement;
                e.Handled = true;
                uie.MoveFocus(new TraversalRequest(FocusNavigationDirection.Previous));
            }
        }

        //---- 设置页面

        private void settingViewInitial()
        {
            textBoxFilePath.Text = GlobalVar.filePath;
        }


        private void btnSOK_Click(object sender, RoutedEventArgs e)
        {
            GlobalVar.filePath = textBoxFilePath.Text;
			Properties.Settings.Default.FilePath = GlobalVar.filePath;
			Properties.Settings.Default.Save();
            TabMain.IsSelected = true;
        }

        private void btnSCancel_Click(object sender, RoutedEventArgs e)
        {
            TabMain.IsSelected = true;
        }

        private void btnSOpen_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFile = new System.Windows.Forms.OpenFileDialog();
            openFile.Title = "选择日报文件";
            int pathLen = textBoxFilePath.Text.LastIndexOf("\\");
            
            openFile.InitialDirectory = textBoxFilePath.Text.Substring(0, pathLen);
            openFile.FileName = textBoxFilePath.Text.Substring(pathLen + 1, textBoxFilePath.Text.Length - pathLen - 1);
            openFile.DefaultExt = "*.txt";
            openFile.Filter = "txt文件|*.txt|所有文件|*.*";
            System.Windows.Forms.DialogResult result =  openFile.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK
                || result == System.Windows.Forms.DialogResult.Yes)
            {
                textBoxFilePath.Text = openFile.FileName.Trim();
            }
        }
    }
}
