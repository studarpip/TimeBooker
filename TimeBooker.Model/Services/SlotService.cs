using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimeBooker.Model.Entities;
using TimeBooker.Model.Entities.Enums;
using TimeBooker.Model.Entities.Requests;
using TimeBooker.Model.Repositories;

namespace TimeBooker.Model.Services
{
    public class SlotService : ISlotService
    {
        private readonly ISlotRepository _slotRepository;

        public SlotService(ISlotRepository slotRepository)
        {
            _slotRepository = slotRepository;
        }

        public async Task<ServerResult> CreateSlotsAsync(CreateSlotsRequest request)
        {
            try
            {
                if (!request.IsRepeating)
                {
                    var slot = new Slot()
                    {
                        DateTime = request.StartDateTime
                    };
                    await _slotRepository.InsertAsync(slot);
                    return new() { Success = true };
                }
                var duration = request.EndDateTime.Value - request.StartDateTime;
                var totalSlots = (int)(duration.TotalMinutes / request.RepeatIntervalInMinutes);
                var currentDateTime = request.StartDateTime;
                for (int i = 0; i <= totalSlots; i++)
                {
                    var slot = new Slot { DateTime = currentDateTime };
                    await _slotRepository.InsertAsync(slot);

                    currentDateTime = currentDateTime.AddMinutes(request.RepeatIntervalInMinutes.Value);
                }
                return new() { Success = true };
            }
            catch (Exception ex)
            {
                return new() { Success = false, Message = ex.Message };
            }
        }

        public async Task<ServerResult> DeleteSlotAsync(int slotId)
        {
            try
            {
                await _slotRepository.DeleteSlotAsync(slotId);
                return new() { Success = true };
            }
            catch (Exception ex)
            {
                return new() { Success = false, Message = ex.Message };
            }
        }

        public async Task<ServerResult<List<Slot>>> GetSlotsAsync(int? status, DateTime? date)
        {
            try
            {
                var slots = await _slotRepository.GetSlotsAsync(status, date);
                return new() { Success = true, Data = slots };
            }
            catch (Exception ex)
            {
                return new() { Success = false, Message = ex.Message };
            }
        }

        public ServerResult<List<SlotStatusResponse>> GetSlotStatuses()
        {
            var slotStatuses = Enum.GetValues(typeof(SlotStatus));
            var statusList = new List<SlotStatusResponse>();

            foreach (SlotStatus status in slotStatuses)
            {
                statusList.Add(new SlotStatusResponse { Name = status.ToString(), Value = (int)status });
            }

            return new ServerResult<List<SlotStatusResponse>>
            {
                Success = true,
                Data = statusList
            };
        }

        public async Task<ServerResult> ReserveSlotAsync(ReserveSlotRequest request)
        {
            try
            {
                await _slotRepository.ReserveSlotAsync(request);
                return new() { Success = true };
            }
            catch (Exception ex)
            {
                return new() { Success = false, Message = ex.Message };
            }
        }

        public async Task<ServerResult> UpdateSlotAsync(UpdateSlotRequest request)
        {
            try
            {
                await _slotRepository.UpdateSlotAsync(request);
                return new() { Success = true };
            }
            catch (Exception ex)
            {
                return new() { Success = false, Message = ex.Message };
            }
        }
    }
}
