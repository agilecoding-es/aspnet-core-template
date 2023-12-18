﻿using System.ComponentModel.DataAnnotations;

namespace Template.WebApp.Areas.SampleMvc.Models.SampleList
{
    public class SampleItemViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage = "The {0} field is required.")]
        public string Description { get; set; }

        public int ListId { get; set; }

    }
}
