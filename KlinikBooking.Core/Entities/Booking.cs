namespace KlinikBooking.Core.Entitites;

public class Booking
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; }
    public int PatientId { get; set; }
    public int TreatmentRoomId { get; set; }
    public virtual Patient Patient { get; set; }
    public virtual TreatmentRoom TreatmentRoom { get; set; }
}