using Vietmap.Tracking.Tools.Plate.Models;

namespace Vietmap.Tracking.Tools.Plate.Repositories
{
    public interface IVehicleRepository
    {
        /// <summary>Update actual plate to vehicle.</summary>
        /// <param name="vehicle">The vehicle.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<bool> UpdateActualPlateToVehicle(Vehicle vehicle);

        /// <summary>Get vehicle without actual plate.</summary>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<IEnumerable<Vehicle>> GetVehicleWithoutActualPlate();
    }
}
