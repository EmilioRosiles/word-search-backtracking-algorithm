using System.Linq;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Threading;

namespace WordSearch
{
    public partial class Form1 : Form
    {
        static string FILENAMEWORDS = "words.txt";
        static string FILENAMEPUZZLE = "puzzle.txt";
        string[] wordsToFind = new string[100];
        string[] wordArray = new string[100];
        string[] puzzleArray = new string[100];
        DataGridView dataGridViewPuzzle;
        DataGridView dataGridViewWords;
        public Form1()
        {
            InitializeComponent();
            GetArrayFromFile(FILENAMEWORDS, wordArray);
            GetArrayFromFile(FILENAMEWORDS, wordsToFind);
            GetArrayFromFile(FILENAMEPUZZLE, puzzleArray);
            dataGridViewPuzzle = dataGridView1;
            dataGridViewWords = dataGridView2;
            CreateGrid(dataGridViewPuzzle, puzzleArray);
            CreateGrid(dataGridViewWords, wordArray);
        }
        public void Button1_Click(object sender, EventArgs e)
        {
            Stream myStream = null;
            OpenFileDialog openThePuzzle = new OpenFileDialog();
            openThePuzzle.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
            openThePuzzle.Filter = "txt files (*.txt)|*.txt";
            openThePuzzle.FilterIndex = 2;
            openThePuzzle.RestoreDirectory = true;

            if (openThePuzzle.ShowDialog() == DialogResult.OK)
            {
                RestartGrid(puzzleArray, dataGridViewPuzzle);
                try
                {
                    if ((myStream = openThePuzzle.OpenFile()) != null)
                    {
                        FILENAMEPUZZLE = openThePuzzle.FileName;
                        GetArrayFromFile(FILENAMEPUZZLE, puzzleArray);
                        CreateGrid(dataGridViewPuzzle, puzzleArray);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }
        public void Button2_Click(object sender, EventArgs e)
        {
            Stream myStream = null;
            OpenFileDialog openThePuzzle = new OpenFileDialog();
            openThePuzzle.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
            openThePuzzle.Filter = "txt files (*.txt)|*.txt";
            openThePuzzle.FilterIndex = 2;
            openThePuzzle.RestoreDirectory = true;
            if (openThePuzzle.ShowDialog() == DialogResult.OK)
            {
                RestartGrid(wordArray, dataGridViewWords);
                try
                {
                    if ((myStream = openThePuzzle.OpenFile()) != null)
                    {
                        FILENAMEWORDS = openThePuzzle.FileName;
                        GetArrayFromFile(FILENAMEWORDS, wordArray);
                        GetArrayFromFile(FILENAMEWORDS, wordsToFind);
                        CreateGrid(dataGridViewWords, wordArray);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }
        public void Button3_Click(object sender, EventArgs e)
        {
            GetArrayFromFile(FILENAMEWORDS, wordsToFind);
            RestartGridColor(wordArray, dataGridViewWords);
            RestartGridColor(puzzleArray, dataGridViewPuzzle);
            bool useTracker = false;
            CheckWords(useTracker);
            int numWFounded = 0;
            int NumWords = NumOfWordsInArray(wordArray);
            for (int c = 0; c < NumWords; c++)
            {
                if (dataGridViewWords[0, c].Style.BackColor == Color.Yellow)
                {
                    numWFounded++;
                }
            }
            Form2 mssg = new Form2(NumWords, numWFounded);
            DialogResult dialogresult = mssg.ShowDialog();
        }
        public void Button4_Click(object sender, EventArgs e)
        {
            GetArrayFromFile(FILENAMEWORDS, wordsToFind);
            RestartGridColor(wordArray, dataGridViewWords);
            RestartGridColor(puzzleArray, dataGridViewPuzzle);
            bool useTracker = true;
            CheckWords(useTracker);
            int numWFounded = 0;
            int NumWords = NumOfWordsInArray(wordArray);
            for (int c =0; c< NumWords; c++)
            {
                if(dataGridViewWords[0, c].Style.BackColor == Color.Yellow)
                {
                    numWFounded++;
                }
            }
            Form2 mssg = new Form2(NumWords, numWFounded);
            DialogResult dialogresult = mssg.ShowDialog();
        }
        public void GetArrayFromFile(string FILENAME, string[] array)
        {
            FileStream file = new FileStream(FILENAME, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(file);
            string readLine;
            readLine = reader.ReadLine();
            int rows = 0;
            while (readLine != null)
            {
                array[rows] = readLine;
                rows++;
                readLine = reader.ReadLine();
            }
            reader.Close();
            file.Close();
        }
        public void CreateGrid(DataGridView Grid, string[] Array)
        {
            Grid.Columns.Clear();
            Grid.Rows.Clear();
            Grid.RowHeadersVisible = false;
            Grid.ColumnHeadersVisible = false;
            int columns = NumOfLettersInLongestWordInArray(Array);
            int rows = NumOfWordsInArray(Array);
            for (int i = 0; i < columns; i++)
            {
                Grid.Columns.Add("x" + i.ToString(), "x" + i.ToString());
                DataGridViewColumn column = Grid.Columns[i];
                column.Width = 20;
            }
            Grid.Rows.Add(rows - 1);
            LoadWordsInGrid(Array, Grid);
        }
        public int NumOfLettersInLongestWordInArray(string[] Array)
        {
            int columns = Array[0].Length;
            for (int a = 0; a < Array.Length; a++)
            {
                if (Array[a] != null)
                {
                    if (columns < Array[a].Length)
                    {
                        columns = Array[a].Length;
                    }
                }
            }
            return columns;
        }
        public int NumOfWordsInArray(string[] Array)
        {
            int rows = 0;
            for (int a = 0; a < Array.Length; a++)
            {
                if (Array[a] != null)
                {
                    rows += 1;
                }
            }
            return rows;
        }
        public void LoadWordsInGrid(String[] Array, DataGridView Grid)
        {
            int NumWords = NumOfWordsInArray(Array);
            for (int x = 0; x < NumWords; x++)
            {
                string theword = Array[x];

                for (int y = 0; y < theword.Length; y++)
                {
                    Grid[y, x].Value = theword[y];
                    Grid[y, x].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }
        }
        public void RestartGridColor(String[] Array, DataGridView Grid)
        {
            int rows = NumOfWordsInArray(Array);
            int columns = NumOfLettersInLongestWordInArray(Array);
            for (int x = 0; x < rows; x++)
            {
                for (int y = 0; y < columns; y++)
                {
                    Grid[y, x].Style.BackColor = Color.White;
                }
            }
        }
        public void RestartGrid(String[] Array, DataGridView Grid)
        {
            int rows = NumOfWordsInArray(Array);
            for (int x = 0; x < rows; x++)
            {
                RestartGridColor(Array, Grid);
            }
            for (int x = 0; x < rows; x++)
            {
                Array[x] = null;
            }
        }
        public void CheckWords(bool trackerOn)
        {
            
            int num = NumOfWordsInArray(wordArray);
            int rows = NumOfWordsInArray(puzzleArray);
            int columns = NumOfLettersInLongestWordInArray(puzzleArray);
            string wordsFirstLetters = "";
            for (int i = 0; i < num; i++)
            {
                string word = wordArray[i];
                wordsFirstLetters = wordsFirstLetters + Convert.ToString(word[0]);
            }
            if (trackerOn)
            {
                for(int a = 0; a < num; a++)
                {
                    dataGridViewWords[0, a].Style.BackColor = Color.Orange;
                }
                dataGridViewWords.Update();
            }
            for(int y = 0; y < rows; y++)
            {
                string str = puzzleArray[y];
                for(int x = 0; x < columns; x++)
                {
                    
                    if (trackerOn && dataGridViewPuzzle[x, y].Style.BackColor != Color.Red && dataGridViewPuzzle[x, y].Style.BackColor != Color.Yellow)
                    { 
                        dataGridViewPuzzle[x, y].Style.BackColor = Color.Orange;
                        dataGridViewPuzzle.Update();
                    }
                    if (wordsFirstLetters.Contains(str[x]))
                    {
                        SearchWord(wordsFirstLetters, str[x], x, y, trackerOn);
                        
                    }
                    if (trackerOn && dataGridViewPuzzle[x, y].Style.BackColor == Color.Orange)
                    {
                        dataGridViewPuzzle[x, y].Style.BackColor = Color.White;
                    }
                }
            }
        }
        public void SearchWord(string wordsFirstLetters, char letterInPuzzle, int x, int y, bool trackerOn)
        {
            int location;
            string word;
            for (int l = 0; l < wordsFirstLetters.Length; l++)
            {
                if (wordsFirstLetters[l] == letterInPuzzle)
                {
                    location = l;
                    word = wordArray[location];
                    CheckAllDirections(word, x, y, trackerOn, location);
                }
            }
            
        }
        public void CheckAllDirections(string word, int x, int y, bool trackerOn, int location)
        {
            CheckHorizontally(x, y, word, trackerOn, location);
            CheckHorizontallyInv(x, y, word, trackerOn, location);
            CheckVertically(x, y, word, trackerOn, location);
            CheckVerticallyInv(x, y, word, trackerOn, location);
        }
        public void CheckHorizontally(int x, int y, string word, bool trackerOn, int location)
        {
            int numCorrect = 0;
            string str = puzzleArray[y];
            
            for (int w = 0; w < word.Length; w++)
            {
                if (x + w < str.Length)
                {
                    if (word[w] == str[x + w])
                    {
                        if(trackerOn)
                        {
                            if (dataGridViewPuzzle[x + w, y].Style.BackColor != Color.Yellow)
                            {
                                dataGridViewPuzzle[x + w, y].Style.BackColor = Color.Red;
                            }
                            if (dataGridViewWords[0, location].Style.BackColor != Color.Yellow)
                            {
                                dataGridViewWords[w, location].Style.BackColor = Color.Red;
                            }
                            dataGridViewWords.Update();
                            dataGridViewPuzzle.Update();
                        }
                        numCorrect++;
                    }

                    if (numCorrect != w + 1)
                    {
                        if (trackerOn && dataGridViewWords[0, location].Style.BackColor != Color.Yellow)
                        {
                            for (int k = 1; k < word.Length; k++)
                            {
                                dataGridViewWords[k, location].Style.BackColor = Color.White;
                            }
                            dataGridViewWords[0, location].Style.BackColor = Color.Orange;
                            dataGridViewWords.Refresh();
                        }
                        break;
                    }
                    if (numCorrect == word.Length)
                    {
                        int lenght = word.Length - 1;

                        dataGridViewPuzzle[x + lenght, y].Style.BackColor = Color.Yellow;
                        
                        while (lenght >= 0)
                        {
                            dataGridViewPuzzle[x + lenght, y].Style.BackColor = Color.Yellow;
                            dataGridViewWords[lenght, location].Style.BackColor = Color.Yellow;
                            lenght--;
                        }
                        break;
                    }
                }
            }
        }
        public void CheckHorizontallyInv(int x, int y, string word, bool trackerOn, int location)
        {
            int numCorrect = 0;
            string str = puzzleArray[y];
            for (int w = 0; w < word.Length; w++)
            {
                if (x - w >= 0)
                {
                    if (word[w] == str[x - w])
                    {
                        if (trackerOn)
                        {
                            if (dataGridViewPuzzle[x - w, y].Style.BackColor != Color.Yellow)
                            {
                                dataGridViewPuzzle[x - w, y].Style.BackColor = Color.Red;
                            }
                            if (dataGridViewWords[0, location].Style.BackColor != Color.Yellow)
                            {
                                dataGridViewWords[w, location].Style.BackColor = Color.Red;
                            }
                            dataGridViewWords.Update();
                            dataGridViewPuzzle.Update();
                        }
                        numCorrect++;
                    }

                    if (numCorrect != w + 1)
                    {
                        if (trackerOn && dataGridViewWords[0, location].Style.BackColor != Color.Yellow)
                        {
                            for (int k = 1; k < word.Length; k++)
                            {
                                dataGridViewWords[k, location].Style.BackColor = Color.White;
                            }
                            dataGridViewWords[0, location].Style.BackColor = Color.Orange;
                            dataGridViewWords.Refresh();
                        }
                        break;
                    }
                    if (numCorrect == word.Length)
                    {
                        int lenght = word.Length-1;
                        
                        dataGridViewPuzzle[x - lenght, y].Style.BackColor = Color.Yellow;
                        while (lenght >= 0)
                        {
                            dataGridViewPuzzle[x - lenght, y].Style.BackColor = Color.Yellow;
                            dataGridViewWords[lenght, location].Style.BackColor = Color.Yellow;
                            lenght--;
                        }
                        break;
                    }
                }
            }
        }
        public void CheckVertically(int x, int y, string word, bool trackerOn, int location)
        {
            int numCorrect = 0;
            
            int colN = NumOfLettersInLongestWordInArray(puzzleArray);
            int rowN = NumOfWordsInArray(puzzleArray);
            for (int w = 0; w < word.Length; w++)
            {
                if ( y + w < rowN)
                {
                    if (Convert.ToString(word[w]) == Convert.ToString(dataGridViewPuzzle[x, y + w].Value))
                    {
                        if (trackerOn)
                        {
                            if (dataGridViewPuzzle[x , y+w].Style.BackColor != Color.Yellow)
                            {
                                dataGridViewPuzzle[x , y+w].Style.BackColor = Color.Red;
                            }
                            if (dataGridViewWords[0, location].Style.BackColor != Color.Yellow)
                            {
                                dataGridViewWords[w, location].Style.BackColor = Color.Red;
                            }
                            dataGridViewWords.Update();
                            dataGridViewPuzzle.Update();
                        }
                        numCorrect++;
                        
                    }

                    if (numCorrect != w + 1)
                    {
                        if (trackerOn && dataGridViewWords[0, location].Style.BackColor != Color.Yellow)
                        {
                            for (int k = 1; k < word.Length; k++)
                            {
                                dataGridViewWords[k, location].Style.BackColor = Color.White;
                            }
                            dataGridViewWords[0, location].Style.BackColor = Color.Orange;
                            dataGridViewWords.Refresh();
                        }
                        break;
                    }
                    if (numCorrect == word.Length)
                    {
                        int lenght = word.Length - 1;

                        dataGridViewPuzzle[x, y + lenght].Style.BackColor = Color.Yellow;
                        while (lenght >= 0)
                        {
                            dataGridViewPuzzle[x, y + lenght].Style.BackColor = Color.Yellow;
                            dataGridViewWords[lenght, location].Style.BackColor = Color.Yellow;
                            lenght--;
                        }
                        break;
                    }
                }
            }
        }
        public void CheckVerticallyInv(int x, int y, string word, bool trackerOn, int location)
        {
            int numCorrect = 0;
            int rowN = NumOfWordsInArray(puzzleArray);
            for (int w = 0; w < word.Length; w++)
            {
                if ( y - w >= 0)
                {
                    if (Convert.ToString(word[w]) == Convert.ToString(dataGridViewPuzzle[x, y - w].Value))
                    {
                        if (trackerOn)
                        {
                            if (dataGridViewPuzzle[x, y-w].Style.BackColor != Color.Yellow)
                            {
                                dataGridViewPuzzle[x, y-w].Style.BackColor = Color.Red;
                            }
                            if (dataGridViewWords[0, location].Style.BackColor != Color.Yellow)
                            {
                                dataGridViewWords[w, location].Style.BackColor = Color.Red;
                            }
                            dataGridViewWords.Update();
                            dataGridViewPuzzle.Update();
                        }
                        numCorrect++;
                    }

                    if (numCorrect != w + 1)
                    {
                        if (trackerOn && dataGridViewWords[0, location].Style.BackColor != Color.Yellow)
                        {
                            for (int k = 1; k < word.Length; k++)
                            {
                                dataGridViewWords[k, location].Style.BackColor = Color.White;
                            }
                            dataGridViewWords[0, location].Style.BackColor = Color.Orange;
                            dataGridViewWords.Refresh();
                        }
                        break;
                    }
                    if (numCorrect == word.Length)
                    {
                        int lenght = word.Length - 1;

                        dataGridViewPuzzle[x, y - lenght].Style.BackColor = Color.Yellow;
                        while (lenght >= 0)
                        {
                            dataGridViewPuzzle[x, y - lenght].Style.BackColor = Color.Yellow;
                            dataGridViewWords[lenght, location].Style.BackColor = Color.Yellow;
                            lenght--;
                        }
                        break;
                    }
                }
            }
        }
    }
}
