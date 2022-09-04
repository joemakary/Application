using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ObjectClasses;
using QRCoder;

namespace Application.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        Context _context;
        public UserController(Context context)
        {

            _context = context;
          
        }
       

        public Byte[] ImageToByteArray(System.Drawing.Image image) 
        {
            MemoryStream ms = new MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }
        [Authorize]

        [HttpGet]

        public async Task<ActionResult> GenerateQRCode(int id)
        {
            var user = _context.Employees.Where(s => s.Id == id).FirstOrDefault();
            if (user.UserName.Equals("1DayRef") || user.UserName.Equals("1WeekRef"))
                return null;
            QRCodeGenerator _qr = new QRCodeGenerator();
            QRCodeData _qrData = _qr.CreateQrCode(DateTime.Now + "_" + user.Id + "_" + user.Name + "_" + user.Department + "_" + user.MealType, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(_qrData);
            Image qrCodeImage = qrCode.GetGraphic(20);

            var bytes = ImageToByteArray(qrCodeImage);
            return File(bytes, "image/bmp"); 

        }

        
    }
}
