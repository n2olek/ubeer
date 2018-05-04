using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;

using UBeer.Models;

namespace UBeer.Services
{
    public class HeartbeatService : BaseService<Heartbeat, Entities>
    {
        IMapper mapper;
        public HeartbeatService(Entities db) : base(db)
        {
        }

        public Heartbeat GetHeartbeat(string serviceName)
        {
            var heartBeat = (from o in GetRepo().Heartbeats
                             where o.ServiceName == serviceName
                             select o).FirstOrDefault();

            return heartBeat;
        }

        public void SaveHeartbeat(string serviceName)
        {
            if (serviceName == "")
                return;

            Heartbeat heartbeatRecordFromDB = GetHeartbeat(serviceName);
            if (heartbeatRecordFromDB == null)
            {
                // new, create the new record
                Heartbeat heartBeat = new Heartbeat();
                heartBeat.ServiceName = serviceName;
                heartBeat.UpdateDate = (new S9.Utility.Environment()).Now();

                Insert(heartBeat);
                Save();
            }
            else
            {
                // already exist, update the existing records if need
                heartbeatRecordFromDB.UpdateDate = (new S9.Utility.Environment()).Now();

                Update(heartbeatRecordFromDB);
                Save();
            }
        }

    }
}
