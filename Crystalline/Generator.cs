using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Crystalline
{
    public partial class Generator : Form
    {
        public Generator()
        {
            InitializeComponent();
        }

        //Generate
        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowNewFolderButton = true;
            DialogResult result = fbd.ShowDialog();
            string selectedPath;
            List<byte[]> keys = new List<byte[]>();
            List<byte[]> salts = new List<byte[]>();

            if (result == DialogResult.OK)
                selectedPath = fbd.SelectedPath;
            else
                return;

            CreateFolders(selectedPath);

            for (int i = 0; i < (int)numericUpDown1.Value; i++)
            {
                keys.Add(CreateRandom());
                salts.Add(CreateRandom());
            }

            WriteFiles(selectedPath, keys, salts);
        }

        private void CreateFolders(string selectedPath)
        {
            if (!Directory.Exists(selectedPath + "\\Keys"))
            {
                Directory.CreateDirectory(selectedPath + "\\Keys");
            }

            if (!Directory.Exists(selectedPath + "\\Salts"))
            {
                Directory.CreateDirectory(selectedPath + "\\Salts");
            }
        }

        private byte[] CreateRandom()
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            var byteArray = new byte[(int)numericUpDown2.Value * 1024];
            provider.GetBytes(byteArray);
            return byteArray;
        }

        private void WriteFiles(string selectedPath, List<byte[]> keys, List<byte[]> salts)
        {
            for (int i = 0; i < keys.Count; i++)
            {
                File.WriteAllBytes(selectedPath + "\\Keys\\" + i + ".clk", keys[i]);
                File.WriteAllBytes(selectedPath + "\\Salts\\" + i + ".cls", salts[i]);
            }
        }
    }
}
