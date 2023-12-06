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

using System.Xml;

using ToKBR.Lib;

namespace ToKBR.Forms;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        string zk = AppContext.GetData("ZK.Dir") as string ?? ".";
        string ka = AppContext.GetData("KA.Dir") as string ?? ".";
        string kbr = AppContext.GetData("KBR.Dir") as string ?? ".";

        if (!Directory.Exists(zk))
            Directory.CreateDirectory(zk);
        openFileZK.InitialDirectory = zk;

        if (!Directory.Exists(ka))
            Directory.CreateDirectory(ka);
        openFileZK.InitialDirectory = ka;

        if (!Directory.Exists(kbr))
            Directory.CreateDirectory(kbr);
    }

    private void ResetForm()
    {
        textFileZK.Text = string.Empty;
        textFileZKok.Text = string.Empty;

        textFileKA.Text = string.Empty;
        textFileKAok.Text = string.Empty;
        textFileSend.Text = string.Empty;

        textFileZK.BackColor = SystemColors.Control;
        textFileZKok.BackColor = SystemColors.Control;

        textFileKA.BackColor = SystemColors.Control;
        textFileKAok.BackColor = SystemColors.Control;
        textFileSend.BackColor = SystemColors.Control;

        buttonAddZK.Enabled = false;
        buttonAddKA.Enabled = false;
        buttonSend.Enabled = false;
    }

    private void ButtonFileZK_Click(object sender, EventArgs e)
    {
        if (openFileZK.ShowDialog() == DialogResult.OK)
        {
            string file = openFileZK.FileName;
            string zk = Path.ChangeExtension(file, "zk.xml");

            try
            {
                Transformator.OprCheck(file);

                ResetForm();
                textFileZK.Text = file;
                textFileZKok.Text = zk;
                buttonAddZK.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,
                    "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private void ButtonFileKA_Click(object sender, EventArgs e)
    {
        if (openFileKA.ShowDialog() == DialogResult.OK)
        {
            string file = openFileKA.FileName;
            string ka = Path.ChangeExtension(file, "ka.xml");

            //string kbr = Path.Combine(
            //    Path.GetFullPath(AppContext.GetData("KBR.Dir") as string ?? "."),
            //    Path.GetFileName(file));

            try
            {
                Transformator.CtrCheck(file);

                ResetForm();
                textFileKA.Text = file;
                textFileKAok.Text = ka;
                buttonAddKA.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,
                    "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private void ButtonFileSend_Click(object sender, EventArgs e)
    {
        if (openFileSend.ShowDialog() == DialogResult.OK)
        {
            string file = openFileSend.FileName;
            string kbr = Path.Combine(
                Path.GetFullPath(AppContext.GetData("KBR.Dir") as string ?? "."),
                Path.GetFileName(file));

            try
            {
                Transformator.KbrCheck(file);

                ResetForm();
                textFileKAok.Text = file;
                textFileKAok.BackColor = Color.LightGreen;
                textFileSend.Text = kbr;
                buttonAddKA.Enabled = false;
                buttonSend.Enabled = true;

                MessageBox.Show("Этот файл можно отправлять.",
                    "Подписано КА", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,
                    "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private void ButtonAddZK_Click(object sender, EventArgs e)
    {
        string file = textFileZK.Text;

        try
        {
            Transformator.OprRole(file);

            textFileZKok.BackColor = Color.LightGreen;
            buttonAddZK.Enabled = false;

            MessageBox.Show("Скажите Контролеру о готовности, программу можно закрыть.",
                "Подписано ЗК", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message,
                "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ButtonAddKA_Click(object sender, EventArgs e)
    {
        string file = textFileKA.Text;
        string kbr = Path.Combine(
            Path.GetFullPath(AppContext.GetData("KBR.Dir") as string ?? "."),
            Path.GetFileName(file));

        try
        {
            Transformator.CtrRole(file);

            textFileKAok.BackColor = Color.LightGreen;
            textFileSend.Text = kbr;
            buttonAddKA.Enabled = false;
            buttonSend.Enabled = true;

            MessageBox.Show("Этот файл можно отправлять.",
                "Подписано КА", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message,
                "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ButtonSend_Click(object sender, EventArgs e)
    {
        string file = textFileKAok.Text;
        string kbr = textFileSend.Text;

        try
        {
            Transformator.KbrCheck(file);
            Transformator.KbrRole(file, kbr);

            textFileSend.BackColor = Color.LightGreen;
            buttonSend.Enabled = false;

            MessageBox.Show("Программу можно закрыть.",
                "Передано на отправку", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message,
                "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
