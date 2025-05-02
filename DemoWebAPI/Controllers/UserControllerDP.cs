//using DemoWebAPI.Models;
//using Microsoft.AspNetCore.Mvc;
//using DemoWebAPI.Data;
//using DemoWebAPI.Dtos;

//namespace DemoWebAPI.Controllers
//{
//    [ApiController]
//    [Route("api/{controller}")]
//    public class UserController:ControllerBase
//    {
//        private DataContextDapper _dapper;

//        public UserController(IConfiguration config)
//        {
//            _dapper = new DataContextDapper(config);
//        }

//        [HttpGet]
//        public IEnumerable<User> GetUser()
//        {
//            string sql = @"SELECT * FROM UserMaster";

//            var result = _dapper.LoadData<User>(sql);

//            return result;
//        }

//        [HttpGet("{userId}")]
//        public IActionResult GetUserById(int userId)
//        {
//            string sql = @$"SELECT * FROM UserMaster where UserMasterId={userId}";

//            User result = _dapper.LoadDataSingle<User>(sql);

//            return Ok(result);
//        }

//        [HttpPost]
//        public IActionResult CreateUser(UserDto userDto)
//        {
//            string sql = @"INSERT INTO UserMaster (
//                            FirstName,
//                            LastName,
//                            Email,
//                            Gender,
//                            IsActive
//                        ) VALUES (
//                            '" + userDto.FirstName + @"',
//                            '" + userDto.LastName + @"',
//                            '" + userDto.Email + @"',
//                            '" + userDto.Gender + @"',
//                            '" + userDto.IsActive + @"'
//                        )";


//            if (_dapper.ExecuteSql(sql))
//            {
//                return Ok();
//            }

//            throw new Exception("Failed to add the user");
//        }

//        [HttpPut("{userMasterId}")]
//        public IActionResult UpdateUser(int userMasterId, User user)
//        {
//            string sql = @"Update UserMaster
//                            SET [FirstName]='" + user.FirstName +
//                            "', [LastName]='" + user.LastName +
//                            "',[Email]='" + user.Email +
//                            "',[Gender]='" + user.Gender +
//                            "',[IsActive]='" + user.IsActive +
//                            "' WHERE UserMasterId=" + userMasterId;

//            if (_dapper.ExecuteSql(sql))
//            {
//                return Ok();
//            }

//            throw new Exception("Failed to update the user");
//        }

//        [HttpDelete("{userMasterId}")]
//        public IActionResult DeleteUser(int userMasterId)
//        {
//            string sql = @"DELETE FROM UserMaster WHERE UserMasterId=" + userMasterId;

//            if (_dapper.ExecuteSql(sql))
//            {
//                return Ok();
//            }

//            throw new Exception("Failed to delete the user");
//        }
//    }
//}
