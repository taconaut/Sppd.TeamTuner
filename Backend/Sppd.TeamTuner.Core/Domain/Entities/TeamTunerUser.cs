using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Sppd.TeamTuner.Core.Domain.Enumerations;
using Sppd.TeamTuner.Core.Domain.Interfaces;
using Sppd.TeamTuner.Core.Domain.Validation;
using Sppd.TeamTuner.Core.Utils.Extensions;
using Sppd.TeamTuner.Core.Validation;

namespace Sppd.TeamTuner.Core.Domain.Entities
{
    /// <summary>
    ///     Application user
    /// </summary>
    /// <seealso cref="DescriptiveEntity" />
    /// <seealso cref="ITeamTunerUser" />
    public sealed class TeamTunerUser : DescriptiveEntity, ITeamTunerUser
    {
        public IEnumerable<CardLevel> CardLevels { get; set; }

        public UserProfileVisibility ProfileVisibility { get; set; }

        [Required, StringLength(CoreConstants.StringLength.TeamTunerUser.SPPD_NAME)]
        public string SppdName { get; set; }

        [Required, MaxLength(CoreConstants.ArrayLength.TeamTunerUser.PASSWORD_HASH)]
        public byte[] PasswordHash { get; set; }

        [Required, MaxLength(CoreConstants.ArrayLength.TeamTunerUser.PASSWORD_SALT)]
        public byte[] PasswordSalt { get; set; }

        [Required, StringLength(CoreConstants.StringLength.TeamTunerUser.EMAIL)]
        public string Email { get; set; }

        [Required, StringLength(CoreConstants.StringLength.TeamTunerUser.APPLICATION_ROLE)]
        public string ApplicationRole { get; set; }

        public Guid? TeamId { get; set; }

        public Team Team { get; set; }

        [StringLength(CoreConstants.StringLength.TeamTunerUser.TEAM_ROLE)]
        public string TeamRole { get; set; }

        public Guid? FederationId { get; set; }

        public Federation Federation { get; set; }

        [StringLength(CoreConstants.StringLength.TeamTunerUser.FEDERATION_ROLE)]
        public string FederationRole { get; set; }

        public override IEnumerable<EntityValidationError> Validate(IValidationContext context)
        {
            if (Email == null || !Email.IsValidEmail())
            {
                yield return new EntityValidationError($"Invalid: {Email}", nameof(Email));
            }
        }
    }
}