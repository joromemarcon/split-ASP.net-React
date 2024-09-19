using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using split_api.Data;

namespace split_api.Controllers
{
    public class SplitUserController : ControllerBase
    {
        private readonly DataContext _context;
        public SplitUserController(DataContext context)
        {
            _context = context;
        }

    }
}