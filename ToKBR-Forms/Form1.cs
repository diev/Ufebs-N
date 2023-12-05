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

    private void ButtonFileZK_Click(object sender, EventArgs e)
    {
        if (openFileZK.ShowDialog() == DialogResult.OK)
        {
            string file = openFileZK.FileName;
            string zk = Path.ChangeExtension(file, "zk.xml");

            try
            {
                XmlDocument xml = new();
                xml.Load(file);

                if (xml.DocumentElement?.LocalName == "SigEnvelope")
                    throw new ApplicationException("Передайте этот файл на отправку в КБР.");

                if (xml.DocumentElement?.FirstChild?.LocalName == "SigValue")
                    throw new ApplicationException("Передайте этот файл Контролеру.");

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

            string kbr = Path.Combine(
                Path.GetFullPath(AppContext.GetData("KBR.Dir") as string ?? "."),
                Path.GetFileName(file));

            try
            {
                XmlDocument xml = new();
                xml.Load(file);

                if (xml.DocumentElement?.LocalName == "SigEnvelope")
                {
                    //throw new ApplicationException("Передайте этот файл на отправку в КБР.");

                    textFileKAok.Text = file;
                    textFileKAok.BackColor = Color.LightGreen;
                    textFileKBR.Text = kbr;
                    buttonAddKA.Enabled = false;
                    buttonSend.Enabled = true;

                    MessageBox.Show("Этот файл можно отправлять.",
                        "Подписано КА", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    if (xml.DocumentElement?.FirstChild?.LocalName != "SigValue")
                        throw new ApplicationException("Передайте этот файл Операционисту.");

                    textFileKA.Text = file;
                    textFileKAok.Text = ka;
                    buttonAddKA.Enabled = true;
                }
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
        string zk = textFileZKok.Text;

        string normal = Path.ChangeExtension(file, "normal.xml");
        string p7d = Path.ChangeExtension(normal, "p7d");

        try
        {
            Transformator.Normalize(file, normal);

            if (!SpkiUtl.CreateSignDetached(normal, p7d))
                throw new ApplicationException("Файл ZK не создан.");

            Transformator.CreateSigValue(file, p7d, zk);

            textFileZKok.BackColor = Color.LightGreen;
            buttonAddZK.Enabled = false;

            MessageBox.Show("Скажите Контролеру о готовности, программу можно закрыть.",
                "Подписано ЗК", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (Program.Delete)
            {
                File.Delete(file);
                File.Delete(normal);
                File.Delete(p7d);
            }
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
        string ka = textFileKAok.Text;

        string p7d = Path.ChangeExtension(file, "p7d");

        string kbr = Path.Combine(
            Path.GetFullPath(AppContext.GetData("KBR.Dir") as string ?? "."),
            Path.GetFileName(file));

        try
        {
            if (!SpkiUtl.CreateSignDetached(file, p7d))
                throw new ApplicationException("Файл KA не создан.");

            Transformator.CreateSigEnvelope(file, p7d, ka);

            textFileKAok.Text = file;
            textFileKAok.BackColor = Color.LightGreen;
            textFileKBR.Text = kbr;
            buttonAddKA.Enabled = false;
            buttonSend.Enabled = true;

            MessageBox.Show("Этот файл можно отправлять.",
                "Подписано КА", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (Program.Delete)
            {
                File.Delete(file);
                File.Delete(p7d);
            }
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
        string kbr = textFileKBR.Text;

        try
        {
            XmlDocument xml = new();
            xml.Load(file);

            if (xml.DocumentElement?.LocalName != "SigEnvelope")
                throw new ApplicationException("Этот файл не подготовлен для отправки в КБР.");

            File.Copy(file, kbr, true);
            
            textFileKBR.BackColor = Color.LightGreen;
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
