using Dapper;
using Vietmap.Tracking.Tools.Plate.DbContexts;
using Vietmap.Tracking.Tools.Plate.Models;

namespace Vietmap.Tracking.Tools.Plate.Repositories
{
    public class VehicleRepository(DapperDbContext context, ILogger<VehicleRepository> logger) : IVehicleRepository
    {
        /// <summary>Get all vehicle without actual plate.</summary>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<IEnumerable<Vehicle>> GetVehicleWithoutActualPlate()
        {
            try
            {
                var query = @"SELECT Id, Plate, ActualPlate FROM dbo.tbl_Vehicle WHERE ActualPlate IS NULL AND Inactive = 0";
                using var connection = await context.CreateConnection();
                var vehicles = await connection.QueryAsync<Vehicle>(query);
                return vehicles;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "VehicleRepository - GetVehicleWithoutActualPlate - Error - {Message}", ex.Message);
                throw;
            }
        }

        /// <summary>Update actual plate to vehicle.</summary>
        /// <param name="vehicle">The vehicle.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<bool> UpdateActualPlateToVehicle(Vehicle vehicle)
        {
            try
            {
                var query = @"UPDATE dbo.tbl_Vehicle SET ActualPlate = @ActualPlate, LastModified = GETDATE() WHERE Id = @Id";
                var parameters = new DynamicParameters();
                parameters.Add("Id", vehicle.Id, System.Data.DbType.Int32);
                parameters.Add("ActualPlate", vehicle.ActualPlate, System.Data.DbType.String);

                using var connection = await context.CreateConnection();
                return await connection.ExecuteAsync(query, parameters) > 0;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "VehicleRepository - UpdateActualPlateToVehicle - Error - {Message}", ex.Message);
                throw;
            }
        }
    }
}
