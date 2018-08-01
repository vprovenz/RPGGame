using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Engine;

namespace ShreksSwamp //all hitpoints change to just points
{
    public partial class ShreksSwamp : Form
    {
        private Player player;

        public ShreksSwamp()
        {
            player = new Player(10, 10, 20, 0, 1);

            lblPoints.Text = player.CurrentPoints.ToString();
            lblGold.Text = player.Gold.ToString();
            lblExperience.Text = player.ExperiencePoints.ToString();
            lblLevel.Text = player.Level.ToString();
        }
    }
}
