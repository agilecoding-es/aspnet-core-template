﻿using System.ComponentModel.DataAnnotations;

namespace Template.WebApp.Areas.SampleMvc.Models.SampleList
{
    public class EditSampleListViewModel
    {
        public EditSampleListViewModel() { }

        public EditSampleListViewModel(int id)
        {
            Id = id;
            NewItem = new SampleItemViewModel()
            {
                ListId = id
            };
        }

        public int Id { get; set; }

        [Display(Name = "List Name")]
        [Required(ErrorMessage = "The {0} field is required.")]
        public string Name { get; set; }

        public string UserId { get; set; }

        [Display(Name = "Items Count")]
        public int ItemsCount => Items.Count;

        public List<SampleItemViewModel> Items { get; set; } = new List<SampleItemViewModel>();
        public SampleItemViewModel NewItem { get; set; }

    }
}
