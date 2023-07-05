
namespace UTranslator
{
    partial class DonateForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DonateForm));
            this.label1 = new System.Windows.Forms.Label();
            this.QiwiLink = new System.Windows.Forms.LinkLabel();
            this.YandexLink = new System.Windows.Forms.LinkLabel();
            this.PaypalLink = new System.Windows.Forms.LinkLabel();
            this.VkDonutLink = new System.Windows.Forms.LinkLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(28, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(303, 62);
            this.label1.TabIndex = 0;
            this.label1.Text = "Щёлкните на ссылку, чтобы скопировать в буфер обмена ID кошелька и перейти на сай" +
    "т. Все средства уйдут на развитие проекта и улучшение его качества. Спасибо!";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // QiwiLink
            // 
            this.QiwiLink.AutoSize = true;
            this.QiwiLink.Location = new System.Drawing.Point(28, 121);
            this.QiwiLink.Name = "QiwiLink";
            this.QiwiLink.Size = new System.Drawing.Size(33, 15);
            this.QiwiLink.TabIndex = 1;
            this.QiwiLink.TabStop = true;
            this.QiwiLink.Text = "QIWI";
            this.QiwiLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.QiwiLink_LinkClicked);
            // 
            // YandexLink
            // 
            this.YandexLink.AutoSize = true;
            this.YandexLink.Location = new System.Drawing.Point(28, 145);
            this.YandexLink.Name = "YandexLink";
            this.YandexLink.Size = new System.Drawing.Size(136, 15);
            this.YandexLink.TabIndex = 2;
            this.YandexLink.TabStop = true;
            this.YandexLink.Text = "ЮMoney: +79858433082";
            this.YandexLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.YandexLink_LinkClicked);
            // 
            // PaypalLink
            // 
            this.PaypalLink.AutoSize = true;
            this.PaypalLink.Location = new System.Drawing.Point(28, 169);
            this.PaypalLink.Name = "PaypalLink";
            this.PaypalLink.Size = new System.Drawing.Size(42, 15);
            this.PaypalLink.TabIndex = 3;
            this.PaypalLink.TabStop = true;
            this.PaypalLink.Text = "PayPal";
            this.PaypalLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.PaypalLink_LinkClicked);
            // 
            // VkDonutLink
            // 
            this.VkDonutLink.AutoSize = true;
            this.VkDonutLink.Location = new System.Drawing.Point(28, 97);
            this.VkDonutLink.Name = "VkDonutLink";
            this.VkDonutLink.Size = new System.Drawing.Size(57, 15);
            this.VkDonutLink.TabIndex = 4;
            this.VkDonutLink.TabStop = true;
            this.VkDonutLink.Text = "VK Donut";
            this.VkDonutLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.VkDonutLink_LinkClicked);
            this.VkDonutLink.MouseMove += new System.Windows.Forms.MouseEventHandler(this.VkDonutLink_MouseMove);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(28, 193);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 15);
            this.label2.TabIndex = 5;
            this.label2.Text = "Сбер: +79104065201";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(28, 208);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(152, 15);
            this.label3.TabIndex = 6;
            this.label3.Text = "Тинькофф: +79858433082";
            // 
            // DonateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(359, 246);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.VkDonutLink);
            this.Controls.Add(this.PaypalLink);
            this.Controls.Add(this.YandexLink);
            this.Controls.Add(this.QiwiLink);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "DonateForm";
            this.Text = "Поддержать разработчика";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel QiwiLink;
        private System.Windows.Forms.LinkLabel YandexLink;
        private System.Windows.Forms.LinkLabel PaypalLink;
        private System.Windows.Forms.LinkLabel VkDonutLink;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}