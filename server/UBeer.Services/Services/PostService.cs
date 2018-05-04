using System;
using System.Collections.Generic;
using System.Linq;

using AutoMapper;

using UBeer.Models;

namespace UBeer.Services
{
    public class PostService : BaseService<Models.Post, Entities>
    {
        IMapper mapper;
        public PostService(Entities db) : base(db)
        {
            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.CreateMap<MediaPost, Models.Post>()
                    .ForMember(f => f.SocialType, dts => dts.MapFrom(s => s.Source))
                    .ForMember(f => f.PostDate, dts => dts.MapFrom(s => s.CreateDate));
            });
            mapper = config.CreateMapper();
        }

        public List<Models.Post> GetPosts()
        {
            var lst = (
                        from
                            o in GetRepo().Posts
                        where
                            o.IsDeleted == false
                        orderby
                            o.PostDate descending
                        select
                             o
                    ).ToList();

            return lst;
        }

        public List<Models.Post> GetPosts(int limitPost)
        {
            var lst = (
                        from
                            o in GetRepo().Posts
                        where
                            o.IsDeleted == false
                        orderby
                            o.PostDate descending
                        select
                             o
                    ).Take(limitPost).ToList();

            return lst;
        }

        public List<Models.Post> GetSocialPosts(int limitPost)
        {
            string source = UBeerEnum.SOURCE.UploadFile.ToString();
            var lst = (
                        from
                            o in GetRepo().Posts
                        where
                            o.IsDeleted == false &&
                            o.SocialType != source
                        orderby
                            o.PostDate descending
                        select
                             o
                    ).Take(limitPost).ToList();

            return lst;
        }

        public UBeer.Models.Post GetPost(string postID, string source)
        {
            var post = (
                        from
                            o in GetRepo().Posts
                        where
                            o.ID == postID && o.SocialType == source
                        select
                            o
                        ).FirstOrDefault();

            return post;
        }

        public void SavePosts(List<MediaPost> posts)
        {
            if (posts == null)
                return;

            posts.OrderByDescending(o => o.CreateDate).ToList().ForEach(o => SavePost(o));
        }

        public void SavePost(MediaPost post)
        {
            if (post == null)
                return;

            Models.Post postRecordFromDB = GetPost(post.ID, post.Source);
            if (postRecordFromDB == null)
            {
                // new, create the new record
                Models.Post postNew = mapper.Map<Models.Post>(post);
                postNew.UpdateDate = (new S9.Utility.Environment()).Now();
                postNew.IsDeleted = false;

                Insert(postNew);
                Save();
            }
            else
            {
                // already exist, update the existing records if need
                if (NeedUpdate(postRecordFromDB, post))
                {
                    postRecordFromDB.MissionID = post.MissionID;
                    postRecordFromDB.ImageURL = post.ImageURL;
                    postRecordFromDB.OriginURL = post.OriginURL;
                    postRecordFromDB.PostURL = post.PostURL;
                    postRecordFromDB.LikeCount = post.LikeCount;
                    postRecordFromDB.CommentCount = post.CommentCount;
                    postRecordFromDB.UpdateDate = (new S9.Utility.Environment()).Now();
                    postRecordFromDB.IsDeleted = false;

                    Update(postRecordFromDB);
                    Save();
                }
            }
        }

        public bool DeletePost(string postID, string source)
        {
            UBeer.Models.Post postRecordFromDB = GetPost(postID, source);
            if (postRecordFromDB == null)
                return false;

            postRecordFromDB.IsDeleted = true;
            Update(postRecordFromDB);
            Save();
            return true;
        }

        private bool NeedUpdate(Models.Post postRecordFromDB, MediaPost post)
        {
            return (
                    (postRecordFromDB.LikeCount.GetValueOrDefault() != post.LikeCount.GetValueOrDefault()) ||
                    (postRecordFromDB.CommentCount.GetValueOrDefault() != post.CommentCount.GetValueOrDefault()) ||
                    (postRecordFromDB.OriginURL != post.OriginURL) ||
                    (postRecordFromDB.PostURL != post.PostURL) ||
                    (postRecordFromDB.MissionID != post.MissionID)
                  );
        }

    }
}
