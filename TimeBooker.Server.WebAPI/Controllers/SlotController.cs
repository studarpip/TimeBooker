using Microsoft.AspNetCore.Mvc;
using TimeBooker.Model.Entities;
using TimeBooker.Model.Entities.Requests;
using TimeBooker.Model.Services;

namespace TimeBooker.Server.WebAPI.Controllers
{
    [Controller, Route("/slots")]
    public class SlotController
    {
        private readonly ISlotService _slotService;
        private readonly ISessionService _sessionService;

        public SlotController(ISlotService slotService, ISessionService sessionService)
        {
            _slotService = slotService;
            _sessionService = sessionService;
        }

        [HttpGet("getSlotStatuses")]
        public ServerResult<List<SlotStatusResponse>> GetSlotStatuses() => _slotService.GetSlotStatuses();

        [HttpPost("create")]
        public async Task<ServerResult> CreateSlotsAsync([FromBody] CreateSlotsRequest request)
        {
            var isSessionValid = await _sessionService.ValidateSessionAsync(request.SessionId, request.UserId);
            if (!isSessionValid)
                return new() { Success = false, Message = "Session expired or incorrect" };

            return await _slotService.CreateSlotsAsync(request);
        }

        [HttpDelete("delete")]
        public async Task<ServerResult> DeleteSlotAsync(int slotId, string sessionId, int userId)
        {
            var isSessionValid = await _sessionService.ValidateSessionAsync(sessionId, userId);
            if (!isSessionValid)
                return new ServerResult { Success = false, Message = "Session expired or incorrect" };

            return await _slotService.DeleteSlotAsync(slotId);
        }

        [HttpPut("update")]
        public async Task<ServerResult> UpdateSlotAsync([FromBody] UpdateSlotRequest request)
        {
            var isSessionValid = await _sessionService.ValidateSessionAsync(request.SessionId, request.UserId);
            if (!isSessionValid)
                return new ServerResult { Success = false, Message = "Session expired or incorrect" };

            return await _slotService.UpdateSlotAsync(request);
        }

        [HttpGet("getList")]
        public async Task<ServerResult<List<Slot>>> GetSlotsAsync(int? status = null, DateTime? date = null) => await _slotService.GetSlotsAsync(status, date);

        [HttpPost("reserve")]
        public async Task<ServerResult> ReserveSlotAsync([FromBody] ReserveSlotRequest request) => await _slotService.ReserveSlotAsync(request);
    }
}
