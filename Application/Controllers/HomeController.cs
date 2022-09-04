using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ObjectClasses;

namespace Application.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        Context _context;
        public HomeController(Context context)
        {

            _context = context;

        }

        [Authorize]
        public ResultViewModel GetEmployee(int id)
        {
            var user = _context.Employees.Where(s => s.Id == id).FirstOrDefault();
            if(user!=null)
                return new ResultViewModel() { IsSuccess=true,Message="Employee successfully retrieved",Data=user,IdOfUser=user.Id};
        return new ResultViewModel() { IsSuccess=false,Message="Employee could not be retrieved"};
        }
    }
}
