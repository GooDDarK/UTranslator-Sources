using System.Diagnostics;
using System.Windows.Forms;

namespace UTranslator
{
    public partial class DonateForm : Form
    {
        public DonateForm()
        {
            InitializeComponent();
        }

        private void QiwiLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Clipboard.SetText("qiwi.com/n/GOODDARK");

            Process.Start(new ProcessStartInfo
            {
                FileName = "https://qiwi.com/n/GOODDARK",
                UseShellExecute = true
            });
        }

        private void YandexLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Clipboard.SetText("410011176218423");

            Process.Start(new ProcessStartInfo
            {
                FileName = "https://yoomoney.ru/transfer/a2w",
                UseShellExecute = true
            });
        }

        private void PaypalLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Clipboard.SetText("https://www.paypal.com/paypalme/GooDDarK");

            Process.Start(new ProcessStartInfo
            {
                FileName = "https://www.paypal.com/paypalme/GooDDarK",
                UseShellExecute = true
            });
        }

        private void VkDonutLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Clipboard.SetText("https://vk.com/donut/utranslator");

            Process.Start(new ProcessStartInfo
            {
                FileName = "https://vk.com/donut/utranslator", 
                UseShellExecute = true
            });
        }

        private void VkDonutLink_MouseMove(object sender, MouseEventArgs e)
        {
            ToolTip t = new ToolTip();
            t.SetToolTip(VkDonutLink, "Поддержать разработчика и получить эксклюзивные привелегии в группе ВК");
        }
    }
}
