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

using System.Reflection;

namespace ToKBR.Lib;

public static class AppInfo
{
    public static string Banner()
    {
        var assembly = Assembly.GetEntryAssembly();
        var assemName = assembly?.GetName();
        var app = assemName?.Name ?? "App";
        var ver = assemName?.Version?.ToString() ?? Environment.Version.ToString(2); // "8.0"

        return $"{app}, v{ver}";
    }
}
