using System;
using TimeBooker.Model.Entities.Enums;

namespace TimeBooker.Model.Entities.Requests
{
    public class UpdateSlotRequest
    {
        public int UserId { get; set; }
        public string SessionId { get; set; }
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Email { get; set; }
        public SlotStatus Status { get; set; }
    }
}
