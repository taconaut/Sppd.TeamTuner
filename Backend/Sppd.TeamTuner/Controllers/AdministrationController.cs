using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Sppd.TeamTuner.Core.Services;
using Sppd.TeamTuner.DTOs;

namespace Sppd.TeamTuner.Controllers
{
    /// <summary>
    ///     Offers administration functionality.
    /// </summary>
    /// <seealso cref="ControllerBase" />
    [Authorize]
    [ApiController]
    [Route("administration")]
    public class AdministrationController : ControllerBase
    {
        private readonly IAdministrationService _administrationService;
        private readonly IMapper _mapper;

        public AdministrationController(IAdministrationService administrationService, IMapper mapper)
        {
            _administrationService = administrationService;
            _mapper = mapper;
        }

        /// <summary>
        ///     Gets the system information containing the version, GIT commit hash and build time.
        /// </summary>
        /// <returns>The system information.</returns>
        [AllowAnonymous]
        [HttpGet("system-info")]
        public ActionResult<SystemInfoDto> GetSystemInfo()
        {
            return Ok(_mapper.Map<SystemInfoDto>(_administrationService.GetSystemInfo()));
        }
    }
}