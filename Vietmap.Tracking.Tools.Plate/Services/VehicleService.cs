using System.Text.RegularExpressions;
using Vietmap.Tracking.Tools.Plate.Repositories;

namespace Vietmap.Tracking.Tools.Plate.Services
{
    public partial class VehicleService(IVehicleRepository repository, ILogger<VehicleService> logger) : IVehicleService
    {
        public async Task UpdateActualPlateForVehicle()
        {
            try
            {
                var vehicles = await repository.GetVehicleWithoutActualPlate() ?? throw new Exception("List vehicle without actual plate is null!");
                logger.LogInformation("GetVehicleWithoutActualPlate - {Count}", vehicles.Count());

                Regex rg1 = new(@"^\d{2}[A-Za-z]-\d{3}\.\d{2}$"); // 76C-106.51
                Regex rg2 = new(@"^\d{2}[A-Za-z]-\d{3}\d{2}$"); // 76C-10651
                Regex rg3 = new(@"^\d{2}[A-Za-z]\d{3}\d{2}$"); // 76C10651
                Regex rg4 = new(@"^\d{2}[A-Za-z]\d{3}\.\d{2}$"); // 76C 106.51
                Regex rg5 = new(@"^\d{2}[A-Za-z]-\d{3}\.\d{2}"); // 76C-106.51 ABC; 76C-106.51_ABC
                Regex rg6 = new(@"^\d{2}[A-Za-z]{2}-\d{4}$"); // 29LD-4775
                Regex rg7 = new(@"^\d{2}[A-Za-z]-\d{4}$"); // 30K-1342
                Regex rg8 = new(@"^\d{2}[A-Za-z]{2}-\d{3}\.\d{2}$"); // 29LD-069.42
                Regex rg9 = new(@"^\d{2}[A-Za-z0-9]{2}-\d{3}\.[0-9]{2}."); // 36B1-332.10 (Thai Quang Phuc_Thu Ap)

                foreach (var vehicle in vehicles)
                {
                    if (string.IsNullOrEmpty(vehicle.Plate))
                        continue;

                    var tempPlate = vehicle.Plate.Replace(" ", "");
                    var actualPlate = string.Empty;

                    if (rg1.IsMatch(tempPlate) || rg2.IsMatch(tempPlate) || rg4.IsMatch(tempPlate) ||
                        rg6.IsMatch(tempPlate) || rg7.IsMatch(tempPlate) || rg8.IsMatch(tempPlate))
                    {
                        actualPlate = SpecialCharacter().Replace(tempPlate, string.Empty);
                    }
                    else if (rg3.IsMatch(tempPlate))
                    {
                        actualPlate = tempPlate;
                    }
                    else if (rg5.IsMatch(tempPlate))
                    {
                        tempPlate = SpecialCharacter().Replace(tempPlate, string.Empty);
                        actualPlate = tempPlate[..8];
                    }
                    else if (rg9.IsMatch(tempPlate))
                    {
                        tempPlate = SpecialCharacter().Replace(tempPlate, string.Empty);
                        actualPlate = tempPlate[..9];
                    }

                    if (string.IsNullOrEmpty(actualPlate))
                        continue;
                    // Update actual plate.
                    vehicle.ActualPlate = actualPlate.ToUpper();
                    await repository.UpdateActualPlateToVehicle(vehicle);
                    logger.LogInformation("Process update plate for vehicle {VehicleId} - Plate {Plate} - Actual Plate {ActualPlate} success", vehicle.Id, vehicle.Plate, vehicle.ActualPlate);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "VehicleService - UpdateActualPlateForVehicle - Error - {Message}", ex.Message);
                throw;
            }
        }

        /// <summary>Regex use to remove special character.</summary>
        /// <returns>
        ///   <br />
        /// </returns>
        [GeneratedRegex("[^a-zA-Z0-9]")]
        private static partial Regex SpecialCharacter();
    }
}
