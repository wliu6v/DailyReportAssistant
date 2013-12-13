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
using System.Collections.ObjectModel;

namespace DailyReportAssistant
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    /// 
    public partial class MainWindow
    {
		private bool fileChanged = false;	// 如果日报文件在设置里面发生了改变，那么跳转回主页面时需重新读取

        public MainWindow()
        {
            InitializeComponent();

            uint errorCode = Transcation.AppInitialize();

			if (errorCode == ERR.AI_NO_FILE)
			{
				//  令用户输入日报文件路径
				jmpToTabSetting();
				MessageBox.Show("请填写日报路径", "日报小助手友情提示", MessageBoxButton.OK);
				
			}
			else if (errorCode == ERR.SUCCESS)
			{
				// 主界面初始化
				getDailyReportText();
			}


        }

		private void getDailyReportText()
		{
			String[] texts = Transcation.Read();
			textBox1.Text = "";
			textBox2.Text = "";
			textBox3.Text = "";
			textBox4.Text = "";
			textBox5.Text = "";
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
				jmpToTabSetting();
            }
            else if (TabSetting.IsSelected == true)
            {
                // 丢弃更改
				jmpToTabMain();
            }

        }

		private void jmpToTabMain()
		{
			if (GlobalVar.filePath.Length == 0 && textBoxFilePath.Text.Length == 0)
			{
				MessageBox.Show("请填写日报路径", "日报小助手友情提示", MessageBoxButton.OK);
				return;
			}
			if (fileChanged)
			{
				getDailyReportText();
				fileChanged = false;
			}
			TabMain.IsSelected = true;
			btnSetting.Content = "设置";
		}

		private void jmpToTabSetting()
		{
			TabSetting.IsSelected = true;
			settingViewInitial();
			btnSetting.Content = "取消";
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
			if (GlobalVar.shouldSvnCommit)
			{
				this.WindowState = WindowState.Minimized;
				String output = Transcation.SVNCommit();
			}
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
			comboBoxFileEncoding.SelectedValue = GlobalVar.fileEncoding;
			checkboxAutoCommit.IsChecked = GlobalVar.shouldSvnCommit;
            textBoxFilePath.Text = GlobalVar.filePath;
			textBoxSvnUsername.Text = GlobalVar.svnUsername;
			textBoxSvnPassword.Password = GlobalVar.svnPassword;
			textBoxSvnUsername.IsEnabled = (bool)checkboxAutoCommit.IsChecked;
			textBoxSvnPassword.IsEnabled = (bool)checkboxAutoCommit.IsChecked;
        }

        private void btnSOK_Click(object sender, RoutedEventArgs e)
        {
            GlobalVar.filePath = textBoxFilePath.Text;
			GlobalVar.shouldSvnCommit = (bool)checkboxAutoCommit.IsChecked || false;
			GlobalVar.fileEncoding = (Encoding)comboBoxFileEncoding.SelectedValue;
			GlobalVar.svnUsername = textBoxSvnUsername.Text;
			GlobalVar.svnPassword = textBoxSvnPassword.Password;
			Transcation.SaveConfig();
			fileChanged = true;	// 偷懒了，其实日报文件是否改变不是这么判断的。
			jmpToTabMain();
        }

        private void btnSCancel_Click(object sender, RoutedEventArgs e)
        {
			jmpToTabMain();
        }

        private void btnSOpen_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFile = new System.Windows.Forms.OpenFileDialog();
            openFile.Title = "选择日报文件";
            openFile.DefaultExt = "*.txt";
            openFile.Filter = "txt文件|*.txt|所有文件|*.*";

			if (textBoxFilePath.Text != null && textBoxFilePath.Text.Length > 0)
			{
				int pathLen = textBoxFilePath.Text.LastIndexOf("\\");
				openFile.InitialDirectory = textBoxFilePath.Text.Substring(0, pathLen);
				openFile.FileName = textBoxFilePath.Text.Substring(pathLen + 1, textBoxFilePath.Text.Length - pathLen - 1);
			}

            System.Windows.Forms.DialogResult result =  openFile.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK
                || result == System.Windows.Forms.DialogResult.Yes)
            {
                textBoxFilePath.Text = openFile.FileName.Trim();
            }
        }

		private void checkBoxAutoCommit_Unchecked(object sender, RoutedEventArgs e)
		{
			textBoxSvnUsername.IsEnabled = false;
			textBoxSvnPassword.IsEnabled = false;
		}

		private void checkBoxAutoCommit_Checked(object sender, RoutedEventArgs e)
		{
			textBoxSvnUsername.IsEnabled = true;
			textBoxSvnPassword.IsEnabled = true;
		}
    }


	// 数据项
	public class TextEncoding
	{
		public TextEncoding() {}
		public String encodeName { set; get; }
		public Encoding encoding { set; get; }
	}

	public class TextEncodingList : ObservableCollection<TextEncoding>
	{
		public TextEncodingList()
		{
			this.Add(new TextEncoding { encodeName = "ANSI", encoding = Encoding.Default });
			this.Add(new TextEncoding { encodeName = "UTF8", encoding = Encoding.UTF8 });
			this.Add(new TextEncoding { encodeName = "Unicode/UCS2LE", encoding = Encoding.Unicode });
		}
	}

}
