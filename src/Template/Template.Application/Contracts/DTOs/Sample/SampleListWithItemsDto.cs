using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Application.Contracts.DTOs.Sample
{
    public  class SampleListWithItemsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }

        public List<SampleItemDto> Items { get; set; }

        public string SuccessMessage { get; set; }
    }
}
