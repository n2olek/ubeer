using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UBeer.Models
{
    public class PostModel
    {
        public string Message { get; set; }
        public string OriginURL { get; set; }
        public string PostURL { get; set; }
        public DateTime PostDate { get; set; }
        public string MobilePhone { get; set; }
        public Nullable<int> MissionID { get; set; }
    }
}
