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

namespace ToKBR.Lib;

/// <summary>
/// Класс работы с утилитой командной строки.
/// </summary>
public static class SpkiUtl
{
    /// <summary>
    /// Полный путь к утилите командной строки.
    /// </summary>
    public static string Exe { get; set; }

    /// <summary>
    /// Командная строка для создания отсоединенной электронной подписи.
    /// </summary>
    public static string SignDetached { get; set; }

    static SpkiUtl()
    {
        Exe = @"C:\Program Files\MDPREI\spki\spki1utl.exe";
        SignDetached = "-sign -data {0} -out {1} -detached";

        //if (!File.Exists(Exe))
        //{
        //    Console.WriteLine("DEMO режим - файлы не подписываются!"); //TODO

        //    Exe = @"C:\Windows\System32\cmd.exe";
        //    SignDetached = "/c copy {0} {1}";
        //}
    }

    /// <summary>
    /// Создание отсоединенной электронной подписи с помощью утилиты командной строки.
    /// </summary>
    /// <param name="sourceFileName">Исходный файл.</param>
    /// <param name="p7d">Двоичный файл создаваемой электронной подписи.</param>
    /// <param name="overwrite">Переписывать ли файл, если он уже есть.</param>
    /// <returns>Создан ли файл электронной подписи.</returns>
    public static bool CreateSignDetached(string sourceFileName, string p7d, bool overwrite = true)
    {
        if (File.Exists(Exe))
        {
            if (File.Exists(p7d))
            {
                if (overwrite)
                {
                    File.Delete(p7d);
                }
                else
                {
                    return true;
                }
            }

            string cmdline = string.Format(SignDetached, sourceFileName, p7d);
            //Task.Run(async () =>
            //{
            //    await Executor.StartAsync(Exe, cmdline); 
            //});
            Executor.Start(Exe, cmdline);

            return File.Exists(p7d);
        }

        //DEMO
        Console.WriteLine("DEMO режим - файлы не подписываются!"); //TODO
        File.WriteAllText(p7d, "DEMO-SIGN");
        return true;
    }
}
