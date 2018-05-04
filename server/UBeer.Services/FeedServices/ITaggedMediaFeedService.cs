using System;
using System.Collections.Generic;

using UBeer.Models;

namespace UBeer.Services
{
    public interface ITaggedMediaFeedService
    {
        List<MediaPost> GetRecentTaggedMediaPosts(string[] tags);
    }
}
