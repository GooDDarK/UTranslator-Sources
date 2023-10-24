
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
            btnStart = new System.Windows.Forms.Button();
            progressBar = new System.Windows.Forms.ProgressBar();
            lblPercent = new System.Windows.Forms.Label();
            txtPath = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            choosePathBtn = new System.Windows.Forms.Button();
            updateReinstall = new System.Windows.Forms.Button();
            recoverBD = new System.Windows.Forms.Button();
            gameNameTxt = new System.Windows.Forms.Label();
            gamesListButton = new System.Windows.Forms.Button();
            pictureBox1 = new System.Windows.Forms.PictureBox();
            vkBtn = new System.Windows.Forms.Button();
            label2 = new System.Windows.Forms.Label();
            myVkLink = new System.Windows.Forms.LinkLabel();
            instructionLink = new System.Windows.Forms.LinkLabel();
            versionLabel = new System.Windows.Forms.Label();
            deleteTranslate = new System.Windows.Forms.Button();
            fontSetup = new System.Windows.Forms.Button();
            UpdateButton = new System.Windows.Forms.Button();
            txtStatus = new System.Windows.Forms.RichTextBox();
            skillsNameCheckTranslate = new System.Windows.Forms.CheckBox();
            border = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            border2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            skillsDescriptionCheckTranslate = new System.Windows.Forms.CheckBox();
            itemsCheckTranslate = new System.Windows.Forms.CheckBox();
            npcCheckTranslate = new System.Windows.Forms.CheckBox();
            temtemCheckTranslate = new System.Windows.Forms.CheckBox();
            label5 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            deeplButton = new System.Windows.Forms.RadioButton();
            googleButton = new System.Windows.Forms.RadioButton();
            yandexButton = new System.Windows.Forms.RadioButton();
            donateButton = new System.Windows.Forms.Button();
            machineNameText = new System.Windows.Forms.TextBox();
            itemsDesc = new System.Windows.Forms.CheckBox();
            button1 = new System.Windows.Forms.Button();
            TranslateEditorBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // btnStart
            // 
            btnStart.Location = new System.Drawing.Point(170, 211);
            btnStart.Name = "btnStart";
            btnStart.Size = new System.Drawing.Size(117, 36);
            btnStart.TabIndex = 0;
            btnStart.Text = "&Установить";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            btnStart.MouseMove += btnStart_MouseMove;
            // 
            // progressBar
            // 
            progressBar.Location = new System.Drawing.Point(12, 296);
            progressBar.Name = "progressBar";
            progressBar.Size = new System.Drawing.Size(815, 23);
            progressBar.Step = 1;
            progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            progressBar.TabIndex = 2;
            // 
            // lblPercent
            // 
            lblPercent.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            lblPercent.AutoSize = true;
            lblPercent.Location = new System.Drawing.Point(795, 278);
            lblPercent.Name = "lblPercent";
            lblPercent.RightToLeft = System.Windows.Forms.RightToLeft.No;
            lblPercent.Size = new System.Drawing.Size(23, 15);
            lblPercent.TabIndex = 6;
            lblPercent.Text = "0%";
            lblPercent.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // txtPath
            // 
            txtPath.BackColor = System.Drawing.SystemColors.Control;
            txtPath.Location = new System.Drawing.Point(564, 35);
            txtPath.Name = "txtPath";
            txtPath.Size = new System.Drawing.Size(200, 23);
            txtPath.TabIndex = 7;
            txtPath.TextChanged += txtPath_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(485, 39);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(73, 15);
            label1.TabIndex = 8;
            label1.Text = "Путь к игре:";
            // 
            // choosePathBtn
            // 
            choosePathBtn.Location = new System.Drawing.Point(770, 37);
            choosePathBtn.Name = "choosePathBtn";
            choosePathBtn.Size = new System.Drawing.Size(31, 20);
            choosePathBtn.TabIndex = 9;
            choosePathBtn.Text = "...";
            choosePathBtn.UseVisualStyleBackColor = true;
            choosePathBtn.Click += choosePathBtn_Click;
            choosePathBtn.MouseMove += choosePathBtn_MouseMove;
            // 
            // updateReinstall
            // 
            updateReinstall.Enabled = false;
            updateReinstall.Location = new System.Drawing.Point(293, 211);
            updateReinstall.Name = "updateReinstall";
            updateReinstall.Size = new System.Drawing.Size(171, 36);
            updateReinstall.TabIndex = 10;
            updateReinstall.Text = "Обновить";
            updateReinstall.UseVisualStyleBackColor = true;
            updateReinstall.Click += updateReinstall_Click;
            updateReinstall.MouseMove += updateReinstall_MouseMove;
            // 
            // recoverBD
            // 
            recoverBD.Enabled = false;
            recoverBD.Location = new System.Drawing.Point(809, 322);
            recoverBD.Name = "recoverBD";
            recoverBD.Size = new System.Drawing.Size(17, 10);
            recoverBD.TabIndex = 11;
            recoverBD.Text = "Восстановить БД";
            recoverBD.UseVisualStyleBackColor = true;
            recoverBD.Visible = false;
            recoverBD.Click += recoverBD_Click;
            // 
            // gameNameTxt
            // 
            gameNameTxt.AutoSize = true;
            gameNameTxt.BackColor = System.Drawing.SystemColors.Window;
            gameNameTxt.ForeColor = System.Drawing.Color.ForestGreen;
            gameNameTxt.Location = new System.Drawing.Point(188, 16);
            gameNameTxt.Name = "gameNameTxt";
            gameNameTxt.Size = new System.Drawing.Size(0, 15);
            gameNameTxt.TabIndex = 13;
            // 
            // gamesListButton
            // 
            gamesListButton.Location = new System.Drawing.Point(656, 171);
            gamesListButton.Name = "gamesListButton";
            gamesListButton.Size = new System.Drawing.Size(164, 52);
            gamesListButton.TabIndex = 14;
            gamesListButton.Text = "Список поддерживаемых для русификации игр";
            gamesListButton.UseVisualStyleBackColor = true;
            gamesListButton.Click += gamesListButton_Click;
            gamesListButton.MouseMove += gamesListButton_MouseMove;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (System.Drawing.Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new System.Drawing.Point(12, 46);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new System.Drawing.Size(147, 142);
            pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 15;
            pictureBox1.TabStop = false;
            // 
            // vkBtn
            // 
            vkBtn.BackColor = System.Drawing.Color.Transparent;
            vkBtn.BackgroundImage = (System.Drawing.Image)resources.GetObject("vkBtn.BackgroundImage");
            vkBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            vkBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            vkBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            vkBtn.ForeColor = System.Drawing.Color.Transparent;
            vkBtn.Location = new System.Drawing.Point(35, 194);
            vkBtn.Name = "vkBtn";
            vkBtn.Size = new System.Drawing.Size(44, 46);
            vkBtn.TabIndex = 16;
            vkBtn.UseVisualStyleBackColor = false;
            vkBtn.Click += vkBtn_Click;
            vkBtn.MouseMove += vkBtn_MouseMove;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(668, 335);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(83, 15);
            label2.TabIndex = 17;
            label2.Text = "Разработчик: ";
            // 
            // myVkLink
            // 
            myVkLink.AutoSize = true;
            myVkLink.Location = new System.Drawing.Point(746, 335);
            myVkLink.Name = "myVkLink";
            myVkLink.Size = new System.Drawing.Size(81, 15);
            myVkLink.TabIndex = 18;
            myVkLink.TabStop = true;
            myVkLink.Text = "Петя Лебедев";
            myVkLink.LinkClicked += myVkLink_LinkClicked;
            myVkLink.MouseMove += myVkLink_MouseMove;
            // 
            // instructionLink
            // 
            instructionLink.AutoSize = true;
            instructionLink.Location = new System.Drawing.Point(12, 335);
            instructionLink.Name = "instructionLink";
            instructionLink.Size = new System.Drawing.Size(73, 15);
            instructionLink.TabIndex = 19;
            instructionLink.TabStop = true;
            instructionLink.Text = "Инструкция";
            instructionLink.LinkClicked += instructionLink_LinkClicked;
            // 
            // versionLabel
            // 
            versionLabel.AutoSize = true;
            versionLabel.Location = new System.Drawing.Point(343, 329);
            versionLabel.Name = "versionLabel";
            versionLabel.Size = new System.Drawing.Size(121, 15);
            versionLabel.TabIndex = 20;
            versionLabel.Text = "Версия программы: ";
            // 
            // deleteTranslate
            // 
            deleteTranslate.Enabled = false;
            deleteTranslate.Location = new System.Drawing.Point(243, 253);
            deleteTranslate.Name = "deleteTranslate";
            deleteTranslate.Size = new System.Drawing.Size(117, 27);
            deleteTranslate.TabIndex = 21;
            deleteTranslate.Text = "Удалить";
            deleteTranslate.UseVisualStyleBackColor = true;
            deleteTranslate.Click += deleteTranslate_Click;
            deleteTranslate.MouseMove += deleteTranslate_MouseMove;
            // 
            // fontSetup
            // 
            fontSetup.Enabled = false;
            fontSetup.Location = new System.Drawing.Point(214, 253);
            fontSetup.Name = "fontSetup";
            fontSetup.Size = new System.Drawing.Size(171, 27);
            fontSetup.TabIndex = 22;
            fontSetup.Text = "Установить шрифт";
            fontSetup.UseVisualStyleBackColor = true;
            fontSetup.Visible = false;
            fontSetup.Click += fontSetup_Click;
            // 
            // UpdateButton
            // 
            UpdateButton.BackColor = System.Drawing.Color.Transparent;
            UpdateButton.BackgroundImage = (System.Drawing.Image)resources.GetObject("UpdateButton.BackgroundImage");
            UpdateButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            UpdateButton.Cursor = System.Windows.Forms.Cursors.Hand;
            UpdateButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            UpdateButton.ForeColor = System.Drawing.Color.Transparent;
            UpdateButton.Location = new System.Drawing.Point(85, 194);
            UpdateButton.Name = "UpdateButton";
            UpdateButton.Size = new System.Drawing.Size(47, 46);
            UpdateButton.TabIndex = 23;
            UpdateButton.UseVisualStyleBackColor = false;
            UpdateButton.Click += UpdateButton_Click;
            UpdateButton.MouseMove += UpdateButton_MouseMove;
            // 
            // txtStatus
            // 
            txtStatus.BackColor = System.Drawing.SystemColors.Control;
            txtStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtStatus.Location = new System.Drawing.Point(170, 37);
            txtStatus.Name = "txtStatus";
            txtStatus.ReadOnly = true;
            txtStatus.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            txtStatus.Size = new System.Drawing.Size(294, 168);
            txtStatus.TabIndex = 24;
            txtStatus.Text = "";
            // 
            // skillsNameCheckTranslate
            // 
            skillsNameCheckTranslate.Enabled = false;
            skillsNameCheckTranslate.Location = new System.Drawing.Point(485, 106);
            skillsNameCheckTranslate.Name = "skillsNameCheckTranslate";
            skillsNameCheckTranslate.Size = new System.Drawing.Size(156, 19);
            skillsNameCheckTranslate.TabIndex = 25;
            skillsNameCheckTranslate.Text = "Умения (название)";
            skillsNameCheckTranslate.UseVisualStyleBackColor = true;
            skillsNameCheckTranslate.CheckedChanged += skillsNameCheckTranslate_CheckedChanged;
            // 
            // border
            // 
            border.BackColor = System.Drawing.Color.Transparent;
            border.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            border.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            border.Location = new System.Drawing.Point(470, 28);
            border.Name = "border";
            border.Size = new System.Drawing.Size(357, 240);
            border.TabIndex = 26;
            border.Click += border_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(474, 20);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(67, 15);
            label4.TabIndex = 27;
            label4.Text = "Настройки";
            // 
            // border2
            // 
            border2.BackColor = System.Drawing.Color.Transparent;
            border2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            border2.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            border2.Location = new System.Drawing.Point(654, 71);
            border2.Name = "border2";
            border2.Size = new System.Drawing.Size(164, 64);
            border2.TabIndex = 28;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(658, 63);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(118, 15);
            label3.TabIndex = 29;
            label3.Text = "Выбор переводчика";
            // 
            // skillsDescriptionCheckTranslate
            // 
            skillsDescriptionCheckTranslate.Enabled = false;
            skillsDescriptionCheckTranslate.Location = new System.Drawing.Point(485, 126);
            skillsDescriptionCheckTranslate.Name = "skillsDescriptionCheckTranslate";
            skillsDescriptionCheckTranslate.Size = new System.Drawing.Size(156, 19);
            skillsDescriptionCheckTranslate.TabIndex = 30;
            skillsDescriptionCheckTranslate.Text = "Умения (описание)";
            skillsDescriptionCheckTranslate.UseVisualStyleBackColor = true;
            skillsDescriptionCheckTranslate.CheckedChanged += skillsDescriptionCheckTranslate_CheckedChanged;
            // 
            // itemsCheckTranslate
            // 
            itemsCheckTranslate.Enabled = false;
            itemsCheckTranslate.Location = new System.Drawing.Point(485, 145);
            itemsCheckTranslate.Name = "itemsCheckTranslate";
            itemsCheckTranslate.Size = new System.Drawing.Size(156, 38);
            itemsCheckTranslate.TabIndex = 31;
            itemsCheckTranslate.Text = "Предметы\r\n(название и описание)";
            itemsCheckTranslate.UseVisualStyleBackColor = true;
            itemsCheckTranslate.CheckedChanged += itemsNameCheckTranslate_CheckedChanged;
            // 
            // npcCheckTranslate
            // 
            npcCheckTranslate.Enabled = false;
            npcCheckTranslate.Location = new System.Drawing.Point(485, 85);
            npcCheckTranslate.Name = "npcCheckTranslate";
            npcCheckTranslate.Size = new System.Drawing.Size(156, 19);
            npcCheckTranslate.TabIndex = 33;
            npcCheckTranslate.Text = "Имена NPC";
            npcCheckTranslate.UseVisualStyleBackColor = true;
            npcCheckTranslate.CheckedChanged += npcCheckTranslate_CheckedChanged;
            // 
            // temtemCheckTranslate
            // 
            temtemCheckTranslate.Enabled = false;
            temtemCheckTranslate.Location = new System.Drawing.Point(485, 182);
            temtemCheckTranslate.Name = "temtemCheckTranslate";
            temtemCheckTranslate.Size = new System.Drawing.Size(156, 37);
            temtemCheckTranslate.TabIndex = 34;
            temtemCheckTranslate.Text = "Стандартные имена Темов (рекомендуется)";
            temtemCheckTranslate.UseVisualStyleBackColor = true;
            temtemCheckTranslate.Visible = false;
            temtemCheckTranslate.CheckedChanged += temtemCheckTranslate_CheckedChanged;
            temtemCheckTranslate.MouseMove += temtemCheckTranslate_MouseMove;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(482, 63);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(117, 15);
            label5.TabIndex = 37;
            label5.Text = "Отключить перевод";
            // 
            // label6
            // 
            label6.BackColor = System.Drawing.Color.Transparent;
            label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            label6.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            label6.Location = new System.Drawing.Point(478, 71);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(172, 152);
            label6.TabIndex = 36;
            // 
            // deeplButton
            // 
            deeplButton.Enabled = false;
            deeplButton.Location = new System.Drawing.Point(659, 81);
            deeplButton.Name = "deeplButton";
            deeplButton.Size = new System.Drawing.Size(154, 26);
            deeplButton.TabIndex = 38;
            deeplButton.Text = "Deepl (рекомендуется)";
            deeplButton.UseVisualStyleBackColor = true;
            deeplButton.CheckedChanged += deeplButton_CheckedChanged;
            // 
            // googleButton
            // 
            googleButton.AutoSize = true;
            googleButton.Enabled = false;
            googleButton.Location = new System.Drawing.Point(668, 138);
            googleButton.Name = "googleButton";
            googleButton.Size = new System.Drawing.Size(131, 19);
            googleButton.TabIndex = 39;
            googleButton.Text = "Google переводчик";
            googleButton.UseVisualStyleBackColor = true;
            googleButton.Visible = false;
            googleButton.CheckedChanged += googleButton_CheckedChanged;
            // 
            // yandexButton
            // 
            yandexButton.AutoSize = true;
            yandexButton.Checked = true;
            yandexButton.Location = new System.Drawing.Point(659, 105);
            yandexButton.Name = "yandexButton";
            yandexButton.Size = new System.Drawing.Size(131, 19);
            yandexButton.TabIndex = 40;
            yandexButton.TabStop = true;
            yandexButton.Text = "Yandex переводчик";
            yandexButton.UseVisualStyleBackColor = true;
            yandexButton.CheckedChanged += yandexButton_CheckedChanged;
            // 
            // donateButton
            // 
            donateButton.AccessibleDescription = "";
            donateButton.BackColor = System.Drawing.Color.FromArgb(255, 128, 0);
            donateButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            donateButton.Cursor = System.Windows.Forms.Cursors.Hand;
            donateButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            donateButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            donateButton.ForeColor = System.Drawing.SystemColors.Control;
            donateButton.Location = new System.Drawing.Point(40, 244);
            donateButton.Name = "donateButton";
            donateButton.Size = new System.Drawing.Size(87, 22);
            donateButton.TabIndex = 41;
            donateButton.Text = "Поддержать";
            donateButton.UseVisualStyleBackColor = true;
            donateButton.Click += donateButton_Click;
            donateButton.MouseMove += donateButton_MouseMove;
            // 
            // machineNameText
            // 
            machineNameText.Enabled = false;
            machineNameText.Location = new System.Drawing.Point(12, 13);
            machineNameText.Name = "machineNameText";
            machineNameText.Size = new System.Drawing.Size(147, 23);
            machineNameText.TabIndex = 42;
            machineNameText.Visible = false;
            machineNameText.TextChanged += machineNameText_TextChanged;
            // 
            // itemsDesc
            // 
            itemsDesc.Enabled = false;
            itemsDesc.Location = new System.Drawing.Point(485, 181);
            itemsDesc.Name = "itemsDesc";
            itemsDesc.Size = new System.Drawing.Size(156, 38);
            itemsDesc.TabIndex = 43;
            itemsDesc.Text = "Предметы (описание)";
            itemsDesc.UseVisualStyleBackColor = true;
            itemsDesc.Visible = false;
            itemsDesc.CheckedChanged += itemsDesc_CheckedChanged;
            // 
            // button1
            // 
            button1.Location = new System.Drawing.Point(536, 234);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(75, 23);
            button1.TabIndex = 44;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // TranslateEditorBtn
            // 
            TranslateEditorBtn.Location = new System.Drawing.Point(656, 232);
            TranslateEditorBtn.Name = "TranslateEditorBtn";
            TranslateEditorBtn.Size = new System.Drawing.Size(164, 27);
            TranslateEditorBtn.TabIndex = 45;
            TranslateEditorBtn.Text = "Редактор перевода";
            TranslateEditorBtn.UseVisualStyleBackColor = true;
            TranslateEditorBtn.Click += TranslateEditorBtn_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoSize = true;
            AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            BackColor = System.Drawing.SystemColors.Window;
            ClientSize = new System.Drawing.Size(839, 353);
            Controls.Add(TranslateEditorBtn);
            Controls.Add(button1);
            Controls.Add(itemsDesc);
            Controls.Add(machineNameText);
            Controls.Add(donateButton);
            Controls.Add(yandexButton);
            Controls.Add(googleButton);
            Controls.Add(deeplButton);
            Controls.Add(temtemCheckTranslate);
            Controls.Add(npcCheckTranslate);
            Controls.Add(itemsCheckTranslate);
            Controls.Add(skillsDescriptionCheckTranslate);
            Controls.Add(skillsNameCheckTranslate);
            Controls.Add(label5);
            Controls.Add(label6);
            Controls.Add(label3);
            Controls.Add(label4);
            Controls.Add(gamesListButton);
            Controls.Add(txtStatus);
            Controls.Add(UpdateButton);
            Controls.Add(deleteTranslate);
            Controls.Add(versionLabel);
            Controls.Add(instructionLink);
            Controls.Add(myVkLink);
            Controls.Add(label2);
            Controls.Add(vkBtn);
            Controls.Add(pictureBox1);
            Controls.Add(gameNameTxt);
            Controls.Add(recoverBD);
            Controls.Add(updateReinstall);
            Controls.Add(choosePathBtn);
            Controls.Add(label1);
            Controls.Add(txtPath);
            Controls.Add(lblPercent);
            Controls.Add(progressBar);
            Controls.Add(btnStart);
            Controls.Add(fontSetup);
            Controls.Add(border2);
            Controls.Add(border);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "Form1";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "UTranslator by GooDDarK";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
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
        private System.Windows.Forms.Button TranslateEditorBtn;
    }
}

