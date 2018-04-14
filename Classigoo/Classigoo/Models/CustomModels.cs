using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Classigoo
{
    public class AddsModel
    {
        public List<CustomAdd> Adds { get; set; }

        public int CurrentPageIndex { get; set; }

        public int PageCount { get; set; }
    }

    public class CustomAdd
    {
        public int AddId { get; set; }
        public string Title { get; set; }
        public string CreatedDate { set; get; }
        public string Description { set; get; }
        public string Location { set; get; }
        public string Price { set; get; }
        public string Category { set; get; }
        public RealEstate RealEstate { set; get; }
        public Car Cars { set; get; }
        public Electronic Electronics { set; get; }
        //public List<tbl_Messages> tbl_Messages { get; set; }
    }

    public class MessageDetail
    {
        public int MsgId { get; set; }
        public string FromUserId { get; set; }
        public string ToUserId { get; set; }
        public string FromUser { get; set; }
        public string ToUser { get; set; }
        public int AdId { get; set; }
        public string AdTitle { get; set; }
        public string Message { get; set; }
        public bool IsRed { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }

    }



    public class GroupedMessageDetail
    {
        public string User { get; set; }
        public List<MessageDetail> GroupedMessages { get; set; }
        public int Notification { get { return GroupedMessages.Count(x => x.IsRed == false); } }

    }



    public class Messages
    {
        public List<GroupedMessageDetail> Inbox { get; set; }
        public List<GroupedMessageDetail> Sent { get; set; }
        public List<GroupedMessageDetail> Archive { get; set; }
        public int InboxCount { get; set; }
        public int SentCount { get; set; }
        public int ArchiveCount { get; set; }
        public bool IsBold { get; set; }
        public int TotalMsgNotification { get { return (Inbox.Sum(item => item.Notification) + Sent.Sum(item => item.Notification) + Archive.Sum(item => item.Notification)); } }
    }


}