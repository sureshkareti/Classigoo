using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Classigoo.Controllers
{
    public class CustomActions
    {
        public CustomAdd CheckCategory(Add add)
        {
            CustomAdd customAdd = new CustomAdd();
            customAdd.Location = add.Location;
            customAdd.CreatedDate = add.Created.ToString();
            customAdd.AddId = add.AddId;
            customAdd.Price = "500Rs";
            switch (add.Category)
            {
                case "RealEstate":
                    {
                        foreach (var item in add.RealEstates)
                        {
                            customAdd.Description = item.Description;
                            customAdd.Title = item.Title;
                            customAdd.Category = "RealEstate";
                            customAdd.RealEstate = item;
                        }
                        break;
                    }
                case "Cars":
                    {
                        foreach (var item in add.Cars)
                        {
                            customAdd.Description = item.Description;
                            customAdd.Title = item.Title;
                            customAdd.Category = "Cars";
                            customAdd.Cars = item;

                        }
                        break;
                    }
                case "Electronics":
                    {
                        foreach (var item in add.Electronics)
                        {
                            customAdd.Description = item.Description;
                            customAdd.Title = item.Title;
                            customAdd.Category = "Electronics";
                            customAdd.Electronics = item;
                        }
                        break;
                    }

            }
            return customAdd;
        }
    }
}