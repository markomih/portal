﻿using System;
using System.Collections.Generic;
using System.Linq;
using Data.DTOs;

namespace Data.Entities
{
    public class Message : IMessage
    {
        public List<MessageDTO> GetConversation(int senderId, int receiverId)
        {
            using (var dc = new DataContext())
            {
                return
                    (from o in dc.Messages
                     where (o.SenderId == senderId && o.ReceiverId == receiverId) || (o.SenderId == receiverId && o.ReceiverId == senderId)
                     select new MessageDTO
                        {
                            MessageId = o.MessageId,
                            Text = o.Text,
                            Time = o.Time,
                            SenderId = o.Sender.MemberId,
                            ReceiverId = o.Receiver.MemberId
                        }).ToList();
            }
        }



        public List<MessageDTO> GetConversationOrderedByDate(int senderId, int receiverId)
        {
            using (var dc = new DataContext())
            {
                return
                    (from o in dc.Messages
                     orderby o.Time
                     where (o.SenderId == senderId && o.ReceiverId == receiverId) || (o.SenderId == receiverId && o.ReceiverId == senderId)
                     select new MessageDTO
                     {
                         MessageId = o.MessageId,
                         Text = o.Text,
                         Time = o.Time,
                         SenderId = o.Sender.MemberId,
                         ReceiverId = o.Receiver.MemberId
                     }).ToList();
            }
        }

        public List<MessageDTO> GetConversationOrderedByDateBuffer(int senderId, int receiverId, int numberOfMessages)
        {
            using (var dc = new DataContext())
            {
                return
                    (from o in dc.Messages
                     orderby o.Time
                     where (o.SenderId == senderId && o.ReceiverId == receiverId) || (o.SenderId == receiverId && o.ReceiverId == senderId)
                     select new MessageDTO
                     {
                         MessageId = o.MessageId,
                         Text = o.Text,
                         Time = o.Time,
                         SenderId = o.Sender.MemberId,
                         ReceiverId = o.Receiver.MemberId
                     }).Take(numberOfMessages).ToList();
            }
        }

        public List<MessageDTO> GetConversationWithIds(List<int> ids)
        {
            using (var dc = new DataContext())
            {
                return
                    (from o in dc.Messages
                     orderby o.Time
                     where ids.Contains(o.MessageId)
                     select new MessageDTO
                     {
                         MessageId = o.MessageId,
                         Text = o.Text,
                         Time = o.Time,
                         SenderId = o.Sender.MemberId,
                         ReceiverId = o.Receiver.MemberId
                     }).ToList();
            }
        }

        #region CRUD

        public List<MessageDTO> GetAll()
        {
            using (var dc = new DataContext())
            {
                return
                    (from o in dc.Messages
                     orderby o.Time
                        select new MessageDTO
                        {
                            MessageId = o.MessageId,
                            Text = o.Text,
                            Time = o.Time,
                            SenderId = o.Sender.MemberId,
                            ReceiverId = o.Receiver.MemberId
                        }).ToList();
            }
        }

        public MessageDTO Get(int id)
        {
            using (var dc = new DataContext())
            {
                return (from o in dc.Messages
                    where o.MessageId == id
                    select new MessageDTO
                    {
                        MessageId = o.MessageId,
                        Text = o.Text,
                        Time = o.Time,
                        SenderId = o.Sender.MemberId,
                        ReceiverId = o.Receiver.MemberId
                    }).First();
            }
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(MessageDTO message)
        {
            throw new NotImplementedException();
        }

        public void Save(ref MessageDTO messageDTO)
        {
            using (var dc = new DataContext())
            {
                var recId = messageDTO.ReceiverId;
                var sId = messageDTO.SenderId;
                var message = new DataClasses.Message
                {
                    Receiver = (from m in dc.Members where m.MemberId == recId select m).First(),
                    Sender = (from m in dc.Members where m.MemberId == sId select m).First(),
                    Text = messageDTO.Text,
                    Time = messageDTO.Time,
                    IsDeleted = false
                };

                dc.Messages.Add(message);
                dc.SaveChanges();

                messageDTO.MessageId = message.MessageId;
            }
        }

        #endregion
    }
}