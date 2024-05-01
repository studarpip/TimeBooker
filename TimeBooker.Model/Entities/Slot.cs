using System;
using TimeBooker.Model.Entities.Enums;

namespace TimeBooker.Model.Entities
{
    public class Slot
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Email { get; set; }
        public SlotStatus Status { get; set; }
    }
}
