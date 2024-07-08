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

public class Config
{
    public string? IN { get; set; }
    public string? ZK { get; set; }
    public string? KA { get; set; }
    public string? OUT { get; set; }
    public string? Backup { get; set; }
    public string? Temp { get; set; }
    public bool Delete { get; set; }
    public string? OPR { get; set; }
    public string? CTR { get; set; }
    public string? KBR { get; set; }
}
