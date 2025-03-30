namespace Webb_Labb02_version2_ApiAndBlazor.Api.Entities
{
    public class User
    {
        /*
         * CREATE TABLE Users (
            UserID INT IDENTITY(1,1) PRIMARY KEY,
            FirstName NVARCHAR(100) NOT NULL,
            LastName NVARCHAR(100) NOT NULL,
            Email NVARCHAR(100) NOT NULL UNIQUE,
            PhoneNumber NVARCHAR(20),
            HomeAddress NVARCHAR(200) NOT NULL,
	        IsAdmin BIT Default 0
           );
        */

        public int UserID { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public required string HomeAddress { get; set; }


        //public bool? IsAdmin { get; set; } <- Jag använder Role istället,
        //för det blir mer flexibelt om jag behöver fler roller sen.
        public required string Role { get; set; } = "user";

        // Password är bara för kontroll.
        // Jag vet att man aldrig ska lagra lösenord i klartext i riktiga projekt.
        public required string Password { get; set; }
        public required string PasswordHash { get; set; }
    }
}
