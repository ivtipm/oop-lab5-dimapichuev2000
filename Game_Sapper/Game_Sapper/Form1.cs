using Game_Sapper.Control;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game_Sapper
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
            MapControl.Init(this);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        private void ОПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string Info = "Игра Сапёр" + "\n\n" +
                "Работал над программой Пичуев ДА" + "\n\n" +
                "Github:https://github.com/dimapichuev2000" + "\n\n" +
                "Дата релиза 27.05.2020, версия 1.2";
            MessageBox.Show(Info, "Справка", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
