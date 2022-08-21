using Ecommerce.DAL;
using ECommerce.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECommerce.UI.Util
{
    public class CartManager
    {

        public static void AddItemToCart(CartViewModel model)
        {
            List<CartViewModel> lst = new List<CartViewModel>();
            if (HttpContext.Current.Session["Cart"] == null)
            {
                lst.Add(model);
            }
            else
            {
                lst = (List<CartViewModel>)HttpContext.Current.Session["Cart"];
                var md = lst.SingleOrDefault(x => x.StockId == model.StockId);
                if (md != null && md.StockId > 0)
                {
                    md.Quantity += model.Quantity;
                    md.PurchasePrice = model.PurchasePrice;
                     
                }
                else
                {
                    
                    lst.Add(model);
                }
            }
            HttpContext.Current.Session["Cart"] = lst;
        }

        public static CartViewModel GetCartItem(long id )
        {
            List<CartViewModel> lst = new List<CartViewModel>();
            lst = (List<CartViewModel>)HttpContext.Current.Session["Cart"];
            var md = lst.SingleOrDefault(x => x.StockId == id);
            return md;

        }

        public static void Clear()
        {
            HttpContext.Current.Session["Cart"] = null;
            HttpContext.Current.Session["Custom"] = null;
        }

        public static int ItemsCount()
        {
            if (HttpContext.Current.Session["Cart"] == null)
            {
                return 0;
            }
            else
            {
                List<CartViewModel> lst = new List<CartViewModel>();
                lst = (List<CartViewModel>)HttpContext.Current.Session["Cart"];
                return lst.Count;
            }
        }

        public static void RemoveItem(long Id)
        {
            if (HttpContext.Current.Session["Cart"] != null)
            {
                List<CartViewModel> lst = new List<CartViewModel>();
                lst = (List<CartViewModel>)HttpContext.Current.Session["Cart"];
                var item = lst.Where(x => x.StockId == Id).FirstOrDefault();
                lst.Remove(item);
            }
        }

        public static List<CartViewModel> GetAll()
        {
            List<CartViewModel> lst = new List<CartViewModel>();
            if (HttpContext.Current.Session["Cart"] != null)
            {
                lst = (List<CartViewModel>)HttpContext.Current.Session["Cart"];
            }
            return lst;
        }

        public static decimal GetAmount()
        {
            decimal totalAmount = 0;
            List<CartViewModel> lst = new List<CartViewModel>();
            if (HttpContext.Current.Session["Cart"] != null)
            {
                lst = (List<CartViewModel>)HttpContext.Current.Session["Cart"];
            }
            foreach (var item in lst)
            {
                var prod = item.Product;
                if (prod.Sale && prod.Discount != null)
                {
                    totalAmount += item.Quantity * (prod.Price - (prod.Price * (prod.Discount.Value / 100)));
                }
                else
                {
                    totalAmount += item.Quantity * prod.Price;
                }
            }
            if (HttpContext.Current.Session["Custom"] != null)
            {
                List<CustomShirtsOrder> lstCustom = (List<CustomShirtsOrder>)HttpContext.Current.Session["Custom"];
                totalAmount += lstCustom.Sum(x => (x.ShirtPrice+x.FrontPrice.GetValueOrDefault()+x.BackPrice.GetValueOrDefault()) * x.Quantity);
            }
            return totalAmount;
        }

        public static void ShippingInfo(Order order)
        {
            if (HttpContext.Current.Session["Cart"] != null)
            {
                List<CartViewModel> lst = new List<CartViewModel>();
                lst = (List<CartViewModel>)HttpContext.Current.Session["Cart"];
                lst.FirstOrDefault().ShippingInfo = order;
                HttpContext.Current.Session["Cart"] = lst;
            }
            else
            {
                List<CustomShirtsOrder> lstCustom = (List<CustomShirtsOrder>)HttpContext.Current.Session["Custom"];
                lstCustom.FirstOrDefault().Order = order;
                HttpContext.Current.Session["Custom"] = lstCustom;
            }
        }

        public static Order GetShippingInfo()
        {
            if (HttpContext.Current.Session["Cart"] == null)
            {
                return null;
            }
            List<CartViewModel> lst = new List<CartViewModel>();
            lst = (List<CartViewModel>)HttpContext.Current.Session["Cart"];
            return lst.FirstOrDefault().ShippingInfo;
        }

        #region "Custom Shirts"
        public static void AddCustomShirt(CustomShirtsOrder model)
        {
            if (HttpContext.Current.Session["Custom"] != null)
            {
                List<CustomShirtsOrder> lst = (List<CustomShirtsOrder>)HttpContext.Current.Session["Custom"];
                lst.Add(model);
                HttpContext.Current.Session["Custom"] = lst;
            }
            else
            {
                List<CustomShirtsOrder> lst = new List<CustomShirtsOrder>();
                lst.Add(model);
                HttpContext.Current.Session["Custom"] = lst;
            }
        }

        public static List<CustomShirtsOrder> GetCustomShirts()
        {
            if (HttpContext.Current.Session["Custom"] != null)
            {
                List<CustomShirtsOrder> lst = (List<CustomShirtsOrder>)HttpContext.Current.Session["Custom"];
                return lst;
            }
            return null;
        }

        public static void RemoveCustomShirt(int index)
        {
            if (HttpContext.Current.Session["Custom"] != null)
            {
                List<CustomShirtsOrder> lst = (List<CustomShirtsOrder>)HttpContext.Current.Session["Custom"];
                lst.RemoveAt(index);
                HttpContext.Current.Session["Custom"] = lst;
            }
        }
        #endregion

        #region Admin Cart
        public static void AddItemToAdminCart(CartViewModel model)
        {
            List<CartViewModel> lst = new List<CartViewModel>();
            if (HttpContext.Current.Session["AdminCart"] == null)
            {
                lst.Add(model);
            }
            else
            {
                lst = (List<CartViewModel>)HttpContext.Current.Session["AdminCart"];
                var md = lst.SingleOrDefault(x => x.StockId == model.StockId);
                if (md != null && md.StockId > 0)
                {
                    md.Quantity += model.Quantity;
                }
                else
                {
                    lst.Add(model);
                }
            }
            HttpContext.Current.Session["AdminCart"] = lst;
        }


        public static void ClearAdminCart()
        {
            HttpContext.Current.Session["AdminCart"] = null;
        }

        public static int ItemsAdminCount()
        {
            if (HttpContext.Current.Session["AdminCart"] == null)
            {
                return 0;
            }
            else
            {
                List<CartViewModel> lst = new List<CartViewModel>();
                lst = (List<CartViewModel>)HttpContext.Current.Session["AdminCart"];
                return lst.Count;
            }
        }

        public static void RemoveAdminItem(long Id)
        {
            if (HttpContext.Current.Session["AdminCart"] != null)
            {
                List<CartViewModel> lst = new List<CartViewModel>();
                lst = (List<CartViewModel>)HttpContext.Current.Session["AdminCart"];
                var item = lst.Where(x => x.StockId == Id).FirstOrDefault();
                lst.Remove(item);
            }
        }

        public static List<CartViewModel> GetAdminAll()
        {
            List<CartViewModel> lst = new List<CartViewModel>();
            if (HttpContext.Current.Session["AdminCart"] != null)
            {
                lst = (List<CartViewModel>)HttpContext.Current.Session["AdminCart"];
            }
            return lst;
        }

        public static decimal GetAdminAmount()
        {
            decimal totalAmount = 0;
            List<CartViewModel> lst = new List<CartViewModel>();
            if (HttpContext.Current.Session["AdminCart"] != null)
            {
                lst = (List<CartViewModel>)HttpContext.Current.Session["AdminCart"];
            }
            foreach (var item in lst)
            {
                var prod = item.Product;
                if (prod.Sale)
                {
                    totalAmount += item.Quantity * (prod.Price - (prod.Price * (prod.Discount.Value / 100)));
                }
                else
                {
                    totalAmount += item.Quantity * prod.Price;
                }
            }
            return totalAmount;
        }

        #endregion
    }
}