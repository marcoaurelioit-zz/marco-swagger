using System.Collections.Generic;

namespace Marco.Swagger
{
    public class ApiMetaData
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string DefaultVersion { get; set; }
        public IDictionary<string, string> VersionIngDescriptions { get; set; }
    }
}