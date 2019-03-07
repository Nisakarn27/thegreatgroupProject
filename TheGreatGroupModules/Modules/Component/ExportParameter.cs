using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace TheGreatGroupModules.Modules.Component
{
    public class ExportParameter
    {
        public class PDF
        {
            public string FileName { get; set; }
            public string PathCrystalReport { get; set; }
            public IEnumerable<object> DataSource { get; set; }
            public IList<Parameter> Parameter { get; set; }

        }
        public class TxtFile
        {
            public string FileName { get; set; }
            public IList<string> ListData { get; set; }

        }
        public class Parameter
        {
            public string Name { get; set; }
            public object Value { get; set; }
        }
        public class CSV
        {
            public string FileName { get; set; }
            public PropertyDescriptorCollection HeaderExport { get; set; }
            public IEnumerable<object> BodyExport { get; set; }
        }
        public class Excel
        {
            public string FileName { get; set; }
            public PropertyDescriptorCollection HeaderExport { get; set; }
            public IEnumerable<object> BodyExport { get; set; }
        }
    }
}