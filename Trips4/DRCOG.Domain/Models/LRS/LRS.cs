using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Collections;

namespace DRCOG.Domain.Models
{
    public class LRS
    {
        public string RouteName { get; set; }
        public double BeginMeasure { get; set; }
        public double EndMeasure { get; set; }
        public string ImprovementType { get; set; }
        public string Comments { get; set; }
        public int NetworkId { get; set; }
        public int Offset { get; set; }

    }

    public class LRSRecord
    {
        public int Id { get; set; }
        public Scheme Scheme { get; set; }
        public Dictionary<string,string> Columns { get; set; }
    }

    public enum SchemeName
    {
        LRSProjects = 1
    }

    public class SchemeRecord
    {
        public int Id { get; set; }
        public string ColumnName { get; set; }
        public string FriendlyName { get; set; }
        public string DataType { get; set; }
        public string DisplayType { get; set; }
        public int MaxLenght { get; set; }
        public string ColumnDefault { get; set; }
        public bool IsNullable { get; set; }
        public bool IsRequired { get; set; }
    }


    public class Scheme : System.Collections.CollectionBase
    {
        public int Add(SchemeRecord item)
        {
            return this.List.Add(item);
        }

        public void Remove(int index)
        {
            if (index > Count - 1 || index < 0)
            {

            }
            else
            {
                this.List.RemoveAt(index);
            }
        }

        public SchemeRecord Item(int index)
        {
            return (SchemeRecord) List[index];
        }
    }

    public class LRSRecords : System.Collections.CollectionBase
    {
        public int Add(LRSRecord item)
        {
            return this.List.Add(item);
        }

        public void Remove(int index)
        {
            if (index > Count - 1 || index < 0)
            {

            }
            else
            {
                this.List.RemoveAt(index);
            }
        }

        public bool Contains(LRSRecord item)
        {
            return this.List.Contains(item);
        }

        public List<int> GetRecordIds()
        {
            List<int> list = new List<int>();

            for (int i = 0; i < this.List.Count; i++)
            {
                list.Add(((LRSRecord)List[i]).Id);
            }
            return list;
        }

        public LRSRecord Item(int index)
        {
            return (LRSRecord)List[index];
        }

    }

}
