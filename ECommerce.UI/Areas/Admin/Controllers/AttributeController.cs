using Ecommerce.BLL.Handler;
using Ecommerce.DAL;
using ECommerce.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECommerce.UI.Areas.Admin.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AttributeController : Controller
    {
        private CategoryHandler categoryHandler;
        EcommerceDBEntities context;
        AttributeHandler attributeHandler;
        // GET: Admin/Attribute

            public AttributeController()
        {
            context = new EcommerceDBEntities();
            categoryHandler = new CategoryHandler(context);
            attributeHandler = new AttributeHandler(context);
        }
        public ActionResult Index()
        {
            ViewBag.NavigationHeading = categoryHandler.NavigationHeadings();
            return View();
        }

        public PartialViewResult Create()
        {
            return PartialView("_CreatePartial");
        }

        public PartialViewResult Save(AttributeViewModel model)
        {
            var attrModel = model.Attribute;
            foreach (var item in model.Values)
            {
                item.CreatedOn = DateTime.UtcNow;
                item.IsActive = true;
            }
            attrModel.CreatedOn = DateTime.UtcNow;
            attrModel.IsActive = true;
            attrModel.AttributeValues = model.Values;
            attributeHandler.Add(attrModel);

            var attr = attributeHandler.List.Where(x => x.CategoryId == model.Attribute.CategoryId);
            return PartialView("_LinksAttributePartial", attr);
        }

        public PartialViewResult LinksAttribute(long Id)
        {
            var attr = attributeHandler.List.Where(x => x.CategoryId == Id);
            return PartialView("_LinksAttributePartial", attr);
        }

        public PartialViewResult Delete(long Id)
        {
            var delAtt = attributeHandler.FindById(Id);
            delAtt.IsActive = false;
            attributeHandler.Delete(delAtt);
            var attr = attributeHandler.List.Where(x => x.CategoryId == delAtt.CategoryId);
            return PartialView("_LinksAttributePartial", attr);
        }

        public PartialViewResult Edit(long Id)
        {
            AttributeViewModel model = new AttributeViewModel();
            model.Attribute = attributeHandler.FindById(Id);
            model.Values = model.Attribute.AttributeValues.Where(x => x.IsActive).ToList();
            return PartialView("_EditPartial", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult Update(AttributeViewModel model)
        {
            var oldAttr = attributeHandler.FindById(model.Attribute.Id);
            oldAttr.Name = model.Attribute.Name;
            foreach (var item in oldAttr.AttributeValues)
            {
                item.IsActive = false;
                item.ModifiedOn = DateTime.UtcNow;
            }

            foreach (var item in model.Values)
            {
                item.IsActive = true;
                if (item.Id > 0)
                {
                   var oldVal = oldAttr.AttributeValues.SingleOrDefault(x => x.Id == item.Id);
                    oldVal.IsActive = true;
                    oldVal.Value = item.Value;
                    oldVal.ModifiedOn = DateTime.UtcNow;
                }
                else
                {
                    item.IsActive = true;
                    item.CreatedOn = DateTime.UtcNow;
                    oldAttr.AttributeValues.Add(item);
                }
            }

            attributeHandler.Update(oldAttr);

            var attr = attributeHandler.List.Where(x => x.CategoryId == model.Attribute.CategoryId);
            return PartialView("_LinksAttributePartial", attr);
        }
    }
}