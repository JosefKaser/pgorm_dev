﻿using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.ComponentModel;
using System.Reflection;

namespace PGORM.CodeBuilder
{
    public class AssemblyInfoData
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Configuration { get; set; }
        public string Company { get; set; }
        public string Product { get; set; }
        public string Copyright { get; set; }
        public string Trademark { get; set; }
        public string Culture { get; set; }
        public string Guid { get; set; }
        public string Version { get; set; }
        public string FileVersion { get; set; }

        public AssemblyInfoData()
        {
            Company = (string)Microsoft.Win32.Registry.GetValue(@"HKEY_LOCAL_MACHINE\Software\Microsoft\Windows NT\CurrentVersion", "RegisteredOrganization", "none");
            Copyright = string.Format("Copyright © {0} {1}", Company, DateTime.Now.Year);
            Guid = System.Guid.NewGuid().ToString();
            Version = "1.0.0.0";
            FileVersion = "1.0.0.0";
            Guid = System.Guid.NewGuid().ToString();
        }
    }
}