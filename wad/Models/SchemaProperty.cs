using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace wad.Models
{
    public class SchemaProperty
    {
        public string id { get; set; }
        public string label { get; set; }
        public string comment { get; set; }
        public List<string> domain { get; set; }
        public List<string> ranges { get; set; }
        public static List<SchemaProperty> GetAllProperties()
        {
            HttpClient cl = new HttpClient();
            List<SchemaProperty> listOfProp = new List<SchemaProperty>();
            cl.BaseAddress = new Uri("http://schema.rdfs.org/all-properties.csv");
            var response = cl.GetAsync("http://schema.rdfs.org/all-properties.csv").Result;
            var csvStream = response.Content.ReadAsStreamAsync().Result;
            var reader = new StreamReader(csvStream);
            var nrFields = reader.ReadLine().Split(',').Count();
            while (!reader.EndOfStream)
            {
                var splittedStr = reader.ReadLine().Split(',');
                if (splittedStr.Count() == nrFields)
                {
                    SchemaProperty p = new SchemaProperty()
                    {
                        id = splittedStr[0],
                        label = splittedStr[1],
                        comment = splittedStr[2],
                        domain = splitString(splittedStr[3]),
                        ranges = splitString(splittedStr[4])

                    };
                    listOfProp.Add(p);
                }


            }
            return listOfProp;
        }
        public static List<string> splitString(string str)
        {
            List<string> list = new List<string>();
            if (str.Split(' ').Count() != 1)
            {
                foreach (var s in str.Split(' '))
                    list.Add(s);
            }
            else
            {
                list.Add(str);
            }
            return list;
        }
    }
}