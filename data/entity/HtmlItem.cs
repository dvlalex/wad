using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace data.entity
{
    public class HtmlItem : EntityBase
    {
        public virtual UserSession User { get; set; }
        public virtual List<HtmlSnippet> Snippets { get; set; }
        public HtmlType Type { get; set; }
    }

    public enum HtmlType
    {
        MicroData = 0,
        MicroFormat = 1,
        Rdfa = 2
    }
}
