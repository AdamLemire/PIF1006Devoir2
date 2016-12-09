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
            string msgChiffre = "";
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
            char vide = ' ';  //vecteur d'ititialisation identique au chiffrement
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
                    else msgTable[i, j] = videByte;
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


            //texte transposé en une colonne selon la clé fournie

            byte[] msgTransList = new byte[keyLength * x];
            int positionTrans = 0;
            if (msgLength % keyLength != 0)

                for (int i = 0; i < x; i++)
                {
                    for (int j = 0; j < keyLength; j++)
                    {
                        msgTransList[positionTrans] = msgTableTrans[i, j];
                        positionTrans++;
                    }
                }
            //byte[] msgTransList = new byte[keyLength * x];//[msgLength];
            //int positionTrans = 0;
            //int nbCharNextRow;
            //if (msgLength % keyLength != 0)
            //    nbCharNextRow = (msgLength % keyLength);
            //else nbCharNextRow = keyLength;


            //for (int j = 0; j < keyLength; j++)//lecture colonne par colonne
            //{
            //    for (int i = 0; i < x; i++)
            //    {
            //        //if ((msgTableTrans[i, j] !=0)) //(positionTrans < msgLength) &&(i < x-1 || (keyMatrix[j]) <= nbCharNextRow) )
            //        //{
            //        msgTransList[positionTrans] = msgTableTrans[i, j]; //marche pas, commence pas par la bonne colonne
            //        positionTrans++;
            //        //}

            //    }
            //}

            //chiffrement du message

            for (int i = 0; i < msgLength; i++)
            {
                if (i == 0) msgTransList[i] = (byte)(msgTransList[i] ^ VIbyte);
                else msgTransList[i] = (byte)(msgTransList[i] ^ msgTransList[i - 1]);
            }
            msgChiffre += System.Text.Encoding.UTF8.GetString(msgTransList);

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

            //conversion du message chiffré en tableau de byte


            byte[] msgMatrix = new byte[msgLength];
            byte[] msgMatrixTmp = new byte[msgLength];
            for (int i = 0; i < msgLength; i++)
            {
                msgMatrixTmp[i] = Convert.ToByte(message[i]);
                if (i == 0) msgMatrix[i] = (byte)(msgMatrixTmp[i] ^ VIbyte);
                else msgMatrix[i] = (byte)(msgMatrixTmp[i] ^ msgMatrixTmp[i - 1]);
            }

            // placement du message dans une un tableau 2D pour retransposition
            int x = msgLength / keyLength;
            int position = 0;
            if (msgLength % keyLength != 0) x++;
            byte[,] msgTable = new byte[x, keyLength];

            int nbCharNextRow;
            if (msgLength % keyLength != 0)
                nbCharNextRow = (msgLength % keyLength);
            else nbCharNextRow = keyLength;

            for (int j = 0; j < keyLength; j++)
            {
                for (int i = 0; i < x; i++)
                {
                    if ((position < msgLength))//&& (i < x - 1 || (j) < nbCharNextRow))
                    {
                        msgTable[i, j] = msgMatrix[position];
                        position++;
                    }
                }
            }

            //tableau 2D retransposé sur liste
            byte[] msgRetrans = new byte[msgLength];
            int retransPosition = 0;
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < keyLength; j++)
                {
                    if ((retransPosition < msgLength) && msgTable[i, keyMatrix[j] - 1] != 0)
                    {
                        msgRetrans[retransPosition] = msgTable[i, keyMatrix[j] - 1];
                        retransPosition++;
                    }
                }
            }

            byte[] msgListSansZero = new byte[msgLength];

            for (int i = 0; i < msgLength; i++)
            {
                msgListSansZero[i] = msgRetrans[i];
            }

            string messageDechiffre = System.Text.Encoding.UTF8.GetString(msgRetrans);
           


            return messageDechiffre;
        }


        /// <summary>
        /// /////////////////////////////////////////////////TEST//////////////////////////////////////////////////////////////////////////////
        /// </summary>
        /// <param name="message"></param>
        /// <param name="cle"></param>
        /// <returns></returns>
        //test

        public static string TestChiffrer(String message, String cle)
        {
            char VI = 'a';  //vecteur d'ititialisation identique au chiffrement
            byte VIbyte = Convert.ToByte(VI);
            int keyLength = cle.Length;
            int msgLength = message.Length;
            byte[] msgMatrix = new byte[msgLength];

            msgMatrix = System.Text.Encoding.UTF8.GetBytes(message);


            //écriture du message dans tableau 2D
            int x = msgLength / keyLength;
            int position = 0;
            if (msgLength % keyLength != 0) x++;
            byte[,] msgTable = new byte[x, keyLength];
            char vide = ' ';  //vecteur d'ititialisation identique au chiffrement
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
                    else msgTable[i, j] = videByte;
                }
            }

            //texte transposé en une colonne
            byte[] msgTransList = new byte[keyLength * x];
            int positionTrans = 0;
            if (msgLength % keyLength != 0)

            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < keyLength; j++)
                {
                    msgTransList[positionTrans] = msgTable[i, j];
                    positionTrans++;
                }
            }


            //chiffrement cpc
            int w = 1;
            for (int i = 0; i < msgTransList.Length; i++)
            {
                if (msgTransList[i] != 0)
                {
                   
                    if (i == 0) msgTransList[i] = (byte)(msgTransList[i] ^ VIbyte);
                    else msgTransList[i] = (byte)(msgTransList[i] ^ msgTransList[i - 1]);
                }
            }

            //dechiffrement
            byte[] msgMatrixTmp = new byte[msgTransList.Length];

            for (int i = 0; i < msgTransList.Length; i++)
            {


                    if (i == 0) msgMatrixTmp[i] = (byte)(msgTransList[i] ^ VIbyte);
                    else  msgMatrixTmp[i] = (byte)(msgTransList[i] ^ msgTransList[i - 1]);
            
            }

            byte[] msgListSansZero = new byte[msgLength];

            for (int i = 0; i < msgLength; i++)
            {
                    msgListSansZero[i] = msgMatrixTmp[i];
            }

            string messageDechiffre = System.Text.Encoding.UTF8.GetString(msgListSansZero);
            return messageDechiffre;
        }


    }
}
