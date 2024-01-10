namespace ToKBR.Forms
{
    partial class MainForm
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
            OprGroup = new GroupBox();
            InLabel = new Label();
            ZKButton = new Button();
            FileInButton = new Button();
            FileInText = new TextBox();
            CtrGroup = new GroupBox();
            ZKLabel = new Label();
            KAButton = new Button();
            FileZKButton = new Button();
            FileZKText = new TextBox();
            KALabel = new Label();
            FileKAButton = new Button();
            FileKAText = new TextBox();
            OutButton = new Button();
            FileOutText = new TextBox();
            FileInDialog = new OpenFileDialog();
            FileZKDialog = new OpenFileDialog();
            FileKADialog = new OpenFileDialog();
            ExitButton = new Button();
            OutGroup = new GroupBox();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            button4 = new Button();
            OutLabel = new Label();
            statusStrip1 = new StatusStrip();
            UserStatus = new ToolStripStatusLabel();
            AllowedStatus = new ToolStripStatusLabel();
            AppStatus = new ToolStripStatusLabel();
            OprGroup.SuspendLayout();
            CtrGroup.SuspendLayout();
            OutGroup.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // OprGroup
            // 
            OprGroup.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            OprGroup.Controls.Add(InLabel);
            OprGroup.Controls.Add(ZKButton);
            OprGroup.Controls.Add(FileInButton);
            OprGroup.Controls.Add(FileInText);
            OprGroup.Location = new Point(10, 7);
            OprGroup.Name = "OprGroup";
            OprGroup.Size = new Size(762, 65);
            OprGroup.TabIndex = 0;
            OprGroup.TabStop = false;
            OprGroup.Text = "Шаг 1. Операционист:";
            // 
            // InLabel
            // 
            InLabel.Location = new Point(6, 28);
            InLabel.Name = "InLabel";
            InLabel.Size = new Size(150, 15);
            InLabel.TabIndex = 1;
            InLabel.Text = "Исходный файл:";
            InLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // ZKButton
            // 
            ZKButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            ZKButton.AutoSize = true;
            ZKButton.Enabled = false;
            ZKButton.Location = new Point(606, 23);
            ZKButton.Name = "ZKButton";
            ZKButton.Size = new Size(150, 25);
            ZKButton.TabIndex = 4;
            ZKButton.Text = "Поставить ЗК";
            ZKButton.UseVisualStyleBackColor = true;
            ZKButton.Click += ZKButton_Click;
            // 
            // FileInButton
            // 
            FileInButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            FileInButton.AutoSize = true;
            FileInButton.Location = new Point(570, 23);
            FileInButton.Name = "FileInButton";
            FileInButton.Size = new Size(30, 25);
            FileInButton.TabIndex = 3;
            FileInButton.Text = "...";
            FileInButton.UseVisualStyleBackColor = true;
            FileInButton.Click += FileInButton_Click;
            // 
            // FileInText
            // 
            FileInText.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            FileInText.Location = new Point(162, 25);
            FileInText.MaxLength = 255;
            FileInText.Name = "FileInText";
            FileInText.PlaceholderText = "Выберите файл, полученный извне";
            FileInText.ReadOnly = true;
            FileInText.Size = new Size(407, 23);
            FileInText.TabIndex = 2;
            FileInText.TabStop = false;
            FileInText.WordWrap = false;
            // 
            // CtrGroup
            // 
            CtrGroup.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            CtrGroup.Controls.Add(ZKLabel);
            CtrGroup.Controls.Add(KAButton);
            CtrGroup.Controls.Add(FileZKButton);
            CtrGroup.Controls.Add(FileZKText);
            CtrGroup.Location = new Point(10, 78);
            CtrGroup.Name = "CtrGroup";
            CtrGroup.Size = new Size(762, 65);
            CtrGroup.TabIndex = 5;
            CtrGroup.TabStop = false;
            CtrGroup.Text = "Шаг 2. Контролер:";
            // 
            // ZKLabel
            // 
            ZKLabel.Location = new Point(6, 27);
            ZKLabel.Name = "ZKLabel";
            ZKLabel.Size = new Size(150, 15);
            ZKLabel.TabIndex = 6;
            ZKLabel.Text = "Файл с ЗК:";
            ZKLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // KAButton
            // 
            KAButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            KAButton.AutoSize = true;
            KAButton.Enabled = false;
            KAButton.Location = new Point(606, 22);
            KAButton.Name = "KAButton";
            KAButton.Size = new Size(150, 25);
            KAButton.TabIndex = 9;
            KAButton.Text = "Поставить КА";
            KAButton.UseVisualStyleBackColor = true;
            KAButton.Click += KAButton_Click;
            // 
            // FileZKButton
            // 
            FileZKButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            FileZKButton.AutoSize = true;
            FileZKButton.Location = new Point(570, 22);
            FileZKButton.Name = "FileZKButton";
            FileZKButton.Size = new Size(30, 25);
            FileZKButton.TabIndex = 8;
            FileZKButton.Text = "...";
            FileZKButton.UseVisualStyleBackColor = true;
            FileZKButton.Click += FileZKButton_Click;
            // 
            // FileZKText
            // 
            FileZKText.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            FileZKText.Location = new Point(162, 24);
            FileZKText.MaxLength = 255;
            FileZKText.Name = "FileZKText";
            FileZKText.PlaceholderText = "Выберите файл, полученный на шаге 1";
            FileZKText.ReadOnly = true;
            FileZKText.Size = new Size(407, 23);
            FileZKText.TabIndex = 7;
            FileZKText.TabStop = false;
            FileZKText.WordWrap = false;
            // 
            // KALabel
            // 
            KALabel.Location = new Point(5, 25);
            KALabel.Name = "KALabel";
            KALabel.Size = new Size(150, 15);
            KALabel.TabIndex = 10;
            KALabel.Text = "Конверт с КА:";
            KALabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // FileKAButton
            // 
            FileKAButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            FileKAButton.AutoSize = true;
            FileKAButton.Location = new Point(569, 20);
            FileKAButton.Name = "FileKAButton";
            FileKAButton.Size = new Size(30, 25);
            FileKAButton.TabIndex = 12;
            FileKAButton.Text = "...";
            FileKAButton.UseVisualStyleBackColor = true;
            FileKAButton.Click += FileKAButton_Click;
            // 
            // FileKAText
            // 
            FileKAText.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            FileKAText.Location = new Point(161, 22);
            FileKAText.MaxLength = 255;
            FileKAText.Name = "FileKAText";
            FileKAText.PlaceholderText = "Выберите файл, полученный на шаге 2";
            FileKAText.ReadOnly = true;
            FileKAText.Size = new Size(407, 23);
            FileKAText.TabIndex = 11;
            FileKAText.TabStop = false;
            FileKAText.WordWrap = false;
            // 
            // OutButton
            // 
            OutButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            OutButton.AutoSize = true;
            OutButton.Enabled = false;
            OutButton.Location = new Point(605, 19);
            OutButton.Name = "OutButton";
            OutButton.Size = new Size(150, 25);
            OutButton.TabIndex = 13;
            OutButton.Text = "Отправить в КБР";
            OutButton.UseVisualStyleBackColor = true;
            OutButton.Click += OutButton_Click;
            // 
            // FileOutText
            // 
            FileOutText.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            FileOutText.Location = new Point(172, 230);
            FileOutText.MaxLength = 255;
            FileOutText.Name = "FileOutText";
            FileOutText.PlaceholderText = "Файл для отправки в КБР";
            FileOutText.ReadOnly = true;
            FileOutText.Size = new Size(407, 23);
            FileOutText.TabIndex = 15;
            FileOutText.TabStop = false;
            FileOutText.WordWrap = false;
            // 
            // FileInDialog
            // 
            FileInDialog.DefaultExt = "xml";
            FileInDialog.Filter = "Файлы XML|*.xml|Все файлы|*.*";
            FileInDialog.SupportMultiDottedExtensions = true;
            FileInDialog.Title = "Выберите файл для установки ЗК";
            // 
            // FileZKDialog
            // 
            FileZKDialog.DefaultExt = "xml";
            FileZKDialog.Filter = "Файлы XML|*.xml|Файлы с ZK|*.zk.xml|Все файлы|*.*";
            FileZKDialog.SupportMultiDottedExtensions = true;
            FileZKDialog.Title = "Выберите файл для установки КА";
            // 
            // FileKADialog
            // 
            FileKADialog.DefaultExt = "xml";
            FileKADialog.Filter = "Файлы XML|*.xml|Конверты с KA|*.ka.xml|Все файлы|*.*";
            FileKADialog.SupportMultiDottedExtensions = true;
            FileKADialog.Title = "Выберите файл для отправки";
            // 
            // ExitButton
            // 
            ExitButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            ExitButton.AutoSize = true;
            ExitButton.Location = new Point(615, 228);
            ExitButton.Name = "ExitButton";
            ExitButton.Size = new Size(150, 25);
            ExitButton.TabIndex = 16;
            ExitButton.Text = "Выход";
            ExitButton.UseVisualStyleBackColor = true;
            ExitButton.Click += ExitButton_Click;
            // 
            // OutGroup
            // 
            OutGroup.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            OutGroup.Controls.Add(KALabel);
            OutGroup.Controls.Add(button1);
            OutGroup.Controls.Add(button2);
            OutGroup.Controls.Add(FileKAButton);
            OutGroup.Controls.Add(button3);
            OutGroup.Controls.Add(FileKAText);
            OutGroup.Controls.Add(OutButton);
            OutGroup.Controls.Add(button4);
            OutGroup.Location = new Point(10, 149);
            OutGroup.Name = "OutGroup";
            OutGroup.Size = new Size(762, 65);
            OutGroup.TabIndex = 17;
            OutGroup.TabStop = false;
            OutGroup.Text = "Шаг 3. Отправка:";
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button1.AutoSize = true;
            button1.Location = new Point(1132, 51);
            button1.Name = "button1";
            button1.Size = new Size(30, 25);
            button1.TabIndex = 12;
            button1.Text = "...";
            button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button2.AutoSize = true;
            button2.Enabled = false;
            button2.Location = new Point(1168, 50);
            button2.Name = "button2";
            button2.Size = new Size(150, 25);
            button2.TabIndex = 13;
            button2.Text = "Отправить в КБР";
            button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            button3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button3.AutoSize = true;
            button3.Enabled = false;
            button3.Location = new Point(1168, 22);
            button3.Name = "button3";
            button3.Size = new Size(150, 25);
            button3.TabIndex = 9;
            button3.Text = "Установить КА";
            button3.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            button4.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button4.AutoSize = true;
            button4.Location = new Point(1132, 22);
            button4.Name = "button4";
            button4.Size = new Size(30, 25);
            button4.TabIndex = 8;
            button4.Text = "...";
            button4.UseVisualStyleBackColor = true;
            // 
            // OutLabel
            // 
            OutLabel.Location = new Point(16, 233);
            OutLabel.Name = "OutLabel";
            OutLabel.Size = new Size(150, 15);
            OutLabel.TabIndex = 18;
            OutLabel.Text = "Результат:";
            OutLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { UserStatus, AllowedStatus, AppStatus });
            statusStrip1.Location = new Point(0, 269);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(784, 22);
            statusStrip1.TabIndex = 19;
            statusStrip1.Text = "statusStrip1";
            // 
            // UserStatus
            // 
            UserStatus.Margin = new Padding(10, 3, 10, 2);
            UserStatus.Name = "UserStatus";
            UserStatus.Size = new Size(30, 17);
            UserStatus.Text = "User";
            // 
            // AllowedStatus
            // 
            AllowedStatus.Name = "AllowedStatus";
            AllowedStatus.Size = new Size(639, 17);
            AllowedStatus.Spring = true;
            AllowedStatus.Text = "Выберите файл кнопкой с \"...\"";
            AllowedStatus.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // AppStatus
            // 
            AppStatus.Margin = new Padding(10, 3, 10, 2);
            AppStatus.Name = "AppStatus";
            AppStatus.Size = new Size(29, 17);
            AppStatus.Text = "App";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 291);
            Controls.Add(statusStrip1);
            Controls.Add(OutLabel);
            Controls.Add(OutGroup);
            Controls.Add(ExitButton);
            Controls.Add(CtrGroup);
            Controls.Add(FileOutText);
            Controls.Add(OprGroup);
            MinimumSize = new Size(640, 330);
            Name = "MainForm";
            Text = "Поставить ЗК, КА и отправить в КБР";
            OprGroup.ResumeLayout(false);
            OprGroup.PerformLayout();
            CtrGroup.ResumeLayout(false);
            CtrGroup.PerformLayout();
            OutGroup.ResumeLayout(false);
            OutGroup.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox OprGroup;
        private Button FileInButton;
        private TextBox FileInText;
        private Button ZKButton;
        private GroupBox CtrGroup;
        private Button KAButton;
        private Button FileZKButton;
        private TextBox FileZKText;
        private Button OutButton;
        private OpenFileDialog FileInDialog;
        private OpenFileDialog FileZKDialog;
        private TextBox FileKAText;
        private TextBox FileOutText;
        private Button FileKAButton;
        private OpenFileDialog FileKADialog;
        private Label InLabel;
        private Label KALabel;
        private Label ZKLabel;
        private Button ExitButton;
        private GroupBox OutGroup;
        private Button button1;
        private Button button2;
        private Button button3;
        private Button button4;
        private Label OutLabel;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel UserStatus;
        private ToolStripStatusLabel AppStatus;
        private ToolStripStatusLabel AllowedStatus;
    }
}
