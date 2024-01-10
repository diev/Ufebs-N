#region License
/*
Copyright 2022-2023 Dmitrii Evdokimov
Open source software

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/
#endregion

using System.Reflection;

using ToKBR.Lib;

namespace ToKBR.Forms;

public partial class MainForm : Form
{
    public MainForm()
    {
        InitializeComponent();

        var assembly = Assembly.GetEntryAssembly();
        var assemName = assembly?.GetName();
        var app = assemName?.Name ?? "App";
        var ver = assemName?.Version?.ToString() ?? Environment.Version.ToString(2); // "8.0"

        AppStatus.Text = $"{app}, ver.{ver}";
        UserStatus.Text = $"{Environment.UserName} @ {Environment.MachineName}";

        FileInDialog.InitialDirectory = PathHelper.InDir;
        FileZKDialog.InitialDirectory = PathHelper.ZKDir;
        FileKADialog.InitialDirectory = PathHelper.KADir;

        //TODO

        //var file = Directory.GetFiles(FileInDialog.InitialDirectory, "*.xml");

        //if (file.Length == 1)
        //{
        //    FileInText.Text = file[0];
        //    Transformator.OprCheck(FileInText.Text);
        //    ZKButton.Enabled = true;
        //}

        //var zk = Directory.GetFiles(FileZKDialog.InitialDirectory, "*.xml");

        //if (zk.Length == 1)
        //{
        //    FileZKText.Text = zk[0];
        //    Transformator.CtrCheck(FileZKText.Text);
        //    KAButton.Enabled = true;
        //}

        //var ka = Directory.GetFiles(FileKADialog.InitialDirectory, "*.xml");

        //if (ka.Length == 1)
        //{
        //    FileKAText.Text = ka[0];
        //    Transformator.KbrCheck(FileKAText.Text);
        //    OutButton.Enabled = true;
        //}
    }

    private void ResetControls()
    {
        FileInText.Text = string.Empty;
        FileZKText.Text = string.Empty;
        FileKAText.Text = string.Empty;
        FileOutText.Text = string.Empty;

        AllowedStatus.Text = string.Empty;

        FileInText.BackColor = SystemColors.Control;
        FileZKText.BackColor = SystemColors.Control;
        FileKAText.BackColor = SystemColors.Control;
        FileOutText.BackColor = SystemColors.Control;

        ZKButton.Enabled = false;
        KAButton.Enabled = false;
        OutButton.Enabled = false;
    }

    private void FileInButton_Click(object sender, EventArgs e)
    {
        if (FileInDialog.ShowDialog() == DialogResult.OK)
        {
            string file = FileInDialog.FileName;
            string zk = PathHelper.GetZKFileName(file);
            string ka = PathHelper.GetKAFileName(file);
            string kbr = PathHelper.GetOutFileName(file);

            try
            {
                Transformator.OprCheck(file);

                ResetControls();
                FileInText.Text = file;
                FileInText.BackColor = Color.LightGreen;
                FileZKText.Text = zk;
                FileKAText.Text = ka;
                FileOutText.Text = kbr;

                if (UserHelper.ZK)
                {
                    ZKButton.Enabled = true;
                }
                else
                {
                    AllowedStatus.Text = "��� ����� ������� ��!";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,
                    "������!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private void FileZKButton_Click(object sender, EventArgs e)
    {
        if (FileZKDialog.ShowDialog() == DialogResult.OK)
        {
            string zk = FileZKDialog.FileName;
            string ka = PathHelper.GetKAFileName(zk);
            string kbr = PathHelper.GetOutFileName(zk);

            try
            {
                Transformator.CtrCheck(zk);

                ResetControls();
                FileZKText.Text = zk;
                FileZKText.BackColor = Color.LightGreen;
                FileKAText.Text = ka;
                FileOutText.Text = kbr;

                if (UserHelper.KA)
                {
                    KAButton.Enabled = true;
                }
                else
                {
                    AllowedStatus.Text = "��� ����� ������� ��!";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,
                    "������!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private void FileKAButton_Click(object sender, EventArgs e)
    {
        if (FileKADialog.ShowDialog() == DialogResult.OK)
        {
            string ka = FileKADialog.FileName;
            string kbr = PathHelper.GetOutFileName(ka);

            try
            {
                Transformator.KbrCheck(ka);

                ResetControls();
                FileKAText.Text = ka;
                FileKAText.BackColor = Color.LightGreen;
                FileOutText.Text = kbr;
                KAButton.Enabled = false;

                if (UserHelper.Out)
                {
                    OutButton.Enabled = true;
                }
                else
                {
                    AllowedStatus.Text = "��� ����� ����������!";
                }

                MessageBox.Show("���� ���� ����� ����������.",
                    "�� ����������",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,
                    "������!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private void ZKButton_Click(object sender, EventArgs e)
    {
        string file = FileInText.Text;
        string zk = FileZKText.Text;

        try
        {
            Transformator.OprRole(file, zk);

            if (File.Exists(zk))
            {
                FileZKText.BackColor = Color.LightGreen;
                ZKButton.Enabled = false;

                if (UserHelper.KA)
                {
                    KAButton.Enabled = true;
                }
                else
                {
                    AllowedStatus.Text = "��� ����� ������� ��!";
                }

                if (MessageBox.Show("�������� ���������� � ����������.\n\n��������� ����� �������?",
                    "�� ����������",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    Close();
                }
            }
            else
            {
                FileZKText.BackColor = Color.LightPink;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message,
                "������!", 
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void KAButton_Click(object sender, EventArgs e)
    {
        string zk = FileZKText.Text;
        string ka = FileKAText.Text;

        try
        {
            Transformator.CtrRole(zk, ka);

            if (File.Exists(ka))
            {
                FileKAText.BackColor = Color.LightGreen;
                KAButton.Enabled = false;

                if (UserHelper.Out)
                {
                    OutButton.Enabled = true;
                }
                else
                {
                    AllowedStatus.Text = "��� ����� ����������!";
                }

                MessageBox.Show("���� ���� ����� ����������.",
                    "�� ����������",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                FileKAText.BackColor = Color.LightPink;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message,
                "������!", 
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void OutButton_Click(object sender, EventArgs e)
    {
        string ka = FileKAText.Text;
        string kbr = FileOutText.Text;

        try
        {
            Transformator.KbrCheck(ka);
            Transformator.KbrRole(ka, kbr);

            if (File.Exists(kbr))
            {
                FileOutText.BackColor = Color.LightGreen;
                OutButton.Enabled = false;

                if (MessageBox.Show("���������������� ����� �� ��������.\n\n��������� ����� �������?",
                    "�������� �� ��������",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    Close();
                }
            }
            else
            {
                FileOutText.BackColor = Color.LightPink;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message,
                "������!", 
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ExitButton_Click(object sender, EventArgs e)
    {
        Close();
    }
}
