using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Classigoo;

namespace Classigoo.Controllers
{
    public class CustomActions
    {
        public CustomAdd CheckCategory(Add add)
        {
            CustomAdd customAdd = new CustomAdd();
           // customAdd.Location = add.Location;
            customAdd.CreatedDate = add.Created.ToString();
            customAdd.AddId = add.AddId;
            customAdd.Price = "500Rs";
            switch (add.Category)
            {
                case Constants.RealEstate:
                    {
                        foreach (var item in add.RealEstates)
                        {
                            customAdd.Description = item.Description;
                            //customAdd.Title = item.Title;
                            customAdd.Category = Constants.RealEstate;
                            customAdd.RE = item;
                            customAdd.ImgUrlPrimary = item.ImgUrlPrimary;
                            customAdd.ImgUrlSeconday = item.ImgUrlSeconday;
                            customAdd.ImgUrlThird = item.ImgUrlThird;
                            customAdd.ImgUrlFourth = item.ImgUrlFourth;
                        }
                        break;
                    }
                case Constants.TransportationVehicle:
                    {
                        foreach (var item in add.TransportationVehicles)
                        {
                            customAdd.Description = item.Description;
                           // customAdd.Title = item.Title;
                            customAdd.Category = Constants.TransportationVehicle;
                            customAdd.TV = item;
                            customAdd.ImgUrlPrimary = item.ImgUrlPrimary;
                            customAdd.ImgUrlSeconday = item.ImgUrlSeconday;
                            customAdd.ImgUrlThird = item.ImgUrlThird;
                            customAdd.ImgUrlFourth = item.ImgUrlFourth;

                        }
                        break;
                    }
                case Constants.ConstructionVehicle:
                    {
                        foreach (var item in add.ConstructionVehicles)
                        {
                            customAdd.Description = item.Description;
                            //customAdd.Title = item.Title;
                            customAdd.Category = Constants.ConstructionVehicle;
                            customAdd.CV = item;
                            customAdd.ImgUrlPrimary = item.ImgUrlPrimary;
                            customAdd.ImgUrlSeconday = item.ImgUrlSeconday;
                            customAdd.ImgUrlThird = item.ImgUrlThird;
                            customAdd.ImgUrlFourth = item.ImgUrlFourth;
                        }
                        break;
                    }
                case "Agricultural Vehicles":
                    {
                        foreach (var item in add.AgriculturalVehicles)
                        {
                            customAdd.Description = item.Description;
                            //customAdd.Title = item.Title;
                            customAdd.Category = "Agricultural Vehicles";
                            customAdd.AV = item;
                            customAdd.ImgUrlPrimary = item.ImgUrlPrimary;
                            customAdd.ImgUrlSeconday = item.ImgUrlSeconday;
                            customAdd.ImgUrlThird = item.ImgUrlThird;
                            customAdd.ImgUrlFourth = item.ImgUrlFourth;
                        }
                        break;
                    }
                case "Passenger Vehicles":
                    {
                        foreach (var item in add.PassengerVehicles)
                        {
                            customAdd.Description = item.Description;
                            //customAdd.Title = item.Title;Passenger Vehicles	
                            customAdd.Category = "Passenger Vehicles";
                            customAdd.PV = item;
                            customAdd.ImgUrlPrimary = item.ImgUrlPrimary;
                            customAdd.ImgUrlSeconday = item.ImgUrlSeconday;
                            customAdd.ImgUrlThird = item.ImgUrlThird;
                            customAdd.ImgUrlFourth = item.ImgUrlFourth;
                        }
                        break;
                    }

            }
            return customAdd;
        }
    }
}