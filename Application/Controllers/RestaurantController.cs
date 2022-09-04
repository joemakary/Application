using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ObjectClasses;


namespace Application.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        Context _context;
        public RestaurantController(Context context)
        {

            _context = context;

        }
        public ResultViewModel ReportCheck()
        {
            var tmp1 = _context.Employees.Where(s => s.UserName.Equals("1DayRef")).FirstOrDefault();

            if (tmp1 != null)
            {
                TimeSpan Day = new TimeSpan(hours: 24, minutes: 0, seconds: 0);
                TimeSpan Week = new TimeSpan(days: 7, hours: 0, minutes: 0, seconds: 0);
                DateTime TimeNow = DateTime.Now;
                TimeSpan diff1 = TimeNow.Subtract(tmp1.LastClaimed);

                if (diff1.CompareTo(Day) >= 0)
                {
                    return GenerateReport();
                    
                }
                return new ResultViewModel() { IsSuccess = false, Message = "Report cannot be generated now" };
            }
            return new ResultViewModel() { IsSuccess = false, Message = "Report cannot be generated as no reference was found" };

        }

        public ResultViewModel GenerateReport()
        {
            Item items = new Item() { Name = "Total Meals" };
            int tmp = 0;
            foreach (var item in _context.Employees)
            {

                tmp += item.MealsClaimed;
                item.MealsClaimed = 0;
            }
            var tmp1 = _context.Employees.Where(s => s.UserName.Equals("1DayRef")).FirstOrDefault();
            tmp1.LastClaimed = DateTime.Now;
            _context.SaveChanges();
            items.Count = tmp;
            return new ResultViewModel() { IsSuccess=true,Message="Report successfully generated",Data=items};
        }

        public ResultViewModel AddRef()
        {
            
            var tmp1 = _context.Employees.Where(s => s.UserName.Equals("1DayRef")).Any();
            var tmp2 = _context.Employees.Where(s => s.UserName.Equals("1WeekRef")).Any();
            if (tmp1 || tmp2)
                return new ResultViewModel() { IsSuccess = false, Message = "Reference already exists" };

            _context.Employees.Add(new Employee() { Name = "1DayRef", UserName = "1DayRef", LastClaimed = DateTime.Now });
            _context.Employees.Add(new Employee() { Name = "1WeekRef", UserName = "1WeekRef", LastClaimed = DateTime.Now });
            _context.SaveChanges();
            return new ResultViewModel() { IsSuccess = true, Message = "Reference successfully added" };
        }

        

        [HttpPost]
        public ResultViewModel Register(Employee model)
        {
            var user = _context.Employees.Where(s => s.UserName.Equals(model.UserName)).Any();
            if (user)
            {

                return new ResultViewModel() { IsSuccess = false, Message = "This Username is already taken" };
            }
            var res = _context.Employees.Where(s => s.UserName.Equals(model.UserName)).FirstOrDefault();
            if (user && res.Password.Length < 8)
            {
                return new ResultViewModel() { IsSuccess = false, Message = "Password must be at least 8 characters" };
            }


            Employee tmp = new Employee()
            {
                Name = model.Name,
                UserName = model.UserName,
                Password = model.Password,
                Level = model.Level,
                Department = model.Department,
                IsEligible = false,
                IsExpired = false,
                MealType = model.MealType,
                MealsClaimed = model.MealsClaimed,
            };

            _context.Employees.Add(tmp);
            _context.SaveChanges();
            return new ResultViewModel() { IsSuccess = true ,Message="User successfully added"};

        }
        public ResultViewModel UploadSheet()
        {
            string connString = "Provider= Microsoft.ACE.OLEDB.12.0;" + "Data Source=C:/Users/DELL/Downloads/21.8.xlsx" + ";Extended Properties='Excel 8.0;HDR=Yes'";
            // Create the connection object
            OleDbConnection oledbConn = new OleDbConnection(connString);

            // Open connection
            oledbConn.Open();

                            //here sheet name is Sample-spreadsheet-file, usually it is Sheet1, Sheet2 etc..
                            OleDbCommand cmd = new OleDbCommand("SELECT * FROM [Pharma_Employees_Daily_Attenda_$]", oledbConn);

            // Create new OleDbDataAdapter
            OleDbDataAdapter oleda = new OleDbDataAdapter();

            oleda.SelectCommand = cmd;

            // Create a DataSet which will hold the data extracted from the worksheet.
            DataSet ds = new DataSet();

            // Fill the DataSet from the data extracted from the worksheet.
            oleda.Fill(ds, "Employees");

            var table = ds.Tables[0];

            object id = null;
            object name = null;
            object department = null;
            object mealtype = null;
            //loop through each row
            foreach (DataRow row in table.Rows)
            {
                /*Employee tmp =new Employee() { Name = row.ItemArray[1] as string, Department = row.ItemArray[2] as string, MealType = row.ItemArray[4] as string };
                _context.Employees.Add(tmp);
                _context.SaveChanges();*/
            }

            return new ResultViewModel() { IsSuccess = true, Message = "Sheet successfully added" };


        }



    }
        
    }

