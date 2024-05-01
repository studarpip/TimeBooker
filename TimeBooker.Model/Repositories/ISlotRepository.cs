using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimeBooker.Model.Entities;
using TimeBooker.Model.Entities.Requests;

namespace TimeBooker.Model.Repositories
{
    public interface ISlotRepository
    {
        Task DeleteSlotAsync(int id);
        Task<List<Slot>> GetSlotsAsync(int? status, DateTime? date);
        Task InsertAsync(Slot slot);
        Task ReserveSlotAsync(ReserveSlotRequest request);
        Task UpdateSlotAsync(UpdateSlotRequest request);
    }
}
