﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

    public class PostAdd
    {
        [Required]
        public string txtTitle { set; get; }
        public string ddlRentOrSale { set; get; }
        public string hdnCateFristLevel { set; get; }
        public string hdnCateSecondLevel { set; get; }
        public string AVCompany_list { set; get; }
        public string AVOtherCompany { set; get; }
        public string txtAV_Price { set; get; }

        public string CVCompany_list { set; get; }
        public string CVOtherCompany { set; get; }
        public string txtCV_Price { set; get; }

        public string TVCompany_list { set; get; }
        public string TVOtherCompany { set; get; }
        public string txtTV_Price { set; get; }

        public string OtherCompany { set; get; }

        public string PVCompany_list { set; get; }
        public string PVOtherCompany { set; get; }
        public string txtPV_price { set; get; }       
        public string PVModel_list { set; get; }
        public string txtPV_Year { set; get; }
        public string PVfueltype_list { set; get; }
        public string txtPV_kmdriven { set; get; }


        public string txtPro_Price { set; get; }
        public string ddlBedrooms { set; get; }
        public string ddlFurnishing { set; get; }
        public string ddlPostedBy { set; get; }
        public string ddlAvailability { set; get; }
        public string txtSquareFeet { set; get; }
        public string txtSquareYards { set; get; }
        public string txtAcres { set; get; }


        public string txtAddDetails { set; get; }
        public string Img1 { set; get; }
        public string Img2 { set; get; }
        public string Img3 { set; get; }
        public string Img4 { set; get; }
        public string Img5 { set; get; }
        public string Img6 { set; get; }
        public string Img7 { set; get; }
        public string Img8 { set; get; }

        public string Name { set; get; }
        public string PhoneNumber { set; get; }

        public string State { set; get; }
        public string District { set; get; }
        public string Mandal { set; get; }
        public string LocalArea { set; get; }
    }
}