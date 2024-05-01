using System;

namespace TimeBooker.Model.Entities
{
    public class Session
    {
        public string SessionId { get; set; }
        public int UserId { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
