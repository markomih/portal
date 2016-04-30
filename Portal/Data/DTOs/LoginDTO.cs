﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs
{
    public class LoginDTO
    {
        public int MemberID { get; set; }
        public string Name { get; set; }
        public string Gmail { get; set; }
        public bool RememberMe { get; set; }
        public int Role { get; set; }
        public List<string> Permissions { get; set; }
        public int loginStatus { get; set; }
    }
}
