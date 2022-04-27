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
        public string ChatpostUser { get; set; }
        public Chatroom Chatroom { get; set; }
        
    }
}