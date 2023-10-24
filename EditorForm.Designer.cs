namespace UTranslator
{
    partial class EditorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditorForm));
            dataGridView1 = new System.Windows.Forms.DataGridView();
            OriginalText = new System.Windows.Forms.DataGridViewTextBoxColumn();
            HandTranslate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            YandexTranslate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DeeplTranslate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            button1 = new System.Windows.Forms.Button();
            button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { OriginalText, HandTranslate, YandexTranslate, DeeplTranslate });
            dataGridView1.Location = new System.Drawing.Point(-1, 106);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            dataGridView1.RowTemplate.Height = 25;
            dataGridView1.Size = new System.Drawing.Size(1216, 703);
            dataGridView1.TabIndex = 0;
            // 
            // OriginalText
            // 
            OriginalText.FillWeight = 93.4945755F;
            OriginalText.HeaderText = "Английский текст";
            OriginalText.Name = "OriginalText";
            // 
            // HandTranslate
            // 
            HandTranslate.FillWeight = 102.573692F;
            HandTranslate.HeaderText = "Ручной перевод";
            HandTranslate.Name = "HandTranslate";
            // 
            // YandexTranslate
            // 
            YandexTranslate.FillWeight = 104.017F;
            YandexTranslate.HeaderText = "Yandex перевод";
            YandexTranslate.Name = "YandexTranslate";
            // 
            // DeeplTranslate
            // 
            DeeplTranslate.FillWeight = 99.91476F;
            DeeplTranslate.HeaderText = "Deepl перевод";
            DeeplTranslate.Name = "DeeplTranslate";
            // 
            // button1
            // 
            button1.Location = new System.Drawing.Point(170, 46);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(75, 23);
            button1.TabIndex = 1;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new System.Drawing.Point(319, 51);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(75, 23);
            button2.TabIndex = 2;
            button2.Text = "button2";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // EditorForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1212, 807);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(dataGridView1);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Name = "EditorForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "UTranslator - Редактор перевода";
            WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn OriginalText;
        private System.Windows.Forms.DataGridViewTextBoxColumn HandTranslate;
        private System.Windows.Forms.DataGridViewTextBoxColumn YandexTranslate;
        private System.Windows.Forms.DataGridViewTextBoxColumn DeeplTranslate;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}