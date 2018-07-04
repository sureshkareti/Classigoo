using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Classigoo.Models
{
    public class MessageDBOperations
    {
        public bool AddChat(Message msg)
        {
            int statusCode = 0;
            try
            {
                using (ClassigooEntities db = new ClassigooEntities())
                {

                    //var distinctMsgColl1 = from msg1 in db.Messages
                    //                       where (msg1.FromUserId == userId || msg1.ToUserId == userId)
                    //                       && (msg1.AdId == msg.AdId)
                    //                       group msg1 by new
                    //                       {
                    //                           msg.AdId
                    //                       } into grp
                    //                       select grp.ToList();

                    //if(distinctMsgColl1.ToList().Count > 0)
                    //{

                    //}

                    db.Messages.Add(msg);
                    statusCode = db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("At AddChat db", ex);
            }
            if (statusCode == 1)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public Guid? GetAddOwnerUserId(int addId)
        {
            Guid? userId = Guid.Empty;
            try
            {
                using (ClassigooEntities db = new ClassigooEntities())
                {
                    Add add = db.Adds.Find(addId);
                    userId = add.UserId;
                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("At Get addownseruserid", ex);

            }
            return userId;
        }

        public List<CustomMessage> GetMyChats(Guid userId)
        {
            List<CustomMessage> myChatColl = new List<CustomMessage>();
            try
            {
                using (ClassigooEntities db = new ClassigooEntities())
                {

                    //var distinctMsgColl1 = from msg in db.Messages
                    //                       where msg.FromUserId == userId || msg.ToUserId == userId
                    //                       group msg by new
                    //                       {
                    //                           msg.AdId
                    //                       } into grp
                    //                       select grp;


                    var distinctMsgColl = from msg in db.Messages
                                          where msg.FromUserId == userId || msg.ToUserId == userId
                                          group msg by new
                                          {
                                              msg.AdId,
                                              msg.RequestorUserId
                                          } into grp
                                          select grp.FirstOrDefault();


                    myChatColl = FillChat(distinctMsgColl, userId);
                    // myChatColl = myChatColl.OrderByDescending(msg => msg.Msg.CreatedOn).ToList();
                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("At GetMyChats", ex);
            }

            return myChatColl;
        }

        public List<CustomMessage> FillChat(IQueryable<Message> distinctMsgColl, Guid userId)
        {
            List<CustomMessage> myChatColl = new List<CustomMessage>();
            try
            {
                foreach (Message msg in distinctMsgColl)
                {
                    CustomMessage chat = new CustomMessage();
                    

                    chat.Msg = msg;

                    CommonDBOperations objCommonOperatoins = new CommonDBOperations();
                    Add add = objCommonOperatoins.GetAdd(msg.AdId.ToString());
                    if (add != null)
                    {
                        chat.AddTitle = add.Title;
                        chat.MessageSentDate = msg.CreatedOn.ToString();
                    }
                    UserDBOperations objUserDbOperations = new UserDBOperations();
                    User user = objUserDbOperations.GetUser(msg.ToUserId);
                    if (user != null)
                    {
                        chat.ToUserName = user.Name;
                        chat.ToUserId = user.UserId;
                    }
                    user = objUserDbOperations.GetUser(msg.FromUserId);
                    if (user != null)
                    {
                        chat.FromUserName = user.Name;
                        chat.FromUserId = user.UserId;
                    }
                    if (msg.FromUserId == userId)
                    {
                        chat.Status = "send";
                        chat.FromUserName = "[Me]";
                    }
                    else if (msg.ToUserId == userId)
                    {
                        chat.Status = "receive";
                        chat.ToUserName = "[Me]";
                    }
                    myChatColl.Add(chat);
                }

            }
            catch (Exception ex)
            {
                Library.WriteLog("At fill chat", ex);
            }
            return myChatColl;
        }

        public List<CustomMessage> LoadChat(Guid userId, int addId,Guid requestorUserId)
        {
            List<CustomMessage> myChatColl = new List<CustomMessage>();
            try
            {
                using (ClassigooEntities db = new ClassigooEntities())
                {

                    var msgColl = db.Messages.Where(msg => msg.FromUserId == userId ||
                           msg.ToUserId == userId).Where(msg => msg.AdId == addId).Where(msg=>msg.RequestorUserId==requestorUserId);
                    myChatColl = FillChat(msgColl, userId);
                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("At LoadChats db", ex);
            }

            return myChatColl;
        }
    }
}