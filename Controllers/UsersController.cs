using AltermedManager.Data;
using AltermedManager.Helpers;
using AltermedManager.Models.Dtos;
using AltermedManager.Models.Entities;
using AltermedManager.Models.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AltermedManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ILogger<AddressController> _log;

        public UsersController(ApplicationDbContext dbContext, ILogger<AddressController> log)
        {
            this.dbContext = dbContext;
            _log = log;

            }
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var allUsers = dbContext.Users.ToList();
            return Ok(allUsers);
        }


        [HttpGet("{name}")]
        public IActionResult GetUserByUName(string name)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.name == name);
            if (user == null)
            {
                _log.LogWarning($"No user found with name: {name}");
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet("uid/{uuid}")]
        public async Task<IActionResult> GetUserByUidAsync(Guid uuid)
            {
            var authHeader = Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authHeader))
                {
                _log.LogWarning("Missing or invalid Authorization header");
                return Unauthorized("Missing or invalid Authorization header");
                }
            var token = authHeader.Substring("Bearer ".Length).Trim();

            var firebaseUid = await FirebaseTokenValidator.VerifyTokenAsync(token);
            if (firebaseUid == null || firebaseUid != uuid.ToString())
                {
                _log.LogWarning("Invalid or mismatched token");
                return Unauthorized("Invalid or mismatched token");
            }
               
            var user = dbContext.Users.FirstOrDefault(u => u.id == uuid);
            if (user == null)
                {
                _log.LogWarning($"No user found with ID: {uuid}");
                return NotFound();
                }
            return Ok(user);
            }

        [HttpGet("fuid/{uid}")]
        public async Task<IActionResult> GetUserByFirebaseIdAsync(String uid)
            {
            var authHeader = Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authHeader))
                {
                _log.LogWarning("Missing or invalid Authorization header");
                return Unauthorized("Missing or invalid Authorization header");
                }
            var token = authHeader.Substring("Bearer ".Length).Trim();

            var firebaseUid = await FirebaseTokenValidator.VerifyTokenAsync(token);
            if (firebaseUid == null || firebaseUid != uid.ToString())
                {
                _log.LogWarning("Invalid or mismatched token");
                return Unauthorized("Invalid or mismatched token");
                }
            /*
            var user = new User()
                {
                name = "בדיקה  בדיקה",
                firebaseId = "edr93Eb4nmPVRNtoRgeV0ukNIiR2",
                id = System.Guid.Parse("da66b18d-fdb4-4ea4-acc6-6c67a4570cd0"),
                role = UserRoleConst.PATIENT

                };*/
            var user = dbContext.Users.FirstOrDefault(u => u.firebaseId == uid);
            if (user == null)
                {
                _log.LogWarning($"No user found with Firebase ID: {uid}");
                return NotFound();
                }
            return Ok(user);
            }


        // -----------------FOR TEST ONLY----------------------
        [HttpGet("email{email}")]
        public IActionResult GetUserByEmail(string email)
            {
            var user = dbContext.Patients.FirstOrDefault(u => u.patientEmail == email);
            if (user == null)
                {
                _log.LogWarning($"No user found with email: {email}");
                return NotFound();
                }
            return Ok(user);
            }




        [HttpPost]
        public IActionResult AddUser(NewUserDto newUser)
        {
            var userEntity = new User()
            {
                //appointmentId = newAppointment.appointmentId,
                name = newUser.name,
                firebaseId = newUser.firebaseId,
                msgToken = newUser.msgToken ?? string.Empty, // Default to empty string if null
                id = newUser.id,
                role = newUser.role
                };

            dbContext.Users.Add(userEntity);
            dbContext.SaveChanges();

            return Ok(userEntity);
        }


        [HttpPut]
        [Route("{id:guid}")]
        public IActionResult UpdateUser(Guid id, UpdateUserDto updateUser)
        {
            var user
                = dbContext.Users.Find(id);
            if (user is null)
            {
                _log.LogWarning($"No user found with ID: {id}");
                return NotFound();
            }
            user.id = updateUser.id;
            user.firebaseId = updateUser.firebaseId;
            user.name = updateUser.name;
            dbContext.SaveChanges();
            return Ok(user);
        }


        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult DeleteUser(Guid id)
        {
            var user = dbContext.Users.Find(id);
            if (user is null)
            {
                _log.LogWarning($"No user found with ID: {id}");
                return NotFound();
            }
            dbContext.Users.Remove(user);
            dbContext.SaveChanges();
            return Ok("User deleted");
        }




        public class UpdateTokenRequest
            {
            public string Uid { get; set; }
            public string Token { get; set; }
            }

        [HttpPost("update-token")]
        public async Task<IActionResult> UpdateFcmToken([FromBody] UpdateTokenRequest request)
            {
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.id.ToString() == request.Uid);
            if (user == null)
                return NotFound("User not found");

            user.msgToken = request.Token;
            await dbContext.SaveChangesAsync();

            return Ok("Token updated");
            }

        //================================================================
        //                     Notifications Endpoints
        //================================================================

        [HttpGet("by-user/{userId}")]
        public async Task<IActionResult> GetNotificationsByUser(Guid userId)
            {
            var notifs = await dbContext.StoredNotifications
                .Where(n => n.userId == userId)
                .OrderByDescending(n => n.createdAt)
                .ToListAsync();

            return Ok(notifs);
            }

        [HttpPatch("{id}/read")]
        public async Task<IActionResult> MarkAsRead(int id)
            {
            var notif = await dbContext.StoredNotifications.FindAsync(id);
            if (notif == null) return NotFound();

            notif.isRead = true;
            await dbContext.SaveChangesAsync();

            return Ok();
            }
        }
    }
