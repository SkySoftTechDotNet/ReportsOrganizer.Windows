﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportsOrganizer.DAL
{
    public partial class ReportDTO : IEntity
    {
        public int Id { get; set; }
        
        public DateTime Date { get; set; }

        public string Description { get; set; } 
    }
}