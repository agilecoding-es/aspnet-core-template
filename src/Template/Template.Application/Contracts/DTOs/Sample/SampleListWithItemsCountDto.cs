﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Application.Contracts.DTOs.Sample
{
    public  class SampleListWithItemsCountDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int ItemsCount { get; set; }

    }
}
