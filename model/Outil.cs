using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hammings6.model {
    public class Outil {
        public static int nbBitApina;
        public static string binaireDepart;
        public static Dictionary<int, int[]> erreurGenerer;
        public static Random rnd = new Random();

        public static string mamerinaMotDeCodeHoMotDInformation(string motdeCode) {

            StringBuilder sb = new StringBuilder();
            char[] tab = motdeCode.ToCharArray();
            int test = 0;
            for (int i = 0; i < tab.Length; i++) {
                //alana le bite de controlle
                if ((i + 1) != Convert.ToInt32(Math.Pow(Convert.ToDouble(2), Convert.ToDouble(test)))) {
                    sb.Append(tab[i]);
                }
                else {
                    test++;
                }
            }
            return sb.ToString();
        }
        public static string reconstituerKblocApresErreur(string[] Kbloc, int nbBitNapina) {
            StringBuilder sb = new StringBuilder();
            foreach (var item in Kbloc) {
                sb.Append(item);
            }
            string ret = sb.ToString();
            int reste = ret.Length - nbBitNapina;
            return ret.Substring(nbBitNapina, reste);
        }
        public static string correction(string motDeCodeNandaloErreur, int positionErreur) {
            StringBuilder correction = new StringBuilder();
            char[] tab = motDeCodeNandaloErreur.ToCharArray();
            for (int i = 0; i < tab.Length; i++) {
                if ((i + 1) == positionErreur) {
                    if (tab[i].Equals('1')) tab[i] = '0';
                    else tab[i] = '1';
                }
            }
            foreach (var item in tab) {
                correction.Append(item);
            }
            return correction.ToString();
        }
        public static object[] bruit(string motDeCode, int erreurMax) {
            char[] tab = motDeCode.ToArray();
            Dictionary<int, int[]> dic = generationErreur(erreurMax, tab.Length);
            object[] ret = new object[2];
           
            foreach (var item in dic) {
                if (item.Key == 0) break;
                else {
                    foreach (var item2 in item.Value) {
                        for (int i = 0; i < tab.Length; i++) {
                            if (i == (item2 - 1)) {
                                //ovaina
                                if (tab[i].Equals('1')) tab[i] = '0';
                                else tab[i] = '1';
                            }
                        }
                    }
                }

            }
            StringBuilder s = new StringBuilder();
            foreach (var item in tab) {
                s.Append(item);
            }
            //s.Append("  posErr:");
            //foreach (var item in dic) {
            //    if (item.Key == 0) break;
            //    else {
            //        foreach (var item2 in item.Value) {
            //            s.Append(item2 + ",");
            //        }
            //    }

            //}
            ret[0] = s.ToString();
            ret[1] = dic;
            return ret;
        }
        public static Dictionary<int, int[]> generationErreur(int erreurMax, int k) {
            if (erreurMax > k) {
                throw new Exception("erreur max ne peut pas depasser le nombre de bit dans un bloc");
            }
           
            int nberreur = rnd.Next(0, (erreurMax + 1));
            List<int> positions = new List<int>();
            int pos, temp;
            for (int i = 0; i < nberreur; i++) {
                pos = rnd.Next(1, (k + 1));
                while (positions.Contains(pos)) {
                    pos = rnd.Next(1, (k + 1));
                }
                positions.Add(pos);
            }

            Dictionary<int, int[]> ret = new Dictionary<int, int[]>();
            ret.Add(nberreur, positions.ToArray());
            return ret;
        }
        public static double[,] getMatrGeneratriceHamming(int r) {
            int n = Convert.ToInt32(Math.Pow(Convert.ToDouble(2), Convert.ToDouble(r))) - 1;
            int k = Convert.ToInt32(Math.Pow(Convert.ToDouble(2), Convert.ToDouble(r))) - 1 - r;

            Dictionary<int, int[]> test = getBiteDeControle(r);
            return getMatrGeneratrice(test, n, k);
        }
        public static double[,] getMatrGeneratrice(Dictionary<int, int[]> biteDeControleAvecindiceBitetsotra, int n, int k) {
            double[,] ret = new double[n, k];
            int compteurbiteconteol = 0;
            for (int i = 0; i < n; i++) {
                if ((i + 1) == Math.Pow(Convert.ToDouble(2), Convert.ToDouble(compteurbiteconteol))) {
                    //raha eo amle ligne anle bite de controle
                    for (int j = 0; j < k; j++) {
                        if (biteDeControleAvecindiceBitetsotra[compteurbiteconteol].Contains(j)) {
                            ret[i, j] = 1;
                        }
                        else {
                            ret[i, j] = 0;
                        }
                    }
                    compteurbiteconteol++;
                }
                else {
                    int indice = positionToIndex((i + 1), n, k);
                    for (int j = 0; j < k; j++) {
                        if (j == indice) {
                            ret[i, j] = 1;
                        }
                        else {
                            ret[i, j] = 0;
                        }
                    }
                }
            }
            return ret;
        }
        public static int[] controlerPar(string positionEnBinnaire) {
            char[] tab = positionEnBinnaire.ToCharArray();
            List<int> ret = new List<int>();
            int temp = 0;
            for (int i = tab.Length - 1; i >= 0; i--) {
                if (tab[i].Equals('1')) {
                    ret.Add(temp);
                }
                temp++;
            }

            return ret.ToArray();
        }
        public static Dictionary<int, int[]> getBiteDeControle(int r) {
            string binary = "";
            int[] tempcontrole;
            //1 jusqua n, liste indice de bite de parité
            Dictionary<int, int[]> positionToBiteControle = new Dictionary<int, int[]>();
            int n = Convert.ToInt32(Math.Pow(Convert.ToDouble(2), Convert.ToDouble(r))) - 1;
            int k = Convert.ToInt32(Math.Pow(Convert.ToDouble(2), Convert.ToDouble(r))) - 1 - r;

            //maka ny position rehetra  atram n miaraka am
            //bite de parité mcontrole azy tsirairay
            for (int i = 1; i <= n; i++) {
                //avadika binaire
                binary = Convert.ToString(i, 2);
                //jerena hoe controler par inona
                tempcontrole = controlerPar(binary);
                positionToBiteControle.Add(i, tempcontrole);
            }

            int[] temp;
            //maka ny position rehetra controler pour chaque bite de parité
            //indice bite de parité, liste position
            Dictionary<int, int[]> bitecontroleTopostion = new Dictionary<int, int[]>();
            List<int> tempresparligne;
            for (int i = 0; i < r; i++) {
                tempresparligne = new List<int>();
                for (int j = 1; j <= positionToBiteControle.Count; j++) {
                    temp = positionToBiteControle[j];
                    for (int x = 0; x < temp.Length; x++) {
                        if (temp[x] == i) {
                            //raha le position misy anle bite de parité de tsy raisina
                            if (j != Math.Pow(Convert.ToDouble(2), Convert.ToDouble(i))) {
                                tempresparligne.Add(j);
                                break;
                            }
                        }
                    }
                }
                bitecontroleTopostion.Add(i, tempresparligne.ToArray());
            }

            //avadika indicenle bi amzay ilay position teo
            //bite de parité, liste indice bitetsotra am mot d'information
            Dictionary<int, int[]> bitecontroleToindicemotInformation = new Dictionary<int, int[]>();
            int index;
            for (int i = 0; i < bitecontroleTopostion.Count; i++) {
                for (int j = 0; j < bitecontroleTopostion[i].Length; j++) {
                    index = positionToIndex(bitecontroleTopostion[i][j], n, k);
                    bitecontroleTopostion[i][j] = index;
                }

                bitecontroleToindicemotInformation.Add(i, bitecontroleTopostion[i]);
            }
            return bitecontroleToindicemotInformation;
        }
        public static double[,] getH(int n, int k) {
            int ligne = n - k;
            int colonne = n;

            string[] nb = new string[colonne];
            for (int i = 0; i < colonne; i++) {
                string binary = Convert.ToString((i + 1), 2);
                if (binary.Length < ligne) {
                    int test = ligne - binary.Length;
                    string aha = "";
                    for (int j = 0; j < test; j++) {
                        aha += "0";
                    }
                    binary = aha + binary;
                }
                nb[i] = binary;
            }
            double[,] ret = new double[ligne, colonne];
            int temp = ligne;
            //avadika matrice amzay
            for (int i = 0; i < ligne; i++) {
                for (int j = 0; j < colonne; j++) {
                    ret[i, j] = Convert.ToDouble(nb[j].ToCharArray()[temp - 1].ToString());
                }
                temp--;
            }
            return ret;

        }
        public static bool siErreurPresent(double[,] h, double[,] motDeCode) {
            double[,] ret = produitMatrice(h, motDeCode);
            for (int i = 0; i < ret.GetLength(0); i++) {
                for (int j = 0; j < ret.GetLength(1); j++) {
                    if (ret[i, j] == 1) {
                        return true;
                    }
                }
            }
            return false;
        }
        public static double[,] getSyndrome(double[,] h, double[,] motDeCode) {
            return produitMatrice(h, motDeCode);
        }
        public static int localiserErreur(double[,] h, double[,] syndrome) {
            int compteur = 0;
            int position = 0;
            //bouclerna le colonne anle h
            for (int i = 0; i < h.GetLength(1); i++) {
                //bouclena ny ligne
                for (int j = 0; j < h.GetLength(0); j++) {
                    if (h[j, i] == syndrome[j, 0]) {
                        compteur++;
                    }
                }
                if (compteur == h.GetLength(0)) {
                    position = (i + 1);
                    break;
                }
                else {
                    compteur = 0;
                }
            }
            return position;
        }
        public static Dictionary<int, int[]> getPositionControlerParBiteDeControle(int r) {
            string binary = "";
            int[] tempcontrole;
            //1 jusqua n, liste indice de bite de parité
            Dictionary<int, int[]> positionToBiteControle = new Dictionary<int, int[]>();
            int n = Convert.ToInt32(Math.Pow(Convert.ToDouble(2), Convert.ToDouble(r))) - 1;
            int k = Convert.ToInt32(Math.Pow(Convert.ToDouble(2), Convert.ToDouble(r))) - 1 - r;

            //maka ny position rehetra  atram n miaraka am
            //bite de parité mcontrole azy tsirairay
            for (int i = 1; i <= n; i++) {
                //avadika binaire
                binary = Convert.ToString(i, 2);
                //jerena hoe controler par inona
                tempcontrole = controlerPar(binary);
                positionToBiteControle.Add(i, tempcontrole);
            }

            int[] temp;
            //maka ny position rehetra controler pour chaque bite de parité
            //indice bite de parité, liste position
            Dictionary<int, int[]> bitecontroleTopostion = new Dictionary<int, int[]>();
            List<int> tempresparligne;
            for (int i = 0; i < r; i++) {
                tempresparligne = new List<int>();
                for (int j = 1; j <= positionToBiteControle.Count; j++) {
                    temp = positionToBiteControle[j];
                    for (int x = 0; x < temp.Length; x++) {
                        if (temp[x] == i) {
                            tempresparligne.Add(j);
                            break;
                        }
                    }
                }
                bitecontroleTopostion.Add(i, tempresparligne.ToArray());
            }
            return bitecontroleTopostion;
        }
        private static int positionToIndex(int position, int n, int k) {
            int index = 0;
            //ampidirina daholo aloha ilay bi
            List<string> motInformation = new List<string>();
            for (int i = 0; i < k; i++) {
                motInformation.Add("b" + (i + 1));
            }
            //mampiditra anle mot de code indray
            List<string> motDeCode = new List<string>();
            int compteurbiteparite = 0;
            int compteurbitetsotra = 1;
            for (int i = 0; i < n; i++) {
                if ((i + 1) == Math.Pow(Convert.ToDouble(2), Convert.ToDouble(compteurbiteparite))) {
                    motDeCode.Add("p" + compteurbiteparite);
                    compteurbiteparite++;
                }
                else {
                    motDeCode.Add("b" + compteurbitetsotra);
                    compteurbitetsotra++;
                }
            }
            for (int i = 0; i < motDeCode.Count; i++) {
                if ((i + 1) == position) {
                    index = motInformation.IndexOf(motDeCode[i]);
                    break;
                }
            }
            return index;
        }
        public static double[,] produitMatrice(double[,] matriceA, double[,] matriceB) {

            int ligne, colonne;
            int ligne2, colonne2;
            ligne = matriceA.GetLength(0);
            colonne = matriceA.GetLength(1);

            ligne2 = matriceB.GetLength(0);
            colonne2 = matriceB.GetLength(1);
            double[,] produitM = new double[ligne, colonne2];

            for (int i = 0; i < ligne; i++) {
                for (int j = 0; j < colonne2; j++) {
                    produitM[i, j] = 0;
                    for (int k = 0; k < colonne; k++) {
                        produitM[i, j] += matriceA[i, k] * matriceB[k, j];
                    }
                    if (produitM[i, j] > 1) {
                        if (produitM[i, j] % 2 == 0) produitM[i, j] = 0;
                        else produitM[i, j] = 1;
                    }
                }
            }
            return produitM;
        }
        public static string StringToBinary(string data) {
            StringBuilder sb = new StringBuilder();

            foreach (char c in data.ToCharArray()) {
                sb.Append(Convert.ToString(c, 2).PadLeft(8, '0'));
            }
            return sb.ToString();
        }
        public static string BinaryToString(string data) {
            List<Byte> byteList = new List<Byte>();

            for (int i = 0; i < data.Length; i += 8) {
                byteList.Add(Convert.ToByte(data.Substring(i, 8), 2));
            }
            return Encoding.ASCII.GetString(byteList.ToArray());
        }
        public static double[,] eachKBlockToDouble(string oneKblock) {
            char[] tochar = oneKblock.ToCharArray();
            double[,] ret = new double[tochar.Length, 1];
            for (int i = 0; i < tochar.Length; i++) {
                ret[i, 0] = Convert.ToDouble(tochar[i].ToString());
            }
            return ret;
        }
        public static string motDeCodeToString(double[,]motdecode) {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < motdecode.GetLength(0); i++) {
                sb.Append(motdecode[i, 0].ToString());
            }
            return sb.ToString();
        }
        public static string[] diviserKBloc(string binary, int r) {
            int k = Convert.ToInt32(Math.Pow(Convert.ToDouble(2), Convert.ToDouble(r))) - 1 - r;
            int reste = binary.Length % k;
            if (reste > 0) {
                Console.WriteLine("ato");
                nbBitApina = k - reste;
                //apina zero any  am volou
                string temp = "";
                for (int i = 0; i < k - reste; i++) {
                    temp += "0";
                }
                binary = temp + binary;
            }
            binaireDepart = binary;
            char[] tab = binary.ToCharArray();

            List<string> ret = new List<string>();
            int compteur = 0;
            var temp2 = new StringBuilder();
            for (int i = 0; i < tab.Length; i++) {
                temp2.Append(tab[i]);
                compteur++;
                if (compteur == k) {
                    compteur = 0;
                    ret.Add(temp2.ToString());
                    temp2 = new StringBuilder();
                }
            }
            return ret.ToArray();
        }

    }
}
