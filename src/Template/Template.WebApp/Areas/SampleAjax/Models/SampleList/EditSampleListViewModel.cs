﻿using System.ComponentModel.DataAnnotations;

namespace Template.WebApp.Areas.SampleAjax.Models.SampleList
{
    public class EditSampleListViewModel
    {
        public EditSampleListViewModel() { }

        public int Id { get; set; }

        [Display(Name = "List Name")]
        [Required(ErrorMessage = "The {0} field is required.")]
        public string Name { get; set; }

        public string UserId { get; set; }
    }
}
