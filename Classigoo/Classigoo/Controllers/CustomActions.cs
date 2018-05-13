using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Classigoo;
using System.Globalization;

namespace Classigoo.Controllers
{
    public class CustomActions
    {
        public CustomAdd CheckCategory(Add add)
        {
            CustomAdd customAdd = new CustomAdd();
            customAdd.Location = add.Mandal + "," + add.State;
            customAdd.CreatedDate = add.Created.ToString();
            customAdd.AddId = add.AddId;
            customAdd.Title = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(add.Title);
            switch (add.Category)
            {
                case Constants.RealEstate:
                    {
                        foreach (var item in add.RealEstates)
                        {
                            customAdd.Description = item.Description;
                            customAdd.Price = item.Price;
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
                            customAdd.Price = item.Price;
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
                            customAdd.Price = item.Price;
                            customAdd.Category = Constants.ConstructionVehicle;
                            customAdd.CV = item;
                            customAdd.ImgUrlPrimary = item.ImgUrlPrimary;
                            customAdd.ImgUrlSeconday = item.ImgUrlSeconday;
                            customAdd.ImgUrlThird = item.ImgUrlThird;
                            customAdd.ImgUrlFourth = item.ImgUrlFourth;
                        }
                        break;
                    }
                case Constants.AgriculturalVehicle:
                    {
                        foreach (var item in add.AgriculturalVehicles)
                        {
                            customAdd.Description = item.Description;
                            customAdd.Price = item.Price;
                            customAdd.Category = Constants.AgriculturalVehicle;
                            customAdd.AV = item;
                            customAdd.ImgUrlPrimary = item.ImgUrlPrimary;
                            customAdd.ImgUrlSeconday = item.ImgUrlSeconday;
                            customAdd.ImgUrlThird = item.ImgUrlThird;
                            customAdd.ImgUrlFourth = item.ImgUrlFourth;
                        }
                        break;
                    }
                case Constants.PassengerVehicle:
                    {
                        foreach (var item in add.PassengerVehicles)
                        {
                            customAdd.Description = item.Description;
                            customAdd.Price = item.Price;
                            customAdd.Category = Constants.PassengerVehicle;
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