using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace wad.Models
{
    public class SchemaType
    {
        public string id { get; set; }
        public string label { get; set; }
        public string comment { get; set; }
        public List<string> ancestors { get; set; }
        public List<string> supertypes { get; set; }
        public List<string> subtypes { get; set; }
        public List<string> properties { get; set; }
        public static List<SchemaType> GetAllTypes()
        {
            HttpClient cl = new HttpClient();
            List<SchemaType> listOfTypes = new List<SchemaType>();
            //cl.BaseAddress = new Uri("http://schema.rdfs.org/all-classes.csv");
            var response = cl.GetAsync("http://schema.rdfs.org/all-classes.csv").Result;
            var csvStream = response.Content.ReadAsStreamAsync().Result;
            var reader = new StreamReader(csvStream);
            var nrFields = reader.ReadLine().Split(',').Count();
            while (!reader.EndOfStream)
            {
                var splittedStr = reader.ReadLine().Split(',');
                if (splittedStr.Count() == nrFields)
                {
                    SchemaType p = new SchemaType()
                    {
                        id = splittedStr[0],
                        label = splittedStr[1],
                        comment = splittedStr[2],
                        ancestors = splitString(splittedStr[3]),
                        supertypes = splitString(splittedStr[4]),
                        subtypes = splitString(splittedStr[5]),
                        properties = splitString(splittedStr[6])

                    };
                    listOfTypes.Add(p);
                }


            }
            return listOfTypes;
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