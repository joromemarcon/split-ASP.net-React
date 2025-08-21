// using System;
// using System.Collections.Generic;
// using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using split_api.DTO.SplitUser;
using split_api.Interfaces;
using split_api.Models;
using split_api.Service;
// using Microsoft.AspNetCore.Mvc;
// using split_api.Data;
// using split_api.DTO.SplitUser;
// using split_api.Helpers;
// using split_api.Interfaces;
// using split_api.Mappers;
// using split_api.Models;

namespace split_api.Controllers
{
    [Route("SplitUser")]
    [ApiController]
    public class SplitUserController : ControllerBase
    {
        private readonly UserManager<SplitUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<SplitUser> _signInManager;
        public SplitUserController(UserManager<SplitUser> userManager, ITokenService tokenService, SignInManager<SplitUser> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == loginDto.Email.ToLower());

            if (user == null) return Unauthorized("Invalid email!");

            //Last argument sets lockoutfailure to false to avoid account locking up when logged incorrectly
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded) return Unauthorized("Email not found or password incorrect");

            return Ok(
                new NewUserDto
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Token = _tokenService.CreateToken(user)
                }
            );
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var splitUser = new SplitUser
                {
                    UserName = registerDto.Username,
                    Email = registerDto.Email,
                    FullName = registerDto.FullName ?? ""
                };

                var createdUser = await _userManager.CreateAsync(splitUser, registerDto.Password);

                /*
                    -   This part assigns a role to created user.
                    -   Initially as User for now since not everyone can be admin

                    -   Future work, will add another end point that if a link is sent to
                        another user from an admin they will be assigned user
                    -   Future work, will assign admin to initial owner/user of receipt
                */
                if (createdUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(splitUser, "User");
                    if (roleResult.Succeeded)
                    {
                        return Ok(
                            new NewUserDto
                            {
                                UserName = splitUser.UserName,
                                Email = splitUser.Email,
                                Token = _tokenService.CreateToken(splitUser)
                            }
                        );
                    }
                    else
                    {
                        return StatusCode(500, roleResult.Errors);
                    }
                }
                else
                {
                    return StatusCode(500, createdUser.Errors);
                }

            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        [HttpGet("all-users")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userManager.Users.Select(u => new {
                    username = u.UserName,
                    email = u.Email,
                    fullName = u.FullName,
                    id = u.Id
                }).ToListAsync();
                
                return Ok(new { 
                    count = users.Count,
                    users = users 
                });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        //         /*
        //             GET REQUEST
        //         */
        //         [HttpGet]
        //         public async Task<IActionResult> GetAll([FromQuery] UserQueryObject query)
        //         {
        //             var users = await _splitUserRepo.GetAllAsync(query);
        //             var usersDto = users.Select(s => s.ToSplitUserDto());

        //             return Ok(usersDto);
        //         }

        //         /*
        //             GET BY ID
        //         */

        //         [HttpGet("{id}")]
        //         public async Task<IActionResult> GetById([FromRoute] string id)
        //         {
        //             var user = await _splitUserRepo.GetByIdAsync(id);
        //             if (user is null) return NotFound();

        //             return Ok(user);
        //         }

        //         /*
        //             POST REQUEST
        //         */

        //         [HttpPost]
        //         public async Task<IActionResult> Create([FromBody] CreateSplitUserDto userDto)
        //         {
        //             var userModel = userDto.ToSplitUserFromCreateSplitUserDto();
        //             await _splitUserRepo.CreateAsync(userModel);

        //             return CreatedAtAction(nameof(GetById), new { id = userModel.Id }, userModel.ToSplitUserDto());
        //         }

        //         /*
        //             UPDATE REQUEST
        //         */
        //         [HttpPut("{id}")]

        //         public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateUserDto updateUserDto)
        //         {
        //             var splitUserModel = await _splitUserRepo.UpdateAsync(id, updateUserDto);
        //             if (splitUserModel is null) return NotFound();
        //             return NoContent();
        //         }



        //         /*
        //             DELETE REQUEST
        //         */
        //         [HttpDelete("DeleteById/{id}")]
        //         public async Task<IActionResult> Delete([FromRoute] string id)
        //         {
        //             var splitUserModel = await _splitUserRepo.DeleteAsync(id);
        //             if (splitUserModel is null) return NotFound();

        //             return NoContent();
        //         }
    }
}