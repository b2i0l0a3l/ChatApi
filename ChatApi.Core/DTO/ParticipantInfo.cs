using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApi.Core.DTO
{
    public record ParticipantInfo
    {
        public Guid Id { get; set; }
        public string? FullName { get; set; }
    }
}