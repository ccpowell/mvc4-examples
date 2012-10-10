using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DRCOG.Domain.Interfaces;
using DRCOG.Domain.Models;
using System.IO;
using System.Xml;

namespace DRCOG.TIP.Services
{
    public class XMLService
    {
        protected readonly ITransportationRepository RtpProjectRepository;

        public XMLService(ITransportationRepository repo)
        {
            RtpProjectRepository = repo;
        }

        public Scheme GetScheme(int id)
        {
            return RtpProjectRepository.GetLRSScheme(id);
        }

        public LRSRecord GetSegmentLRSData(int schemeId, int segmentId)
        {
            return RtpProjectRepository.GetSegmentLRSData(schemeId, segmentId);
        }
        
        public LRSRecord LoadRecord(int schemeId, int lrsId)
        {
            return GetSegmentLRSData(schemeId, lrsId);
        }

        public LRSRecords LoadRecords(int schemeId, int segmentId)
        {
            return RtpProjectRepository.GetSegmentLRSSummary(schemeId, segmentId);
        }

        public string GenerateXml(Scheme scheme, LRSRecord record)
        {
            MemoryStream ms = new MemoryStream();
            XmlWriter xw = XmlWriter.Create(ms);

            xw.WriteStartDocument();
            xw.WriteStartElement("item");

            if (scheme != null)
            {
                foreach (SchemeRecord col in scheme)
                {
                    xw.WriteStartElement(col.ColumnName);
                    string value = record.Columns.SingleOrDefault(x => x.Key.Equals(col.ColumnName)).Value;
                    xw.WriteString(value);
                    xw.WriteEndElement();
                }
            }

            xw.WriteEndElement();
            xw.WriteEndDocument();

            xw.Flush();

            Byte[] buffer = new Byte[ms.Length];
            buffer = ms.ToArray();

            string xmlOutput = Encoding.UTF8.GetString(buffer);

            return xmlOutput;
        }

    }

    


}
