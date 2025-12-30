namespace KlinikBooking.Core.Entitites;

public class Booking
{
    public int Id { get; set; }
    public DateTime appointmentStart { get; set; }
    public DateTime appointmentEnd { get; set; }
    public bool IsActive { get; set; }
    public int PatientId { get; set; }
    public int TreatmentRoomId { get; set; }
    public virtual Patient? Patient { get; set; }
    public virtual TreatmentRoom? TreatmentRoom { get; set; }
}