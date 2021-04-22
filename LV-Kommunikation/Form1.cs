using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LV_Kommunikation {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        int kap;
        int VarNr;
        int k;
        string value;
        LVCommunicator lvComm;
        string exeToStart = "LVStress";
        bool hidden = false;


        private void textBox2_TextChanged(object sender, EventArgs e) {
            SetKap();
        }

        private void textBox3_TextChanged(object sender, EventArgs e) {
            SetVarNr();
        }

        private void textBox5_TextChanged(object sender, EventArgs e) {
            SetValue();
        }

        private void start_Click(object sender, EventArgs e) {
            textBox1.Text = "Starting Atlas ...";
            bool started = lvComm.StartAtlas(exeToStart);
            if (started) {
                textBox1.Text = "Atlas started";
            }
            else {
                textBox1.Text = "Atlas failed to start";
            }
        }

        private void HideShow_Click(object sender, EventArgs e) {
            if (!hidden) {
                lvComm.Hide();
                hidden = true;
            }
            else {
                lvComm.Show();
                hidden = false;
            }
        }

        private void DoInput_Click(object sender, EventArgs e) {
            if (InputComplete()) {
                lvComm.SetValue(kap, VarNr, value);
            }
            else {
                textBox1.Text = "Incomplete Input";
            }
            
        }

        private void Form1_Load(object sender, EventArgs e) {
            
            exeToStart = "LVStress";
            lvComm = new LVCommunicator();
            SetKap();
            SetVarNr();
            SetValue();
        }

        private void GetValue_Click(object sender, EventArgs e) {
            double atlasValue = lvComm.GetValue(kap, VarNr);
            textBox5.Text = atlasValue.ToString();
        }

        private void StartModul_Click(object sender, EventArgs e) {
            string text = textBox6.Text;
            if(text != String.Empty) {
                lvComm.StartModule(text);

            }
            
        }

        private bool InputComplete() {
            return kap > 0 && VarNr > 0 && value != String.Empty;
        }
        private void SetKap() {
            string text = textBox2.Text;
            if (int.TryParse(text, out _)) {
                kap = Convert.ToInt32(text);
            }
        }
        private void SetVarNr() {
            string text = textBox3.Text;
            if (int.TryParse(text, out _)) {
                VarNr = Convert.ToInt32(text);
            }
        }
        private void SetValue() {
            value = textBox5.Text;
        }

        private void Shutdown_Click(object sender, EventArgs e) {
            lvComm.Shutdown();
        }

        private void hiddenB1_Click(object sender, EventArgs e) {
            lvComm.StartB1WithPresettings();
            hidden = true;
        }
    }
}
