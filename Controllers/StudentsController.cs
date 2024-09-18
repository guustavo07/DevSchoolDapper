using Dapper;
using DevSchool.Entities;
using DevSchool.Helpers;
using DevSchool.InputModel;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace DevSchool.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        public StudentsController()
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            using (var sqlConnection = new SqlConnection(ConfigHelper.GetConnectionString()))
            {
                const string sql = "SELECT * FROM Students WHERE IsActive = 1";

                var students = await sqlConnection.QueryAsync<Student>(sql);

                return Ok(students);
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var parameters = new { id };

            using (var sqlConnection = new SqlConnection(ConfigHelper.GetConnectionString()))
            {
                const string sql = "SELECT * FROM Students WHERE Id = @id";

                var student = await sqlConnection.QuerySingleOrDefaultAsync<Student>(sql, parameters);

                if (student is null)
                    return NotFound();

                return Ok(student);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(StudentsInputModel model)
        {
            var student = new Student(model.FullName, model.Birthdate, model.SchoolClass);

            var parameters = new
            {
                student.FullName,
                student.Birthdate,
                student.SchoolClass,
                student.IsActive
            };

            using (var sqlConnection = new SqlConnection(ConfigHelper.GetConnectionString()))
            {
                const string sql = "INSERT INTO Students OUTPUT INSERTED.Id VALUES(@FullName, @Birthdate, @SchoolClass, @IsActive)";

                var id = await sqlConnection.ExecuteScalarAsync<int>(sql, parameters);

                return Ok(id);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, StudentsInputModel model)
        {
            var parameters = new
            {
                id,
                model.FullName,
                model.Birthdate,
                model.SchoolClass
            };

            using (var sqlConnection = new SqlConnection(ConfigHelper.GetConnectionString()))
            {
                const string sql = "UPDATE Students SET FullName = @FullName, Birthdate = @Birthdate, SchoolClass = @SchoolClass WHERE Id = @id";

                await sqlConnection.ExecuteAsync(sql, parameters);

                return NoContent();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var parameters = new { id };

            using (var sqlConnection = new SqlConnection(ConfigHelper.GetConnectionString()))
            {
                const string sql = "UPDATE Students SET IsActive = 0 WHERE Id = @id";

                await sqlConnection.ExecuteAsync(sql, parameters);

                return NoContent();
            }
        }
    }
}