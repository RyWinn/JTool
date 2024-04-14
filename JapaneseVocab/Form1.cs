using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace JTool
{
    public partial class Form1 : Form
    {
        private List<VocabModel> Vocabs = new List<VocabModel>();
        private VocabModel JapaneseRandomVocab;
        private VocabModel EnglishRandomVocab;

        private KeyValuePair<string, string> HiraganaChar;
        private KeyValuePair<string, string> KatakanaChar;

        private string RandomNumber;

        private Random Random = new Random();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            XmlSerializer XS = new XmlSerializer(typeof(List<VocabModel>));

            if (File.Exists("Data.xml"))
            {
                using (FileStream fs = new FileStream("Data.xml", FileMode.Open))
                {
                    Vocabs = XS.Deserialize(fs) as List<VocabModel>;
                }
            }

            vocabModelBindingSource.DataSource = Vocabs;

            GetRandomJapaneseWord();
            GetRandomEnglishWord();
            GetRandomHiraganaChar();
            GetRandomKatakanaChar();
            GetRandomNumber();

            LoadNotes();
        }

        private void button_Save_Click(object sender, EventArgs e)
        {
            VocabModel Vocab = new VocabModel();
            Vocab.EnglishWord = textBox_English.Text;
            Vocab.JapaneseWord = textBox_Japanese.Text;

            if (Vocab.EnglishWord != "" && Vocab.JapaneseWord != "")
            {
                //Check to see English/Japanese word combo already exists
                bool IsDuplicate = Vocabs.Any(v => v.EnglishWord.Trim().ToLower() == Vocab.EnglishWord.Trim().ToLower() && v.JapaneseWord.Trim().ToLower() == Vocab.JapaneseWord.Trim().ToLower());

                if (!IsDuplicate)
                {
                    Vocabs.Add(Vocab);

                    XmlSerializer XS = new XmlSerializer(typeof(List<VocabModel>));

                    using (FileStream fs = new FileStream("Data.xml", FileMode.Create))
                    {
                        XS.Serialize(fs, Vocabs);
                    }

                    //Have to set to null to refresh for some reason
                    vocabModelBindingSource.DataSource = null;
                    vocabModelBindingSource.DataSource = Vocabs;

                    textBox_English.Clear();
                    textBox_Japanese.Clear();
                }
                else
                {
                    MessageBox.Show("This word has already been added");
                }
            }
            else
            {
                MessageBox.Show("Please enter both words");
            }
        }

        private void button_Delete_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow SelectedRow in dataGridView1.SelectedRows)
            {
                var VocabToRemove = SelectedRow.DataBoundItem as VocabModel;

                Vocabs.Remove(VocabToRemove);
            }

            XmlSerializer XS = new XmlSerializer(typeof(List<VocabModel>));

            using (FileStream fs = new FileStream("Data.xml", FileMode.Create))
            {
                XS.Serialize(fs, Vocabs);
            }

            //Have to set to null to refresh for some reason
            vocabModelBindingSource.DataSource = null;
            vocabModelBindingSource.DataSource = Vocabs;

            GetRandomJapaneseWord();
            GetRandomEnglishWord();
            GetRandomHiraganaChar();
            GetRandomKatakanaChar();
        }

        private void GetRandomJapaneseWord()
        {
            if (Vocabs.Count > 0)
            {
                JapaneseRandomVocab = Vocabs[Random.Next(Vocabs.Count)];

                if (label_JapaneseWord.Text != JapaneseRandomVocab.JapaneseWord)
                {
                    label_JapaneseWord.Text = JapaneseRandomVocab.JapaneseWord;
                }
                else //We don't want repeat random words so we run it again.
                {
                    GetRandomJapaneseWord();
                }
            }
        }

        private void GetRandomEnglishWord()
        {
            if (Vocabs.Count > 0)
            {
                EnglishRandomVocab = Vocabs[Random.Next(Vocabs.Count)];

                if (label_EnglishWord.Text != EnglishRandomVocab.EnglishWord)
                {
                    label_EnglishWord.Text = EnglishRandomVocab.EnglishWord;
                }
                else //We don't want repeat random words so we run it again.
                {
                    GetRandomEnglishWord();
                }
            }
        }

        private void GetRandomHiraganaChar()
        {
            HiraganaChar = cHiraganaKatakana.HiraganaMap.ElementAt(Random.Next(cHiraganaKatakana.HiraganaMap.Count));

            if (label_HiraganaChar.Text != HiraganaChar.Value)
            {
                label_HiraganaChar.Text = HiraganaChar.Value;
            }
            else
            {
                GetRandomHiraganaChar();
            }
        }

        private void GetRandomKatakanaChar()
        {
            KatakanaChar = cHiraganaKatakana.KatakanaMap.ElementAt(Random.Next(cHiraganaKatakana.KatakanaMap.Count));

            if (label_KatakanaChar.Text != KatakanaChar.Value)
            {
                label_KatakanaChar.Text = KatakanaChar.Value;
            }
            else
            {
                GetRandomKatakanaChar();
            }
        }

        private void button_Submit_Click(object sender, EventArgs e)
        {
            if (textBox_EnglishAnswer.Text.ToLower().Trim() == JapaneseRandomVocab.EnglishWord.ToLower().Trim())
            {
                MessageBox.Show("Correct!");

                GetRandomJapaneseWord();

                textBox_EnglishAnswer.Clear();
            }
            else
            {
                MessageBox.Show("Wrong answer. Try again");
            }
        }

        private void textBox_English_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                button_Save.PerformClick();
                e.Handled = true;
            }
        }

        private void textBox_Japanese_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                button_Save.PerformClick();
                e.Handled = true;

                textBox_English.Focus();
            }
        }

        private void textBox_EnglishAnswer_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                button_SubmitEnglish.PerformClick();
                e.Handled = true;
            }
        }

        private void button_SubmitJapanese_Click(object sender, EventArgs e)
        {
            if (textBox_JapaneseAnswer.Text.ToLower().Trim() == EnglishRandomVocab.JapaneseWord.ToLower().Trim())
            {
                MessageBox.Show("Correct!");

                GetRandomEnglishWord();

                textBox_JapaneseAnswer.Clear();
            }
            else
            {
                MessageBox.Show("Wrong answer. Try again");
            }
        }

        private void textBox_JapaneseAnswer_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                button_SubmitJapanese.PerformClick();
                e.Handled = true;
            }
        }

        private void button_SubmitHiragana_Click(object sender, EventArgs e)
        {
            if (textBox_Hiragana.Text.ToLower().Trim() == HiraganaChar.Key.ToLower().Trim())
            {
                MessageBox.Show("Correct!");

                GetRandomHiraganaChar();

                textBox_Hiragana.Clear();
            }
            else
            {
                MessageBox.Show("Wrong answer. Try again");
            }
        }

        private void button_SubmitKatakana_Click(object sender, EventArgs e)
        {
            if (textBox_Katakana.Text.ToLower().Trim() == KatakanaChar.Key.ToLower().Trim())
            {
                MessageBox.Show("Correct!");

                GetRandomKatakanaChar();

                textBox_Katakana.Clear();
            }
            else
            {
                MessageBox.Show("Wrong answer. Try again");
            }
        }

        private void textBox_Hiragana_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                button_SubmitHiragana.PerformClick();
                e.Handled = true;
            }
        }

        private void textBox_Katakana_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                button_SubmitKatakana.PerformClick();
                e.Handled = true;
            }
        }

        private void checkBox_HideVocabList_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_HideVocabList.Checked)
            {
                vocabModelBindingSource.DataSource = null;
            }
            else
            {
                vocabModelBindingSource.DataSource = Vocabs;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            tableLayoutPanel_Hiragana.Visible = !checkBox_HideLetters.Checked;
            label44.Visible = !checkBox_HideLetters.Checked;
            label46.Visible = !checkBox_HideLetters.Checked;
            label54.Visible = !checkBox_HideLetters.Checked;
            label56.Visible = !checkBox_HideLetters.Checked;
            label58.Visible = !checkBox_HideLetters.Checked;
        }

        private void SaveNotes()
        {
            File.WriteAllText("Notes.txt", richTextBox1.Text);
        }

        private void LoadNotes()
        {
            if (File.Exists("Notes.txt"))
            {
                richTextBox1.Text = File.ReadAllText("Notes.txt");
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveNotes();
        }

        private void checkBox_Katakana_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Katakana.Checked) //Change labels to Katakana translation
            {
                label5.Text = "A = ア";
                label9.Text = "I = イ";
                label10.Text = "U = ウ";
                label11.Text = "E = エ";
                label12.Text = "O = オ";
                label13.Text = "Ka = カ";
                label14.Text = "Ki = キ";
                label15.Text = "Ku = ク";
                label16.Text = "Ke = ケ";
                label17.Text = "Ko = コ";
                label18.Text = "Sa = サ";
                label19.Text = "Shi = シ";
                label20.Text = "Su = ス";
                label21.Text = "Se = セ";
                label22.Text = "So = ソ";
                label23.Text = "Ta = タ";
                label24.Text = "Chi = チ";
                label25.Text = "Tsu = ツ";
                label26.Text = "Te = テ";
                label27.Text = "To = ト";
                label28.Text = "Na = ナ";
                label29.Text = "Ni = ニ";
                label30.Text = "Nu = ヌ";
                label31.Text = "Ne = ネ";
                label32.Text = "No = ノ";
                label33.Text = "Ha = ハ";
                label34.Text = "Hi = ヒ";
                label35.Text = "Fu = フ";
                label36.Text = "He = ヘ";
                label37.Text = "Ho = ホ";
                label38.Text = "Ma = マ";
                label39.Text = "Mi = ミ";
                label40.Text = "Mu = ム";
                label41.Text = "Me = メ";
                label42.Text = "Mo = モ";
                label43.Text = "Ya = ヤ";
                label45.Text = "Yu = ユ";
                label47.Text = "Yo = ヨ";
                label48.Text = "Ra = ラ";
                label49.Text = "Ri = リ";
                label50.Text = "Ru = ル";
                label51.Text = "Re = レ";
                label52.Text = "Ro = ロ";
                label53.Text = "Wa = ワ";
                label55.Text = "Wo = ヲ";
                label57.Text = "N = ン";

                label44.Text = "G letters are the same as K but with a \" on top. E.g. Ka = カ and Ga = ガ";
                label58.Text = "P letters are the same as H but with a dot above. E.g. Ha = ハ and Pa = パ";
            }
            else //Change back to Hiragana
            {
                label5.Text = "A = あ";
                label9.Text = "I = い";
                label10.Text = "U = う";
                label11.Text = "E = え";
                label12.Text = "O = お";
                label13.Text = "Ka = か";
                label14.Text = "Ki = き";
                label15.Text = "Ku = く";
                label16.Text = "Ke = け";
                label17.Text = "Ko = こ";
                label18.Text = "Sa = さ";
                label19.Text = "Shi = し";
                label20.Text = "Su = す";
                label21.Text = "Se = せ";
                label22.Text = "So = そ";
                label23.Text = "Ta = た";
                label24.Text = "Chi = ち";
                label25.Text = "Tsu = つ";
                label26.Text = "Te = て";
                label27.Text = "To = と";
                label28.Text = "Na = な";
                label29.Text = "Ni = に";
                label30.Text = "Nu = ぬ";
                label31.Text = "Ne = ね";
                label32.Text = "No = の";
                label33.Text = "Ha = は";
                label34.Text = "Hi = ひ";
                label35.Text = "Fu = ふ";
                label36.Text = "He = へ";
                label37.Text = "Ho = ほ";
                label38.Text = "Ma = ま";
                label39.Text = "Mi = み";
                label40.Text = "Mu = む";
                label41.Text = "Me = め";
                label42.Text = "Mo = も";
                label43.Text = "Ya = や";
                label45.Text = "Yu = ゆ";
                label47.Text = "Yo = よ";
                label48.Text = "Ra = ら";
                label49.Text = "Ri = り";
                label50.Text = "Ru = る";
                label51.Text = "Re = れ";
                label52.Text = "Ro = ろ";
                label53.Text = "Wa = わ";
                label55.Text = "Wo = を";
                label57.Text = "N = ん";

                label44.Text = "G letters are the same as K but with a \" on top. E.g. Ka = か and Ga = が";
                label58.Text = "P letters are the same as H but with a dot above. E.g. Ha = は and Pa = ぱ";
            }
        }

        private void GetRandomNumber()
        {
            Dictionary<int, string> numbers = cNumber.Numbers;

            //Random number
            int randomNumberIndex = Random.Next(1, cNumber.Numbers.Count + 1);
            KeyValuePair<int, string> randomNumber = cNumber.Numbers.ElementAt(randomNumberIndex - 1);

            //Random number counter
            var numberCounts = Enum.GetValues(typeof(cNumber.NumberCounts));
            int numberCount = Random.Next(0, numberCounts.Length);

            //Logic to generate number plus the counter
            if (numberCount == (int)cNumber.NumberCounts.Things)
            {
                label_Number.Text = cNumber.GetThingCounter(randomNumber);

                RandomNumber = randomNumber.Key.ToString() + " Things";
            }
            else if (numberCount == (int)cNumber.NumberCounts.People)
            {
                label_Number.Text = cNumber.GetPeopleCounter(randomNumber);

                RandomNumber = randomNumber.Key.ToString() + " People";
            }
            else if (numberCount == (int)cNumber.NumberCounts.Machines)
            {
                label_Number.Text = randomNumber.Value + "dai";

                RandomNumber = randomNumber.Key.ToString() + " Machines";
            }
            else if (numberCount == (int)cNumber.NumberCounts.Years)
            {
                label_Number.Text = randomNumber.Value + "nen";

                RandomNumber = randomNumber.Key.ToString() + " Years";
            }
            else if (numberCount == (int)cNumber.NumberCounts.Months)
            {
                label_Number.Text = cNumber.GetMonthCounter(randomNumber);

                RandomNumber = randomNumber.Key.ToString() + " Months";
            }
            else if (numberCount == (int)cNumber.NumberCounts.Weeks)
            {
                label_Number.Text = cNumber.GetWeekCounter(randomNumber);

                RandomNumber = randomNumber.Key.ToString() + " Weeks";
            }
            else if (numberCount == (int)cNumber.NumberCounts.Days)
            {
                label_Number.Text = cNumber.GetDayCounter(randomNumber);

                RandomNumber = randomNumber.Key.ToString() + " Days";
            }
            else if (numberCount == (int)cNumber.NumberCounts.Hours)
            {
                label_Number.Text = randomNumber.Value + "jikan";

                RandomNumber = randomNumber.Key.ToString() + " Hours";
            }
            else if (numberCount == (int)cNumber.NumberCounts.Minutes)
            {
                label_Number.Text = cNumber.GetMinuteCounter(randomNumber);

                RandomNumber = randomNumber.Key.ToString() + " Minutes";
            }
            else if (numberCount == (int)cNumber.NumberCounts.FlatThings)
            {
                label_Number.Text = randomNumber.Value + "mai";

                RandomNumber = randomNumber.Key.ToString() + " Flat things";
            }
            else if (numberCount == (int)cNumber.NumberCounts.Times)
            {
                label_Number.Text = cNumber.GetTimeCounter(randomNumber);

                RandomNumber = randomNumber.Key.ToString() + " Times";
            }
        }

        private void button_SubmitNumber_Click(object sender, EventArgs e)
        {
            if (textBox_Number.Text.ToLower().Trim() == RandomNumber.ToLower().Trim())
            {
                MessageBox.Show("Correct!");

                GetRandomNumber();

                textBox_Number.Clear();
            }
            else
            {
                MessageBox.Show("Wrong answer. Try again");
            }
        }

        private void textBox_Number_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                button_SubmitNumber.PerformClick();
                e.Handled = true;
            }
        }

        private void checkBox_HideCounters_CheckedChanged(object sender, EventArgs e)
        {
            label59.Visible = !checkBox_HideCounters.Checked;
            label6.Visible = !checkBox_HideCounters.Checked;
            label7.Visible = !checkBox_HideCounters.Checked;
            label8.Visible = !checkBox_HideCounters.Checked;
            label60.Visible = !checkBox_HideCounters.Checked;
            label61.Visible = !checkBox_HideCounters.Checked;
            label62.Visible = !checkBox_HideCounters.Checked;
            label63.Visible = !checkBox_HideCounters.Checked;
            label64.Visible = !checkBox_HideCounters.Checked;
            label65.Visible = !checkBox_HideCounters.Checked;
            label66.Visible = !checkBox_HideCounters.Checked;
            label67.Visible = !checkBox_HideCounters.Checked;
            label68.Visible = !checkBox_HideCounters.Checked;
            label69.Visible = !checkBox_HideCounters.Checked;
        }
    }
}