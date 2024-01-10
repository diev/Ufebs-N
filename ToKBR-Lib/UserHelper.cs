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

public static class UserHelper
{
    public static bool ZK => GetAllowed("Step.1");
    public static bool KA => GetAllowed("Step.2");
    public static bool Out => GetAllowed("Step.3");

    private static bool GetAllowed(string key)
    {
        string? value = AppContext.GetData(key) as string;

        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        string[] allowed = value.Split(',');

        if (allowed.Contains("*") ||
            allowed.Contains(Environment.MachineName) ||
            allowed.Contains(Environment.UserName))
        {
            return true;
        }

        return false;
    }
}
