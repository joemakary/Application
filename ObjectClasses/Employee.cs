using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectClasses
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Level { get; set; }
        public string MealType { get; set; }
        public string Department { get; set; }
        public bool IsExpired { get; set; } = true;
        public bool IsEligible { get; set; } = false;
        public int MealsClaimed { get; set; }
        public DateTime LastClaimed { get; set; }
    }
}
