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

using System;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;
using System.Xml.Linq;

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
    public static bool Delete { get; set; } = PathHelper.Delete;

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
            throw new ApplicationException(@$"Ошибка загрузки ""{sourceFileName}"".");
        }

        if (doc.DocumentElement is null)
        {
            throw new ApplicationException(@$"Ошибка разбора ""{sourceFileName}"".");
        }

        //*[namespace-uri()='{uri}']
        //*[starts-with(name(),'nil:')]

        //string uri = doc.DocumentElement.NamespaceURI;
        //var xpath = $"//*[namespace-uri()='{uri}']";

        //doc.DocumentElement.RemoveAttribute("xmlns");
        //doc.DocumentElement.SetAttribute("xmlns:n1", uri); //TODO n2+

        //var nodes = doc.SelectNodes(xpath) ??
        //    throw new ApplicationException(@$"Ошибка выборки нод в ""{sourceFileName}"".");

        //foreach (XmlElement node in nodes)
        //{
        //    node.Prefix = "n1";
        //}

        string uri1 = doc.DocumentElement.NamespaceURI;
        doc.DocumentElement.RemoveAttribute("xmlns");
        doc.DocumentElement.SetAttribute("xmlns:n1", uri1);

        List<string> list = [];
        list.Add(uri1);

        var xpath = "//*";
        var nodes = doc.SelectNodes(xpath) ??
            throw new ApplicationException(@$"Ошибка выборки нод в ""{sourceFileName}"".");

        foreach (XmlElement node in nodes)
        {
            // fast line by default
            if (node.NamespaceURI.Equals(uri1, StringComparison.OrdinalIgnoreCase))
            {
                node.RemoveAttribute("xmlns");
                node.Prefix = "n1";
                continue;
            }

            // second class
            string uri = node.NamespaceURI;
            int n = list.IndexOf(uri, 1) + 1;

            if (n == 0)
            {
                list.Add(uri);
                n = list.Count;
                doc.DocumentElement.SetAttribute($"xmlns:n{n}", uri);
            }

            node.RemoveAttribute("xmlns");
            node.Prefix = $"n{n}";
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

            XmlDocument xml = new();
            xml.Load(destFileName);

            for (int i = 0; i < count; i++)
            {
                string file = PathHelper.GetNormalFileName(sourceFileName, i);
                File.WriteAllText(file, xml.DocumentElement?.ChildNodes[i]?.OuterXml);
            }

            return count;
        }

        return -1;
    }

    /// <summary>
    /// Создание файла XML со вставленным защитным кодом (ЗК) для одиночного документа.
    /// </summary>
    /// <param name="sourceFileName">Исходный файл.</param>
    /// <param name="zkFileName">Двоичный файл с уже посчитанным ЗК.</param>
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
            File.Delete(PathHelper.GetNormalFileName(sourceFileName));
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
            string p7d = PathHelper.GetSignFileName(sourceFileName, i);
            byte[] sigValue = File.ReadAllBytes(p7d);

            XmlText ZK64 = doc.CreateTextNode(Convert.ToBase64String(sigValue));
            XmlElement dsigSigValue = doc.CreateElement("dsig", "SigValue", _ns);
            dsigSigValue.AppendChild(ZK64);

            _ = doc.DocumentElement?.ChildNodes[i]?.PrependChild(dsigSigValue);

            if (Delete)
            {
                File.Delete(PathHelper.GetNormalFileName(sourceFileName, i));
                File.Delete(p7d);
            }
        }

        doc.Save(destFileName);

        if (Delete)
        {
            File.Delete(sourceFileName);
            File.Delete(PathHelper.GetNormalFileName(sourceFileName));
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

        var root = xml.DocumentElement ?? throw new ApplicationException("Неполный XML: нет Root.");

        if (root.LocalName.Equals("SigEnvelope", StringComparison.OrdinalIgnoreCase))
            throw new ApplicationException("Это конверт с КА - передайте его на отправку в КБР.");

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
    /// <param name="sourceFileName">Исходный файл.</param>
    /// <exception cref="ApplicationException"></exception>
    public static void OprCheck(string sourceFileName)
    {
        XmlDocument xml = new();
        xml.Load(sourceFileName);

        var root = xml.DocumentElement ?? throw new ApplicationException("Неполный XML: нет Root.");

        if (root.LocalName.Equals("SigEnvelope", StringComparison.OrdinalIgnoreCase))
            throw new ApplicationException("Это конверт с КА - передайте его на отправку в КБР.");

        if (root.LocalName.StartsWith("Packet", StringComparison.OrdinalIgnoreCase))
        {
            var ed = root.FirstChild ?? throw new ApplicationException("Неполный XML: нет ED в Packet.");

            if (ed.LocalName.StartsWith("ED", StringComparison.OrdinalIgnoreCase))
            {
                var sig = ed.FirstChild;

                if (sig != null 
                    && sig.LocalName.Equals("SigValue", StringComparison.OrdinalIgnoreCase))
                    throw new ApplicationException("Это файл с ЗК - передайте его Контролеру.");
            }
        }
        else // EDxxx
        {
            var ed = root;

            if (ed.LocalName.StartsWith("ED", StringComparison.OrdinalIgnoreCase))
            {
                var sig = ed.FirstChild;

                if (sig != null
                    && sig.LocalName.Equals("SigValue", StringComparison.OrdinalIgnoreCase))
                    throw new ApplicationException("Это файл с ЗК - передайте его Контролеру.");
            }
        }
    }

    /// <summary>
    /// Исполнение роли Операциониста - установка ЗК.
    /// </summary>
    /// <param name="sourceFileName">Исходный файл XML.</param>
    /// <param name="destFileName">Файл результата с ЗК.</param>
    /// <exception cref="ApplicationException"></exception>
    public static void OprRole(string sourceFileName, string? destFileName = null)
    {
        destFileName ??= PathHelper.GetZKFileName(sourceFileName);

        string normal = PathHelper.GetNormalFileName(sourceFileName);
        string p7d = PathHelper.GetSignFileName(sourceFileName);

        int count = Normalize(sourceFileName, normal);

        if (count == -1)
        {
            if (!SpkiUtl.CreateSignDetached(normal, p7d))
                throw new ApplicationException("Файл ZK не создан.");

            CreateEDSigValue(sourceFileName, p7d, destFileName);
        }
        else
        {
            for (int i = 0; i < count; i++)
            {
                string normalX = PathHelper.GetNormalFileName(sourceFileName, i);
                string p7dX = PathHelper.GetSignFileName(sourceFileName, i);

                if (!SpkiUtl.CreateSignDetached(normalX, p7dX))
                    throw new ApplicationException("Файл ZK не создан.");
            }

            CreatePacketSigValues(sourceFileName, count, destFileName);
        }
    }

    /// <summary>
    /// Проверка исходного файла для роли Контролера.
    /// </summary>
    /// <param name="sourceFileName">Исходный файл.</param>
    /// <exception cref="ApplicationException"></exception>
    public static void CtrCheck(string sourceFileName)
    {
        XmlDocument xml = new();
        xml.Load(sourceFileName);

        var root = xml.DocumentElement ?? throw new ApplicationException("Неполный XML: нет Root.");

        //if (root.LocalName.Equals("SigEnvelope", StringComparison.OrdinalIgnoreCase))
        //    throw new ApplicationException("Передайте этот файл на отправку в КБР.");

        if (root.LocalName.StartsWith("Packet", StringComparison.OrdinalIgnoreCase))
        {
            var ed = root.FirstChild ?? throw new ApplicationException("Неполный XML: нет ED в Packet.");

            if (ed.LocalName.StartsWith("ED", StringComparison.OrdinalIgnoreCase))
            {
                var sig = ed.FirstChild ?? throw new ApplicationException("Неполный XML: нет элементов в ED.");

                if (!sig.LocalName.Equals("SigValue", StringComparison.OrdinalIgnoreCase))
                    throw new ApplicationException("Это файл без ЗК - передайте его Операционисту.");
            }
        }
        else
        {
            var ed = root;

            if (ed.LocalName.StartsWith("ED", StringComparison.OrdinalIgnoreCase))
            {
                var sig = ed.FirstChild ?? throw new ApplicationException("Неполный XML: нет элементов в ED.");

                if (!sig.LocalName.Equals("SigValue", StringComparison.OrdinalIgnoreCase))
                    throw new ApplicationException("Это файл без ЗК - передайте его Операционисту.");
            }
        }
    }

    /// <summary>
    /// Исполнение роли Контролера - установка КА.
    /// </summary>
    /// <param name="sourceFileName">Исходный файл с ЗК.</param>
    /// <param name="destFileName">Файл результата с КА.</param>
    /// <exception cref="ApplicationException"></exception>
    public static void CtrRole(string sourceFileName, string? destFileName = null)
    {
        destFileName ??= PathHelper.GetKAFileName(sourceFileName);

        string p7d = PathHelper.GetSignFileName(sourceFileName);
        File.Copy(sourceFileName, PathHelper.GetBackupFileName(sourceFileName), true);

        if (!SpkiUtl.CreateSignDetached(sourceFileName, p7d))
            throw new ApplicationException("Файл KA не создан.");

        CreateSigEnvelope(sourceFileName, p7d, destFileName);
    }

    /// <summary>
    /// Проверка исходного файла для роли КБР.
    /// </summary>
    /// <param name="sourceFileName">Исходный файл с КА.</param>
    /// <exception cref="ApplicationException"></exception>
    public static void KbrCheck(string sourceFileName)
    {
        XmlDocument xml = new();
        xml.Load(sourceFileName);

        var root = xml.DocumentElement ?? throw new ApplicationException("Неполный XML: нет Root.");

        if (root.LocalName.StartsWith("Packet", StringComparison.OrdinalIgnoreCase))
        {
            var ed = root.FirstChild ?? throw new ApplicationException("Неполный XML: нет ED в Packet.");

            if (ed.LocalName.StartsWith("ED", StringComparison.OrdinalIgnoreCase))
            {
                var sig = ed.FirstChild ?? throw new ApplicationException("Неполный XML: нет элементов в ED.");

                if (!sig.LocalName.Equals("SigValue", StringComparison.OrdinalIgnoreCase))
                    throw new ApplicationException("Это файл без ЗК - передайте его Операционисту.");
            }
        }
        else
        {
            var ed = root;

            if (ed.LocalName.StartsWith("ED", StringComparison.OrdinalIgnoreCase))
            {
                var sig = ed.FirstChild ?? throw new ApplicationException("Неполный XML: нет элементов в ED.");

                if (!sig.LocalName.Equals("SigValue", StringComparison.OrdinalIgnoreCase))
                    throw new ApplicationException("Это файл без ЗК - передайте его Операционисту.");
            }
        }

        if (!root.LocalName.Equals("SigEnvelope", StringComparison.OrdinalIgnoreCase))
            throw new ApplicationException("Это файл без КА - передайте его Контролеру.");
    }

    /// <summary>
    /// Исполнение роли КБР - копирование в папку отправки.
    /// </summary>
    /// <param name="sourceFileName">Исходный файл с КА.</param>
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
