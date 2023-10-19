
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
            label1 = new System.Windows.Forms.Label();
            QiwiLink = new System.Windows.Forms.LinkLabel();
            YandexLink = new System.Windows.Forms.LinkLabel();
            PaypalLink = new System.Windows.Forms.LinkLabel();
            VkDonutLink = new System.Windows.Forms.LinkLabel();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Location = new System.Drawing.Point(28, 20);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(303, 62);
            label1.TabIndex = 0;
            label1.Text = "Щёлкните на ссылку, чтобы скопировать в буфер обмена ID кошелька и перейти на сайт. Все средства уйдут на развитие проекта и улучшение его качества. Спасибо!";
            label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // QiwiLink
            // 
            QiwiLink.AutoSize = true;
            QiwiLink.Location = new System.Drawing.Point(28, 121);
            QiwiLink.Name = "QiwiLink";
            QiwiLink.Size = new System.Drawing.Size(33, 15);
            QiwiLink.TabIndex = 1;
            QiwiLink.TabStop = true;
            QiwiLink.Text = "QIWI";
            QiwiLink.LinkClicked += QiwiLink_LinkClicked;
            // 
            // YandexLink
            // 
            YandexLink.AutoSize = true;
            YandexLink.Location = new System.Drawing.Point(28, 145);
            YandexLink.Name = "YandexLink";
            YandexLink.Size = new System.Drawing.Size(160, 15);
            YandexLink.TabIndex = 2;
            YandexLink.TabStop = true;
            YandexLink.Text = "ЮMoney: +7 (985) 843-30-82";
            YandexLink.LinkClicked += YandexLink_LinkClicked;
            // 
            // PaypalLink
            // 
            PaypalLink.AutoSize = true;
            PaypalLink.Location = new System.Drawing.Point(28, 169);
            PaypalLink.Name = "PaypalLink";
            PaypalLink.Size = new System.Drawing.Size(42, 15);
            PaypalLink.TabIndex = 3;
            PaypalLink.TabStop = true;
            PaypalLink.Text = "PayPal";
            PaypalLink.LinkClicked += PaypalLink_LinkClicked;
            // 
            // VkDonutLink
            // 
            VkDonutLink.AutoSize = true;
            VkDonutLink.Location = new System.Drawing.Point(28, 97);
            VkDonutLink.Name = "VkDonutLink";
            VkDonutLink.Size = new System.Drawing.Size(57, 15);
            VkDonutLink.TabIndex = 4;
            VkDonutLink.TabStop = true;
            VkDonutLink.Text = "VK Donut";
            VkDonutLink.LinkClicked += VkDonutLink_LinkClicked;
            VkDonutLink.MouseMove += VkDonutLink_MouseMove;
            // 
            // label2
            // 
            label2.Location = new System.Drawing.Point(28, 193);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(250, 15);
            label2.TabIndex = 5;
            label2.Text = "Сбер: +7 (985) 843-30-82";
            // 
            // label3
            // 
            label3.Location = new System.Drawing.Point(28, 208);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(303, 15);
            label3.TabIndex = 6;
            label3.Text = "Тинькофф: +7 (985) 843-30-82";
            // 
            // DonateForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(359, 246);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(VkDonutLink);
            Controls.Add(PaypalLink);
            Controls.Add(YandexLink);
            Controls.Add(QiwiLink);
            Controls.Add(label1);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "DonateForm";
            Text = "Поддержать разработчика";
            ResumeLayout(false);
            PerformLayout();
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