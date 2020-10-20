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
    public partial class ResultCodage : Form {
        private Hamming hm;
        public ResultCodage() {
           
            InitializeComponent();
        }

        internal Hamming Hm { get => hm; set => hm = value; }

        private void ResultCodage_Load(object sender, EventArgs e) {
            original.Text = hm.MotAcoder;
            kview.Text = hm.K.ToString();
            nview.Text = hm.N.ToString();
            //diviser en k bloc
            hm.makekbloc();
            //coder les k bloc
            hm.coderChaqueKbloc();
            //manapotr matrice G sy matrice de controle
            for (int i = 0; i < hm.MatriceG.GetLength(0); i++) {
                matriceGView.AppendText("");
                matriceGView.AppendText("");
             
                for (int j = 0; j < hm.MatriceG.GetLength(1); j++) {
                    matriceGView.AppendText(""+hm.MatriceG[i, j]);
                    matriceGView.AppendText("       ");
                }
                matriceGView.AppendText(" ");
                matriceGView.AppendText(Environment.NewLine);
            }
            for (int i = 0; i < hm.MatriceControle.GetLength(0); i++) {
                matriceControleView.AppendText("");
                matriceControleView.AppendText("");

                for (int j = 0; j < hm.MatriceControle.GetLength(1); j++) {
                    matriceControleView.AppendText("" + hm.MatriceControle[i, j]);
                    matriceControleView.AppendText("       ");
                }
                matriceControleView.AppendText(" ");
                matriceControleView.AppendText(Environment.NewLine);
            }

            binAvantCodage.Text=hm.BinaireAvantDecodage;
            for (int i = 0; i < hm.KBloc.Length; i++) {
                kblockView.AppendText(hm.KBloc[i]);
                kblockView.AppendText(Environment.NewLine);
            }
            for (int i = 0; i < hm.MotDeCodeParBloc.Length; i++) {
                kblockViewCode.AppendText(hm.MotDeCodeParBloc[i]);
                kblockViewCode.AppendText(Environment.NewLine);
            }
            //erreur
            hm.simulerErreur();
            for (int i = 0; i < hm.MotDeCodeAvecErreurAffiche.Length; i++) {
                erreurView.AppendText(hm.MotDeCodeAvecErreurAffiche[i]);
                erreurView.AppendText(Environment.NewLine);
            }
            //correction
            hm.correction();
            for (int i = 0; i < hm.MotDeCodeApresCorrection.Length; i++) {
                correctionView.AppendText(hm.MotDeCodeApresCorrection[i]);
                correctionView.AppendText(Environment.NewLine);
            }
            //decodage
            hm.decoder();
            textApreCodage.Text=hm.MotApresDecodage;
            binaireApreDecodView.Text=hm.BinaireApresDecodage;
        }
    }
}
