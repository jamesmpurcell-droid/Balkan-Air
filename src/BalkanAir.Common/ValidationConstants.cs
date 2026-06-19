namespace BalkanAir.Common;

public static class ValidationConstants
{
    // Aircraft
    public const int AircraftNameMinLength = 2;
    public const int AircraftNameMaxLength = 30;
    public const int AircraftMinSeats = 2;
    public const int AircraftMaxSeats = 180;
    public const int AircraftModelMinLength = 3;
    public const int AircraftModelMaxLength = 20;

    // Baggage
    public const int CabinBagMaxKilograms = 10;
    public const int CheckedInBagMaxKilograms = 32;
    public const int CabinBagFreePrice = 0;
    public const int CheckedInBagPriceEuros = 30;

    // Booking
    public const int MinRowNumber = 1;
    public const int MaxRowNumber = 30;
    public const int ConfirmationCodeLength = 6;

    // Comments
    public const int CommentContentMaxLength = 250;

    // Flight
    public const int FlightNumberLength = 6;
    public const int FlightDestinationMinLength = 2;
    public const int FlightDestinationMaxLength = 50;
    public const int FlightTerminalMinLength = 1;
    public const int FlightTerminalMaxLength = 20;
    public const int FlightMinBaggageKg = 0;
    public const int FlightMaxCheckInBaggageKg = 50;
    public const int FlightMaxCabinBaggageKg = 10;

    // Passenger / User settings
    public const int PassengerNameMinLength = 2;
    public const int PassengerNameMaxLength = 50;
    public const int PassengerIdentityDocMinLength = 5;
    public const int PassengerIdentityDocMaxLength = 20;

    // TravelClass
    public const int EconomyClassCabinBags = 1;
    public const int EconomyClassCheckedInBags = 1;
    public const int FirstClassCabinBags = 1;
    public const int FirstClassCheckedInBags = 2;
    public const int BusinessClassCabinBags = 2;
    public const int BusinessClassCheckedInBags = 2;
    public const int TravelClassMinPrice = 0;
    public const int TravelClassMaxPrice = 100_000;
    public const int FirstAndBusinessClassRows = 2;
    public const int FirstAndBusinessClassSeats = 12;
    public const int EconomyClassRows = 26;
    public const int EconomyClassSeats = 156;

    // Airport / Country
    public const int AirportNameMinLength = 2;
    public const int AirportNameMaxLength = 50;
    public const int AirportAbbreviationMinLength = 1;
    public const int AirportAbbreviationMaxLength = 3;
    public const int CountryNameMinLength = 2;
    public const int CountryNameMaxLength = 50;
    public const int CountryAbbreviationMinLength = 1;
    public const int CountryAbbreviationMaxLength = 2;

    // FlightStatus
    public const int FlightStatusNameMinLength = 2;
    public const int FlightStatusNameMaxLength = 15;

    // News category
    public const int CategoryNameMaxLength = 50;

    // CreditCard
    public const int CvvLength = 3;
}
