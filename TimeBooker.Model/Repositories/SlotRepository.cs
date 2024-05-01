using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeBooker.Model.Entities;
using TimeBooker.Model.Entities.Enums;
using TimeBooker.Model.Entities.Requests;

namespace TimeBooker.Model.Repositories
{
    public class SlotRepository : ISlotRepository
    {
        private readonly IRepository _repository;

        public SlotRepository(IRepository repository)
        {
            _repository = repository;
        }

        public async Task DeleteSlotAsync(int id)
        {
            var sql = "DELETE FROM slots WHERE Id = @Id";
            await _repository.ExecuteAsync(sql, new { Id = id });
        }

        public async Task<List<Slot>> GetSlotsAsync(int? status, DateTime? date)
        {
            var sql = "SELECT * FROM slots";
            var conditions = new List<string>();
            if (status != null)
                conditions.Add("Status = @Status");
            if (date != null)
                conditions.Add("DATE(DateTime) = @Date");
            if (conditions.Count > 0)
                sql += " WHERE " + string.Join(" AND ", conditions);
            DateTime? dateValue = date.HasValue ? date.Value.Date : null;
            int? statusValue = status.HasValue ? status : null;
            return (await _repository.QueryListAsync<Slot, dynamic>(sql, new { Status = statusValue, Date = dateValue })).ToList();
        }

        public async Task InsertAsync(Slot slot)
        {
            var sql = @"
            INSERT INTO slots (`DateTime`)
            VALUES (@DateTime);";

            await _repository.ExecuteAsync(sql, new { slot.DateTime });
        }

        public async Task ReserveSlotAsync(ReserveSlotRequest request)
        {
            var sql = @"UPDATE slots SET
            Email = @Email,
            Status = @Status
            Where Id = @SlotId";

            var status = (int)SlotStatus.Reserved;

            await _repository.ExecuteAsync(sql, new { request.Email, status, request.SlotId });
        }

        public async Task UpdateSlotAsync(UpdateSlotRequest request)
        {
            var sql = @"
            UPDATE slots 
            SET DateTime = @DateTime,
                Email = @Email,
                Status = @Status
            WHERE Id = @Id;";

            var status = (int)request.Status;
            var email = !string.IsNullOrWhiteSpace(request.Email) ? request.Email : null;

            await _repository.ExecuteAsync(sql, new { request.DateTime, email, status, request.Id });
        }
    }
}
