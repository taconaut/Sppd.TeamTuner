﻿using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.DTOs;

namespace Sppd.TeamTuner.MappingProfiles
{
    internal class TeamMembershipRequestMappingProfile : BaseMappingProfile<TeamMembershipRequest>
    {
        public TeamMembershipRequestMappingProfile()
        {
            CreateMap<TeamMembershipRequest, TeamMembershipRequestResponseDto>()
                .ForMember(dst => dst.UserName, exp => exp.MapFrom(src => src.User.Name))
                .ForMember(dst => dst.Comment, exp => exp.MapFrom(src => src.Comment));
        }
    }
}