using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FamilyTreeBackend.Core.Domain.Entities
{
    public class RequestResponseDataModel
    {
        public string RequestSchema { get; }
        public string RequestHost { get; }
        public string RequestPath { get; }
        public string RequestBody { get; }
        public string ResponseBody { get; }
        private static XmlSerializer xmlSerializer = new XmlSerializer(typeof(RequestResponseDataModel));
        public static RequestResponseDataModel GetDataFromXML(string xml)
        {
            using (var reader = new StringReader(xml))
            {
                var result = (RequestResponseDataModel)xmlSerializer.Deserialize(reader);
                return result;
            }
        }

        public static string GetXMLStringFromData(RequestResponseDataModel data)
        {
            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, data);
                return textWriter.ToString();
            }
        }
    }
}
