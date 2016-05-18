﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.DataClasses;
using Data.DTOs;

namespace Data.Entities
{
    public class Members
    {
        public static Member AddMember(string gmail, string password, string name, string surname, int roleId,
                                       string nickname = null)
        {
            using (var dc = new DataContext())
            {
                var role = (from r in dc.Roles where r.RoleId == roleId select r).First();

                Member m = new Member
                {
                    Gmail = gmail,
                    Password = password,
                    Name = name,
                    Surname = surname,
                    Nickname = nickname,
                    Role = role,
                    JoinDate = DateTime.Now,
                };

                dc.Members.Add(m);
                dc.SaveChanges();

                return m;
            }
        }

        public static Member GetMember(int memberID)
        {
            using (var dc = new DataContext())
            {
                return (from m in dc.Members where m.MemberId == memberID select m).First();
            }
        }

        public static Member MemberExists(string gmail, string pass)
        {
            using (var dc = new DataContext())
            {
                var find = (from m in dc.Members where m.Gmail == gmail && m.Password == pass select m);
                if (find.Any())
                    return find.First();
                else
                    return null;
            }
                
        }

        public static bool GmailExists(string gmail)
        {
            using (var dc = new DataContext())
            {
                return (from m in dc.Members where m.Gmail == gmail select m).Any();
            }
        }

        public static List<int> GetPermissions(int roleID)
        {
            using (var dc = new DataContext())
            {
                var permissions = (from r in dc.Roles where r.RoleId == roleID select r).First().Permissions.ToList();

                return permissions.Select(x => x.PermissionId).ToList();
            }
        }

        public static List<MemberThumbnailDTO> GetAllMemberThumbnails()
        {
            using (var dc = new DataContext())
            {
                List<Member> members = (from m in dc.Members select m).ToList();

                List<MemberThumbnailDTO> memberthumbnails = new List<MemberThumbnailDTO>();

                foreach (var m in members)
                {
                    memberthumbnails.Add(new MemberThumbnailDTO
                    {
                        Id = m.MemberId,
                        Name = m.Name,
                        Surname = m.Surname,
                        Nickname = m.Nickname,
                        Faculty = m.Faculty
                    });
                }

                return memberthumbnails;
            }
        }

        public static List<int> SearchMembers(string keyword = "")
        {
            using (var dc = new DataContext())
            {
                List<string> searchTerms = new List<string>();
                searchTerms = keyword.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList().ConvertAll(m => m.ToLower());

                // trazimo da li ime, prezime ili nadimak clana sadrzi
                // neku od reci koje su unete kao search parametar
                List<Member> members = new List<Member>();
                if (searchTerms.Count == 0)
                {
                    members = (from m in dc.Members select m).ToList();
                }
                else
                {
                    members = (from m in dc.Members where
                                        searchTerms.Any(s => m.Name.Contains(s)) ||
                                        searchTerms.Any(s => m.Surname.Contains(s)) ||
                                        searchTerms.Any(s => m.Nickname.Contains(s))
                                            select m).ToList();
                }

                List<int> memberIds = members.Select(m => m.MemberId).ToList();

                return memberIds;
            }
        }

        public static void EditProfile(int memberID, string name, string surname, string nickname,
                                       string faculty, DateTime? dob, Enumerations.MemberStatus? status,
                                       string phone, string facebook, string linkedin, string skype)
        {
            using (var dc = new DataContext())
            {
                var mem = (from m in dc.Members where m.MemberId == memberID select m).First();

                mem.Name = name ?? mem.Name;
                mem.Surname = surname ?? mem.Surname;
                mem.Nickname = nickname ?? mem.Nickname;
                mem.Faculty = faculty ?? mem.Faculty;
                mem.Status = status ?? mem.Status;
                mem.DateOfBirth = dob ?? mem.DateOfBirth;
                mem.Phone = phone ?? mem.Phone;
                mem.Facebook = facebook ?? mem.Facebook;
                mem.LinkedIn = linkedin ?? mem.LinkedIn;

                dc.SaveChanges();
            }
        }
    }
}
