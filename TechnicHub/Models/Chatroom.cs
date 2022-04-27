namespace TechnicHub.Models
{
    public class Chatroom
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Password { get; set; }
        public string Image { get; set; }
        public string Color { get; set; }
        public string Owner { get; set; }
        public int OwnerId { get; set; }
        public string OwnerUsername { get; set; }
        public int MemberCount { get; set; }
        public int MessageCount { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsNSFW { get; set; }
        public bool IsLocked { get; set; }
        public bool IsNSFWLocked { get; set; }
        public bool IsNSFWAllowed { get; set; }
        
        }
}
