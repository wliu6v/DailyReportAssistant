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
            
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            String[] texts = {textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text};
            Transcation.Write(texts);
            
            // 添加进行Svn commit 的代码

            Environment.Exit(Environment.ExitCode);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
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


        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key==Key.Enter)
            {
                btnOK_Click(sender, e);
            }

            if (e.Key == Key.Escape)
            {
                btnCancel_Click(sender, e);
            }

            if (e.Key == Key.Enter)
            {
                textBox2.Focus();
            }
        }
        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.Enter)
            {
                btnOK_Click(sender, e);
            }

            if (e.Key == Key.Escape)
            {
                btnCancel_Click(sender, e);
            }

            if (e.Key == Key.Enter)
            {
                textBox3.Focus();
            }
        }
        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.Enter)
            {
                btnOK_Click(sender, e);
            }

            if (e.Key == Key.Escape)
            {
                btnCancel_Click(sender, e);
            }

            if (e.Key == Key.Enter)
            {
                textBox4.Focus();
            }
        }    
        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.Enter)
            {
                btnOK_Click(sender, e);
            }

            if (e.Key == Key.Escape)
            {
                btnCancel_Click(sender, e);
            }

            if (e.Key == Key.Enter)
            {   
                textBox5.Focus();
            }
        }
        private void textBox5_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.Enter)
            {
                btnOK_Click(sender, e);
            }

            if (e.Key == Key.Escape)
            {
                btnCancel_Click(sender, e);
            }

            if (e.Key == Key.Enter)
            {
                btnOK.Focus();
            }
        }
    }
}
