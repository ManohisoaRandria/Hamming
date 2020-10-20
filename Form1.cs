using hammingWinform.model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace hammingWinform {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e) {
            string texteAcoder = richTextBox1.Text;
            int r =Convert.ToInt32(rr.Value);
            int erreurMax = Convert.ToInt32(erreurMaxx.Value);
            Hamming hm = new Hamming(r, erreurMax, texteAcoder);
            ResultCodage res = new ResultCodage();
            res.Hm = hm;
            res.ShowDialog();
           
        }

        private void Form1_Load(object sender, EventArgs e) {
            error.ForeColor = Color.Red;
            erreurMaxx.Minimum = 0;
            rr.Minimum = 2;
        }
    }
}
