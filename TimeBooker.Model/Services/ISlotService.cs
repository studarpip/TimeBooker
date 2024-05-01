using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimeBooker.Model.Entities;
using TimeBooker.Model.Entities.Requests;

namespace TimeBooker.Model.Services
{
    public interface ISlotService
    {
        ServerResult<List<SlotStatusResponse>> GetSlotStatuses();
        Task<ServerResult> CreateSlotsAsync (CreateSlotsRequest request);
        Task<ServerResult> DeleteSlotAsync(int slotId);
        Task<ServerResult> UpdateSlotAsync(UpdateSlotRequest request);
        Task<ServerResult<List<Slot>>> GetSlotsAsync(int? status, DateTime? date);
        Task<ServerResult> ReserveSlotAsync(ReserveSlotRequest request);
    }
}
