using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIF1006Devoir2
{
    static class Chiffrement
    {
        //##########################################################################//
        //####################### ===== Chiffrement ===== ##########################//
        public static string Chiffrer(String message, String cle)
        {
            string msgChiffre;
            char VI = 'a';
            byte VIbyte = Convert.ToByte(VI);

            //conversion de la clé en byte
            string[] sKeyMatrix = cle.Split(' ');
            int keyLength = sKeyMatrix.Length; //aussi nombre de colonnes
            int[] keyMatrix = new int[keyLength];
            for (int i = 0; i < keyLength; i++)
            {
                keyMatrix[i] = Convert.ToInt32(sKeyMatrix[i]);
            }

            //conversion du message en byte

            int msgLength = message.Length;
            byte[] msgMatrix = new byte[msgLength];
            msgMatrix = System.Text.Encoding.UTF8.GetBytes(message);


            //écriture du message dans tableau 2D
            int x = msgLength / keyLength;
            int position = 0;
            if (msgLength % keyLength != 0) x++;
            byte[,] msgTable = new byte[x, keyLength];
            char vide = ' ';
            byte videByte = Convert.ToByte(vide);

            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < keyLength; j++)
                {
                    if (position < msgLength)
                    {
                        msgTable[i, j] = msgMatrix[position];
                        position++;
                    }
                    else msgTable[i, j] = videByte; //pour les espace vide dans le tableau
                }
            }

            //transposition de la matrice 2D dans l'ordre de la clé
            byte[,] msgTableTrans = new byte[x, keyLength];
            for (int j = 0; j < keyLength; j++)//lecture colonne par colonne
            {
                for (int i = 0; i < x; i++)
                {
                    msgTableTrans[i, keyMatrix[j] - 1] = msgTable[i, j];
                }
            }


            //texte transposé remis en une colonne

            byte[] msgTransList = new byte[keyLength * x];
            int positionTrans = 0;

            for (int j = 0; j < keyLength; j++)
            {
                for (int i = 0; i < x; i++)
                {
                    msgTransList[positionTrans] = msgTableTrans[i, j];
                    positionTrans++;
                }
            }


            //chiffrement du message

            byte[] msgChiffreList = new byte[keyLength * x];

            for (int i = 0; i < keyLength * x; i++)
            {
                if (i == 0) msgChiffreList[i] = (byte)(msgTransList[i] ^ VIbyte);
                else msgChiffreList[i] = (byte)(msgTransList[i] ^ msgTransList[i - 1]);
            }
            msgChiffre = System.Text.Encoding.UTF8.GetString(msgChiffreList);

            return msgChiffre;
        }

        //############################################################################//
        //####################### ===== Déchiffrement ===== ##########################//
        public static string Dechiffrer(String message, String cle)
        {
            char VI = 'a';  //vecteur d'ititialisation identique au chiffrement
            byte VIbyte = Convert.ToByte(VI);
            int msgLength = message.Length;



            //conversion de la clé en byte, identique au chiffrement
            string[] sKeyMatrix = cle.Split(' ');
            int keyLength = sKeyMatrix.Length; //aussi nombre de colonnes
            int[] keyMatrix = new int[keyLength];
            for (int i = 0; i < keyLength; i++)
            {
                keyMatrix[i] = Convert.ToInt32(sKeyMatrix[i]);
            }

            //conversion du message chiffré en tableau de byte et déchiffrement


            byte[] msgMatrix = new byte[msgLength];
            byte[] msgMatrixTmp = System.Text.Encoding.UTF8.GetBytes(message);//new byte[msgLength];
            //msgMatrixTmp = 
            for (int i = 0; i < msgLength; i++)
            {
                //msgMatrixTmp[i] = Convert.ToByte(message[i]);
                if (i == 0) msgMatrix[i] = (byte)(msgMatrixTmp[i] ^ VIbyte);
                else msgMatrix[i] = (byte)(msgMatrixTmp[i] ^ msgMatrix[i - 1]);
            }

            // placement du message dans un tableau 2D pour retransposition
            int x = msgLength / keyLength;
            int position = 0;
            //if (msgLength % keyLength != 0) x++; devrait jamais avoir lieu
            byte[,] msgTable = new byte[x, keyLength];

            for (int j = 0; j < keyLength; j++)
            {
                for (int i = 0; i < x; i++)
                {
                    msgTable[i, j] = msgMatrix[position];
                    position++;
                }
            }

            //réordonnancement du tableau en foncton de la clé :
            byte[,] msgTableTrans = new byte[x, keyLength];
            for (int j = 0; j < keyLength; j++)//lecture colonne par colonne
            {
                for (int i = 0; i < x; i++)
                {
                    msgTableTrans[i, j] = msgTable[i, keyMatrix[j] - 1];
                }
            }

            //tableau 2D retransposé sur liste
            byte[] msgRetrans = new byte[msgLength];
            int retransPosition = 0;
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < keyLength; j++)
                {
                    msgRetrans[retransPosition] = msgTableTrans[i, j];
                    retransPosition++;
                }
            }

            //recherche du nombre d'espace vide a la fin
            int count = 0;
            int k = msgLength - 1;
            string messageDechiffreTmp = System.Text.Encoding.UTF8.GetString(msgRetrans);

            while (messageDechiffreTmp[k] == ' ')
            {
                k--;
                count++;
            }

            byte[] msgListSansZero = new byte[msgLength - count];

            for (int i = 0; i < msgLength - count; i++)
            {
                msgListSansZero[i] = msgRetrans[i];
            }

            string messageDechiffre = System.Text.Encoding.UTF8.GetString(msgListSansZero);
            return messageDechiffre;
        }   
    }
}
