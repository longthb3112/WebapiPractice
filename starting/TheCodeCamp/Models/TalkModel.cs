using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TheCodeCamp.Models
{
    public class TalkModel
    {
        public int TalkId { get; set; }
        [Required]
        public string Title { get; set; }
        public string Abstract { get; set; }
        [Range(1,1000)]
        public int Level { get; set; }
        public SpeakerModel Speaker { get; set; }
    }
}