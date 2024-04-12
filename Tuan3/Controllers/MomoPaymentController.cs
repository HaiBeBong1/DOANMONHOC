using Microsoft.AspNetCore.Mvc;

namespace Tuan3.Controllers
{
    //using MoMo;
    using Microsoft.AspNetCore.Mvc;
  
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web;
    using Tuan3.Others;

    namespace testNETCORE.Controllers
    {
        public class MomoPaymentController : Controller
        {
            public static string codeThanhToanTransferFromMoMo;
            public static string soTienTransferFromMoMo;
            public static string idTourTransferFromMoMo;
            public async Task<IActionResult> InitiatePayment(decimal totalAmount)
            {
                // Tạo request và chuyển hướng đến MoMo
                // Đoạn code tạo request và chuyển hướng đã được giải thích ở câu trả lời trước
                // Bạn có thể sao chép và paste đoạn code từ câu trước vào đây
                // Ví dụ:
                string endpoint = "https://test-payment.momo.vn/gw_payment/transactionProcessor";
                string partnerCode = "MOMOOJOI20210710";
                string accessKey = "iPXneGmrJH0G8FOP";
                string serectkey = "sFcbSGRSJjwGxwhhcEktCHWYUuTuPNDB";
                string orderInfo = "Thanh toán phí mua hàng.";
                string returnUrl = "https://yourdomain.com/Home/ConfirmPaymentClient";
                string notifyUrl = "https://yourdomain.com/Home/SavePayment";

                string amount = totalAmount.ToString();
                string orderId = DateTime.Now.Ticks.ToString(); // Mã đơn hàng duy nhất
                string requestId = DateTime.Now.Ticks.ToString();
                string extraData = "";

                // Tạo chữ ký
                string rawSignature = "partnerCode=" + partnerCode + "&accessKey=" + accessKey +
                    "&requestId=" + requestId + "&amount=" + amount +
                    "&orderId=" + orderId + "&orderInfo=" + orderInfo + "&returnUrl=" + returnUrl +
                    "&notifyUrl=" + notifyUrl + "&extraData=" + extraData;
                string signature = CalculateHMACSHA256Hash(rawSignature, serectkey);

                // Tạo URL redirect đến MoMo
                string redirectUrl = endpoint + "?partnerCode=" + partnerCode +
                    "&accessKey=" + accessKey + "&requestId=" + requestId +
                    "&amount=" + amount + "&orderId=" + orderId +
                    "&orderInfo=" + orderInfo + "&returnUrl=" + HttpUtility.UrlEncode(returnUrl) +
                    "&notifyUrl=" + HttpUtility.UrlEncode(notifyUrl) +
                    "&extraData=" + HttpUtility.UrlEncode(extraData) + "&signature=" + signature;

                // Chuyển hướng đến trang thanh toán MoMo
                return Redirect(redirectUrl);
            }

            private string CalculateHMACSHA256Hash(string input, string key)
            {
                using (HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
                {
                    byte[] hashValue = hmac.ComputeHash(Encoding.UTF8.GetBytes(input));
                    return Convert.ToBase64String(hashValue);
                }
            }

            public async Task<IActionResult> Payment(int totalMOMO, string idTour)
            {
                //request params need to request to MoMo system
                string endpoint = "https://test-payment.momo.vn/gw_payment/transactionProcessor";
                string partnerCode = "MOMOOJOI20210710";
                string accessKey = "iPXneGmrJH0G8FOP";
                string serectkey = "sFcbSGRSJjwGxwhhcEktCHWYUuTuPNDB";
                string orderInfo = "Thanh toán online";
                string returnUrl = "https://localhost:7067/Payment_Confirmation/Successful_Payment";
                string notifyurl = "https://4c8d-2001-ee0-5045-50-58c1-b2ec-3123-740d.ap.ngrok.io/Home/SavePayment"; //lưu ý: notifyurl không được sử dụng localhost, có thể sử dụng ngrok để public localhost trong quá trình test

                string amount = totalMOMO.ToString();
                string orderid = DateTime.Now.Ticks.ToString(); //mã đơn hàng
                string requestId = DateTime.Now.Ticks.ToString();
                string extraData = "";

                // truyen du lieu sang paymentconfirm
                soTienTransferFromMoMo = amount.ToString();
                codeThanhToanTransferFromMoMo = orderid.ToString();
                idTourTransferFromMoMo = idTour;
                //Before sign HMAC SHA256 signature
                string rawHash = "partnerCode=" +
                    partnerCode + "&accessKey=" +
                    accessKey + "&requestId=" +
                    requestId + "&amount=" +
                    amount + "&orderId=" +
                    orderid + "&orderInfo=" +
                    orderInfo + "&returnUrl=" +
                    returnUrl + "&notifyUrl=" +
                    notifyurl + "&extraData=" +
                    extraData;

                MoMoSecurity crypto = new MoMoSecurity();
                //sign signature SHA256
                string signature = crypto.signSHA256(rawHash, serectkey);

                //build body json request
                JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                { "accessKey", accessKey },
                { "requestId", requestId },
                { "amount",amount},
                { "orderId", orderid },
                { "orderInfo", orderInfo },
                { "returnUrl", returnUrl },
                { "notifyUrl", notifyurl },
                { "extraData", extraData },
                { "requestType", "captureMoMoWallet" },
                { "signature", signature }
            };

                string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());

                JObject jmessage = JObject.Parse(responseFromMomo);

                return Redirect(jmessage.GetValue("payUrl").ToString());
            }
        }
    }
}
