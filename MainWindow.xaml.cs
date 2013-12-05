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

            textBox1.Text = GlobalVar.yesterdayDailyReport[0];
            textBox2.Text = GlobalVar.yesterdayDailyReport[1];
            textBox3.Text = GlobalVar.yesterdayDailyReport[2];
            textBox4.Text = GlobalVar.yesterdayDailyReport[3];
            textBox5.Text = GlobalVar.yesterdayDailyReport[4];

        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            String[] texts = new String[5];
            texts[0] = textBox1.Text;
            texts[1] = textBox2.Text;
            texts[2] = textBox3.Text;
            texts[3] = textBox4.Text;
            texts[4] = textBox5.Text;

            Transcation.generateDailyReport(texts);
            Transcation.writeDailyReportToFile();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
