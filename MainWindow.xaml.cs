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

        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            String[] texts = { textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text};

            Transcation.Write(texts);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
