using Ecommerce.DAL;
using Ecommerce.DAL.Enums;
using ECommerce.UI.Migrations;
using ECommerce.UI.Models;
using PayPal.Api;
using PayPal.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ECommerce.UI.Util
{
    public class PaymentManager
    {
        public static bool Pay(List<CartViewModel> products, decimal amount, CreditCardViewModel cardVM, long invoiceNum)
        {
            //create and item for which you are taking payment
            //if you need to add more items in the list
            //Then you will need to create multiple item objects or use some loop to instantiate object


            //Now make a List of Item and add the above item to it
            //you can create as many items as you want and add to this list
            List<Item> itms = new List<Item>();
            foreach (var prod in products)
            {
                Item item = new Item();
                item.name = prod.Product.Name;
                item.currency = "USD";
                if (prod.Product.Sale)
                {
                    item.price = (prod.Product.Price - ((prod.Product.Discount.GetValueOrDefault() / 100) * prod.Product.Price)).ToString("0.##");
                }
                else
                {
                    item.price = prod.Product.Price.ToString("0.##");
                }

                item.quantity = prod.Quantity.ToString();
                item.sku = prod.StockId.ToString();
                itms.Add(item);
            }
            ItemList itemList = new ItemList();
            itemList.items = itms;

            //Address for the payment
            Address billingAddress = new Address();
            billingAddress.city = "NewYork";
            billingAddress.country_code = "US";
            billingAddress.line1 = "23rd street kew gardens";
            billingAddress.postal_code = "43210";
            billingAddress.state = "NY";


            //Now Create an object of credit card and add above details to it
            //Please replace your credit card details over here which you got from paypal
            CreditCard crdtCard = new CreditCard();
            crdtCard.billing_address = billingAddress;
            crdtCard.cvv2 = cardVM.CVV;  //card cvv2 number
            crdtCard.expire_month = Convert.ToInt32(cardVM.ExpireMonth); //card expire date
            crdtCard.expire_year = Convert.ToInt32(cardVM.ExpireYear); //card expire year
            crdtCard.first_name = "Aman";
            crdtCard.last_name = "Thakur";
            crdtCard.number = cardVM.Number; //enter your credit card number here
            switch (cardVM.CardName)
            {
                case "0":
                    crdtCard.type = "visa"; //credit card type here paypal allows 4 types
                    break;
                case "1":
                    crdtCard.type = "mastercard"; //credit card type here paypal allows 4 types
                    break;
                case "2":
                    crdtCard.type = "amex"; //credit card type here paypal allows 4 types
                    break;
                default:
                    break;
            }


            //// Specify details of your payment amount.
            //Details details = new Details();
            //details.shipping = "0";
            //details.subtotal = amount.ToString();
            //details.tax = "0";

            // Specify your total payment amount and assign the details object
            Amount amnt = new Amount();
            amnt.currency = "USD";
            // Total = shipping tax + subtotal.
            amnt.total = amount.ToString("0.##");
            //amnt.details = details;

            // Now make a transaction object and assign the Amount object
            PayPal.Api.Transaction tran = new PayPal.Api.Transaction();
            tran.amount = amnt;
            tran.description = "Description about the payment amount.";
            tran.item_list = itemList;
            tran.invoice_number = invoiceNum.ToString();

            // Now, we have to make a list of transaction and add the transactions object
            // to this list. You can create one or more object as per your requirements

            List<PayPal.Api.Transaction> transactions = new List<PayPal.Api.Transaction>();
            transactions.Add(tran);

            // Now we need to specify the FundingInstrument of the Payer
            // for credit card payments, set the CreditCard which we made above

            FundingInstrument fundInstrument = new FundingInstrument();
            fundInstrument.credit_card = crdtCard;

            // The Payment creation API requires a list of FundingIntrument

            List<FundingInstrument> fundingInstrumentList = new List<FundingInstrument>();
            fundingInstrumentList.Add(fundInstrument);

            // Now create Payer object and assign the fundinginstrument list to the object
            Payer payr = new Payer();
            payr.funding_instruments = fundingInstrumentList;
            payr.payment_method = "credit_card";

            // finally create the payment object and assign the payer object & transaction list to it
            Payment pymnt = new Payment();
            pymnt.intent = "sale";
            pymnt.payer = payr;
            pymnt.transactions = transactions;


            //getting context from the paypal
            //basically we are sending the clientID and clientSecret key in this function
            //to the get the context from the paypal API to make the payment
            //for which we have created the object above.

            //Basically, apiContext object has a accesstoken which is sent by the paypal
            //to authenticate the payment to facilitator account.
            //An access token could be an alphanumeric string


            //Create is a Payment class function which actually sends the payment details
            //to the paypal API for the payment. The function is passed with the ApiContext
            //which we received above.
            try
            {
                APIContext apiContext = PayPalConfiguration.GetAPIContext();
                Payment createdPayment = pymnt.Create(apiContext);
            }
            catch (Exception ex)
            {
                return false;
                // Get the details of this exception with ex.Details.  If you have logging setup for your project, this information will also be automatically logged to your logfile.
                //StringBuilder sb = new StringBuilder();
                //sb.AppendLine("Error:    " + this.Details.Name);
                //sb.AppendLine("Message:  " + this.Details.message);
                //sb.AppendLine("URI:      " + this.Details.information_link);
                //sb.AppendLine("Debug ID: " + this.Details.debug_id);

                //foreach (PaymentsErrorDetails errorDetails in this.Details.details)
                //{
                //    sb.AppendLine("Details:  " + errorDetails.field + " -> " + errorDetails.issue);
                //}

                // sb now contains all the error information in an easy-to-read string.
            }

            //try
            //{
            //    Payment createdPayment = pymnt.Create(apiContext);
            //}
            //catch (ConnectionException ex)
            //{
            //    throw;
            //}


            //if the createdPayment.state is "approved" it means the payment was successful else not

            //if (createdPayment.state.ToLower() != "approved")
            //{
            //    return false;
            //}
            return true;

        }


        public static bool Refund(string saleId, decimal refundAmount)
        {
            var apiContext = PayPalConfiguration.GetAPIContext();
            var refund = new Refund()
            {
                amount = new Amount()
                {
                    currency = "USD",
                    total = refundAmount.ToString("0.##")
                }
            };

            var sale = new Sale()
            {
                id = saleId
            };
            //try
            //{
            //    var response = sale.Refund(apiContext, refund);
            //}
            //catch (Exception ex)
            //{

            //    throw;
            //}
            return true;
        }

    }
}