using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApi.Core.DTO;

namespace ChatApi.Application.Contract.Participant.res
{
    public record ParticipantRes
    {
        public List<ParticipantInfo>? participants { get; set; } 
    }
}