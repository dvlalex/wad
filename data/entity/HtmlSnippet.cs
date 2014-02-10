using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace data.entity
{
    public class HtmlSnippet : EntityBase
    {
        public string HtmlCode { get; set; }
        public int DivId { get; set; } 

    }
}
