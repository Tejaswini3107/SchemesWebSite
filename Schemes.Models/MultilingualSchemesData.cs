using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schemes.Models
{
    public class MultilingualSchemesData : Base
    {
        [Key]
        public int MultilingualSchemesDataID { get; set; }
        public int SchemesDetailID { get; set; }
        public string LangCode { get; set; }
        public string? AvaliableFor { get; set; }
        public string? NameOftheScheme { get; set; }
        public string? Description { get; set; }
        public string? EligibilityCriteria { get; set; }
        public string? Benefits { get; set; }
        public string? Area { get; set; }
        public string? DocumentsRequired { get; set; }
        public string? ApplyAndLink { get; set; }
        public bool IsActive { get; set; }
    }
}
