using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WMPLib;

namespace HangmanWind
{
    public partial class Form1 : Form
    {

        string[] listofWordData = { "ICECREAM", "ERASER", "PENCIL", "TRIANGLE", "SQUARE", "POLYGON", "CIRCLE", "RECTANGLE" };
        Random random = new Random((int)DateTime.Now.Ticks);
        private int totalTryAllowed = 5;
        private int userTried = 0;
        private int WonGame = 0;
        private int LossGame = 0;

        private string selectedWord = "";

        private List<Image> imageList = new List<Image>();
        private List<char> displayWord = new List<char>();
        private List<char> remainingWord = new List<char>();
        private List<char> foundWord = new List<char>();

        private System.Timers.Timer gameTimer = new System.Timers.Timer();
        WindowsMediaPlayer Player;
        public Form1()
        {
            InitializeComponent();

            UpdateControlsStatus(false);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //imageList.Add(HangmanWind.Properties.Resources._0);
            imageList.Add(HangmanWind.Properties.Resources._1);
            //imageList.Add(HangmanWind.Properties.Resources._2);
            imageList.Add(HangmanWind.Properties.Resources._3);
            //imageList.Add(HangmanWind.Properties.Resources._4);
            imageList.Add(HangmanWind.Properties.Resources._5);
            //imageList.Add(HangmanWind.Properties.Resources._6);
            imageList.Add(HangmanWind.Properties.Resources._7);
            //imageList.Add(HangmanWind.Properties.Resources._8);
            imageList.Add(HangmanWind.Properties.Resources._9);

            gameTimer.Interval = 247 * 1000;
            gameTimer.AutoReset = false;
            gameTimer.Elapsed += GameTimer_Elapsed;
            gameTimer.Stop();

            Player = new WMPLib.WindowsMediaPlayer();
            if (File.Exists(@"Benny Mardones Into The Night.mp3"))
                Player.URL = @"Benny Mardones Into The Night.mp3";
            Player.controls.play();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbUser.Text))
            {
                MessageBox.Show("Please enter a username first with more than 3 character!");
                return;
            }

            ResetWord();
            StartNewGame();
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbUser.Text))
            {
                MessageBox.Show("Please enter a username first with more than 3 character!");
                return;
            }

            if (tbInput.Text.Length <= 0 || tbInput.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please enter a character you wat to try!");
                return;
            }

            string inputStr = tbInput.Text;
            char inputchar = Convert.ToChar(inputStr);

            if (!remainingWord.Contains(inputchar))
            {
                pictureBox1.Image = imageList[userTried];
                userTried++;
                if (userTried == totalTryAllowed)
                {
                    MessageBox.Show("Game Over! Lets try again");
                    LossGame++;
                    lblLoss.Text = LossGame.ToString();
                    ResetWord();
                }
            }
            else
            {
                int idx = remainingWord.IndexOf(inputchar);
                remainingWord[idx] = ' ';
                foundWord.Add(inputchar);

                displayWord[idx] = inputchar;
                lblGuess.Text = string.Join(" ", displayWord);

                if (foundWord.Count >= selectedWord.Length)
                {
                    WonGame++;
                    lblWin.Text = WonGame.ToString();
                    MessageBox.Show("You won! Great let's try next");
                    ResetWord();
                    return;
                }
            }

            tbInput.Text = "";
            tbInput.Focus();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ResetWord();
        }

        private void GameTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            MessageBox.Show("Time Over! Let's try again");
            LossGame++;
            ResetWord(true);
        }

        void ResetWord(bool lost = false)
        {
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker) (() => ResetWord(lost)));
            }
            else
            {
                lblLoss.Text = LossGame.ToString();

                gameTimer.Stop();
                //if(Player != null)
                //    Player.controls.stop();

                selectedWord = "";
                userTried = 0;
                displayWord.Clear();
                remainingWord.Clear();
                foundWord.Clear();
                lblGuess.Text = "Hang Man Game";
                tbInput.Text = "";
                pictureBox1.Image = Properties.Resources.hangman_wnd;

                UpdateControlsStatus(false);
            }
        }

        void UpdateControlsStatus(bool newstatus)
        {
            tbInput.Enabled = btnCheck.Enabled = btnClear.Enabled = newstatus;
            tbUser.Enabled = btnStart.Enabled = !newstatus;
        }

        void StartNewGame()
        {
            selectedWord = listofWordData[random.Next(0, listofWordData.Length)];
            remainingWord = selectedWord.ToCharArray().ToList();

            foreach (var item in selectedWord.ToCharArray())
            {
                displayWord.Add('_');
            }

            lblGuess.Text = string.Join(" ", displayWord);

            UpdateControlsStatus(true);
            tbInput.Focus();
            gameTimer.Start();

            //Player.controls.play();
        }
    }
}
