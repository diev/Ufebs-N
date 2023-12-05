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
    /// Создание нормализованного XML файла.
    /// </summary>
    /// <param name="srcFile">Исходный файл.</param>
    /// <param name="nrzFile">Нормализованный файл.</param>
    /// <exception cref="ApplicationException"></exception>
    public static void Normalize(string srcFile, string nrzFile)
    {
        XmlDocument doc = new();
        doc.Load(srcFile);

        if (doc is null)
        {
            throw new ApplicationException(@$"Fail to load ""{srcFile}""");
        }

        if (doc.DocumentElement is null)
        {
            throw new ApplicationException(@$"Fail to parse ""{srcFile}""");
        }

        string uri = doc.DocumentElement.NamespaceURI;
        var xpath = $"//*[namespace-uri()='{uri}']";

        doc.DocumentElement.RemoveAttribute("xmlns");
        doc.DocumentElement.SetAttribute("xmlns:n1", uri); //TODO n2+

        var nodes = doc.SelectNodes(xpath) ??
            throw new ApplicationException(@$"Fail to select nodes in ""{srcFile}""");

        foreach (XmlElement node in nodes)
        {
            node.Prefix = "n1";
        }

        XmlDsigC14NTransform transform = new();
        transform.LoadInput(doc);
        using var canonizedStream = (MemoryStream)transform.GetOutput(typeof(Stream));
        var bytes = canonizedStream.ToArray();
        File.WriteAllBytes(nrzFile, bytes);
    }

    /// <summary>
    /// Создание файла XML со вставленным защитным кодом (ЗК) для первой ноды.
    /// </summary>
    /// <param name="nrzFile">Нормализованный файл XML.</param>
    /// <param name="zkFile">Двоичный файл с посчитанным ЗК.</param>
    /// <param name="sgnFile">Файл XML со вставленным ЗК.</param>
    public static void CreateSigValue(string nrzFile, string zkFile, string sgnFile)
    {
        byte[] sigValue = File.ReadAllBytes(zkFile);

        XmlDocument doc = new();
        doc.Load(nrzFile);

        XmlText ZK64 = doc.CreateTextNode(Convert.ToBase64String(sigValue));
        XmlElement dsigSigValue = doc.CreateElement("dsig", "SigValue", _ns);
        dsigSigValue.AppendChild(ZK64);

        _ = doc.DocumentElement?.PrependChild(dsigSigValue);
        doc.Save(sgnFile);
    }

    /// <summary>
    /// Создание конверта XML со вставленным кодом аутентификации (КА) на внутренний контейнер.
    /// </summary>
    /// <param name="inFileName">Исходный файл XML.</param>
    /// <param name="macValueFileName">Двоичный файл с посчитанным КА.</param>
    /// <param name="outFileName">Файл конверта XML со вставленным КА.</param>
    public static void CreateSigEnvelope(string inFileName, string macValueFileName, string outFileName)
    {
        byte[] macValue = File.ReadAllBytes(macValueFileName);
        byte[] senObject = File.ReadAllBytes(inFileName);

        using var writer = XmlWriter.Create(outFileName, _xmlSettings);

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
    }
}
