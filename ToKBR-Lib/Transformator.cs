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

using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;

namespace ToKBR.Lib;

/// <summary>
/// Класс нормализации и подписывания XML.
/// </summary>
public static class Transformator
{
    private const string _ns = "urn:cbr-ru:dsig:v1.1";
    private const string _ns_env = "urn:cbr-ru:dsig:env:v1.1";

    private static readonly XmlWriterSettings _xmlSettings = new()
    {
        Encoding = Encoding.GetEncoding(1251),
        Indent = true,
        NamespaceHandling = NamespaceHandling.OmitDuplicates,
        WriteEndDocumentOnClose = true
    };

    /// <summary>
    /// Удалять временные файлы после обработки.
    /// </summary>
    public static bool Delete { get; set; }

    /// <summary>
    /// Создание нормализованного XML файла.
    /// </summary>
    /// <param name="sourceFileName">Исходный файл.</param>
    /// <param name="destFileName">Нормализованный файл.</param>
    /// <returns>Число документов в пакете или -1 для одиночного документа.</returns>
    /// <exception cref="ApplicationException"></exception>
    public static int Normalize(string sourceFileName, string destFileName)
    {
        XmlDocument doc = new();
        doc.Load(sourceFileName);

        if (doc is null)
        {
            throw new ApplicationException(@$"Fail to load ""{sourceFileName}""");
        }

        if (doc.DocumentElement is null)
        {
            throw new ApplicationException(@$"Fail to parse ""{sourceFileName}""");
        }

        string uri = doc.DocumentElement.NamespaceURI;
        var xpath = $"//*[namespace-uri()='{uri}']";

        doc.DocumentElement.RemoveAttribute("xmlns");
        doc.DocumentElement.SetAttribute("xmlns:n1", uri); //TODO n2+

        var nodes = doc.SelectNodes(xpath) ??
            throw new ApplicationException(@$"Fail to select nodes in ""{sourceFileName}""");

        foreach (XmlElement node in nodes)
        {
            node.Prefix = "n1";
        }

        XmlDsigC14NTransform transform = new();
        transform.LoadInput(doc);
        using var canonizedStream = (MemoryStream)transform.GetOutput(typeof(Stream));
        var bytes = canonizedStream.ToArray();
        File.WriteAllBytes(destFileName, bytes);

        bool packet = doc.DocumentElement.LocalName.StartsWith("Packet", StringComparison.OrdinalIgnoreCase);

        if (packet)
        {
            int count = doc.DocumentElement.ChildNodes.Count;

            XmlDocument nrz = new();
            nrz.Load(destFileName);

            for (int i = 0; i < count; i++)
            {
                string file = Path.ChangeExtension(destFileName, $"{i + 1}.xml");
                File.WriteAllText(file, nrz.DocumentElement?.ChildNodes[i]?.OuterXml);
            }

            return count;
        }

        return -1;
    }

    /// <summary>
    /// Создание файла XML со вставленным защитным кодом (ЗК) для одиночного документа.
    /// </summary>
    /// <param name="sourceFileName">Исходный файл.</param>
    /// <param name="zkFileName">Двоичный файл с посчитанным ЗК.</param>
    /// <param name="destFileName">Файл XML со вставленным ЗК.</param>
    public static void CreateEDSigValue(string sourceFileName, string zkFileName, string destFileName)
    {
        XmlDocument doc = new();
        doc.Load(sourceFileName);

        byte[] sigValue = File.ReadAllBytes(zkFileName);

        XmlText ZK64 = doc.CreateTextNode(Convert.ToBase64String(sigValue));
        XmlElement dsigSigValue = doc.CreateElement("dsig", "SigValue", _ns);
        dsigSigValue.AppendChild(ZK64);

        _ = doc.DocumentElement?.PrependChild(dsigSigValue);

        doc.Save(destFileName);

        if (Delete)
        {
            File.Delete(sourceFileName);
            File.Delete(Path.ChangeExtension(sourceFileName, "normal.xml"));
            File.Delete(zkFileName);
        }
    }

    /// <summary>
    /// Создание файла XML со вставленными защитными кодами (ЗК) для каждого документа в пакете.
    /// </summary>
    /// <param name="sourceFileName">Исходный файл.</param>
    /// <param name="count">Число документов в пакете.</param>
    /// <param name="destFileName">Файл XML со вставленным ЗК.</param>
    public static void CreatePacketSigValues(string sourceFileName, int count, string destFileName)
    {
        XmlDocument doc = new();
        doc.Load(sourceFileName);

        for (int i = 0; i < count; i++)
        {
            string p7d = Path.ChangeExtension(sourceFileName, $"normal.{i + 1}.p7d");
            byte[] sigValue = File.ReadAllBytes(p7d);

            XmlText ZK64 = doc.CreateTextNode(Convert.ToBase64String(sigValue));
            XmlElement dsigSigValue = doc.CreateElement("dsig", "SigValue", _ns);
            dsigSigValue.AppendChild(ZK64);

            _ = doc.DocumentElement?.ChildNodes[i]?.PrependChild(dsigSigValue);

            if (Delete)
            {
                File.Delete(Path.ChangeExtension(sourceFileName, $"normal.{i + 1}.xml"));
                File.Delete(p7d);
            }
        }

        doc.Save(destFileName);

        if (Delete)
        {
            File.Delete(sourceFileName);
            File.Delete(Path.ChangeExtension(sourceFileName, "normal.xml"));
        }
    }

    /// <summary>
    /// Создание конверта XML со вставленным кодом аутентификации (КА) на внутренний контейнер.
    /// </summary>
    /// <param name="sourceFileName">Исходный файл XML.</param>
    /// <param name="macValueFileName">Двоичный файл с посчитанным КА.</param>
    /// <param name="destFileName">Файл конверта XML со вставленным КА.</param>
    public static void CreateSigEnvelope(string sourceFileName, string macValueFileName, string destFileName)
    {
        XmlDocument xml = new();
        xml.Load(sourceFileName);

        var root = xml.DocumentElement ?? throw new ApplicationException("Неполный XML.");

        if (root.LocalName.Equals("SigEnvelope", StringComparison.OrdinalIgnoreCase))
            throw new ApplicationException("Передайте этот файл на отправку в КБР - он уже готов.");

        byte[] macValue = File.ReadAllBytes(macValueFileName);
        byte[] senObject = File.ReadAllBytes(sourceFileName);

        using var writer = XmlWriter.Create(destFileName, _xmlSettings);

        writer.WriteStartElement("sen", "SigEnvelope", _ns_env);
        writer.WriteStartElement("sen", "SigContainer", _ns_env);

        writer.WriteStartElement("dsig", "MACValue", _ns);
        writer.WriteBase64(macValue, 0, macValue.Length);
        writer.WriteEndElement(); //MACValue

        writer.WriteEndElement(); //SigContainer

        writer.WriteStartElement("sen", "Object", _ns_env);
        writer.WriteBase64(senObject, 0, senObject.Length);
        writer.WriteEndElement(); //Object

        writer.WriteEndElement(); //SigEnvelope
        writer.Close();

        if (Delete)
        {
            File.Delete(sourceFileName);
            File.Delete(macValueFileName);
        }
    }

    /// <summary>
    /// Проверка исходного файла для роли Операциониста.
    /// </summary>
    /// <param name="file">Исходный файл.</param>
    /// <exception cref="ApplicationException"></exception>
    public static void OprCheck(string file)
    {
        XmlDocument xml = new();
        xml.Load(file);

        var root = xml.DocumentElement ?? throw new ApplicationException("Неполный XML.");

        if (root.LocalName.Equals("SigEnvelope", StringComparison.OrdinalIgnoreCase))
            throw new ApplicationException("Передайте этот файл на отправку в КБР.");

        if (root.LocalName.StartsWith("Packet", StringComparison.OrdinalIgnoreCase))
        {
            var ed = root.FirstChild ?? throw new ApplicationException("Неполный XML.");

            if (ed.LocalName.StartsWith("ED", StringComparison.OrdinalIgnoreCase))
            {
                var sig = ed.FirstChild ?? throw new ApplicationException("Неполный XML.");

                if (sig.LocalName.Equals("SigValue", StringComparison.OrdinalIgnoreCase))
                    throw new ApplicationException("Передайте этот файл Контролеру.");
            }
        }
        else
        {
            var ed = root;

            if (ed.LocalName.StartsWith("ED", StringComparison.OrdinalIgnoreCase))
            {
                var sig = ed.FirstChild ?? throw new ApplicationException("Неполный XML.");

                if (sig.LocalName.Equals("SigValue", StringComparison.OrdinalIgnoreCase))
                    throw new ApplicationException("Передайте этот файл Контролеру.");
            }
        }
    }
    
    /// <summary>
    /// Исполнение роли Операциониста.
    /// </summary>
    /// <param name="file">Исходный файл.</param>
    /// <exception cref="ApplicationException"></exception>
    public static void OprRole(string file)
    {
        string normal = Path.ChangeExtension(file, "normal.xml");
        string p7d = Path.ChangeExtension(file, "normal.p7d");
        string result = Path.ChangeExtension(file, "zk.xml");

        int count = Normalize(file, normal);

        if (count == -1)
        {
            if (!SpkiUtl.CreateSignDetached(normal, p7d))
                throw new ApplicationException("Файл ZK не создан.");

            CreateEDSigValue(file, p7d, result);
        }
        else
        {
            for (int i = 1; i <= count; i++)
            {
                string normalX = Path.ChangeExtension(normal, $"{i}.xml");
                string p7dX = Path.ChangeExtension(normal, $"{i}.p7d");

                if (!SpkiUtl.CreateSignDetached(normalX, p7dX))
                    throw new ApplicationException("Файл ZK не создан.");
            }

            CreatePacketSigValues(file, count, result);
        }
    }

    /// <summary>
    /// Проверка исходного файла для роли Контролера.
    /// </summary>
    /// <param name="file">Исходный файл.</param>
    /// <exception cref="ApplicationException"></exception>
    public static void CtrCheck(string file)
    {
        XmlDocument xml = new();
        xml.Load(file);

        var root = xml.DocumentElement ?? throw new ApplicationException("Неполный XML.");

        //if (root.LocalName.Equals("SigEnvelope", StringComparison.OrdinalIgnoreCase))
        //    throw new ApplicationException("Передайте этот файл на отправку в КБР.");

        if (root.LocalName.StartsWith("Packet", StringComparison.OrdinalIgnoreCase))
        {
            var ed = root.FirstChild ?? throw new ApplicationException("Неполный XML.");

            if (ed.LocalName.StartsWith("ED", StringComparison.OrdinalIgnoreCase))
            {
                var sig = ed.FirstChild ?? throw new ApplicationException("Неполный XML.");

                if (!sig.LocalName.Equals("SigValue", StringComparison.OrdinalIgnoreCase))
                    throw new ApplicationException("Передайте этот файл Операционисту.");
            }
        }
        else
        {
            var ed = root;

            if (ed.LocalName.StartsWith("ED", StringComparison.OrdinalIgnoreCase))
            {
                var sig = ed.FirstChild ?? throw new ApplicationException("Неполный XML.");

                if (!sig.LocalName.Equals("SigValue", StringComparison.OrdinalIgnoreCase))
                    throw new ApplicationException("Передайте этот файл Операционисту.");
            }
        }
    }
    
    /// <summary>
    /// Исполнение роли Контролера.
    /// </summary>
    /// <param name="file">Исходный файл.</param>
    /// <exception cref="ApplicationException"></exception>
    public static void CtrRole(string file)
    {
        string p7d = Path.ChangeExtension(file, "p7d");
        string result = Path.ChangeExtension(file, "ka.xml");

        if (!SpkiUtl.CreateSignDetached(file, p7d))
            throw new ApplicationException("Файл KA не создан.");

        CreateSigEnvelope(file, p7d, result);
    }

    /// <summary>
    /// Проверка исходного файла для роли КБР.
    /// </summary>
    /// <param name="file">Исходный файл.</param>
    /// <exception cref="ApplicationException"></exception>
    public static void KbrCheck(string file)
    {
        XmlDocument xml = new();
        xml.Load(file);

        var root = xml.DocumentElement ?? throw new ApplicationException("Неполный XML.");

        if (root.LocalName.StartsWith("Packet", StringComparison.OrdinalIgnoreCase))
        {
            var ed = root.FirstChild ?? throw new ApplicationException("Неполный XML.");

            if (ed.LocalName.StartsWith("ED", StringComparison.OrdinalIgnoreCase))
            {
                var sig = ed.FirstChild ?? throw new ApplicationException("Неполный XML.");

                if (!sig.LocalName.Equals("SigValue", StringComparison.OrdinalIgnoreCase))
                    throw new ApplicationException("Передайте этот файл Операционисту.");
            }
        }
        else
        {
            var ed = root;

            if (ed.LocalName.StartsWith("ED", StringComparison.OrdinalIgnoreCase))
            {
                var sig = ed.FirstChild ?? throw new ApplicationException("Неполный XML.");

                if (!sig.LocalName.Equals("SigValue", StringComparison.OrdinalIgnoreCase))
                    throw new ApplicationException("Передайте этот файл Операционисту.");
            }
        }

        if (!root.LocalName.Equals("SigEnvelope", StringComparison.OrdinalIgnoreCase))
            throw new ApplicationException("Передайте этот файл Контролеру.");
    }

    /// <summary>
    /// Исполнение роли КБР.
    /// </summary>
    /// <param name="sourceFileName">Исходный файл.</param>
    /// <param name="destFileName">Финальный файл XML со вставленной КА для отправки.</param>
    public static void KbrRole(string sourceFileName, string destFileName)
    {
        File.Copy(sourceFileName, destFileName, true);

        if (Delete)
        {
            File.Delete(sourceFileName);
        }
    }
}
