using Data.Models;
using Service.Common;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Service
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(Order model, HttpContextBase context);
        PaymentResponse PaymentExecute(NameValueCollection collections);
    }
    public class VnPayService : IVnPayService
    {
        private readonly string _timeZoneId;
        private readonly string _version;
        private readonly string _command;
        private readonly string _tmnCode;
        private readonly string _currCode;
        private readonly string _locale;
        private readonly string _returnUrl;
        private readonly string _baseUrl;
        private readonly string _hashSecret;
        private readonly string _orderType;

        public VnPayService()
        {
            _timeZoneId = ConfigHelper.GetByKey("TimeZoneId");
            _version = ConfigHelper.GetByKey("Version");
            _command = ConfigHelper.GetByKey("Command");
            _tmnCode = ConfigHelper.GetByKey("vnp_TmnCode");
            _currCode = ConfigHelper.GetByKey("CurrCode");
            _locale = ConfigHelper.GetByKey("Locale");
            _returnUrl = ConfigHelper.GetByKey("vnp_Returnurl");
            _baseUrl = ConfigHelper.GetByKey("vnp_Baseurl");
            _hashSecret = ConfigHelper.GetByKey("vnp_HashSecret");
            _orderType = ConfigHelper.GetByKey("vnp_OrderType");

            //_timeZoneId = "SE Asia Standard Time";
            //_version = "2.1.0";
            //_command = "pay";
            //_tmnCode = "A5ABTEUS";
            //_currCode = "VND";
            //_locale = "vn";
            //_returnUrl = "http://tuanpc.xyz/ShoppingCart/PaymentCallback";
            //_baseUrl = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html";
            //_hashSecret = "HJIBUJAHICMHMEZXVBFDZRXGHDNWCMAB";
            //_orderType = "other";
        }

        public string CreatePaymentUrl(Order model, HttpContextBase context)
        {
            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_timeZoneId);
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
            var tick = DateTime.Now.Ticks.ToString();
            var pay = new VnPayLibrary();

            pay.AddRequestData("vnp_Version", _version);
            pay.AddRequestData("vnp_Command", _command);
            pay.AddRequestData("vnp_TmnCode", _tmnCode);
            pay.AddRequestData("vnp_Amount", ((int)model.TotalPrice*100).ToString());
            pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", _currCode);
            pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
            pay.AddRequestData("vnp_Locale", _locale);
            pay.AddRequestData("vnp_OrderInfo", "Thanh toán hóa đơn");
            pay.AddRequestData("vnp_OrderType", _orderType);
            pay.AddRequestData("vnp_ReturnUrl", _returnUrl);
            pay.AddRequestData("vnp_TxnRef", tick);

            var paymentUrl = pay.CreateRequestUrl(_baseUrl, _hashSecret);
            return paymentUrl;
        }

        public PaymentResponse PaymentExecute(NameValueCollection collections)
        {
            var pay = new VnPayLibrary();
            var response = pay.GetFullResponseData(collections, _hashSecret);
            return response;
        }
    }
}
