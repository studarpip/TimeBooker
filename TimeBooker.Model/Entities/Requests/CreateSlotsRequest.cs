using System;

namespace TimeBooker.Model.Entities.Requests
{
    public class CreateSlotsRequest
    {
        public int UserId { get; set; }
        public string SessionId { get; set; }
        public DateTime StartDateTime { get; set; }
        public bool IsRepeating { get; set; }
        public int? RepeatIntervalInMinutes { get; set; }
        public DateTime? EndDateTime { get; set; }
    }
}
