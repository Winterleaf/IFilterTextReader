﻿using System;
using System.Windows.Forms;

/*
   Copyright 2014 Kees van Spelde

   Licensed under The Code Project Open License (CPOL) 1.02;
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

     http://www.codeproject.com/info/cpol10.aspx

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/
using IFilterTextReader;

namespace IFilterTextViewer
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void SelectButton_Click(object sender, EventArgs e)
        {
            // Create an instance of the open file dialog box.
            var openFileDialog1 = new OpenFileDialog
            {
                // ReSharper disable once LocalizableElement
                Filter = "Alle files (*.*)|*.*",
                FilterIndex = 1,
                Multiselect = false
            };

            // Process input if the user clicked OK.
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileLabel.Text = openFileDialog1.FileName;
                FindTextButton.Enabled = true;
                TextToFindTextBox.Enabled = true;
                FindWithRegexButton.Enabled = true;
                TextToFindWithRegexTextBox.Enabled = true;

                try
                {
                    using (var reader = new FilterReader(openFileDialog1.FileName))
                    {
                        var text = reader.ReadToEnd();
                        FilterTextBox.Text = text;
                    }
                }
                catch (Exception ex)
                {
                    FilterTextBox.Text = ex.Message;
                }
            }
        }

        private void FindTextButton_Click(object sender, EventArgs e)
        {
            if (Reader.FileContainsText(FileLabel.Text, TextToFindTextBox.Text))
                FilterTextBox.Text = "Text '" + TextToFindTextBox.Text + "' found inside the file";
            else
                FilterTextBox.Text = "Text '" + TextToFindTextBox.Text + "' not found inside the file";
        }

        private void FindWithRegexButton_Click(object sender, EventArgs e)
        {
            var matches = Reader.GetRegexMatchesFromFile(FileLabel.Text, TextToFindWithRegexTextBox.Text);
            if (matches != null)
                FilterTextBox.Lines = matches;
        }
    }
}
