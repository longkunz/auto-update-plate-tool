using Microsoft.AspNetCore.Mvc;
using Vietmap.Tracking.Tools.Plate.Services;

namespace Vietmap.Tracking.Tools.Plate.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class VehicleController(IVehicleService service) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<bool>> UpdatePlateVehicle()
        {
            await service.UpdateActualPlateForVehicle();
            return Ok(true);
        }
    }
}
