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
                 db.Messages.Add(msg);
                 statusCode=  db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("At AddChat db", ex);
            }
           if(statusCode==1)
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
                    userId= add.UserId;
                }
            }
            catch(Exception ex)
            {
                Library.WriteLog("At Get addownseruserid",ex);

            }
            return userId;
        }

        public List<CustomMessage> GetMyChats(Guid userId)
        {
            List<CustomMessage> myChatColl = new List<CustomMessage>();
            try
            {
                List<Message> myMsgColl = new List<Message>();
                using (ClassigooEntities db = new ClassigooEntities())
                {
                    myMsgColl = (from msg in db.Messages
                                  where (msg.FromUserId == userId) ||
                                  (msg.ToUserId == userId)
                                  select msg).OrderBy(msg => msg.CreatedOn).ToList();


                }
                foreach (Message msg in myMsgColl)
                {
                    CustomMessage chat = new CustomMessage();
                    chat.Msg = msg;
                    CommonDBOperations objCommonOperatoins = new CommonDBOperations();
                    Add add=   objCommonOperatoins.GetAdd(msg.AdId.ToString());
                    if(add!=null)
                    {
                        chat.AddTitle = add.Title;
                    }
                    UserDBOperations objUserDbOperations = new UserDBOperations();
                    User user = objUserDbOperations.GetUser(msg.ToUserId);
                    if(user!=null)
                    {
                        chat.ToUserName = user.Name;
                    }
                    myChatColl.Add(chat);
                }

            }
            catch(Exception ex)
            {
                Library.WriteLog("At GetMyChats", ex);
            }

            return myChatColl;
        }
    }
}