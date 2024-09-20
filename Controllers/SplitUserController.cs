using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using split_api.Data;
using split_api.Interfaces;
using split_api.Mappers;

namespace split_api.Controllers
{
    [Route("split-api/SplitUser")]
    public class SplitUserController : ControllerBase
    {
        private readonly ISplitUserRepository _splitUserRepo;
        public SplitUserController(ISplitUserRepository splitUserRepo)
        {
            _splitUserRepo = splitUserRepo;
        }

        /*
            GET REQUEST
        */
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _splitUserRepo.GetAllAsync();
            var usersDto = users.Select(s => s.ToSplitUserDto());

            return Ok(usersDto);
        }

        /*
            GET BY ID
        */

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var user = await _splitUserRepo.GetByIdAsync(id);
            if (user is null) return NotFound();

            return Ok(user.ToSplitUserDto());
        }
    }
}