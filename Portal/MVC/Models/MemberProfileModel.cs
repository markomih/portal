﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Data.DataClasses;

namespace MVC.Models
{
    public class MemberProfileModel
    {
        public int MemberID { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Nickname { get; set; }
        public string Status { get; set; }
        public DateTime JoinDate { get; set; }
        
        public static MemberProfileModel Load(int memberID)
        {
            Member m = Data.Entities.Members.GetMember(memberID);
            MemberProfileModel model = new MemberProfileModel
            {
                MemberID = m.MemberID,
                Username = m.Username,
                Name = m.Name,
                Surname = m.Surname,
                Nickname = m.Nickname,
                Status = m.Status,
                JoinDate = m.JoinDate,
            };

            return model;
        }
    }
}