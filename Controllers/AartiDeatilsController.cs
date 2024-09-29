using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using NavYuvakMandarApi.Models;
using NavYuvakMandarApi.Repositories;
using System.Security.Cryptography;
using Newtonsoft.Json;

namespace NavYuvakMandarApi.Controllers
{
    [ApiController]
    [Route("api/")]
    [EnableCors]
    public class AartiDeatilsController : Controller
    {

        private readonly IAartiDeatilsRepository _aartiDetailsRepository;
        private readonly ILogger<AartiDeatilsController> _logger;


        public AartiDeatilsController(IAartiDeatilsRepository aartiDetailsRepository, ILogger<AartiDeatilsController> logger)
        {
            _aartiDetailsRepository = aartiDetailsRepository;
            _logger = logger;

        }
        [HttpGet("aartiDetails")]
        public async Task<IActionResult> GetAartiDetails()
        {
            try
            {

                var user_list = await _aartiDetailsRepository.GetAartiDetails();
                return Ok(user_list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user details");
                return StatusCode(500, "Internal server error");
            }

        }
        [HttpPost("add/aartiDetails")]
     
        public async Task<IActionResult> addAartiDetails([FromBody] AartiDetails aartiDetails)
        {
            try
            { 

                await _aartiDetailsRepository.addAartiDetails(aartiDetails);

                _logger.LogInformation($"aarti details '{aartiDetails.name}' added successfully.");
                return Ok("Info added successful.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding  details");
                return StatusCode(500, "Internal server error");
            }


        }
    }

    internal class JsonSerializerSettingsAttribute : Attribute
    {
        public string DateFormatString { get; set; }
    }
}
