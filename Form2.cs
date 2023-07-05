using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UTranslator
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        /*private void bookoftravelsPicture_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://store.steampowered.com/app/1152340/Book_of_Travels/",
                UseShellExecute = true
            });
        }*/

        private void temtemPicture_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://steamcommunity.com/sharedfiles/filedetails/?id=2365045065",
                UseShellExecute = true
            });
        }

        private void corekeeperPicture_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://steamcommunity.com/sharedfiles/filedetails/?id=2801195857",
                UseShellExecute = true
            });
        }
        private void escapeSimulatorPicture_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://steamcommunity.com/sharedfiles/filedetails/?id=2817778047",
                UseShellExecute = true
            });
        }
        private void mortuaryPicture_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://steamcommunity.com/sharedfiles/filedetails/?id=2846351927",
                UseShellExecute = true
            });
        }

        private void spaceunboundPicture_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://vk.com/utranslator?w=wall-76994032_937",
                UseShellExecute = true
            });
        }

        private void temtemPicture_MouseMove(object sender, MouseEventArgs e)
        {
            ToolTip t = new ToolTip();
            t.SetToolTip(temtemPicture, "Русификатор Temtem");
        }

        private void corekeeperPicture_MouseMove(object sender, MouseEventArgs e)
        {
            ToolTip t = new ToolTip();
            t.SetToolTip(corekeeperPicture, "Русификатор Core Keeper");
        }

        private void escapeSimulatorPicture_MouseMove(object sender, MouseEventArgs e)
        {
            ToolTip t = new ToolTip();
            t.SetToolTip(corekeeperPicture, "Русификатор Escape Simulator");
        }

        private void mortuaryPicture_MouseMove(object sender, MouseEventArgs e)
        {
            ToolTip t = new ToolTip();
            t.SetToolTip(corekeeperPicture, "Русификатор The Mortuary Assistant");
        }

        private void spaceunboundPicture_MouseMove(object sender, MouseEventArgs e)
        {
            ToolTip t = new ToolTip();
            t.SetToolTip(corekeeperPicture, "Русификатор A Space for the Unbound");
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
