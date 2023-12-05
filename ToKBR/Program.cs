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

using System.Text;
using System.Xml;

using ToKBR.Lib;

namespace ToKBR;

internal class Program
{
    public static bool Delete { get; set; }

    static int Main(string[] args)
    {
        try
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); //enable Windows-1251

            if (args.Length == 0)
                throw new ArgumentNullException(nameof(args), "Не указан параметр 1 (роль обработки): 1 - OPR, 2 - CTR, 3 - KBR.");

            if (!int.TryParse(args[0], out int mode))
                throw new ArgumentOutOfRangeException(nameof(args), args[0], "Роль обработки должна быть целым числом.");

            if (mode < 1 || mode > 3)
                throw new ArgumentOutOfRangeException(nameof(args), mode, "Параметр 1 (роль обработки): 1 - OPR, 2 - CTR, 3 - KBR.");

            if (args.Length == 1)
                throw new ArgumentNullException(nameof(args), "Не указан параметр 2 (файл УФЭБС XML) для обработки.");

            string file = args[1];

            if (!File.Exists(file))
                throw new FileNotFoundException("Файл не найден.", file);

            if (AppContext.TryGetSwitch("File.Delete", out bool delete))
                Delete = delete;

            switch (mode)
            {
                case 1:
                    OprRole(file);
                    break;

                case 2:
                    CtrRole(file);
                    break;

                case 3:
                    KbrRole(file);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(args), mode, "Роль обработки: 1 - OPR, 2 - CTR, 3 - KBR.");
            }

            Console.WriteLine("Ваша роль исполнена.");
            return 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine("ОШИБКА!");

            if (ex.InnerException != null)
                ex = ex.InnerException;

            if (ex is FileNotFoundException notFoundEx)
            {
                Console.WriteLine(notFoundEx.Message);
                Console.WriteLine($@"Файл не найден: ""{notFoundEx.FileName}""");
                return 2;
            }

            if (ex is ArgumentNullException nullEx)
            {
                Console.WriteLine(nullEx.Message);
                Console.WriteLine($"Пустой параметр {nullEx.ParamName}");
                return 3;
            }

            Console.WriteLine(ex.Message);
            return 1;
        }
    }

    static void OprRole(string file)
    {
        Console.WriteLine("Роль 1: операционист OPR.");
        XmlDocument xml = new();
        Console.WriteLine(@$"Чтение файла ""{file}""...");
        xml.Load(file);

        if (xml.DocumentElement?.LocalName == "SigEnvelope")
            throw new ApplicationException("Передайте этот файл KBR (на отправку).");

        if (xml.DocumentElement?.FirstChild?.LocalName == "SigValue")
            throw new ApplicationException("Передайте этот файл CTR (контролеру).");

        string normal = Path.ChangeExtension(file, "normal.xml");

        Console.WriteLine(@$"Нормализация файла ""{file}"" в ""{normal}"".");
        Transformator.Normalize(file, normal);

        Console.WriteLine(@$"Подпишите файл ""{normal}"" (потребуется токен OPR).");
        Console.WriteLine("Нажмите любую клавишу по готовности...");
        Console.ReadKey();

        string p7d = Path.ChangeExtension(normal, "p7d");
        if (!SpkiUtl.CreateSignDetached(normal, p7d))
            throw new ApplicationException("Файл ZK не создан.");

        Console.WriteLine("Нажмите любую клавишу по готовности...");
        Console.ReadKey();

        string zk = Path.ChangeExtension(file, "zk.xml");
        Transformator.CreateSigValue(file, p7d, zk);
        Console.WriteLine(@$"Передайте файл ""{zk}"" CTR (контролеру).");

        if (Delete)
        {
            File.Delete(file);
            File.Delete(normal);
            File.Delete(p7d);
        }
    }

    static void CtrRole(string file)
    {
        Console.WriteLine("Роль 2: контролер CTR.");
        XmlDocument xml = new();
        Console.WriteLine(@$"Чтение файла ""{file}""...");
        xml.Load(file);

        if (xml.DocumentElement?.LocalName == "SigEnvelope")
            throw new ApplicationException("Передайте этот файл KBR (на отправку).");

        if (xml.DocumentElement?.FirstChild?.LocalName != "SigValue")
            throw new ApplicationException("Передайте этот файл OPR (операционисту).");

        Console.WriteLine(@$"Подпишите файл ""{file}"" (потребуется токен CTR).");
        Console.ReadKey();

        string p7d = Path.ChangeExtension(file, "p7d");
        if (!SpkiUtl.CreateSignDetached(file, p7d))
            throw new ApplicationException("Файл KA не создан.");
        Console.ReadKey();

        string ka = Path.ChangeExtension(file, "ka.xml");
        Transformator.CreateSigEnvelope(file, p7d, ka);
        Console.WriteLine(@$"Передайте файл ""{ka}"" KBR (на отправку).");

        if (Delete)
        {
            File.Delete(file);
            File.Delete(p7d);
        }
    }

    static void KbrRole(string file)
    {
        Console.WriteLine("Роль 3: отправка KBR.");
        XmlDocument xml = new();
        Console.WriteLine(@$"Чтение файла ""{file}""...");
        xml.Load(file);

        if (xml.DocumentElement?.LocalName != "SigEnvelope")
            throw new ApplicationException("Этот файл не подготовлен для отправки KBR.");

        Console.WriteLine(@$"Файл ""{file}"" готов к отправке.");
    }
}
