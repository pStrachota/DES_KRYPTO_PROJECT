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
        private string _ReadFileExtension;
        private string _LoadFileExtension;
        static Random random = new Random();
        private bool _fileOrPlainText = false;
        public MainWindow()
        {
            InitializeComponent();

            List<StringBuilder> dirs = new();
            dirs.Add(new StringBuilder().Append("TekstyKluczaKRYPTO"));
            dirs.Add(new StringBuilder().Append("TekstyJawneKRYPTO"));
            dirs.Add(new StringBuilder().Append("TekstyZaszyfrowaneKRYPTO"));
            dirs.Add(new StringBuilder().Append("OdszyfrowanePliki"));
            dirs.Add(new StringBuilder().Append("ZaszyfrowanePliki"));

            foreach (StringBuilder dir in dirs)
            {
                StringBuilder dirsToCheck = new StringBuilder();
                dirsToCheck.Append(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
                dirsToCheck.Append(Path.DirectorySeparatorChar);
                dirsToCheck.Append(dir);

                if (!Directory.Exists(dirsToCheck.ToString()))
                {
                    Directory.CreateDirectory(dirsToCheck.ToString());
                }
            }
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
            fileDialog.InitialDirectory = BuildInitialDirectory(directory).ToString();

            return fileDialog;
        }

        private void OpenFileWithPlainTextButtonWithDialog_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new();
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

            openFileDialog.InitialDirectory = BuildInitialDirectory("TekstyJawneKRYPTO").ToString();
            if (openFileDialog.ShowDialog() == true)
            {
                TxtEditorForPlainText.Text = File.ReadAllText(openFileDialog.FileName);
            }
        }
        private void SaveFileWithPlainTextButtonWithDialog_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = (SaveFileDialog)BuildSaveFileDialog(true, "ZaszyfrowanePliki");
            saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

            if (saveFileDialog.ShowDialog() == true)
                File.WriteAllText(saveFileDialog.FileName, TxtEditorForPlainText.Text);
            if (File.Exists(saveFileDialog.FileName))
            {
                MessageBox.Show("zapisano do pliku: " + saveFileDialog.FileName, "Zapisano");
            }
            else
            {
                MessageBox.Show("nie udało się zapisać pliku: " + saveFileDialog.FileName);
            }
        }

        private void OpenFileWithCryptogramButtonWithDialog_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = (OpenFileDialog)BuildSaveFileDialog(false, "TekstyZaszyfrowaneKRYPTO");
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                TxtEditorForCryptogram.Text = File.ReadAllText(openFileDialog.FileName);
            }
        }

        private void SaveFileWithCryptogramButtonWithDialog_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = (SaveFileDialog)BuildSaveFileDialog(true, "TekstyZaszyfrowaneKRYPTO");
            saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == true)
                File.WriteAllText(saveFileDialog.FileName, TxtEditorForCryptogram.Text);
            if (File.Exists(saveFileDialog.FileName))
            {
                MessageBox.Show("zapisano do pliku: " + saveFileDialog.FileName);
            }
            else
            {
                MessageBox.Show("nie udało się zapisać pliku: " + saveFileDialog.FileName);
            }
        }

        private void OpenFileWithKeyValueButtonWithDialog_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = (OpenFileDialog)BuildSaveFileDialog(false, "TekstyKluczaKRYPTO");
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                TxtEditorForKey.Text = File.ReadAllText(openFileDialog.FileName);
            }
        }

        private void SaveFileWithKeyValueButtonWithDialog_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = (SaveFileDialog)BuildSaveFileDialog(true, "TekstyKluczaKRYPTO");
            saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

            if (saveFileDialog.ShowDialog() == true)
                File.WriteAllText(saveFileDialog.FileName, TxtEditorForKey.Text);

            if (File.Exists(saveFileDialog.FileName))
            {
                MessageBox.Show("zapisano do pliku: " + saveFileDialog.FileName);
            }
            else
            {
                MessageBox.Show("nie udało się zapisać pliku: " + saveFileDialog.FileName);
            }
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

        public static string GetRandomHexNumber(int digits)
        {
            byte[] buffer = new byte[digits / 2];
            random.NextBytes(buffer);
            string result = String.Concat(buffer.Select(x => x.ToString("X2")).ToArray());
            if (digits % 2 == 0)
                return result;
            return result + random.Next(16).ToString("X");
        }

        private void Encrypt(object sender, RoutedEventArgs e)
        {
            String plaintext = TxtEditorForPlainText.Text;
            String loadedKey = TxtEditorForKey.Text;
            byte[] bytes = null;
            if (loadedKey.Length < 8)
            {
                MessageBox.Show("KLUCZ JEST ZA KRÓTKI", "ZŁA DŁUGOŚĆ KLUCZA");
            }
            else if (loadedKey.Length > 8)
            {
                MessageBox.Show("KLUCZ JEST ZA DŁUGI", "ZŁA DŁUGOŚĆ KLUCZA");
            }
            else
            {

                if (!_fileOrPlainText)
                {
                    bytes = Encoding.ASCII.GetBytes(plaintext);
                }
                else
                {
                    bytes = StringToByteArray(plaintext);
                    _fileOrPlainText = false;
                }

                byte[] key = Encoding.ASCII.GetBytes(loadedKey);
                DES encryptor = new DES();
                byte[] cryptogram = encryptor.Encrypt(bytes, key);
                TxtEditorForCryptogram.Text = ByteArrayToString(cryptogram);
            }
        }

        private void Decrypt(object sender, RoutedEventArgs e)
        {
            String loadedKey = TxtEditorForKey.Text;
            String plaintext = TxtEditorForCryptogram.Text;

            byte[] bytes = StringToByteArray(plaintext);
            byte[] key = Encoding.ASCII.GetBytes(loadedKey);

            DES des = new DES();
            byte[] decode = des.Decrypt(bytes, key);
            String kod = Encoding.ASCII.GetString(decode);
            TxtEditorForPlainText.Text = kod;
        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        private void encryptFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

            if (openFileDialog.ShowDialog() == true)
            {
                byte[] fileToEncrypt = File.ReadAllBytes(openFileDialog.FileName);
                _ReadFileExtension = Path.GetExtension(openFileDialog.FileName);
                _LoadFileExtension = Path.GetExtension(openFileDialog.FileName);
                TxtEditorForPlainText.Text = ByteArrayToString(fileToEncrypt);
                _fileOrPlainText = true;
            }
        }

        private void RebuildFile_Click(object sender, RoutedEventArgs e)
        {

            SaveFileDialog saveFileDialog = (SaveFileDialog)BuildSaveFileDialog(true, "OdszyfrowanePliki");

            if (saveFileDialog.ShowDialog() == true)
            {
                byte[] byteToSave = StringToByteArray(TxtEditorForPlainText.Text);
                File.WriteAllBytes(saveFileDialog.FileName + _ReadFileExtension, byteToSave);

                if (File.Exists(saveFileDialog.FileName + _ReadFileExtension))
                {
                    MessageBox.Show("zapisano do pliku: " + saveFileDialog.FileName + _ReadFileExtension);
                }
                else
                {
                    MessageBox.Show("nie udało się zapisać pliku: " + saveFileDialog.FileName + _ReadFileExtension);
                }

            }
        }

        private void OpenFileWithCryptogramButtonWithDialog1_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog OpenFileDialog = (OpenFileDialog)BuildSaveFileDialog(false, "ZaszyfrowanePliki");

            if (OpenFileDialog.ShowDialog() == true)
            {
                byte[] byteToLoad = File.ReadAllBytes(OpenFileDialog.FileName);
                _LoadFileExtension = Path.GetExtension(OpenFileDialog.FileName);
                TxtEditorForCryptogram.Text = ByteArrayToString(byteToLoad);

            }
        }
        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                .ToArray();
        }

        private void SaveFileWithPlainTextButtonWithDialog_Copy1_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = (SaveFileDialog)BuildSaveFileDialog(true, "ZaszyfrowanePliki");

            if (saveFileDialog.ShowDialog() == true)
            {
                byte[] byteToSave = StringToByteArray(TxtEditorForCryptogram.Text);
                File.WriteAllBytes(saveFileDialog.FileName + _LoadFileExtension, byteToSave);

                if (File.Exists(saveFileDialog.FileName + _LoadFileExtension))
                {
                    MessageBox.Show("zapisano do pliku: " + saveFileDialog.FileName + _LoadFileExtension);
                }
                else
                {
                    MessageBox.Show("nie udało się zapisać pliku: " + saveFileDialog.FileName + _LoadFileExtension);
                }

            }
        }

        private void GenerateKey_OnClick(object sender, RoutedEventArgs e)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            String keyGenerated = new string(Enumerable.Repeat(chars, 8)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            TxtEditorForKey.Text = keyGenerated;
        }
    }
}

