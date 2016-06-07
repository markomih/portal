﻿using Data.DataClasses;
using Data.DTOs;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Polls
    {
        // od najnovije kreiranih ka starijim sortira
        public static List<Poll> GetAllPolls(DataContext dc = null)
        {
            using (dc = dc ?? new DataContext())
            {
                return (from p in dc.Polls select p).OrderByDescending(p => p.StartDate).ToList();
            }
        }

        public static Poll GetPollAt(int pollId, DataContext dc = null)
        {
            using (dc = dc ?? new DataContext())
            {
                return (from p in dc.Polls where p.PollId == pollId select p).Include(p => p.PollCreator).First();
            }
        }

        public static List<PollOption> GetPollOptionsForPoll(int pollId, DataContext dc = null)
        {
            using (dc = dc ?? new DataContext())
            {
                return (from p in dc.PollOptions where p.PollId == pollId select p).ToList();
            }
        }

        public static List<Member> GetVotersForPollOption(int pollOptionId, DataContext dc = null)
        {
            using (dc = dc ?? new DataContext())
            {
                return (from m in dc.MemberPollOptions where m.PollOptionId == pollOptionId select m.Member).ToList();
            }
        }

        public static void UpdatePollState(int pollId, Enumerations.PollState newState, DataContext dc = null)
        {
            using (dc = dc ?? new DataContext())
            {
                Poll poll = (from p in dc.Polls where p.PollId == pollId select p).First();
                poll.State = newState;
                dc.SaveChanges();
            }
        }

        public static void ClosePoll(int pollId, DataContext dc = null)
        {
            UpdatePollState(pollId, Enumerations.PollState.zatvoren, dc);
        }

        public static bool AddVote(int memberId, int pollOptionId, DataContext dc = null)
        {
            using (dc = dc ?? new DataContext())
            {
                Member m = dc.Members.Where(x => x.MemberId == memberId).First();
                PollOption po = dc.PollOptions.Where(x => x.PollOptionId == pollOptionId).First();
                MemberPollOption newVote = new MemberPollOption
                {
                    Member = m,
                    PollOption = po,
                };

                var addedVote = dc.MemberPollOptions.Add(newVote);

                dc.SaveChanges();
                return newVote == addedVote;
            }
        }

        public static bool RemoveVote(int memberId, int  pollOptionId, DataContext dc = null)
        {
            using (dc = dc ?? new DataContext())
            {
                var removeVote = dc.MemberPollOptions.Where(x => x.MemberId == memberId && x.PollOptionId == pollOptionId).First();
                var voteRemoved = dc.MemberPollOptions.Remove(removeVote);

                dc.SaveChanges();
                return removeVote == voteRemoved;
            }
        }
    }
}
