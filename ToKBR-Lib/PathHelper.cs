#region License
/*
Copyright 2022-2025 Dmitrii Evdokimov
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

using System.Text.Json;

namespace ToKBR.Lib;

public static class PathHelper
{
    private static readonly Config _config = new();

    static PathHelper()
    {
        string appsettings = Path.ChangeExtension(Environment.ProcessPath!, ".config.json");

        if (File.Exists(appsettings))
        {
            using var read = File.OpenRead(appsettings);
            _config = JsonSerializer.Deserialize<Config>(read) ?? new(); //TODO
        }
    }

    public static string IN => _config.IN ?? ".";
    public static string ZK => _config.ZK ?? ".";
    public static string KA => _config.KA ?? ".";
    public static string OUT => _config.OUT ?? ".";
    public static string Temp => Directory.CreateDirectory(_config.Temp ?? ".").FullName;
    public static string Backup => Directory.CreateDirectory(
        Path.Combine(_config.Backup ?? ".", DateTime.Now.ToString("yyyy-MM-dd"))).FullName;

    public static string GetZKFileName(string fileName)
        => Path.Combine(ZK, Path.GetFileName(fileName));

    public static string GetKAFileName(string fileName)
        => Path.Combine(KA, Path.GetFileName(fileName));

    public static string GetOutFileName(string fileName)
        => Path.Combine(OUT, Path.GetFileName(fileName));

    public static string GetNormalFileName(string fileName)
        => Path.Combine(Temp, Path.GetFileNameWithoutExtension(fileName) + ".ns.xml");

    public static string GetNormalFileName(string fileName, int i)
        => Path.Combine(Temp, Path.GetFileNameWithoutExtension(fileName) + $".ns{i + 1}.xml");

    public static string GetSignFileName(string fileName)
        => Path.Combine(Temp, Path.GetFileNameWithoutExtension(fileName) + ".ns.p7d");

    public static string GetSignFileName(string fileName, int i)
        => Path.Combine(Temp, Path.GetFileNameWithoutExtension(fileName) + $".ns{i + 1}.p7d");

    public static string GetBackupFileName(string fileName)
        => Path.Combine(Backup, Path.GetFileName(fileName));

    public static string GetTempFileName(string fileName)
        => Path.Combine(Temp, Path.GetFileName(fileName));

    public static bool AreSame(string fileName1, string fileName2)
        => Path.GetFullPath(fileName1).Equals(Path.GetFullPath(fileName2),
            StringComparison.OrdinalIgnoreCase);
}
