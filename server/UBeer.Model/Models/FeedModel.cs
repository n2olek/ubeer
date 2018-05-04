using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Configuration;

namespace UBeer.Models
{
    public class MediaPost
    {
        public string ID;
        public string ImageURL;
        public string OriginURL;
        public string PostURL;
        public Nullable<int> LikeCount;
        public Nullable<int> CommentCount;
        public string Message;
        public string Source;
        public string MobilePhone;
        public string ContentType;
        public int MissionID;
        public DateTime CreateDate;
        public List<string> Comments;
    }
}
