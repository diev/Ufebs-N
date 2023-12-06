namespace ToKBR.Forms
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
            groupZK = new GroupBox();
            textFileZKok = new TextBox();
            buttonAddZK = new Button();
            buttonFileZK = new Button();
            textFileZK = new TextBox();
            groupKA = new GroupBox();
            buttonFileSend = new Button();
            textFileSend = new TextBox();
            textFileKAok = new TextBox();
            buttonSend = new Button();
            buttonAddKA = new Button();
            buttonFileKA = new Button();
            textFileKA = new TextBox();
            openFileZK = new OpenFileDialog();
            openFileKA = new OpenFileDialog();
            openFileSend = new OpenFileDialog();
            groupZK.SuspendLayout();
            groupKA.SuspendLayout();
            SuspendLayout();
            // 
            // groupZK
            // 
            groupZK.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupZK.Controls.Add(textFileZKok);
            groupZK.Controls.Add(buttonAddZK);
            groupZK.Controls.Add(buttonFileZK);
            groupZK.Controls.Add(textFileZK);
            groupZK.Location = new Point(10, 7);
            groupZK.Name = "groupZK";
            groupZK.Size = new Size(839, 90);
            groupZK.TabIndex = 0;
            groupZK.TabStop = false;
            groupZK.Text = "Операционист:";
            // 
            // textFileZKok
            // 
            textFileZKok.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textFileZKok.Location = new Point(206, 52);
            textFileZKok.MaxLength = 255;
            textFileZKok.Name = "textFileZKok";
            textFileZKok.ReadOnly = true;
            textFileZKok.Size = new Size(494, 23);
            textFileZKok.TabIndex = 6;
            textFileZKok.TabStop = false;
            textFileZKok.WordWrap = false;
            // 
            // buttonAddZK
            // 
            buttonAddZK.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonAddZK.AutoSize = true;
            buttonAddZK.Enabled = false;
            buttonAddZK.Location = new Point(709, 23);
            buttonAddZK.Name = "buttonAddZK";
            buttonAddZK.Size = new Size(93, 25);
            buttonAddZK.TabIndex = 2;
            buttonAddZK.Text = "Подписать ЗК";
            buttonAddZK.UseVisualStyleBackColor = true;
            buttonAddZK.Click += ButtonAddZK_Click;
            // 
            // buttonFileZK
            // 
            buttonFileZK.AutoSize = true;
            buttonFileZK.Location = new Point(6, 23);
            buttonFileZK.Name = "buttonFileZK";
            buttonFileZK.Size = new Size(194, 25);
            buttonFileZK.TabIndex = 1;
            buttonFileZK.Text = "Выбрать файл для подписи ЗК...";
            buttonFileZK.UseVisualStyleBackColor = true;
            buttonFileZK.Click += ButtonFileZK_Click;
            // 
            // textFileZK
            // 
            textFileZK.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textFileZK.Location = new Point(206, 23);
            textFileZK.MaxLength = 255;
            textFileZK.Name = "textFileZK";
            textFileZK.PlaceholderText = "Выберите файл ED*.xml";
            textFileZK.ReadOnly = true;
            textFileZK.Size = new Size(494, 23);
            textFileZK.TabIndex = 1;
            textFileZK.TabStop = false;
            textFileZK.WordWrap = false;
            // 
            // groupKA
            // 
            groupKA.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupKA.Controls.Add(buttonFileSend);
            groupKA.Controls.Add(textFileSend);
            groupKA.Controls.Add(textFileKAok);
            groupKA.Controls.Add(buttonSend);
            groupKA.Controls.Add(buttonAddKA);
            groupKA.Controls.Add(buttonFileKA);
            groupKA.Controls.Add(textFileKA);
            groupKA.Location = new Point(10, 103);
            groupKA.Name = "groupKA";
            groupKA.Size = new Size(839, 122);
            groupKA.TabIndex = 3;
            groupKA.TabStop = false;
            groupKA.Text = "Контролер:";
            // 
            // buttonFileSend
            // 
            buttonFileSend.AutoSize = true;
            buttonFileSend.Location = new Point(6, 50);
            buttonFileSend.Name = "buttonFileSend";
            buttonFileSend.Size = new Size(195, 25);
            buttonFileSend.TabIndex = 9;
            buttonFileSend.Text = "Выбрать файл для отправки...";
            buttonFileSend.UseVisualStyleBackColor = true;
            buttonFileSend.Click += ButtonFileSend_Click;
            // 
            // textFileSend
            // 
            textFileSend.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textFileSend.Location = new Point(206, 82);
            textFileSend.MaxLength = 255;
            textFileSend.Name = "textFileSend";
            textFileSend.ReadOnly = true;
            textFileSend.Size = new Size(497, 23);
            textFileSend.TabIndex = 8;
            textFileSend.TabStop = false;
            textFileSend.WordWrap = false;
            // 
            // textFileKAok
            // 
            textFileKAok.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textFileKAok.Location = new Point(206, 52);
            textFileKAok.MaxLength = 255;
            textFileKAok.Name = "textFileKAok";
            textFileKAok.PlaceholderText = "Выберите файл ED*.zk.ka.xml";
            textFileKAok.ReadOnly = true;
            textFileKAok.Size = new Size(497, 23);
            textFileKAok.TabIndex = 7;
            textFileKAok.TabStop = false;
            textFileKAok.WordWrap = false;
            // 
            // buttonSend
            // 
            buttonSend.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonSend.AutoSize = true;
            buttonSend.Enabled = false;
            buttonSend.Location = new Point(709, 51);
            buttonSend.Name = "buttonSend";
            buttonSend.Size = new Size(108, 25);
            buttonSend.TabIndex = 6;
            buttonSend.Text = "Отправить в КБР";
            buttonSend.UseVisualStyleBackColor = true;
            buttonSend.Click += ButtonSend_Click;
            // 
            // buttonAddKA
            // 
            buttonAddKA.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonAddKA.AutoSize = true;
            buttonAddKA.Enabled = false;
            buttonAddKA.Location = new Point(709, 22);
            buttonAddKA.Name = "buttonAddKA";
            buttonAddKA.Size = new Size(94, 25);
            buttonAddKA.TabIndex = 5;
            buttonAddKA.Text = "Подписать КА";
            buttonAddKA.UseVisualStyleBackColor = true;
            buttonAddKA.Click += ButtonAddKA_Click;
            // 
            // buttonFileKA
            // 
            buttonFileKA.AutoSize = true;
            buttonFileKA.Location = new Point(6, 23);
            buttonFileKA.Name = "buttonFileKA";
            buttonFileKA.Size = new Size(195, 25);
            buttonFileKA.TabIndex = 4;
            buttonFileKA.Text = "Выбрать файл для подписи КА...";
            buttonFileKA.UseVisualStyleBackColor = true;
            buttonFileKA.Click += ButtonFileKA_Click;
            // 
            // textFileKA
            // 
            textFileKA.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textFileKA.Location = new Point(206, 23);
            textFileKA.MaxLength = 255;
            textFileKA.Name = "textFileKA";
            textFileKA.PlaceholderText = "Выберите файл ED*.zk.xml";
            textFileKA.ReadOnly = true;
            textFileKA.Size = new Size(497, 23);
            textFileKA.TabIndex = 1;
            textFileKA.TabStop = false;
            textFileKA.WordWrap = false;
            // 
            // openFileZK
            // 
            openFileZK.DefaultExt = "xml";
            openFileZK.Filter = "ED.xml|*.xml|Все файлы|*.*";
            openFileZK.SupportMultiDottedExtensions = true;
            openFileZK.Title = "Выберите файл для подписи ЗК";
            // 
            // openFileKA
            // 
            openFileKA.DefaultExt = "zk.xml";
            openFileKA.Filter = "ED.zk.xml|*.zk.xml|Все файлы|*.*";
            openFileKA.SupportMultiDottedExtensions = true;
            openFileKA.Title = "Выберите файл для подписи КА";
            // 
            // openFileSend
            // 
            openFileSend.DefaultExt = "zk.xml";
            openFileSend.Filter = "ED.zk.ka.xml|*.zk.ka.xml|Все файлы|*.*";
            openFileSend.SupportMultiDottedExtensions = true;
            openFileSend.Title = "Выберите файл для отправки";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(861, 237);
            Controls.Add(groupKA);
            Controls.Add(groupZK);
            Name = "Form1";
            Text = "Подписать ЗК, КА и отправить в КБР";
            groupZK.ResumeLayout(false);
            groupZK.PerformLayout();
            groupKA.ResumeLayout(false);
            groupKA.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupZK;
        private Button buttonFileZK;
        private TextBox textFileZK;
        private Button buttonAddZK;
        private GroupBox groupKA;
        private Button buttonAddKA;
        private Button buttonFileKA;
        private TextBox textFileKA;
        private Button buttonSend;
        private OpenFileDialog openFileZK;
        private OpenFileDialog openFileKA;
        private TextBox textFileZKok;
        private TextBox textFileKAok;
        private TextBox textFileSend;
        private Button buttonFileSend;
        private OpenFileDialog openFileSend;
    }
}
