using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using Path = System.IO.Path;

namespace DES_KRYPTO_PROJECT
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

        StringBuilder BuildPathToFile(String directory, String SaveOrOpen)
        {
            StringBuilder pathToFile = new();
            pathToFile.Append(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
            pathToFile.Append(Path.DirectorySeparatorChar);
            pathToFile.Append(directory);
            pathToFile.Append(Path.DirectorySeparatorChar);
            pathToFile.Append(SaveOrOpen);
            pathToFile.Append(".txt");

            return pathToFile;
        }

        StringBuilder BuildInitialDirectory(String directory)
        {
            StringBuilder initialDirectory = new();
            initialDirectory.Append(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
            initialDirectory.Append(Path.DirectorySeparatorChar);
            initialDirectory.Append(directory);

            return initialDirectory;
        }

        FileDialog BuildSaveFileDialog(bool saveOrOpen, String directory)
        {
            FileDialog fileDialog;

            if (saveOrOpen)
            {
                fileDialog = new SaveFileDialog();
            }
            else
            {
                fileDialog = new OpenFileDialog();
            }
            fileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            fileDialog.InitialDirectory = BuildInitialDirectory(directory).ToString();

            return fileDialog;
        }

        private void OpenFileWithKeyValueButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder pathToFile = BuildPathToFile("TekstyKluczaKRYPTO", TxtEditorForKey.Text);

            if (TxtEditorForKey.Text != "")
            {
                try
                {
                    TxtEditorForKey.Text = File.ReadAllText(pathToFile.ToString());
                    MessageBox.Show("wczytano z pliku: " + pathToFile.ToString());
                }
                catch
                {
                    MessageBox.Show("nie znaleziono pliku o podanej nazwie");
                }
            }
        }

        private void OpenFileWithKeyValueButtonWithDialog_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = (OpenFileDialog)BuildSaveFileDialog(false, "TekstyKluczaKRYPTO");

            if (openFileDialog.ShowDialog() == true)
            {
                TxtEditorForKey.Text = File.ReadAllText(openFileDialog.FileName);
            }
        }

        private void SaveFileWithKeyValueButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder pathToFile = BuildPathToFile("TekstyKluczaKRYPTO", TxtEditorForKey.Text);

            if (TxtEditorForKey.Text != "")
            {
                File.WriteAllText(pathToFile.ToString(), TxtEditorForKey.Text);
                MessageBox.Show("zapisano do pliku: " + pathToFile.ToString());
            }
        }

        private void SaveFileWithKeyValueButtonWithDialog_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = (SaveFileDialog)BuildSaveFileDialog(true, "TekstyKluczaKRYPTO");

            if (saveFileDialog.ShowDialog() == true)
                File.WriteAllText(saveFileDialog.FileName, TxtEditorForKey.Text);
        }

        private void ShowKey_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)ShowKey.IsChecked)
            {
                int len = TxtEditorForKey.Text.Length;

                StringBuilder keyHidden = new();

                for (int i = 0; i < len; i++)
                {
                    keyHidden.Append("*");
                }
                TxtEditorKeyValueHidden.Text = keyHidden.ToString();
                TxtEditorKeyValueHidden.Visibility = Visibility.Visible;
            }
            else
            {
                TxtEditorKeyValueHidden.Visibility = Visibility.Hidden;
            }
        }
    }
}

