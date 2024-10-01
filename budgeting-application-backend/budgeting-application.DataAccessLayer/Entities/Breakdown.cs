﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace budgeting_application.DataAccessLayer.Entities
{
    public class Breakdown
    {
        public string Category { get; set; }
        public string Currency { get; set; }
        public decimal TotalAmount { get; set; }
        public string Type { get; set; }
    }
}
