using API.Classes;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestController : ControllerBase
    {
        // POST api/Rest
        [HttpPost]
        public async Task<Rest> PostAsync([FromBody] Person person)
        {
            DbConnection db = new DbConnection();
            string sql = string.Empty;
            List<Person>? results = null;

            switch (person.PersonAction)
            {
                case PersonAction.SelectAll:
                    results = await db.SqlSelectAsync(Person.SelectAllPersonSQL());
                    break;

                case PersonAction.Select:
                    results = await db.SqlSelectAsync(Person.SelectPersonSQL(person));
                    break;

                case PersonAction.Insert:

                    if (string.IsNullOrEmpty(person.Name) || string.IsNullOrEmpty(person.Email) || string.IsNullOrEmpty(person.IdNumber) || string.IsNullOrEmpty(person.ZIPCode))
                        return new Rest() { Success = false, Data = "Missing informations" };

                    bool checkIdent = Person.CheckIdentNumber(person.IdNumber, person.Entity);
                    bool checkZip = Person.CheckZipCode(person.ZIPCode);

                    if (!checkIdent || !checkZip)
                        return new Rest() { Success = false, Data = "Identification or Zipcode wrong" };

                    sql = Person.InsertPersonSQL(person);
                    break;

                case PersonAction.Update:
                    if (string.IsNullOrEmpty(person.Name) || string.IsNullOrEmpty(person.Email) || string.IsNullOrEmpty(person.IdNumber) || string.IsNullOrEmpty(person.ZIPCode))
                        return new Rest() { Success = false, Data = "Missing informations" };

                    sql = Person.UpdatePersonSQL(person);
                    break;

                case PersonAction.Delete:
                    sql = Person.DeletePersonSQL(person);
                    break;

                default:
                    return new Rest() { Success = false, Data = "Something went wrong" };
            }

            

            bool success;
            object data;

            if (results == null)
            {
                success = await db.SqlQueryAsync(sql);
                data = "Success on query";
            }
            else
            {
                success = true;
                data = results;
            }

            return new Rest()
            {
                Success = success,
                Data = success ? data : "Something went wrong"
            };
        }
    }
}
