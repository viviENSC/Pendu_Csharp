using System;
using System.IO;

namespace Pendu
{
    class Program
    {
        //Fonction qui prend en argument la réponse du joueur à la question :"Voulez vous un rappel des règles du jeu ?" et qui renvoie les règles du jeu si la réponse est positive.
        public static void Regles(string reponse)
        {
            if (reponse == "oui")//Le joueur veut abandonner.
            {
                Console.WriteLine("Vous devez deviner le mot avec les lettres déjà découvertes");
                Console.WriteLine("Vous avez le droit de proposer un mot ou une lettre par tour");
                Console.WriteLine("Si le mot proposé n'est pas celui que vous devez deviner, cela compte comme une erreur");
                Console.WriteLine("Si le mot ne contient pas la lettre proposée, cela compte comme une erreur");
                Console.WriteLine("Vous êtes pendu à la 11 ème erreur");
                Console.WriteLine("(Pensez à mettre les lettres que vous proposez en majuscules)");
            }
        }
        //Fonction qui demande au joueur s'il veut abandonner ou non et qui arrête le jeu si la réponse est positive. 
        public static int Abandon(int nberreurs, int gagne)
        {
            if (nberreurs != 11 && gagne != 1)//Le joueur ne peut pas abandonner si il a perdu ou gagné.
            {
                Console.WriteLine("Voulez vous abandonner ? Tapez oui si vous voulez abandonner");
                if (Console.ReadLine() == "oui")
                {
                    Console.WriteLine("Vous avez décidé d'abandonner ...");
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            else if (nberreurs == 11 && gagne != 1)//Le joueur a perdu.
            {
                return 0;
            }
            else//Le joueur a gagné.
            {
                return 1;
            }
        }

        //Fonction qui compte le nombre de fois qu'apparait une lettre dans un document texte.
        public static int CompteLettre(string lettre, string texte)
        {
            System.Text.Encoding encoding = System.Text.Encoding.GetEncoding("iso-8859-1");
            StreamReader monStreamReader = new StreamReader(texte, encoding);

            int compteur = 0; //le nombre de fois qu'apparait la lettre dans le fichier

            string mot = monStreamReader.ReadLine();
            while (mot != null)
            {
                for (int i = 0; i < mot.Length; i++)
                {
                    if (mot.Substring(i, 1) == lettre)
                    {
                        compteur = compteur + 1;
                    }

                }
                mot = monStreamReader.ReadLine();
            }
            monStreamReader.Close();

            return compteur;
        }

        //Fonction qui renvoie une liste des 26 nombres correspondants aux nombres de fois qu'apparaient les lettres de l'alphabet dans le document texte "dicoFR.txt".
        public static int[] CompteLettre2(string[] alphabet)
        {
            int[] point = new int[26];//Liste de 26 nombres correspondants aux nombres de fois qu'apparaissent les 26 lettres de l'alphabet dans le document texte : "dicoFR.txt".
            for (int j = 0; j < alphabet.Length; j++)
            {
                point[j] = CompteLettre(alphabet[j], "dicoFR.txt");
            }
            return point;
        }

        //Fonction qui compte le nombre total de lettre dans un document texte.
        public static int CompteTotaleLettre(string fichier)
        {
            int nbLettretotale = 0;
            string[] alphabet = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

            for (int i = 0; i < alphabet.Length; i++)
            {
                nbLettretotale = nbLettretotale + CompteLettre(alphabet[i], fichier);
            }

            return nbLettretotale;
        }

        //Fonction qui renvoie le nombre de point que vaut une lettre selon le nombre de fois qu'elle apparrait dans le document texte.
        public static float PointLettre(string[] alphabet, string lettre, float nombretotaledelettre, int[] nbdelettre)
        {
            int positiondelalettre = 0;
            float[] point = new float[26];
            for (int j = 0; j < point.Length; j++)
                if (alphabet[j] != "-")
                {
                    point[j] = 1 - nbdelettre[j] / nombretotaledelettre;
                    if (alphabet[j] == lettre)
                    { positiondelalettre = j; }
                }
            return point[positiondelalettre];
        }

        //Fonction qui renvoie le nombre de point que vaut un mot selon les lettres et leur nombre qu'il possède.
        public static float ComptePoint(string mot, string[] alphabet, float nombretotaldelettre, int[] nbdelettre)
        {
            float pointmot = 0;//Nombre de point que vaut le mot.
            string[] répétition = new string[26] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };//Liste qui stocke les lettres d'un mot pour remarquer les répétition.
            for (int i = 0; i < mot.Length; i++)
            {
                pointmot = PointLettre(alphabet, mot.Substring(i, 1), nombretotaldelettre, nbdelettre) + pointmot;
                int p = 0;
                for (int j = 0; j < 26; j++)
                {
                    if (mot.Substring(i, 1) == répétition[j] && mot.Substring(i, 1) != "")
                    {
                        pointmot = pointmot - PointLettre(alphabet, "E", nombretotaldelettre, nbdelettre);//Les lettres répétées ont un nombre de point réduit, son nouveau nombre de point est égal à au nombre de point que vaut la lettre moins la nombre de point que vaut la lettre qui apparrait le plus de fois dans le texte, c'est à dire le E.
                        p = p + 1;
                    }
                }
                if (p == 0 && mot.Substring(i, 1) != "-")
                {
                    int k = 0;
                    while (répétition[k] != "")
                    {
                        k = k + 1;
                    }
                    répétition[k] = mot.Substring(i, 1);
                }
            }
            return pointmot;
        }

        //Fonction qui répartit les mots d'un document texte source dans trois autres documents textes selon leur nombre de point. 
        public static void CreationFichiers(string fichier_source, string fichier_cible1, string fichier_cible2, string fichier_cible3, string[] alphabet, float nombretotaldelettre, int[] nbdelettre)
        {


            // Création d'une instance de StreamReader pour permettre la lecture de notre fichier source 
            System.Text.Encoding encoding = System.Text.Encoding.GetEncoding("iso-8859-1");
            StreamReader monStreamReader = new StreamReader(fichier_source, encoding);


            string mot = monStreamReader.ReadLine();
            float pointmax = ComptePoint(mot, alphabet, nombretotaldelettre, nbdelettre);
            float pointmin = ComptePoint(mot, alphabet, nombretotaldelettre, nbdelettre);

            while (mot != null)
            {

                if (ComptePoint(mot, alphabet, nombretotaldelettre, nbdelettre) > pointmax)
                {
                    pointmax = ComptePoint(mot, alphabet, nombretotaldelettre, nbdelettre);
                }
                else if (ComptePoint(mot, alphabet, nombretotaldelettre, nbdelettre) < pointmin)
                {
                    pointmin = ComptePoint(mot, alphabet, nombretotaldelettre, nbdelettre);
                }
                mot = monStreamReader.ReadLine();
            }
            monStreamReader.Close();

            // Création d'une instance de StreamReader pour permettre la lecture de notre fichier source
            System.Text.Encoding encoding1 = System.Text.Encoding.GetEncoding("iso-8859-1");
            StreamReader monStreamReader1 = new StreamReader(fichier_source, encoding1);

            // Création d'une instance de StreamWriter pour permettre l'ecriture des fchiers cibles 
            StreamWriter monStreamWriter1 = File.CreateText(fichier_cible1);
            StreamWriter monStreamWriter2 = File.CreateText(fichier_cible2);
            StreamWriter monStreamWriter3 = File.CreateText(fichier_cible3);

            mot = monStreamReader1.ReadLine();

            while (mot != null)
            {
                if (ComptePoint(mot, alphabet, nombretotaldelettre, nbdelettre) > 2 * (pointmax - pointmin) / 3)//Le mot va dans le fichier3
                {
                    monStreamWriter3.WriteLine(mot);
                }
                else if (ComptePoint(mot, alphabet, nombretotaldelettre, nbdelettre) > (pointmax - pointmin) / 3)//Le mot va dans le fichier2
                {
                    monStreamWriter2.WriteLine(mot);
                }
                else//Le mot va dans le fichier1
                {
                    monStreamWriter1.WriteLine(mot);
                }
                mot = monStreamReader1.ReadLine();
            }

            monStreamReader1.Close();

            monStreamWriter1.Close();
            monStreamWriter2.Close();
            monStreamWriter3.Close();
        }

        //Fonction qui renvoie le nombre de mot dans un document texte. 
        public static int CompteMot(string fichier)
        {
            System.Text.Encoding encoding = System.Text.Encoding.GetEncoding("iso-8859-1");
            StreamReader monStreamReader = new StreamReader(fichier, encoding);

            int nbmots = 0;

            string mot = monStreamReader.ReadLine();

            while (mot != null)
            {
                nbmots++;
                mot = monStreamReader.ReadLine();
            }
            monStreamReader.Close();
            return nbmots; //Correspond aussi au nombre de ligne dans le fichier
        }

        //Fonction qui renvoie un mot au hasard d'un document texte.
        public static string ChoixMot(string documenttxt, int nbdemot)
        {
            string mot = "";

            System.Text.Encoding encoding = System.Text.Encoding.GetEncoding("iso-8859-1");
            StreamReader monStreamReader = new StreamReader(documenttxt, encoding);
            Random ligne = new Random();

            int nbligne = ligne.Next(1, nbdemot);//Numéro de ligne au hasard entre 1 et le nombre de mot qu'il y a dans le document texte.

            for (int i = 1; i <= nbligne; i++)
            {
                mot = monStreamReader.ReadLine();
            }
            monStreamReader.Close();
            return mot;//Mot de la ligne prise au hasard.
        }

        //Fonction qui demande au joueur s'il souhaite qu'on lui rappelle le mot et qui affiche le mot si la réponse est positive. 
        public static void Rappel(string mot)
        {
            Console.WriteLine("Voulez vous un rappel du mot ? Si oui, tapez oui");
            string reponse = Console.ReadLine();
            if (reponse == "oui")
            {
                Console.WriteLine("Seul le joueur faisant devinez le mot doit voir cela: Lorsque vous êtes prêt appuyez sur  la touche entrer");
                Console.ReadLine();
                Console.WriteLine("le mot que êtes en train de faire deviner est " + mot);
                Console.WriteLine("Dès que vous avez pris connaissance du mot appuyez sur entrer");
                Console.ReadLine();
                Console.Clear();
            }
            else
            {
                Console.WriteLine("vous ne souhaitez pas de rappel vous pouvez continuer à jouer");
            }
        }

        //Fonction qui renvoie la proposition de la machine qui essaye de deviner le mot.
        public static string MachinePropose(string motcache, string[] liste, string texte)
        {
            System.Text.Encoding encoding = System.Text.Encoding.GetEncoding("iso-8859-1");
            StreamReader monStreamReader = new StreamReader(texte, encoding);
            string mot = monStreamReader.ReadLine();
            int[] nblettre = new int[26];
            while (mot != null)
            {
                if (motcache.Length == mot.Length)//Ne prend en compte que les mots qui ont le même nombre de lettre que le mot caché.
                {
                    int difference = 0;//Nombre de différence entre le mot caché et le mot analysé.
                    for (int i = 0; i < motcache.Length; i++)
                    {
                        if (motcache.Substring(i, 1) != "_" && motcache.Substring(i, 1) != mot.Substring(i, 1))
                        {
                            difference = difference + 1;
                        }
                    }
                    if (difference == 0)//Ne prend en compte que les mots qui possède les mêmes lettres et mêmes endroits que le mot caché.
                    {
                        for (int j = 0; j < liste.Length; j++)
                        {
                            for (int i = 0; i < mot.Length; i++)
                            {
                                if (mot.Substring(i, 1) == liste[j])
                                {
                                    nblettre[j] = nblettre[j] + 1;//Nombre de fois qu'apparait le lettre parmis tous les mots possibles.
                                }
                            }
                        }
                    }
                }
                mot = monStreamReader.ReadLine();
            }
            int nbmax = 0;
            string proposition = "";
            for (int i = 0; i < nblettre.Length; i++)//Prend la lettre qui apparrait le plus de fois parmis les mots possibles.
            {
                if (nblettre[i] > nbmax)
                {
                    nbmax = nblettre[i];
                    proposition = liste[i];
                }
            }
            monStreamReader.Close();
            return proposition;
        }

        //Fonction qui renvoie la proposition du joueur qui essaye de deviner le mot. 
        public static string JoueurPropose(string motcache, int nberreurs)
        {
            Console.WriteLine("Le mot à deviner est " + motcache);
            Console.WriteLine("Pour le moment vous avez fait " + nberreurs + " erreurs");
            Console.WriteLine("Quelle est votre proposition ?");
            string proposition = Console.ReadLine();
            return proposition;
        }

        //Fonction qui demande au joueur s'il veut que la machine lui propose une lettre à proposer et qui affiche la proposition de la machine si la réponse est positive.
        public static void AideIntelligente(string motcache, string[] liste, string texte)
        {
            Console.WriteLine("Voulez-vous que la machine vous donne un conseil de proposition ? Tapez oui ou non");
            if (Console.ReadLine() == "oui")
            {
                Console.WriteLine("La machine vous conseille de proposer la lettre " + MachinePropose(motcache, liste, texte));
            }
        }

        //Fonction qui renvoie le mot que doit trouver le joueur, mais caché, c'est à dire avec des "-" et qui découvre 0, 1 ou 2 lettres selon la volonté du joueur. 
        public static string AjouteLettre(string mot, int nblettreajou)
        {
            string motmystere = "";
            if (nblettreajou == 0)//Le joueur ne veut découvrir aucune lettre au début.
            {
                for (int i = 0; i < mot.Length; i++)
                {
                    if (mot.Substring(i, 1) == "-")
                    {
                        motmystere = motmystere + "-";
                    }
                    else
                    {
                        motmystere = motmystere + "_";
                    }
                }
            }
            else if (nblettreajou == 1)//Le joueur veut découvrir la première lettre au début.
            {
                motmystere = mot.Substring(0, 1);
                for (int j = 1; j < mot.Length; j++)
                {
                    if (mot.Substring(j, 1) == mot.Substring(0, 1))
                    {
                        motmystere = motmystere + mot.Substring(0, 1);
                    }
                    else if (mot.Substring(j, 1) == "-")
                    {
                        motmystere = motmystere + "-";
                    }
                    else
                    {
                        motmystere = motmystere + "_";
                    }
                }
            }
            else if (nblettreajou == 2 && mot.Length == 1)//Affche seulement une lettre si le joueur veut afficher 2 lettres au début et que le mot ne comporte qu'une lettre.
            {
                motmystere = mot.Substring(0, 1);
            }
            else if (nblettreajou == 2)//Affiche la première et le dernière lettre du mot. 
            {
                motmystere = mot.Substring(0, 1);
                for (int j = 1; j < mot.Length - 1; j++)
                {
                    if (mot.Substring(j, 1) == mot.Substring(0, 1))
                    {
                        motmystere = motmystere + mot.Substring(0, 1);
                    }
                    else if (mot.Substring(j, 1) == mot.Substring(mot.Length - 1, 1))
                    {
                        motmystere = motmystere + mot.Substring(mot.Length - 1, 1);
                    }
                    else if (mot.Substring(j, 1) == "-")
                    {
                        motmystere = motmystere + "-";
                    }
                    else
                    {
                        motmystere = motmystere + "_";
                    }
                }
                motmystere = motmystere + mot.Substring(mot.Length - 1, 1);
            }
            return motmystere;
        }

        //Fonction qui remplace les "_" par la lettre proposée cette lettre apparait dans le mot.
        public static string RemplaceLettre(string proposition, string motcache, int position)
        {
            string mot = "";
            for (int i = 0; i < position - 1; i++)
            {
                mot = mot + motcache.Substring(i, 1);
            }
            mot = mot + proposition;
            for (int j = position; j < motcache.Length; j++)
            {
                mot = mot + motcache.Substring(j, 1);
            }
            return mot;
        }

        //Fonction qui affiche et prend en charge une partie si le mode jeu est un joueur contre un autre joueur.
        public static void JoueurContreJoueur(string motcache, string motmystere, int nbajlettre)
        {
            int nberreurs = 0;
            int gagne = 0;//Paramètre pour savoir si le joueur a gagné ou non, si oui le paramètre arrête le jeu.
            int tour = 0;
            string[] liste = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            if (nbajlettre == 1)
            {
                for (int i = 0; i < liste.Length; i++)//Change la liste des lettres non dites pour l'aide intelligente, enlève les mots mis au début.
                {
                    if (liste[i] == motmystere.Substring(0, 1))
                    {
                        liste[i] = "";
                    }
                }
            }
            else if (nbajlettre == 2)
            {
                for (int i = 0; i < liste.Length; i++)//Change la liste des lettres non dites pour l'aide intelligente, enlève les mots mis au début.
                {
                    if (liste[i] == motmystere.Substring(0, 1) || liste[i] == motmystere.Substring(motmystere.Length - 1, 1))
                    {
                        liste[i] = "";
                    }
                }
            }
            Console.WriteLine("Attention !!!! Seule la personne qui doit faire deviner le mot doit voir la ligne suivante ... Tapez sur entrée lorsque vous êtes prêt");
            Console.ReadLine();
            Console.WriteLine("Le mot à faire deviner est " + motmystere);
            Console.Clear();
            Console.ReadLine();
            while (nberreurs < 11 && gagne == 0)
            {
                Console.WriteLine("Voulez-vous un rappel des règles du jeu ? Tapez oui si vous en voulez un ");
                string reponse = Console.ReadLine();
                Regles(reponse);//Affiche les règles du jeu si le joueur le veut.
                AideIntelligente(motcache, liste, "dicoFR.txt");//La machine propose une lettre au joueur s'il le veut; 
                string proposition = JoueurPropose(motcache, nberreurs);//Demande au premier joueur une proposition.
                for (int i = 0; i < liste.Length; i++)//Change la liste des lettres non dites pour l'aide intelligente.
                {
                    if (liste[i] == proposition)
                    {
                        liste[i] = "";
                    }
                }
                Rappel(motmystere);//Demande au joueur s'il veut un rappel du mot.
                if (proposition.Length != 1)//La proposition est un mot.
                {
                    Console.WriteLine("La proposition du joueur est " + proposition);
                    Console.WriteLine("Le mot proposé est-il le mot que vous devez faire deviner ? Tapez oui ou non.");
                    if (Console.ReadLine() == "oui")
                    {
                        Console.WriteLine("Le joueur qui devait deviner le mot à gagné");
                        gagne = 1;
                    }
                    else
                    {
                        Console.WriteLine("La proposition est incorecte");
                        nberreurs = nberreurs + 1;
                    }
                }
                else//La proposition est une lettre.
                {
                    Console.WriteLine("Le joueur propose la lettre " + proposition);
                    Console.WriteLine("La lettre apparait-elle dans le mot à faire deviner ? Tapez oui ou non.");
                    if (Console.ReadLine() == "oui")
                    {
                        Console.WriteLine("Sur quelle position la lettre apparait-elle pour la première fois ? Tapez le numéro de la position.");
                        int position = int.Parse(Console.ReadLine());
                        motcache = RemplaceLettre(proposition, motcache, position);
                        Console.WriteLine("La lettre apparait-elle une autre fois ? Tapez oui ou non.");
                        int k = 2;
                        while (Console.ReadLine() == "oui")
                        {
                            Console.WriteLine("Sur quelle position la lettre apparait-elle pour la " + k + " ième  fois ? Tapez le numéro de la position.");
                            int position2 = int.Parse(Console.ReadLine());
                            motcache = RemplaceLettre(proposition, motcache, position2);
                            Console.WriteLine("La lettre apparait-elle une autre fois ? Tapez oui ou non.");
                        }
                        int compteur1 = 0;
                        for (int z = 0; z < motcache.Length; z++)
                        {
                            if (motcache.Substring(z, 1) == "_")
                            {
                                compteur1 = compteur1 + 1;
                            }
                        }
                        if (compteur1 == 0)
                        {
                            Console.WriteLine("Le joueur qui devait deviner le mot a gagné au tour " + (tour + 1) + ", il a fait " + nberreurs + "erreurs et le mot était " + motcache);
                            gagne = 1;
                        }
                    }
                    else
                    {
                        nberreurs = nberreurs + 1;
                    }
                }
                tour = tour + 1;
                DessinPendu(nberreurs, "Le Joueur qui devait deviner le mot perdu au tour " + tour);//Affiche l'avancement du pendu. 
                gagne = Abandon(nberreurs, gagne);//Demande au joueur s'il veut abandonner et change la valeur de "gagne" pour arrêter la partie ou non.
            }
        }

        //Fonction qui affiche et prend en charge une partie si le mode jeu est un joueur contre la machine.
        public static void JoueurContreMachine(string motcache, string motmystere, int nbajlettre)
        {
            string[] liste = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            if (nbajlettre == 1)
            {
                for (int i = 0; i < liste.Length; i++)//Change la liste des lettres non dites pour l'aide intelligente, enlève les mots mis au début.
                {
                    if (liste[i] == motmystere.Substring(0, 1))
                    {
                        liste[i] = "";
                    }
                }
            }
            else if (nbajlettre == 2)
            {
                for (int i = 0; i < liste.Length; i++)//Change la liste des lettres non dites pour l'aide intelligente, enlève les mots mis au début.
                {
                    if (liste[i] == motmystere.Substring(0, 1) || liste[i] == motmystere.Substring(motmystere.Length - 1, 1))
                    {
                        liste[i] = "";
                    }
                }
            }
            int nberreurs = 0;
            int gagne = 0;
            int tour = 0;
            while (nberreurs < 11 && gagne == 0)
            {
                Console.WriteLine("Voulez-vous un rappel des rèles du jeu ? Tapez oui si vous en voulez un ");
                string reponse = Console.ReadLine();
                Regles(reponse);
                AideIntelligente(motcache, liste, "dicoFR.txt");
                string proposition = JoueurPropose(motcache, nberreurs);
                for (int i = 0; i < liste.Length; i++)
                {
                    if (liste[i] == proposition)
                    {
                        liste[i] = "";
                    }
                }
                if (proposition.Length != 1)
                {
                    if (proposition == motmystere)
                    {
                        Console.WriteLine("Vous avez gagné");
                        gagne = 1;
                    }
                    else
                    {
                        Console.WriteLine("Le mot que vous avez proposé n'est pas celui que vous devez deviné");
                        nberreurs = nberreurs + 1;
                    }
                }
                else
                {
                    int compteur = 0;
                    for (int j = 0; j < motmystere.Length; j++)
                    {
                        if ((motcache.Substring(j, 1) == proposition))//La lettre est déjà proposé.
                        {
                            compteur = -1;
                        }
                        else if (motmystere.Substring(j, 1) == proposition)
                        {
                            motcache = RemplaceLettre(proposition, motcache, j + 1);
                            compteur = compteur + 1;
                        }
                    }
                    if (compteur == -1)
                    {
                        Console.WriteLine("Vous avez déjà proposé cette lettre");
                        nberreurs = nberreurs + 1;
                    }
                    else if (compteur == 0)
                    {
                        Console.WriteLine("Le mot ne contient pas la lettre que vous avez proposée");
                        nberreurs = nberreurs + 1;
                    }
                    else if (compteur != -1)
                    {
                        Console.WriteLine("Le mot contient la lettre que vous avez proposée " + compteur + " fois, bien joué !");
                    }
                    int compteur1 = 0;
                    for (int z = 0; z < motcache.Length; z++)//Est-ce que le mot a été découvert à force de proposer des lettres ?
                    {
                        if (motcache.Substring(z, 1) == "_")
                        {
                            compteur1 = compteur1 + 1;
                        }
                    }
                    if (compteur1 == 0)
                    {
                        Console.WriteLine("Vous avez gagné au tour " + (tour + 1) + " avec " + nberreurs + " erreurs et le mot était " + motcache);
                        gagne = 1;
                    }
                }
                tour = tour + 1;
                DessinPendu(nberreurs, "Vous avez perdu au tour " + tour + " ! Quel dommage ...");
                gagne = Abandon(nberreurs, gagne);
            }
        }

        //Fonction qui affiche et prend en charge une partie si le mode jeu est la machine contre un joueur.
        public static void MachineContreJoueur(string motcache, string motmystere)
        {
            string[] liste = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            int nberreurs = 0;
            int gagne = 0;
            int tour = 0;
            Console.WriteLine("Le mot à faire deviner est " + motmystere);
            while (nberreurs < 11 && gagne == 0)
            {
                Console.WriteLine("Voulez-vous un rappel des rèles du jeu ? Tapez oui si vous en voulez un ");
                string reponse = Console.ReadLine();
                Regles(reponse);
                string proposition = MachinePropose(motcache, liste, "dicoFR.txt");
                for (int i = 0; i < liste.Length; i++)
                {
                    if (liste[i] == proposition)
                    {
                        liste[i] = "";
                    }
                }
                Rappel(motmystere);
                if (proposition.Length != 1)
                {
                    Console.WriteLine("La proposition de la machine est " + proposition);
                    Console.WriteLine("Le mot proposé est-il le mot que vous devez faire deviner ? Tapez oui ou non.");
                    if (Console.ReadLine() == "oui")
                    {
                        Console.WriteLine("La machine à gagné !");
                        gagne = 1;
                    }
                    else
                    {
                        nberreurs = nberreurs + 1;
                    }
                }
                else
                {
                    Console.WriteLine("La machine propose la lettre " + proposition);
                    Console.WriteLine("La lettre apparait-elle dans le mot à faire deviner ? Tapez oui ou non.");
                    if (Console.ReadLine() == "oui")
                    {
                        Console.WriteLine("Sur quelle position la lettre apparait-elle pour la première fois ? Tapez le numéro de la position.");
                        int position = int.Parse(Console.ReadLine());
                        motcache = RemplaceLettre(proposition, motcache, position);
                        Console.WriteLine("La lettre apparait-elle une autre fois ? Tapez oui ou non.");
                        int k = 2;
                        while (Console.ReadLine() == "oui")
                        {
                            Console.WriteLine("Sur quelle position la lettre apparait-elle pour la " + k + " ième  fois ? Tapez le numéro de la position.");
                            int position2 = int.Parse(Console.ReadLine());
                            motcache = RemplaceLettre(proposition, motcache, position2);
                            Console.WriteLine("La lettre apparait-elle une autre fois ? Tapez oui ou non.");
                        }
                        int compteur1 = 0;
                        for (int z = 0; z < motcache.Length; z++)
                        {
                            if (motcache.Substring(z, 1) == "_")
                            {
                                compteur1 = compteur1 + 1;
                            }
                        }
                        if (compteur1 == 0)
                        {
                            Console.WriteLine("La machine a gagné au tour " + (tour + 1) + " avec " + nberreurs + "erreurs et le mot était " + motcache);
                            gagne = 1;
                        }
                    }
                    else
                    {
                        nberreurs = nberreurs + 1;
                    }
                    Console.WriteLine("La machine a fait " + nberreurs + " erreurs");
                }
                tour = tour + 1;
                DessinPendu(nberreurs, "La machine a perdu au tour " + tour + ", bien joué ! ^^");
                gagne = Abandon(nberreurs, gagne);
            }
        }

        //Fonction qui demande au joueur s'il veut comparer son résulat avec la machine et qui affiche le nombre de tour auquel la machine a fini de joueur et combien d'erreurs elle a faite si la réponse du joueur est positive. 
        public static void ComparaisonMachine(string motmystere, string motcache)
        {
            string[] liste = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            int nberreurs = 0;
            int gagne = 0;
            int tour = 0;
            while (nberreurs < 11 && gagne == 0)
            {
                string proposition = MachinePropose(motcache, liste, "dicoFR.txt");
                for (int i = 0; i < liste.Length; i++)
                {
                    if (liste[i] == proposition)
                    {
                        liste[i] = "";
                    }
                }
                int compteur = 0;
                for (int j = 0; j < motmystere.Length; j++)
                {
                    if (motmystere.Substring(j, 1) == proposition)
                    {
                        motcache = RemplaceLettre(proposition, motcache, j + 1);
                        compteur = compteur + 1;
                    }
                }
                if (compteur == 0)
                {
                    nberreurs = nberreurs + 1;
                }
                int compteur1 = 0;
                for (int z = 0; z < motcache.Length; z++)
                {
                    if (motcache.Substring(z, 1) == "_")
                    {
                        compteur1 = compteur1 + 1;
                    }
                }
                if (compteur1 == 0)
                {
                    gagne = 1;
                }
                tour = tour + 1;
            }
            if (nberreurs == 11)
            {
                Console.WriteLine("La machine a perdu au " + tour + " ème tour");
            }
            else
            {
                Console.WriteLine("La machine a gagné au " + tour + " ème tour, en faisant " + nberreurs + " erreurs");
            }
        }

        //Fonction qui affiche l'avancement du pendu en fonction du nombre d'erreurs. 
        public static void DessinPendu(int nberreur, string phrasedefin)
        {
            string labase = "________|________";
            string potence = "______________";


            if (nberreur == 1)
            {
                Console.WriteLine("                 ");
                Console.WriteLine("                 ");
                Console.WriteLine("                 ");
                Console.WriteLine("                 ");
                Console.WriteLine("                 ");
                Console.WriteLine("                 ");
                Console.WriteLine(labase);
            }
            else if (nberreur == 2)
            {
                Console.WriteLine("        |        ");
                Console.WriteLine("        |        ");
                Console.WriteLine("        |        ");
                Console.WriteLine("        |        ");
                Console.WriteLine("        |        ");
                Console.WriteLine("        |        ");
                Console.WriteLine(labase);

            }
            else if (nberreur == 3)
            {
                Console.WriteLine("        " + potence);
                Console.WriteLine("        |        ");
                Console.WriteLine("        |        ");
                Console.WriteLine("        |        ");
                Console.WriteLine("        |        ");
                Console.WriteLine("        |        ");
                Console.WriteLine("        |        ");
                Console.WriteLine(labase);

            }
            else if (nberreur == 4)
            {
                Console.WriteLine("        " + potence);
                Console.WriteLine("        |  /     ");
                Console.WriteLine("        | /      ");
                Console.WriteLine("        |/       ");
                Console.WriteLine("        |        ");
                Console.WriteLine("        |        ");
                Console.WriteLine("        |        ");
                Console.WriteLine(labase);
            }
            else if (nberreur == 5)
            {

                Console.WriteLine("        " + potence);
                Console.WriteLine("        |  /         |     ");
                Console.WriteLine("        | /          |       ");
                Console.WriteLine("        |/       ");
                Console.WriteLine("        |        ");
                Console.WriteLine("        |        ");
                Console.WriteLine("        |        ");
                Console.WriteLine(labase);
            }
            else if (nberreur == 6)
            {

                Console.WriteLine("        " + potence);
                Console.WriteLine("        |  /         |     ");
                Console.WriteLine("        | /          |       ");
                Console.WriteLine("        |/           O");
                Console.WriteLine("        |        ");
                Console.WriteLine("        |        ");
                Console.WriteLine("        |        ");
                Console.WriteLine(labase);
            }
            else if (nberreur == 7)
            {

                Console.WriteLine("        " + potence);
                Console.WriteLine("        |  /         |     ");
                Console.WriteLine("        | /          |       ");
                Console.WriteLine("        |/           O");
                Console.WriteLine("        |            |");
                Console.WriteLine("        |        ");
                Console.WriteLine("        |        ");
                Console.WriteLine(labase);

            }
            else if (nberreur == 8)
            {

                Console.WriteLine("        " + potence);
                Console.WriteLine("        |  /         |     ");
                Console.WriteLine("        | /          |       ");
                Console.WriteLine("        |/           O");
                Console.WriteLine("        |            |>");
                Console.WriteLine("        |        ");
                Console.WriteLine("        |        ");
                Console.WriteLine(labase);
            }
            else if (nberreur == 9)
            {

                Console.WriteLine("        " + potence);
                Console.WriteLine("        |  /         |     ");
                Console.WriteLine("        | /          |       ");
                Console.WriteLine("        |/           O");
                Console.WriteLine("        |           <|>");
                Console.WriteLine("        |           ");
                Console.WriteLine("        |        ");
                Console.WriteLine(labase);
            }
            else if (nberreur == 10)
            {

                Console.WriteLine("        " + potence);
                Console.WriteLine("        |  /         |     ");
                Console.WriteLine("        | /          |       ");
                Console.WriteLine("        |/           O");
                Console.WriteLine("        |           <|>");
                Console.WriteLine("        |             [");
                Console.WriteLine("        |        ");
                Console.WriteLine(labase);
            }
            else if (nberreur == 11)
            {

                Console.WriteLine("        " + potence);
                Console.WriteLine("        |  /         |     ");
                Console.WriteLine("        | /          |       ");
                Console.WriteLine("        |/           O");
                Console.WriteLine("        |           <|>");
                Console.WriteLine("        |           ] [");
                Console.WriteLine("        |        ");
                Console.WriteLine(labase);

                System.Threading.Thread.Sleep(4000);
                Console.Clear();
                Console.WriteLine("      //////////////////    ");
                Console.WriteLine("                           ");
                Console.WriteLine("         X           X     ");
                Console.WriteLine("(                             )");
                Console.WriteLine("      ___________________  ");
                Console.WriteLine("        / / /              ");
                Console.WriteLine("       / / /               ");
                Console.WriteLine("      (___/                ");

                Console.WriteLine(phrasedefin);
            }
        }
        static void Main()
        {
            string[] alphabet = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            float nombretotaledelettre = CompteTotaleLettre("dicoFR.txt");
            int[] nbdelettre = CompteLettre2(alphabet);
            string motmystere = "";
            CreationFichiers("dicoFR.txt", "Liste1", "Liste2", "liste3", alphabet, nombretotaledelettre, nbdelettre);
            Console.WriteLine("Bienvenue dans le Pendu de Vivi et Yoyo ! ");
            Console.WriteLine("A combien de joueur voulez-vous jouer ? (Maximum 2 joueurs)");
            if (Console.ReadLine() == "1")//Un joueur
            {
                Console.WriteLine("Voulez-vous deviner le mot ou voulez-vous le faire deviner ? Tapez 1 pour deviner le mot ou sinon 2");
                if (Console.ReadLine() == "1")//Mode de jeu : Joueur contre Machine (Joueur devine)
                {
                    Console.WriteLine("Quel niveau de difficulté pour le mot voulez-vous ? Tapez 1 pour facile, 2 pour moyenne et 3 pour difficile");
                    if (Console.ReadLine() == "1")
                    {
                        motmystere = ChoixMot("Liste1", CompteMot("Liste1"));
                    }
                    else if (Console.ReadLine() == "2")
                    {
                        motmystere = ChoixMot("Liste2", CompteMot("Liste2"));
                    }
                    else
                    {
                        motmystere = ChoixMot("liste3", CompteMot("liste3"));
                    }
                    Console.WriteLine("Combien de lettre voulez-vous faire apparaître au début de la partie ? Tapez 0, 1 ou 2");
                    int nbajlettre = int.Parse(Console.ReadLine());
                    string motcache = AjouteLettre(motmystere, nbajlettre);//Création du mot caché, c'est à dire ce que le joueur qui doit deviner le mot voit.
                    Console.WriteLine("C'est parti !! Bon jeu ! ^^");
                    JoueurContreMachine(motcache, motmystere, nbajlettre);
                    Console.WriteLine("Voulez-vous comparer vos résultats avec la machine ? Tapez oui ou non");
                    if (Console.ReadLine() == "oui")
                    {
                        ComparaisonMachine(motmystere, motcache);//Affiche le nombre de tour et le nombre d'erreurs qu'a fait la machine avec le même mot.
                    }
                }
                else//Mode de jeu : Machine contre joueur 
                {
                    Console.WriteLine("Quel niveau de difficulté pour le mot voulez-vous ? Tapez 1 pour facile, 2 pour moyenne et 3 pour difficile");
                    if (Console.ReadLine() == "1")
                    {
                        motmystere = ChoixMot("Liste1", CompteMot("Liste1"));
                    }
                    else if (Console.ReadLine() == "2")
                    {
                        motmystere = ChoixMot("Liste2", CompteMot("Liste2"));
                    }
                    else
                    {
                        motmystere = ChoixMot("liste3", CompteMot("liste3"));
                    }
                    string motcache = AjouteLettre(motmystere, 0);
                    Console.WriteLine("C'est parti !! Bon jeu ! ^^");
                    MachineContreJoueur(motcache, motmystere);
                }
            }
            else
            {
                Console.WriteLine("Quel niveau de difficulté pour le mot voulez-vous ? Tapez 1 pour facile, 2 pour moyenne et 3 pour difficile");
                if (Console.ReadLine() == "1")
                {
                    motmystere = ChoixMot("Liste1", CompteMot("Liste1"));
                }
                else if (Console.ReadLine() == "2")
                {
                    motmystere = ChoixMot("Liste2", CompteMot("Liste2"));
                }
                else
                {
                    motmystere = ChoixMot("liste3", CompteMot("liste3"));
                }
                Console.WriteLine("Combien de lettre voulez-vous faire apparaître au début de la partie ? Tapez 0, 1 ou 2");
                int nbajlettre = int.Parse(Console.ReadLine());
                string motcache = AjouteLettre(motmystere, nbajlettre);
                Console.WriteLine("C'est parti !! Bon jeu ! ^^");
                JoueurContreJoueur(motcache, motmystere, nbajlettre);
                Console.WriteLine("Voulez-vous comparer vos résultats avec la machine ? Tapez oui ou non");
                if (Console.ReadLine() == "oui")
                {
                    ComparaisonMachine(motmystere, motcache);
                }
            }
            Console.WriteLine("Merci d'avoir joué à notre pendu ! ");
        }
    }
}
