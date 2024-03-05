namespace Vietmap.Tracking.Tools.Plate.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string Plate { get; set; }
        public string? ActualPlate { get; set; }
        public DateTime LastModified { get; set; } = DateTime.Now;
    }
}
