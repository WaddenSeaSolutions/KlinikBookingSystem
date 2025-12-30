using Reqnroll;
using System;
using FluentAssertions;

namespace Cucumber.XunitTest.StepDefinitions;

[Binding]
public sealed class KlinikBookingStepDefinitions
{
    private DateTime _appointmentDate;
    private bool _doctorAvailable;
    private bool _appointmentCreated;

    // ---------- GIVEN ----------

    [Given(@"at least one doctor is available next week")]
    public void GivenAtLeastOneDoctorIsAvailableNextWeek()
    {
        _appointmentDate = DateTime.Today
            .AddDays(7)
            .AddHours(10);

        _doctorAvailable = true;
    }
    
    [Given(@"the clinic has an available time slot next week")]
    public void GivenTheClinicHasAnAvailableTimeSlotNextWeek()
    {
        // Just a placeholder to indicate the clinic has an available time slot
    }

    [Given(@"no doctors are available next month")]
    public void GivenNoDoctorsAreAvailableNextMonth()
    {
        _appointmentDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1)
            .AddMonths(1)
            .AddDays(5)
            .AddHours(10);

        _doctorAvailable = false;
    }

    // ---------- WHEN ----------

    [When(@"I attempt to create an appointment")]
    public void WhenIAttemptToCreateAnAppointment()
    {
        // Intentionally empty
        // This step represents the intent to create an appointment
        // Actual logic is triggered when the form is submitted
    }
    
    [When(@"I select the available doctor")]
    public void WhenISelectTheAvailableDoctor()
    {
        // Just a placeholder to indicate the doctor selection step
    }

    [When(@"I enter the appointment date and time for next week")]
    public void WhenIEnterTheAppointmentDateAndTimeForNextWeek()
    {
        // Reuse the date set in Given
    }

    [When(@"I enter the appointment date and time for next month")]
    public void WhenIEnterTheAppointmentDateAndTimeForNextMonth()
    {
        // Reuse the date set in Given
    }

    [When(@"I submit the appointment form")]
    public void WhenISubmitTheAppointmentForm()
    {
        _appointmentCreated = _doctorAvailable && _appointmentDate > DateTime.Today;
    }

    // ---------- THEN ----------

    [Then(@"the appointment should be created successfully")]
    public void ThenTheAppointmentShouldBeCreatedSuccessfully()
    {
        _appointmentCreated.Should()
            .BeTrue("the appointment should succeed if a doctor was available and the date was valid");
    }

    [Then(@"the appointment should not be created")]
    public void ThenTheAppointmentShouldNotBeCreated()
    {
        _appointmentCreated.Should()
            .BeFalse("the appointment should fail if no doctors were available");
    }
}
