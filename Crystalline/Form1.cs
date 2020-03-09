using CrystallineCipherLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Crystalline
{
    public partial class Form1 : Form
    {
        List<string> destinationFiles;
        List<string> sourceFiles;
        List<string> selectedDestinationFiles;
        List<string> selectedSourceFiles;
        List<string> selectedKeysFiles;
        List<string> selectedSaltFiles;
        string sourcePath;
        string destinationPath;
        string keySaltPath;

        public Form1()
        {
            InitializeComponent();
            sourceFiles = new List<string>();
            destinationFiles = new List<string>();
            selectedDestinationFiles = new List<string>();
            selectedSourceFiles = new List<string>();
            selectedKeysFiles = new List<string>();
            selectedSaltFiles = new List<string>();

            button2.Enabled = false;
            button1.Enabled = false;
        }

        private void sourceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.Clear();
            sourceFiles.Clear();
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();

            if (result == DialogResult.OK)
            {
                sourceFiles.AddRange(Directory.GetFiles(fbd.SelectedPath));
                sourcePath = fbd.SelectedPath;
            }

            foreach (string s in sourceFiles)
            {
                listView1.Items.Add(s);
            }
        }

        private void destinationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView2.Clear();
            destinationFiles.Clear();
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowNewFolderButton = true;
            DialogResult result = fbd.ShowDialog();

            if (result == DialogResult.OK)
            {
                destinationFiles.AddRange(Directory.GetFiles(fbd.SelectedPath));
                destinationPath = fbd.SelectedPath;
            }

            foreach (string s in destinationFiles)
            {
                if(s.Contains(".cle"))
                    listView2.Items.Add(s);
            }
        }

        private void UpdateFileViews()
        {
            sourceFiles.Clear();
            destinationFiles.Clear();

            sourceFiles.AddRange(Directory.GetFiles(sourcePath));
            destinationFiles.AddRange(Directory.GetFiles(destinationPath));

            listView1.Clear();
            listView2.Clear();

            foreach (string s in sourceFiles)
            {
                listView1.Items.Add(s);
            }

            foreach (string s in destinationFiles)
            {
                if (s.Contains(".cle"))
                    listView2.Items.Add(s);
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void generateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Generator g = new Generator();
            g.Show();
        }

        private void contentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Help h = new Help();
            h.Show();
        }

        private void keysToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();

            if (result == DialogResult.OK)
            {
                keySaltPath = fbd.SelectedPath;
                selectedKeysFiles.AddRange(Directory.GetFiles(fbd.SelectedPath + "\\Keys"));
                selectedSaltFiles.AddRange(Directory.GetFiles(fbd.SelectedPath + "\\Salts"));
            }

            label3.Text = keySaltPath;
        }

        private void helpToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AboutBox1 a = new AboutBox1();
            a.Show();
        }

        //Encrypt
        private void button1_Click(object sender, EventArgs e)
        {
            if(keySaltPath == null || keySaltPath == "")
            {
                MessageBox.Show("Please select a folder for Keys/Salts.", "Error");
                return;
            }
            //Important Note:
            //While this code is setup for multiselect that would result in key/salt reuse.
            //Multiselect has been disabled in the ListView to prevent this.
            //Always use a new destination folder for encrypt as file names are not preserved.
            //Every encrypted file will be called 0.cle.
            
            selectedSourceFiles.Add(listView1.SelectedItems[0].Text);

            for(int i = 0; i < selectedSourceFiles.Count; i++)
            {
                byte[] encryptedFile = File.ReadAllBytes(selectedSourceFiles[i]);

                for (int j = 0; j < selectedKeysFiles.Count; j++)
                {
                    encryptedFile = CrystallineCipher.Encrypt(encryptedFile, File.ReadAllBytes(selectedKeysFiles[j]), File.ReadAllBytes(selectedSaltFiles[j]), (int)numericUpDown1.Value);
                }

                File.WriteAllBytes(destinationPath + "\\" + i + ".cle", encryptedFile);
            }

            UpdateFileViews();
        }

        //Decrypt
        private void button2_Click(object sender, EventArgs e)
        {
            if (keySaltPath == null || keySaltPath == "")
            {
                MessageBox.Show("Please select a folder for Keys/Salts.", "Error");
                return;
            }
            //Important Note:
            //As filename and extension are not preserved, these must be remembered/stored somewhere.
            //Some applications, mainly media, may infer the type from the header/structure and not require the extension.
            //While this code appears to support multiselect, it does not and should not be used anyway.

            selectedDestinationFiles.Add(listView2.SelectedItems[0].Text);

            for (int i = 0; i < selectedDestinationFiles.Count; i++)
            {
                byte[] encryptedFile = File.ReadAllBytes(selectedDestinationFiles[i]);

                for (int j = selectedKeysFiles.Count - 1; j >= 0; j--)
                {
                    encryptedFile = CrystallineCipher.Decrypt(encryptedFile, File.ReadAllBytes(selectedKeysFiles[j]), File.ReadAllBytes(selectedSaltFiles[j]), (int)numericUpDown1.Value);
                }

                File.WriteAllBytes(sourcePath + "\\" + i + ".clc", encryptedFile);
            }

            UpdateFileViews();
        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            listView1.SelectedItems.Clear();
            button2.Enabled = true;
            button1.Enabled = false;
        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            listView2.SelectedItems.Clear();
            button2.Enabled = false;
            button1.Enabled = true;
        }
    }
}
