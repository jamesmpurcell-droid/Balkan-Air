namespace BalkanAir.Common;

public static class ErrorMessages
{
    public const string InvalidId = "Id cannot be less than or equal to zero.";
    public const string EntityCannotBeNull = "Entity cannot be null.";
    public const string NullOrEmptyName = "Name cannot be null or empty.";

    // Airport / Country
    public const string AbbreviationCannotBeNullOrEmpty = "Abbreviation cannot be null or empty.";
    public const string AirportNotFound = "Airport not found.";

    // Baggage
    public const string NullCabinBags = "Cabin bags cannot be null. Passenger is allowed to take at least 1 cabin bag.";
    public const string InvalidBaggageEquipmentType = "Invalid baggage type.";

    // Category
    public const string InvalidCategoryName = "Invalid category name.";

    // Flight
    public const string NullOrEmptyFlightNumber = "Flight number cannot be null or empty.";
    public const string NullOrEmptyFlightStatus = "Flight status cannot be null or empty.";

    // News
    public const string InvalidCountValue = "Count cannot be zero or negative.";

    // TravelClass
    public const string NullOrEmptyType = "Type cannot be null or empty.";
    public const string TravelClassNotFound = "Travel class not found.";

    // Notification
    public const string NullOrEmptyListOfUsers = "Cannot send notification to null or empty list of users.";

    // Users
    public const string NullOrEmptyEmail = "Email cannot be null or empty.";
    public const string InvalidImageToUpload = "Invalid image to upload.";
    public const string InvalidGender = "Invalid gender.";
    public const string NullOrEmptyNationality = "Nationality cannot be null or empty.";
    public const string NullOrEmptyId = "ID cannot be null or empty.";
}
