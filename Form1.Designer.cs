
namespace UTranslator
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btnStart = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblPercent = new System.Windows.Forms.Label();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.choosePathBtn = new System.Windows.Forms.Button();
            this.updateReinstall = new System.Windows.Forms.Button();
            this.recoverBD = new System.Windows.Forms.Button();
            this.gameNameTxt = new System.Windows.Forms.Label();
            this.gamesListButton = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.vkBtn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.myVkLink = new System.Windows.Forms.LinkLabel();
            this.instructionLink = new System.Windows.Forms.LinkLabel();
            this.versionLabel = new System.Windows.Forms.Label();
            this.deleteTranslate = new System.Windows.Forms.Button();
            this.fontSetup = new System.Windows.Forms.Button();
            this.UpdateButton = new System.Windows.Forms.Button();
            this.txtStatus = new System.Windows.Forms.RichTextBox();
            this.skillsNameCheckTranslate = new System.Windows.Forms.CheckBox();
            this.border = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.border2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.skillsDescriptionCheckTranslate = new System.Windows.Forms.CheckBox();
            this.itemsCheckTranslate = new System.Windows.Forms.CheckBox();
            this.npcCheckTranslate = new System.Windows.Forms.CheckBox();
            this.temtemCheckTranslate = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.deeplButton = new System.Windows.Forms.RadioButton();
            this.googleButton = new System.Windows.Forms.RadioButton();
            this.yandexButton = new System.Windows.Forms.RadioButton();
            this.donateButton = new System.Windows.Forms.Button();
            this.machineNameText = new System.Windows.Forms.TextBox();
            this.itemsDesc = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(170, 211);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(117, 36);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "&Установить";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            this.btnStart.MouseMove += new System.Windows.Forms.MouseEventHandler(this.btnStart_MouseMove);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(12, 296);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(815, 23);
            this.progressBar.Step = 1;
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar.TabIndex = 2;
            // 
            // lblPercent
            // 
            this.lblPercent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPercent.AutoSize = true;
            this.lblPercent.Location = new System.Drawing.Point(795, 278);
            this.lblPercent.Name = "lblPercent";
            this.lblPercent.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblPercent.Size = new System.Drawing.Size(23, 15);
            this.lblPercent.TabIndex = 6;
            this.lblPercent.Text = "0%";
            this.lblPercent.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // txtPath
            // 
            this.txtPath.BackColor = System.Drawing.SystemColors.Control;
            this.txtPath.Location = new System.Drawing.Point(564, 35);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(200, 23);
            this.txtPath.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(485, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 15);
            this.label1.TabIndex = 8;
            this.label1.Text = "Путь к игре:";
            // 
            // choosePathBtn
            // 
            this.choosePathBtn.Location = new System.Drawing.Point(770, 37);
            this.choosePathBtn.Name = "choosePathBtn";
            this.choosePathBtn.Size = new System.Drawing.Size(31, 20);
            this.choosePathBtn.TabIndex = 9;
            this.choosePathBtn.Text = "...";
            this.choosePathBtn.UseVisualStyleBackColor = true;
            this.choosePathBtn.Click += new System.EventHandler(this.choosePathBtn_Click);
            this.choosePathBtn.MouseMove += new System.Windows.Forms.MouseEventHandler(this.choosePathBtn_MouseMove);
            // 
            // updateReinstall
            // 
            this.updateReinstall.Enabled = false;
            this.updateReinstall.Location = new System.Drawing.Point(293, 211);
            this.updateReinstall.Name = "updateReinstall";
            this.updateReinstall.Size = new System.Drawing.Size(171, 36);
            this.updateReinstall.TabIndex = 10;
            this.updateReinstall.Text = "Обновить";
            this.updateReinstall.UseVisualStyleBackColor = true;
            this.updateReinstall.Click += new System.EventHandler(this.updateReinstall_Click);
            this.updateReinstall.MouseMove += new System.Windows.Forms.MouseEventHandler(this.updateReinstall_MouseMove);
            // 
            // recoverBD
            // 
            this.recoverBD.Enabled = false;
            this.recoverBD.Location = new System.Drawing.Point(809, 322);
            this.recoverBD.Name = "recoverBD";
            this.recoverBD.Size = new System.Drawing.Size(17, 10);
            this.recoverBD.TabIndex = 11;
            this.recoverBD.Text = "Восстановить БД";
            this.recoverBD.UseVisualStyleBackColor = true;
            this.recoverBD.Visible = false;
            this.recoverBD.Click += new System.EventHandler(this.recoverBD_Click);
            // 
            // gameNameTxt
            // 
            this.gameNameTxt.AutoSize = true;
            this.gameNameTxt.BackColor = System.Drawing.SystemColors.Window;
            this.gameNameTxt.ForeColor = System.Drawing.Color.ForestGreen;
            this.gameNameTxt.Location = new System.Drawing.Point(188, 16);
            this.gameNameTxt.Name = "gameNameTxt";
            this.gameNameTxt.Size = new System.Drawing.Size(0, 15);
            this.gameNameTxt.TabIndex = 13;
            // 
            // gamesListButton
            // 
            this.gamesListButton.Location = new System.Drawing.Point(655, 225);
            this.gamesListButton.Name = "gamesListButton";
            this.gamesListButton.Size = new System.Drawing.Size(164, 38);
            this.gamesListButton.TabIndex = 14;
            this.gamesListButton.Text = "Список поддерживаемых для русификации игр";
            this.gamesListButton.UseVisualStyleBackColor = true;
            this.gamesListButton.Click += new System.EventHandler(this.gamesListButton_Click);
            this.gamesListButton.MouseMove += new System.Windows.Forms.MouseEventHandler(this.gamesListButton_MouseMove);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 46);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(147, 142);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 15;
            this.pictureBox1.TabStop = false;
            // 
            // vkBtn
            // 
            this.vkBtn.BackColor = System.Drawing.Color.Transparent;
            this.vkBtn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("vkBtn.BackgroundImage")));
            this.vkBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.vkBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.vkBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.vkBtn.ForeColor = System.Drawing.Color.Transparent;
            this.vkBtn.Location = new System.Drawing.Point(35, 194);
            this.vkBtn.Name = "vkBtn";
            this.vkBtn.Size = new System.Drawing.Size(44, 46);
            this.vkBtn.TabIndex = 16;
            this.vkBtn.UseVisualStyleBackColor = false;
            this.vkBtn.Click += new System.EventHandler(this.vkBtn_Click);
            this.vkBtn.MouseMove += new System.Windows.Forms.MouseEventHandler(this.vkBtn_MouseMove);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(668, 335);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 15);
            this.label2.TabIndex = 17;
            this.label2.Text = "Разработчик: ";
            // 
            // myVkLink
            // 
            this.myVkLink.AutoSize = true;
            this.myVkLink.Location = new System.Drawing.Point(746, 335);
            this.myVkLink.Name = "myVkLink";
            this.myVkLink.Size = new System.Drawing.Size(81, 15);
            this.myVkLink.TabIndex = 18;
            this.myVkLink.TabStop = true;
            this.myVkLink.Text = "Петя Лебедев";
            this.myVkLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.myVkLink_LinkClicked);
            this.myVkLink.MouseMove += new System.Windows.Forms.MouseEventHandler(this.myVkLink_MouseMove);
            // 
            // instructionLink
            // 
            this.instructionLink.AutoSize = true;
            this.instructionLink.Location = new System.Drawing.Point(12, 335);
            this.instructionLink.Name = "instructionLink";
            this.instructionLink.Size = new System.Drawing.Size(73, 15);
            this.instructionLink.TabIndex = 19;
            this.instructionLink.TabStop = true;
            this.instructionLink.Text = "Инструкция";
            this.instructionLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.instructionLink_LinkClicked);
            // 
            // versionLabel
            // 
            this.versionLabel.AutoSize = true;
            this.versionLabel.Location = new System.Drawing.Point(343, 329);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(121, 15);
            this.versionLabel.TabIndex = 20;
            this.versionLabel.Text = "Версия программы: ";
            // 
            // deleteTranslate
            // 
            this.deleteTranslate.Enabled = false;
            this.deleteTranslate.Location = new System.Drawing.Point(243, 253);
            this.deleteTranslate.Name = "deleteTranslate";
            this.deleteTranslate.Size = new System.Drawing.Size(117, 27);
            this.deleteTranslate.TabIndex = 21;
            this.deleteTranslate.Text = "Удалить";
            this.deleteTranslate.UseVisualStyleBackColor = true;
            this.deleteTranslate.Click += new System.EventHandler(this.deleteTranslate_Click);
            this.deleteTranslate.MouseMove += new System.Windows.Forms.MouseEventHandler(this.deleteTranslate_MouseMove);
            // 
            // fontSetup
            // 
            this.fontSetup.Enabled = false;
            this.fontSetup.Location = new System.Drawing.Point(214, 253);
            this.fontSetup.Name = "fontSetup";
            this.fontSetup.Size = new System.Drawing.Size(171, 27);
            this.fontSetup.TabIndex = 22;
            this.fontSetup.Text = "Установить шрифт";
            this.fontSetup.UseVisualStyleBackColor = true;
            this.fontSetup.Visible = false;
            this.fontSetup.Click += new System.EventHandler(this.fontSetup_Click);
            // 
            // UpdateButton
            // 
            this.UpdateButton.BackColor = System.Drawing.Color.Transparent;
            this.UpdateButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("UpdateButton.BackgroundImage")));
            this.UpdateButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.UpdateButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.UpdateButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.UpdateButton.ForeColor = System.Drawing.Color.Transparent;
            this.UpdateButton.Location = new System.Drawing.Point(85, 194);
            this.UpdateButton.Name = "UpdateButton";
            this.UpdateButton.Size = new System.Drawing.Size(47, 46);
            this.UpdateButton.TabIndex = 23;
            this.UpdateButton.UseVisualStyleBackColor = false;
            this.UpdateButton.Click += new System.EventHandler(this.UpdateButton_Click);
            this.UpdateButton.MouseMove += new System.Windows.Forms.MouseEventHandler(this.UpdateButton_MouseMove);
            // 
            // txtStatus
            // 
            this.txtStatus.BackColor = System.Drawing.SystemColors.Control;
            this.txtStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtStatus.Location = new System.Drawing.Point(170, 37);
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.ReadOnly = true;
            this.txtStatus.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.txtStatus.Size = new System.Drawing.Size(294, 168);
            this.txtStatus.TabIndex = 24;
            this.txtStatus.Text = "";
            // 
            // skillsNameCheckTranslate
            // 
            this.skillsNameCheckTranslate.Enabled = false;
            this.skillsNameCheckTranslate.Location = new System.Drawing.Point(485, 106);
            this.skillsNameCheckTranslate.Name = "skillsNameCheckTranslate";
            this.skillsNameCheckTranslate.Size = new System.Drawing.Size(156, 19);
            this.skillsNameCheckTranslate.TabIndex = 25;
            this.skillsNameCheckTranslate.Text = "Умения (название)";
            this.skillsNameCheckTranslate.UseVisualStyleBackColor = true;
            this.skillsNameCheckTranslate.CheckedChanged += new System.EventHandler(this.skillsNameCheckTranslate_CheckedChanged);
            // 
            // border
            // 
            this.border.BackColor = System.Drawing.Color.Transparent;
            this.border.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.border.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.border.Location = new System.Drawing.Point(470, 28);
            this.border.Name = "border";
            this.border.Size = new System.Drawing.Size(357, 240);
            this.border.TabIndex = 26;
            this.border.Click += new System.EventHandler(this.border_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(474, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 15);
            this.label4.TabIndex = 27;
            this.label4.Text = "Настройки";
            // 
            // border2
            // 
            this.border2.BackColor = System.Drawing.Color.Transparent;
            this.border2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.border2.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.border2.Location = new System.Drawing.Point(654, 71);
            this.border2.Name = "border2";
            this.border2.Size = new System.Drawing.Size(164, 64);
            this.border2.TabIndex = 28;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(658, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(118, 15);
            this.label3.TabIndex = 29;
            this.label3.Text = "Выбор переводчика";
            // 
            // skillsDescriptionCheckTranslate
            // 
            this.skillsDescriptionCheckTranslate.Enabled = false;
            this.skillsDescriptionCheckTranslate.Location = new System.Drawing.Point(485, 126);
            this.skillsDescriptionCheckTranslate.Name = "skillsDescriptionCheckTranslate";
            this.skillsDescriptionCheckTranslate.Size = new System.Drawing.Size(156, 19);
            this.skillsDescriptionCheckTranslate.TabIndex = 30;
            this.skillsDescriptionCheckTranslate.Text = "Умения (описание)";
            this.skillsDescriptionCheckTranslate.UseVisualStyleBackColor = true;
            this.skillsDescriptionCheckTranslate.CheckedChanged += new System.EventHandler(this.skillsDescriptionCheckTranslate_CheckedChanged);
            // 
            // itemsCheckTranslate
            // 
            this.itemsCheckTranslate.Enabled = false;
            this.itemsCheckTranslate.Location = new System.Drawing.Point(485, 145);
            this.itemsCheckTranslate.Name = "itemsCheckTranslate";
            this.itemsCheckTranslate.Size = new System.Drawing.Size(156, 38);
            this.itemsCheckTranslate.TabIndex = 31;
            this.itemsCheckTranslate.Text = "Предметы\r\n(название и описание)";
            this.itemsCheckTranslate.UseVisualStyleBackColor = true;
            this.itemsCheckTranslate.CheckedChanged += new System.EventHandler(this.itemsNameCheckTranslate_CheckedChanged);
            // 
            // npcCheckTranslate
            // 
            this.npcCheckTranslate.Enabled = false;
            this.npcCheckTranslate.Location = new System.Drawing.Point(485, 85);
            this.npcCheckTranslate.Name = "npcCheckTranslate";
            this.npcCheckTranslate.Size = new System.Drawing.Size(156, 19);
            this.npcCheckTranslate.TabIndex = 33;
            this.npcCheckTranslate.Text = "Имена NPC";
            this.npcCheckTranslate.UseVisualStyleBackColor = true;
            this.npcCheckTranslate.CheckedChanged += new System.EventHandler(this.npcCheckTranslate_CheckedChanged);
            // 
            // temtemCheckTranslate
            // 
            this.temtemCheckTranslate.Enabled = false;
            this.temtemCheckTranslate.Location = new System.Drawing.Point(485, 182);
            this.temtemCheckTranslate.Name = "temtemCheckTranslate";
            this.temtemCheckTranslate.Size = new System.Drawing.Size(156, 37);
            this.temtemCheckTranslate.TabIndex = 34;
            this.temtemCheckTranslate.Text = "Стандартные имена Темов (рекомендуется)";
            this.temtemCheckTranslate.UseVisualStyleBackColor = true;
            this.temtemCheckTranslate.Visible = false;
            this.temtemCheckTranslate.CheckedChanged += new System.EventHandler(this.temtemCheckTranslate_CheckedChanged);
            this.temtemCheckTranslate.MouseMove += new System.Windows.Forms.MouseEventHandler(this.temtemCheckTranslate_MouseMove);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(482, 63);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(117, 15);
            this.label5.TabIndex = 37;
            this.label5.Text = "Отключить перевод";
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label6.Location = new System.Drawing.Point(478, 71);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(172, 152);
            this.label6.TabIndex = 36;
            // 
            // deeplButton
            // 
            this.deeplButton.Enabled = false;
            this.deeplButton.Location = new System.Drawing.Point(659, 81);
            this.deeplButton.Name = "deeplButton";
            this.deeplButton.Size = new System.Drawing.Size(154, 26);
            this.deeplButton.TabIndex = 38;
            this.deeplButton.Text = "Deepl (рекомендуется)";
            this.deeplButton.UseVisualStyleBackColor = true;
            this.deeplButton.CheckedChanged += new System.EventHandler(this.deeplButton_CheckedChanged);
            // 
            // googleButton
            // 
            this.googleButton.AutoSize = true;
            this.googleButton.Enabled = false;
            this.googleButton.Location = new System.Drawing.Point(668, 164);
            this.googleButton.Name = "googleButton";
            this.googleButton.Size = new System.Drawing.Size(131, 19);
            this.googleButton.TabIndex = 39;
            this.googleButton.Text = "Google переводчик";
            this.googleButton.UseVisualStyleBackColor = true;
            this.googleButton.Visible = false;
            this.googleButton.CheckedChanged += new System.EventHandler(this.googleButton_CheckedChanged);
            // 
            // yandexButton
            // 
            this.yandexButton.AutoSize = true;
            this.yandexButton.Checked = true;
            this.yandexButton.Location = new System.Drawing.Point(659, 105);
            this.yandexButton.Name = "yandexButton";
            this.yandexButton.Size = new System.Drawing.Size(131, 19);
            this.yandexButton.TabIndex = 40;
            this.yandexButton.TabStop = true;
            this.yandexButton.Text = "Yandex переводчик";
            this.yandexButton.UseVisualStyleBackColor = true;
            this.yandexButton.CheckedChanged += new System.EventHandler(this.yandexButton_CheckedChanged);
            // 
            // donateButton
            // 
            this.donateButton.AccessibleDescription = "";
            this.donateButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.donateButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.donateButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.donateButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.donateButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.donateButton.ForeColor = System.Drawing.SystemColors.Control;
            this.donateButton.Location = new System.Drawing.Point(40, 244);
            this.donateButton.Name = "donateButton";
            this.donateButton.Size = new System.Drawing.Size(87, 22);
            this.donateButton.TabIndex = 41;
            this.donateButton.Text = "Поддержать";
            this.donateButton.UseVisualStyleBackColor = true;
            this.donateButton.Click += new System.EventHandler(this.donateButton_Click);
            this.donateButton.MouseMove += new System.Windows.Forms.MouseEventHandler(this.donateButton_MouseMove);
            // 
            // machineNameText
            // 
            this.machineNameText.Enabled = false;
            this.machineNameText.Location = new System.Drawing.Point(12, 13);
            this.machineNameText.Name = "machineNameText";
            this.machineNameText.Size = new System.Drawing.Size(147, 23);
            this.machineNameText.TabIndex = 42;
            this.machineNameText.Visible = false;
            this.machineNameText.TextChanged += new System.EventHandler(this.machineNameText_TextChanged);
            // 
            // itemsDesc
            // 
            this.itemsDesc.Enabled = false;
            this.itemsDesc.Location = new System.Drawing.Point(485, 181);
            this.itemsDesc.Name = "itemsDesc";
            this.itemsDesc.Size = new System.Drawing.Size(156, 38);
            this.itemsDesc.TabIndex = 43;
            this.itemsDesc.Text = "Предметы (описание)";
            this.itemsDesc.UseVisualStyleBackColor = true;
            this.itemsDesc.Visible = false;
            this.itemsDesc.CheckedChanged += new System.EventHandler(this.itemsDesc_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(536, 234);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 44;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(839, 353);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.itemsDesc);
            this.Controls.Add(this.machineNameText);
            this.Controls.Add(this.donateButton);
            this.Controls.Add(this.yandexButton);
            this.Controls.Add(this.googleButton);
            this.Controls.Add(this.deeplButton);
            this.Controls.Add(this.temtemCheckTranslate);
            this.Controls.Add(this.npcCheckTranslate);
            this.Controls.Add(this.itemsCheckTranslate);
            this.Controls.Add(this.skillsDescriptionCheckTranslate);
            this.Controls.Add(this.skillsNameCheckTranslate);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.gamesListButton);
            this.Controls.Add(this.txtStatus);
            this.Controls.Add(this.UpdateButton);
            this.Controls.Add(this.deleteTranslate);
            this.Controls.Add(this.versionLabel);
            this.Controls.Add(this.instructionLink);
            this.Controls.Add(this.myVkLink);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.vkBtn);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.gameNameTxt);
            this.Controls.Add(this.recoverBD);
            this.Controls.Add(this.updateReinstall);
            this.Controls.Add(this.choosePathBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.lblPercent);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.fontSetup);
            this.Controls.Add(this.border2);
            this.Controls.Add(this.border);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "UTranslator by GooDDarK";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lblPercent;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button choosePathBtn;
        private System.Windows.Forms.Button updateReinstall;
        private System.Windows.Forms.Button recoverBD;
        private System.Windows.Forms.Label gameNameTxt;
        private System.Windows.Forms.Button gamesListButton;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button vkBtn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel myVkLink;
        private System.Windows.Forms.LinkLabel instructionLink;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.Button deleteTranslate;
        private System.Windows.Forms.Button fontSetup;
        private System.Windows.Forms.Button UpdateButton;
        private System.Windows.Forms.RichTextBox txtStatus;
        private System.Windows.Forms.CheckBox skillsNameCheckTranslate;
        private System.Windows.Forms.Label border;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label border2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox skillsDescriptionCheckTranslate;
        private System.Windows.Forms.CheckBox itemsCheckTranslate;
        private System.Windows.Forms.CheckBox npcCheckTranslate;
        private System.Windows.Forms.CheckBox temtemCheckTranslate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.RadioButton deeplButton;
        private System.Windows.Forms.RadioButton googleButton;
        private System.Windows.Forms.RadioButton yandexButton;
        private System.Windows.Forms.Button donateButton;
        private System.Windows.Forms.TextBox machineNameText;
        private System.Windows.Forms.CheckBox itemsDesc;
        private System.Windows.Forms.Button button1;
    }
}

