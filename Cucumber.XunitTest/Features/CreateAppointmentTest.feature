Feature: CreateAppointment
In order to schedule appointments for patients
As a clinic staff member
I want to be able to create appointments for specific dates and times

    @positive @appointment
    Scenario: Successfully create an appointment for next week
        Given at least one doctor is available next week
        And the clinic has an available time slot next week
        When I attempt to create an appointment
        And I select the available doctor
        And I enter the appointment date and time for next week
        And I submit the appointment form
        Then the appointment should be created successfully