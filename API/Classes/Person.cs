namespace API.Classes
{
    public class Person
    {
        private PersonAction personAction;
        private int id;
        private string email;
        private EntityType entity;
        private string idNumber;
        private string zIPCode;
        private string state;
        private string name;
        private string address;
        private string publicArea;
        private string district;
        private string city;

        public PersonAction PersonAction { get => personAction; set => personAction = value; }
        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Email { get => email; set => email = value; }
        public EntityType Entity { get => entity; set => entity = value; }
        public string IdNumber { get => idNumber; set => idNumber = value; }
        public string ZIPCode { get => zIPCode; set => zIPCode = value; }
        public string Address { get => address; set => address = value; }
        public string PublicArea { get => publicArea; set => publicArea = value; }
        public string District { get => district; set => district = value; }
        public string City { get => city; set => city = value; }
        public string State { get => state; set => state = value; }

        public static bool CheckIdentNumber(string _Ident, EntityType _entityType)
        {
            int chCnterIdent = _entityType == EntityType.NaturalPerson ? 14 : 18;
            int chCnterIdentOnlyNumbers = _entityType == EntityType.NaturalPerson ? 11 : 14;

            if (string.IsNullOrEmpty(_Ident))
                return false; // Check to see if there's text inside

            string onlyNumbers = _Ident.Replace("-", string.Empty).Replace(".", string.Empty).Replace("/", string.Empty);

            if (_Ident.Length != chCnterIdent)
                return false; // With mask, it must have 14 or 18 characters
            else if (onlyNumbers.Length != chCnterIdentOnlyNumbers)
                return false; // Without the mask, it must have 11 or 14 characters

            if (long.TryParse(onlyNumbers, out long parsedNumbers))
                return true; // With special characters removed,
                             // and made sure the text can be parsed to int, it passed the checkup
            else return false; // Only numbers must be left
        }
        public static bool CheckZipCode(string _ZipCode)
        {
            if (string.IsNullOrEmpty(_ZipCode))
                return false; // Check to see if there's text inside

            string onlyNumbers = _ZipCode.Replace("-", string.Empty);

            if (_ZipCode.Length != 9)
                return false; // With mask, it must have 9 characters
            else if (onlyNumbers.Length != 8)
                return false; // Without the mask, it must have 11 or 14 characters

            if (int.TryParse(onlyNumbers, out int parsedNumbers))
                return true; // With special characters removed,
                             // and made sure the text can be parsed to int, it passed the checkup
            else return false; // Only numbers must be left
        }

        public static string SelectAllPersonSQL()
        {
            return $"select * from Person limit 10";
        }
        public static string SelectPersonSQL(Person _person)
        {
            return $"select * from Person where id = {_person.Id}";
        }
        public static string InsertPersonSQL(Person _person)
        {
            return $"INSERT INTO `person`(`name`, `email`, `entityType`, `idNumber`, `zipCode`, `address`, `publicArea`, `district`, `city`, `state`) VALUES (" +
                $"'{_person.name}'," +
                $"'{(string)_person.email}'," +
                $"{(int)_person.entity}," +
                $"'{_person.idNumber}'," +
                $"'{_person.zIPCode}'," +
                $"'{_person.address}'," +
                $"'{_person.publicArea}'," +
                $"'{_person.district}'," +
                $"'{_person.city}'," +
                $"'{_person.state}')";
        }
        public static string UpdatePersonSQL(Person _person)
        {
            return $"UPDATE Person SET `name`='{_person.Name}',`email`='{_person.Email}',`entityType`={(int)_person.Entity},`idNumber`='{_person.IdNumber}',`zipCode`='{_person.ZIPCode}',`address`='{_person.Address}',`publicArea`='{_person.PublicArea}',`district`='{_person.District}',`city`='{_person.City}',`state`='{_person.State}' WHERE id = {_person.Id}";
        }
        public static string DeletePersonSQL(Person _person)
        {
            return $"delete from Person where id = {_person.Id}";
        }
    }
    public enum EntityType { NaturalPerson, LegalEntity };
    public enum PersonAction { SelectAll, Select, Insert, Update, Delete }
}
