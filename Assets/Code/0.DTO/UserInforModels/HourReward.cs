using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Assets.Code._0.DTO.UserInforModels
{
    public class HourReward
    {
        public string UserID { get; set; }
        public DateTime LastUpdate { get; set; }
        public int UpdateTime { get; set; }
        public int UpdateRound { get; set; }
        public string NextReward { get; set; }
    }
}
