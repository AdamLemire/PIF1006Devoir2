using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PIF1006Devoir2
{
    /// <summary>
    /// INTERFACE WPF
    /// Devoir 2 - Cipher Block Chaining et chiffrement par transposition
    /// Auteurs : Adam Lemire et Rémi Petiteau
    /// 16 décembre 2016
    /// PIF1006
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ChiffrerButton_Click(object sender, RoutedEventArgs e)
        {
            ResultatsTextBlock.Text = Chiffrement.Chiffrer(MsgBox.Text, KeyBox.Text);
        }

        private void DechiffreButton_Click(object sender, RoutedEventArgs e)
        {
            MsgDechiffreTextBlock.Text = Chiffrement.Dechiffrer(ResultatsTextBlock.Text, KeyBox.Text);
        }

     
    }
}
