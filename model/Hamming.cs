using hammings6.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hammingWinform.model {
    class Hamming {
        private int r;
        private int erreurMax;
        private string motAcoder;
        private string motApresDecodage;
        private string binaireApresDecodage;
        private string binaireAvantDecodage;
        private int positionErreur;
        private string[] kBloc;
        private int k;
        private int n;
        private string[] motDeCodeParBloc;
        double[,] matriceG;
        double[,] matriceControle;
        private string[] motDeCodeAvecErreur;
        private string[] motDeCodeAvecErreurAffiche;
        private string[] motDeCodeApresCorrection;
        public Hamming() {

        }
        public Hamming(int r,int errMax,string motAcoder) {
            this.r = r;
            this.ErreurMax = errMax;
            this.MotAcoder = motAcoder;
            this.N = Convert.ToInt32(Math.Pow(Convert.ToDouble(2), Convert.ToDouble(r))) - 1;
            this.K = Convert.ToInt32(Math.Pow(Convert.ToDouble(2), Convert.ToDouble(r))) - 1 - r;
        }
        public void makekbloc() {
            string binary=Outil.StringToBinary(this.MotAcoder);
           this.kBloc = Outil.diviserKBloc(binary, this.r);
            this.BinaireAvantDecodage = Outil.binaireDepart;
           
        }
        public void coderChaqueKbloc() {
           this.matriceG = Outil.getMatrGeneratriceHamming(this.r);
           this.matriceControle = Outil.getH(this.N,this.K);
            List<string> temp = new List<string>();
            for (int i = 0; i < this.kBloc.Length; i++) {
                double[,] motDeCode = Outil.produitMatrice(MatriceG, Outil.eachKBlockToDouble(this.kBloc[i]));
                string motDeCodeString = Outil.motDeCodeToString(motDeCode);
                temp.Add(motDeCodeString);
            }
            this.motDeCodeParBloc = temp.ToArray();
        }
        public void simulerErreur() {
            List<string> temp = new List<string>();
            List<string> tempAffiche = new List<string>();
            for (int i = 0; i < this.motDeCodeParBloc.Length; i++) {
                object[] retBruit = Outil.bruit(this.motDeCodeParBloc[i], this.erreurMax);
                string bruit =(string)retBruit[0];
                bruit += "  posErr:";
                foreach (var item in (Dictionary<int,int[]>)retBruit[1]) {
                    if (item.Key == 0) break;
                    else {
                        for (int x=0;x<item.Value.Length;x++) {
                            if (x != item.Value.Length - 1) bruit += "" + item.Value[x] + ",";
                            else bruit += "" + item.Value[x];
                        }
                    }

                }
                tempAffiche.Add(bruit);
                temp.Add((string)retBruit[0]);
            }
            this.MotDeCodeAvecErreur = temp.ToArray();
            this.motDeCodeAvecErreurAffiche = tempAffiche.ToArray();
        }
        public void correction() {
            List<string> temp = new List<string>();
            for (int i = 0; i < this.motDeCodeAvecErreur.Length; i++) {
                if(Outil.siErreurPresent(this.matriceControle, Outil.eachKBlockToDouble(this.motDeCodeAvecErreur[i]))) {
                    //raha misy erreur de alaina ny syndrome de amn alalaniny no ahitana anle position erreur
                    double[,] syndrome = Outil.getSyndrome(this.matriceControle, Outil.eachKBlockToDouble(this.motDeCodeAvecErreur[i]));
                    int positionErreur = Outil.localiserErreur(this.matriceControle,syndrome);
                    //de corrigerna le erreur aveo
                    temp.Add(Outil.correction(this.motDeCodeAvecErreur[i], positionErreur));
                }
                else {
                    temp.Add(this.motDeCodeAvecErreur[i]);
                }
            }
            this.MotDeCodeApresCorrection = temp.ToArray();
        }
        public void decoder() {
            List<string> temp = new List<string>();
            for (int i = 0; i < this.motDeCodeApresCorrection.Length; i++) {
                temp.Add(Outil.mamerinaMotDeCodeHoMotDInformation(this.motDeCodeApresCorrection[i]));
            }
           this.BinaireApresDecodage=Outil.reconstituerKblocApresErreur(temp.ToArray(), Outil.nbBitApina);
            this.MotApresDecodage = Outil.BinaryToString(this.binaireApresDecodage);
        }
        public int R { get => r; set => r = value; }
        public int PositionErreur { get => positionErreur; set => positionErreur = value; }
        public string[] KBloc { get => kBloc; set => kBloc = value; }
        public string[] MotDeCodeParBloc { get => motDeCodeParBloc; set => motDeCodeParBloc = value; }
        public int ErreurMax { get => erreurMax; set => erreurMax = value; }
        public double[,] MatriceG { get => matriceG; set => matriceG = value; }
        public double[,] MatriceControle { get => matriceControle; set => matriceControle = value; }
        public string[] MotDeCodeAvecErreur { get => motDeCodeAvecErreur; set => motDeCodeAvecErreur = value; }
        public string MotAcoder { get => motAcoder; set => motAcoder = value; }
        public string[] MotDeCodeApresCorrection { get => motDeCodeApresCorrection; set => motDeCodeApresCorrection = value; }
        public string MotApresDecodage { get => motApresDecodage; set => motApresDecodage = value; }
        public string BinaireApresDecodage { get => binaireApresDecodage; set => binaireApresDecodage = value; }
        public string BinaireAvantDecodage { get => binaireAvantDecodage; set => binaireAvantDecodage = value; }
        public int K { get => k; set => k = value; }
        public int N { get => n; set => n = value; }
        public string[] MotDeCodeAvecErreurAffiche { get => motDeCodeAvecErreurAffiche; set => motDeCodeAvecErreurAffiche = value; }
    }
}
