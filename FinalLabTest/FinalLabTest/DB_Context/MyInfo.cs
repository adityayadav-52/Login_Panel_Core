using System;
using System.Collections.Generic;

#nullable disable

namespace FinalLabTest.DB_Context
{
    public partial class MyInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Salary { get; set; }
        public string City { get; set; }
        public string Dept { get; set; }
    }
}
