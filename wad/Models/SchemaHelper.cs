using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wad.Models
{
    public class SchemaHelper
    {
        public static List<SchemaProperty> Properties
        {
            get
            {
                if (HttpContext.Current.Application["props"] == null)
                {
                    HttpContext.Current.Application["props"] = SchemaProperty.GetAllProperties();
                }
                return HttpContext.Current.Application["props"]as List<SchemaProperty>;
            }
            set
            { HttpContext.Current.Application["props"] = SchemaProperty.GetAllProperties(); }
        }
        public static List<SchemaType> Types
        {
            get
            {
                if (HttpContext.Current.Application["type"] == null)
                {
                    HttpContext.Current.Application["type"] = SchemaType.GetAllTypes();
                }
                return HttpContext.Current.Application["type"] as List<SchemaType>;
            }
            set { HttpContext.Current.Application["type"] = SchemaType.GetAllTypes(); }
        }

        public static List<string> GetAllPropsForType(string typeid)
        {
            var props = new List<string>();
            SchemaType currType = Types.Where(t => t.id.ToLower() == typeid).FirstOrDefault();
            props.AddRange(currType.properties.Where(t => t != ""));
            var parentTypes = currType.ancestors;
            foreach (var type in parentTypes.Where(t => t != ""))
            {
                props.AddRange(Types.Where(t => t.id == type).FirstOrDefault().properties);
            }
            return props;
        }
        public static List<string> GetAllTypes()
        {
            return Types.Select(t => t.id).ToList();
        }

        public static List<string> GetAllTypesAndProperties()
        {
            List<string> types = new List<string>();
            types.AddRange(Types.Select(t => t.id));
            types.AddRange(Properties.Select(p => p.id));

            return types;
        }
        
    }
    
}