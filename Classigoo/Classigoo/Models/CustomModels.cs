using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Classigoo
{
    public class AddsModel
    {
        public List<CustomAdd> Adds { get; set; }

        public List<List<CustomAdd>> AddsGrid { get; set; }

        public int CurrentPageIndex { get; set; }

        public int PageCount { get; set; }

       
        //public SubCategoryCount SubCatCount { set; get; }
    }
    public class SubCategoryCount
    {
        public AV AVSubCat { set; get; }
        public CV CVSubCat { set; get; }
        public TV TVSubCat { set; get; }
        public PV PVSubCat { set; get; }
        public RE RESubCat { set; get; }
    }
    public class AV
    {
        public int BorewellMachineCount { set; get; }
        public int TractorsCount { set; get; }
        public int DozerCount { set; get; }
        public int HarvesterCount { set; get; }
        public int BackhoeLoaderCount { set; get; }
        public int ExcavatorsCount { set; get; }
        //Tractors Dozer Combine Harvester Backhoe Loader Excavators
    }
    public class CV
    {
        //Tractors Dozers Backhoe_Loader Excavators Wheel Loader Crane Transit Mixer Soil Compactor Tippers
        public int TractorsCount { set; get; }
        public int DozerCount { set; get; }
        public int BackhoeLoaderCount { set; get; }
        public int ExcavatorsCount { set; get; }
        public int WheelLoaderCount { set; get; }
        public int CraneCount { set; get; }
        public int TransitMixerCount { set; get; }
        public int SoilCompactorCount { set; get; }
        public int TippersCount { set; get; }
    }

    public class TV
    {
        public int Autos3wheelerCount { set; get; }
        public int MiniTrucks4wheelerCount { set; get; }
        public int LorryTrucksCount { set; get; }
        public int DCMTrucksCount { set; get; }
        //Autos - 3 wheeler  Mini Trucks - 4 wheeler Lorry Trucks DCM Trucks
    }
    public class PV
    {
        public int AutosCount { set; get; }
        public int CarsCount { set; get; }
        public int TravelVansCount { set; get; }
        public int BikesCount { set; get; }
        //Autos Cars Travel Vans Bikes
    }
    public class RE
    {
        public int ApartmentsCount { set; get; }
        public int PlotsLandCount { set; get; }
        public int AgriculturalLandCount { set; get; }
        public int ShopsOfficesCount { set; get; }
        public int IndependentHousesVillasCount { set; get; }
        public int HostelsPGCount { set; get; }
        //Apartments Plots/Land Agricultural Land  Shops & Offices Independent Houses & Villas Hostels & PG
    }

    public class PreviewAdd
    {
        public CustomAdd Add { set; get; }
        public List<List<CustomAdd>> SimilarAddColl { set; get; }
    }
    public class CustomAdd
    {
        public int AddId { get; set; }
        public string Title { get; set; }
        public string CreatedDate { set; get; }
        public string Description { set; get; }
        public string Location { set; get; }
        public int? Price { set; get; }
        public string Status { set; get; }
        public string Category { set; get; }
        public string ImgUrlPrimary { get; set; }
        public string ImgUrlSeconday { get; set; }
        public string ImgUrlThird { get; set; }
        public string ImgUrlFourth { get; set; }
        public string Company { get; set; }
        public string Model { get; set; }
        public string ManufacturingYear { get; set; }
        public RealEstate RE { set; get; }
        public PassengerVehicle PV { set; get; }
    }
    public class FiterOptions
    {
        public string Category { set; get; }
        public string Location { set; get; }
        public string SearchKeyword { set; get; }
        public string Type { set; get; }
        public string SubCategory { set; get; }
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
        public string AddId { set; get; }
        public string txtTitle { set; get; }
        public string ddlRentOrSale { set; get; }
        public string hdnCateFristLevel { set; get; }
        public string hdnCateSecondLevel { set; get; }

        public string AVCompany_list { set; get; }
        public string AVOtherCompany { set; get; }
        //public int txtAV_Price { set; get; }
        public string txtAV_Price { set; get; }
        public string txtAV_MYear { set; get; }
        public string txtAV_Model { set; get; }

        public string CVCompany_list { set; get; }
        public string CVOtherCompany { set; get; }
        public string txtCV_MYear { set; get; }
        public string txtCV_Model { set; get; }
        //public int txtCV_Price { set; get; }
        public string txtCV_Price { set; get; }


        public string TVCompany_list { set; get; }
        public string TVOtherCompany { set; get; }
        public string txtTV_MYear { set; get; }
        public string txtTV_Model { set; get; }
        //public int txtTV_Price { set; get; }
        public string txtTV_Price { set; get; }


        public string OtherCompany { set; get; }

        public string PVCompany_list { set; get; }
        public string PVOtherCompany { set; get; }

        public string txtPV_Model { set; get; }
        //public int txtPV_price { set; get; }
        public string txtPV_price { set; get; }




        public string PVModel_list { set; get; }
        public string txtPV_Year { set; get; }
        public string PVfueltype_list { set; get; }
        //public int txtPV_kmdriven { set; get; }
        public string txtPV_kmdriven { set; get; }



        //public int txtPro_Price { set; get; }
        public string txtPro_Price { set; get; }

        public string ddlBedrooms { set; get; }
        public string ddlFurnishing { set; get; }
        public string ddlPostedBy { set; get; }
        public string ddlAvailability { set; get; }
        //public int txtSquareFeet { set; get; }
        public string txtSquareFeet { set; get; }

        //public int txtSquareYards { set; get; }
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

    public class Constants
    {
        public const string PendingSatus = "Pending";
        public const string ReviewSatus = "In Review";
        public const string ActiveSatus = "Active";
        public const string DeactiveSatus = "Deactive";
        public const string RejectSatus = "Rejected";


        public const string RealEstate = "Real Estate";
        public const string ConstructionVehicle = "Construction Vehicles";
        public const string TransportationVehicle = "Transportation Vehicles";
        public const string AgriculturalVehicle = "Agricultural Vehicles";
        public const string PassengerVehicle = "Passenger Vehicles";

        public const string VerifyOTPFrmRegistration = "VerifyOTPFrmRegistration";
        public const string VerifyOTPFrmLoginWIthOTP = "VerifyOTPFrmLoginWIthOTP";
        public const string VerifyOTPFrmPostAdd = "VerifyOTPFrmPostAdd";
        public const string VerifyOTPFrmChangePhoneNum = "VerifyOTPFrmChangePhoneNum";
        public const string VerifyOTPFrmForgotPwd = "VerifyOTPFrmForgotPwd";

        public static string PostAddUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/api/PostApi/PostAdd";
        public static string PostAgricutureVehicleUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/api/PostApi/AgriculturalVehicle";
        public static string PostConstructionVehicleUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/api/PostApi/ConstructionVehicle";
        public static string PostTransportationVehicleUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/api/PostApi/TransportationVehicle";
        public static string PostPassengerVehicleUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/api/PostApi/PassengerVehicle";
        public static string PostRealEstateUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/api/PostApi/RealEstate";

        public static string UpdateAddUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/api/PostApi/UpdateAdd";


        public static string PostDeleteUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/api/PostApi/DeleteAdd";
        public static string PostDeleteImage = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/api/PostApi/DeleteImage";
        public static string ChangeDefaultImage = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/api/PostApi/ChangeDefaultImage";



        public static string GetAdd = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/api/PostApi/GetAdd";
        public static string GetRealestate = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/api/PostApi/GetRealEstate";



        public const int NoOfAddsPerPage = 30;

        public static string DomainName = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);

        public static string ErrorLogFileName = "ErrorLog.txt";
        public const string AdminPhoneNum = "9603699900";
    }


    public class CompleteAdd
    {
        //public Add add { set; get; }
        public User user { set; get; }
        public RealEstate realEstate { set; get; }
        //public PassengerVehicle passengerVehicle { set; get; }
        //public TransportationVehicle transportationVehicle { set; get; }
        //public ConstructionVehicle constructionVehicle { set; get; }
        //public AgriculturalVehicle agriculturalVehicle { set; get; }
    }
    public class AdminAdd
    {
        public int AddId { get; set; }
        public Nullable<System.DateTime> Created { get; set; }
        public string UserName { get; set; }
        public string PhoneNum { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string Status { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public string Mandal { get; set; }
        public string Type { get; set; }
        public string Remarks { get; set; }
    }

    public class LoginWithOTP
    {
        public string PhoneNumber { get; set; }
        public string OTP { set; get; }
        public string VerifyType { set; get; }
    }
    public class ForgotPwd
    {
        public string PhoneNumber { get; set; }
        public string txtPasscode { set; get; }
        public string txtConfirmPasscode { set; get; }
    }
    public class Status
    {
        public string type { get; set; }
        public string message { get; set; }
    }

    public class DistinctChat
    {
        public Message Msg { set; get; }
        public int Count { set; get; }
    }

    public class CustomMessage
    {
        //public Message Msg;
        public string AddTitle { set; get; }
        public string ToUserName { set; get; }
        public string FromUserName { set; get; }
        public string MessageSentDate { set; get; }
        public Guid ToUserId { set; get; }
        public Guid FromUserId { set; get; }
        public string Status { set; get; }
        public int AddId { set; get; }
        public Guid RequestorUserId { set; get; }
        public string message { set; get; }
        public int ChatCount { set; get; }
    }

    public class IndividualChat
    {
        public List<CustomMessage> CustomMsgColl { set; get; }
        public Guid FromUserId { set; get; }
        public Guid ToUserId { set; get; }
        public Guid RequestorUserId { set; get; }
        public int AddId { set; get; }
        public string AddTitle { set; get; }
        // public int ChatCount { set; get; }
    }

    public class CustomHomeModel
    {
        public List<CustomAdd> AddColl { set; get; }

        public List<CustomMessage> ChatColl { set; get; }

        //public List<CustomMessage> InboxChatColl { set; get; }

        // public List<CustomMessage> SentChatColl { set; get; }
    }

    public class CustomSurvey
    {
        public Survey Survey { set; get; }

        public string Category { set; get; }

        public string SubCategory { set; get; }

        public string Type { set; get; }
    }

    public class AdminDashboard
    {
        public IEnumerable<CustomSurvey> SurveyColl { set; get; }

        public IEnumerable<AdminAdd> AddsColl { set; get; }
    }
}