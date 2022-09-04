using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectClasses
{
    public class ResultViewModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public int IdOfUser
        {
            get; set;
        }
    }
}
