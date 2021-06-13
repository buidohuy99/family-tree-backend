using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FamilyTreeBackend.Core.Domain.Entities
{
    public class RequestResponseDataModel
    {
        [DataMember]
        public string RequestSchema { get; set; }
        [DataMember]
        public string UserAgent { get; set; }
        [DataMember]
        public string UserId { get; set; }
        [DataMember]
        public string RequestHost { get; set; }
        [DataMember]
        public string RequestPath { get; set; }
        [DataMember]
        public string RequestHeader { get; set; }
        [DataMember]
        public string RequestBody { get; set; }
        [DataMember]
        public int StatusCode { get; set; }
        [DataMember]
        public string ResponseBody { get; set; }
        [DataMember]
        public string DateCreated { get; set; }

        private static XmlSerializer xmlSerializer = new XmlSerializer(typeof(RequestResponseDataModel));

       
        public static string GetXMLStringFromData(RequestResponseDataModel data)
        {
            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, data);
                return textWriter.ToString();
            }
        }

        public static RequestResponseDataModel GetDataFromXMLString(string xml)
        {
            using (TextReader reader = new StringReader(xml))
            {
                var result = xmlSerializer.Deserialize(reader) as RequestResponseDataModel;
                return result;
            }
        }
    }
}
