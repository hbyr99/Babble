/*
 * Henry Baldacci (hb43)
 * CS212
 * 10/10/2020
 * Babble program
 */
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
using System.Text.RegularExpressions;
using System.Collections;
using System.Threading;
using System.Diagnostics.Tracing;



namespace Babble
{
    /// Babble framework
    /// Starter code for CS212 Babble assignment
    public partial class MainWindow : Window
    {
        private string input;               // input file
        private string[] words;             // input file broken into array of words
        private int wordCount = 200;        // number of words to babble
        private Dictionary<string, ArrayList> hashTable = new Dictionary<string, ArrayList>(); // creating a dictionary for the analysis
        private string firstWord;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void loadButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.FileName = "Sample"; // Default file name
            ofd.DefaultExt = ".txt"; // Default file extension
            ofd.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

            // Show open file dialog box
            if ((bool)ofd.ShowDialog())
            {
                textBlock1.Text = "Loading file " + ofd.FileName + "\n";
                input = System.IO.File.ReadAllText(ofd.FileName);  // read file
                words = Regex.Split(input, @"\s+");       // split into array of words
            }
        }

        private void analyzeInput(int order)
        {
            if (order > 0)
            {
                MessageBox.Show("Analyzing at order: " + order);
                firstWord = words[0];

                foreach (string word in words) // Going through every word in the array
                {

                    if (!hashTable.ContainsKey(word)) // checking if key is already in hashTable
                    {
                        hashTable.Add(word, new ArrayList());

                        for (int i = 0; i < words.Length - 1; i++)
                        {
                            if (words[i].ToString() == word)
                                hashTable[word].Add(words[i + 1]); // adding new words found to hashTable
                        }
                    }
                }
                textBlock1.Text += "\n Number of words: " + words.Length;
                textBlock1.Text += "\n Number of unique words: " + hashTable.Count + "\n";
            }
        }

        void babbleButton_Click(object sender, RoutedEventArgs e)
        {
            Random random = new Random();
            string lastWord = firstWord;
            
            // printing words to the textblock
            for (int i = 0; i < Math.Min(wordCount, words.Length); i++)
            {
                textBlock1.Text += " " + lastWord;
                int nxt = random.Next(0, hashTable[lastWord].Count - 1); // choosing a random int for the next value in the hashtable with key the previous word
                lastWord = hashTable[lastWord][nxt].ToString();
            }
        }

        void orderComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            analyzeInput(orderComboBox.SelectedIndex);
        }
    }
}
