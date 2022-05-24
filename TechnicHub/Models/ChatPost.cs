using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechnicHub.Models
{

    public class Chatpost
    {
        public int ChatpostId { get; set; }
        public string ChatpostMessage { get; set; }
        public DateTime ChatpostDate { get; set; }
        public int ChatpostUser { get; set; }
        public int ChatroomId { get; set; }
        
    }
}