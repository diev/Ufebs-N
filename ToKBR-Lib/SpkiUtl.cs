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
        string section = nameof(SpkiUtl) + '.';
        string key = section + nameof(Exe);
        Exe = AppContext.GetData(key) as string
            ?? throw new ArgumentNullException(Exe, $"В конфиге не указан параметр {key}");

        if (!File.Exists(Exe))
            throw new FileNotFoundException($"В конфиге ошибка параметра {key}", Exe);

        key = section + nameof(SignDetached);
        SignDetached = AppContext.GetData(key) as string
            ?? throw new ArgumentNullException(SignDetached, $"В конфиге не указан параметр {key}");
    }

    /// <summary>
    /// Создание отсоединенной электронной подписи с помощью утилиты командной строки.
    /// </summary>
    /// <param name="sourceFileName">Исходный файл.</param>
    /// <param name="p7d">Двоичный файл создаваемой электронной подписи.</param>
    /// <param name="overwrite">Переписывать ли файл, если он уже есть.</param>
    /// <returns>Создан ли файл электронной подписи.</returns>
    public static bool CreateSignDetached(string sourceFileName, string p7d, bool overwrite = false)
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
}
