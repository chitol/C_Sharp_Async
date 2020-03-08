using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.IO;


namespace C_Sharp_Async
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void bnTest_Click(object sender, RoutedEventArgs e)
        {

            txtMessage.Text = "start";
            await LongTask();
            txtMessage.Text = "end";
        }

        private Task LongTask()
        {
            return Task.Factory.StartNew(() => { Thread.Sleep(5000); });
        }


        private Task LongTask2(string param)
        {
            return Task.Run(() => MethodWithParameter(param));
        }



        private void MethodWithParameter(string param)
        {
            Thread.Sleep(5000);
        }

        private async void bnTest1_Click(object sender, RoutedEventArgs e)
        {
            txtMessage1.Text = "start";
            await LongTask2("me");
            txtMessage1.Text = "done";
        }

        private async void bnTest2_Click(object sender, RoutedEventArgs e)
        {

            txtMessage2.Text = "start";
            int a = await LongTask3(30);
            txtMessage2.Text = a.ToString();
        }


        private Task<int> LongTask3(int a)
        {
            return Task<int>.Run(() => MethodWithParameter2(a));
        }

        private int MethodWithParameter2(int a)
        {

            Thread.Sleep(5000);
            return a * a;

        }

        private Task<bool>[] LongTask4()
        {

            string[] files = Directory.GetFiles(@"C:\Windows\Temp");

            Task<bool>[] tasks = new Task<bool>[files.Length];
            for (int i = 0; i < tasks.Length; i++)
            {
                string fileName = files[i];
                tasks[i] = Task<bool>.Run(() => CopyFile(fileName, @"c:\temp\temp2"));
            }
            return tasks;

        }

        private async void bnTest3_Click(object sender, RoutedEventArgs e)
        {
            DateTime t1 = DateTime.Now;
            Task<bool>[] tasks = LongTask4();
            Task.WaitAll(tasks);
            DateTime t2 = DateTime.Now;
            TimeSpan ts = t2 - t1;
            txtMessage3.Text = ts.ToString();
        }

        private bool CopyFile(string sourcePath, string destDir)
        {
            try
            {
                if (File.Exists(sourcePath))
                {
                    File.Copy(sourcePath, System.IO.Path.Combine(destDir, System.IO.Path.GetFileName(sourcePath)));
                    return true;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
