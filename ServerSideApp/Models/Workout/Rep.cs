using System;
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;

namespace ServerSideApp.Models.Workout
{
    [Serializable]
    public class Rep
    {
        [Required]
        public string Description { get; set; }
        [Required]
        public int Count { get; set; }
    }
}