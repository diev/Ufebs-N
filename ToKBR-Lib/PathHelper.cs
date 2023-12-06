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

public static class PathHelper
{
    public static string InDir => Path.GetFullPath(AppContext.GetData("IN.Dir") as string ?? ".");
    public static string ZKDir => Path.GetFullPath(AppContext.GetData("ZK.Dir") as string ?? ".");
    public static string KADir => Path.GetFullPath(AppContext.GetData("KA.Dir") as string ?? ".");
    public static string OutDir => Path.GetFullPath(AppContext.GetData("KBR.Dir") as string ?? ".");
    public static string BackupDir => Path.GetFullPath(AppContext.GetData("Backup.Dir") as string ?? ".");
    public static string TempDir => Path.GetFullPath(AppContext.GetData("Temp.Dir") as string ?? ".");

    //public static string InDir => AppContext.GetData("IN.Dir") as string ?? ".";
    //public static string ZKDir => AppContext.GetData("ZK.Dir") as string ?? ".";
    //public static string KADir => AppContext.GetData("KA.Dir") as string ?? ".";
    //public static string OutDir => AppContext.GetData("KBR.Dir") as string ?? ".";
    //public static string BackupDir => AppContext.GetData("Backup.Dir") as string ?? ".";
    //public static string TempDir => AppContext.GetData("Temp.Dir") as string ?? ".";

    public static bool Delete { get; set; }

    static PathHelper()
    {
        if (AppContext.TryGetSwitch("File.Delete", out bool delete))
            Delete = delete;

        Directory.CreateDirectory(InDir);
        Directory.CreateDirectory(ZKDir);
        Directory.CreateDirectory(KADir);
        Directory.CreateDirectory(OutDir);
        Directory.CreateDirectory(BackupDir);
        Directory.CreateDirectory(TempDir);
    }

    //public static string GetZKFileName(string fileName) => Path.Combine(ZKDir, Path.GetFileNameWithoutExtension(fileName) + ".zk.xml");
    //public static string GetKAFileName(string fileName) => Path.Combine(KADir, Path.GetFileNameWithoutExtension(fileName) + ".ka.xml");
    //public static string GetOutFileName(string fileName) => Path.Combine(OutDir, Path.GetFileNameWithoutExtension(fileName) + ".xml");
    //public static string GetNormalFileName(string fileName) => Path.Combine(TempDir, Path.GetFileNameWithoutExtension(fileName) + ".ns.xml");
    //public static string GetNormalFileName(string fileName, int i) => Path.Combine(TempDir, Path.GetFileNameWithoutExtension(fileName) + $".ns{i+1}.xml");
    //public static string GetSignFileName(string fileName) => Path.Combine(TempDir, Path.GetFileNameWithoutExtension(fileName) + ".ns.p7d");
    //public static string GetSignFileName(string fileName, int i) => Path.Combine(TempDir, Path.GetFileNameWithoutExtension(fileName) + $".ns{i+1}.p7d");

    public static string GetZKFileName(string fileName) => Path.Combine(ZKDir, Path.GetFileName(fileName));
    public static string GetKAFileName(string fileName) => Path.Combine(KADir, Path.GetFileName(fileName));
    public static string GetOutFileName(string fileName) => Path.Combine(OutDir, Path.GetFileName(fileName));
    public static string GetNormalFileName(string fileName) => Path.Combine(TempDir, Path.GetFileNameWithoutExtension(fileName) + ".ns.xml");
    public static string GetNormalFileName(string fileName, int i) => Path.Combine(TempDir, Path.GetFileNameWithoutExtension(fileName) + $".ns{i + 1}.xml");
    public static string GetSignFileName(string fileName) => Path.Combine(TempDir, Path.GetFileNameWithoutExtension(fileName) + ".ns.p7d");
    public static string GetSignFileName(string fileName, int i) => Path.Combine(TempDir, Path.GetFileNameWithoutExtension(fileName) + $".ns{i + 1}.p7d");
    public static string GetBackupFileName(string fileName) => Path.Combine(BackupDir, Path.GetFileName(fileName));
}
