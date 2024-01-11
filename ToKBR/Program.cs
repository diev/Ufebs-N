#region License
/*
Copyright 2022-2024 Dmitrii Evdokimov
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

using ToKBR.Lib;

namespace ToKBR;

internal class Program
{
    static int Main(string[] args)
    {
        try
        {
            Console.WriteLine(AppInfo.Banner());

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); //enable Windows-1251

            if (args.Length == 0)
                throw new ArgumentNullException(nameof(args), 
                    "Не указан параметр 1 (роль обработки): 1 - OPR, 2 - CTR, 3 - KBR.");

            if (!int.TryParse(args[0], out int mode))
                throw new ArgumentOutOfRangeException(nameof(args), args[0],
                    "Роль обработки должна быть целым числом.");

            if (mode < 1 || mode > 3)
                throw new ArgumentOutOfRangeException(nameof(args), mode,
                    "Параметр 1 (роль обработки): 1 - OPR, 2 - CTR, 3 - KBR.");

            if (args.Length == 1)
                throw new ArgumentNullException(nameof(args), 
                    "Не указан параметр 2 (файл УФЭБС XML) для обработки.");

            string file = args[1];

            if (!File.Exists(file))
                throw new FileNotFoundException(
                    "Файл не найден.", file);

            if (AppContext.TryGetSwitch("File.Delete", out bool delete))
            {
                Console.WriteLine("Внимание: промежуточные файлы будут " + (delete ? "удаляться!" : "оставаться!"));
                Transformator.Delete = delete;
            }

            string ext = Path.GetExtension(file);

            switch (mode)
            {
                case 1:
                    Console.WriteLine(@$"Роль 1: операционист OPR - установка ЗК в ""{file}""");
                    Transformator.OprCheck(file);
                    string zk = Path.ChangeExtension(file, "zk" + ext);
                    Transformator.OprRole(file, zk);
                    Console.WriteLine(@$"Передайте файл ""{zk}"" Контролеру.");
                    break;

                case 2:
                    Console.WriteLine(@$"Роль 2: контролер CTR - установка КА в ""{file}""");
                    Transformator.CtrCheck(file);
                    string ka = Path.ChangeExtension(file, "ka" + ext);
                    Transformator.CtrRole(file, ka);
                    Console.WriteLine(@$"Передайте конверт ""{ka}"" на отправку в КБР.");
                    break;

                case 3:
                    Console.WriteLine(@$"Роль 3: отправка KBR - проверка КА в ""{file}""");
                    Transformator.KbrCheck(file);
                    Console.WriteLine(@$"Конверт ""{file}"" с КА готов к отправке.");
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(args), mode, 
                        "Роль обработки: 1 - OPR, 2 - CTR, 3 - KBR.");
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
                Console.WriteLine(@$"Пустой параметр ""{nullEx.ParamName}""");
                return 3;
            }

            Console.WriteLine(ex.Message);
            return 1;
        }
    }
}
