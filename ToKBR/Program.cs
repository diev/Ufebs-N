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
            {
                Console.WriteLine("Промежуточные файлы будут " + (delete ? "удаляться." : "оставаться."));
                Transformator.Delete = delete;
            }

            switch (mode)
            {
                case 1:
                    Console.WriteLine("Роль 1: операционист OPR.");
                    Transformator.OprCheck(file);
                    Transformator.OprRole(file);
                    Console.WriteLine("Передайте файл ZK.xml Контролеру.");
                    break;

                case 2:
                    Console.WriteLine("Роль 2: контролер CTR.");
                    Transformator.CtrCheck(file);
                    Transformator.CtrRole(file);
                    Console.WriteLine("Передайте файл ZK.KA.xml на отправку в КБР.");
                    break;

                case 3:
                    Console.WriteLine("Роль 3: отправка KBR.");
                    Transformator.KbrCheck(file);
                    Console.WriteLine("Файл ZK.KA.xml готов к отправке.");
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
}
