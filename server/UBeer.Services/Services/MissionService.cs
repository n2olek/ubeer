using System;
using System.Linq;

using UBeer.Models;

namespace UBeer.Services
{
    public class MissionService : BaseService<Mission, Entities>
    {
        public MissionService(Entities db) : base(db)
        {
        }

        public string[] GetMissionHashtags(DateTime targetDate)
        {
            DateTime datePartOfTargetDate = targetDate.Date;

            var hashtag = (
                            from
                                o in GetRepo().Missions
                            where
                                o.StartDate <= targetDate &&
                                o.EndDate >= datePartOfTargetDate
                            select
                                o.Hashtag
                        ).FirstOrDefault();

            if (null == hashtag)
                return null;

            return hashtag.Split(',');
        }

        public int FindMissionFromHashtagsInMessage(string message)
        {
            int missionID = 0;
            var missions = (
                                from 
                                    o in GetRepo().Missions
                                select 
                                    o
                            ).ToList();

            // find the right one
            foreach (var mission in missions)
            {
                string[] hashtags = mission.Hashtag.Split(',');

                if (hashtags.All(hashtag => message.ToLower().Contains(hashtag.Trim().ToLower())))
                {
                    // found
                    missionID = mission.ID;
                    break;
                }
            }

            return missionID;
        }

        public int GetMissionID(DateTime targetDate)
        {
            DateTime endDate = targetDate.AddDays(-1);

            var id = (
                            from
                                o in GetRepo().Missions
                            where
                                o.StartDate <= targetDate &&
                                o.EndDate >= endDate
                            select
                                o.ID
                        ).FirstOrDefault();

            return id;
        }

    }
}
