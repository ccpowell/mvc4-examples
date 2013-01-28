using System;
using System.Runtime.Serialization;

namespace DTS.Web.MVC
{
    [DataContract]
    public class JsonServerResponse
    {
        public JsonServerResponse(string error = null, object data = null, bool sessionError = false)
        {
            Error = error;
            Data = data;
            SessionError = sessionError;
        }

        [DataMember(Name = "data")]
        public object Data { get; set; }
        [DataMember(Name = "dataType")]
        public string DataType { get; set; }
        [DataMember(Name = "error")]
        public string Error { get; set; }
        [DataMember(Name = "sessionError")]
        public bool? SessionError { get; set; }
    }
}