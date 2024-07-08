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

using System.Text.Json;

namespace ToKBR.Lib;

public static class UserHelper
{
    private static readonly Config _config = new();

    static UserHelper()
    {
        string appsettings = Path.ChangeExtension(Environment.ProcessPath!, ".config.json");

        if (File.Exists(appsettings))
        {
            using var read = File.OpenRead(appsettings);
            _config = JsonSerializer.Deserialize<Config>(read) ?? new(); //TODO
        }
    }

    public static bool ZK => GetAllowed(_config.OPR);
    public static bool KA => GetAllowed(_config.CTR);
    public static bool Out => GetAllowed(_config.KBR);

    public static bool GetAllowed(string? value)
    {
        if (string.IsNullOrEmpty(value))
            return false;

        var allowed = value.Split(',');

        return
            allowed.Contains("*") ||
            allowed.Contains(Environment.MachineName) ||
            allowed.Contains(Environment.UserName);
    }
}
