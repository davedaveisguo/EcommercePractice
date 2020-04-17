using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Dtos;
using API.Errors;
using API.Extensions;
using AutoMapper;
using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;


        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        ITokenService tokenService,
        IMapper mapper 
        )
        {
            _tokenService = tokenService;
            _mapper = mapper;
            _signInManager = signInManager;
            _userManager = userManager;

        }



        // get current user
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            // var email = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?
            // .Value;

             var user = await _userManager.FindByEmailFromClaimsPrinciple(HttpContext.User);

            return new UserDto
            {
                Email = user.Email,
                Token = _tokenService.CreateToken(user),
                DisplayName = user.DisplayName
            };

        }


        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
        {
            // return true if email does exist  
            return await _userManager.FindByEmailAsync(email) != null;

        }

        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            // var email = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?
            // .Value;
            

            var user = await _userManager.FindByEmailWithAddressAsync(HttpContext.User);

            //return user.Address;
            return _mapper.Map<Address, AddressDto>(user.Address);
        }

        // add address to the user
        [Authorize]
        [HttpPut("address")]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto address)
        {
                // give user with address
                var user =await _userManager.FindByEmailWithAddressAsync(HttpContext.User);

                user.Address = _mapper.Map<AddressDto,Address>(address);

                var result =await _userManager.UpdateAsync(user);

                if(result.Succeeded) return Ok(_mapper.Map<Address,AddressDto>(user.Address));

                return BadRequest("Problem updating the user");

        }




        // login method dto is the returned class to the client
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null) return Unauthorized(new ApiResponse(401));

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);


            if (!result.Succeeded) return Unauthorized(new ApiResponse(401));

            // if succeeded
            return new UserDto
            {
                Email = user.Email,
                Token = _tokenService.CreateToken(user),

                DisplayName = user.DisplayName
            };
        }


        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            // check if email exists
            if(CheckEmailExistsAsync(registerDto.Email).Result.Value)
            {
                    return new BadRequestObjectResult(new ApiValidationErrorResponse{
                        Errors =new []
                        {
                            "Email address already in use"
                        }});
            }

            var user = new AppUser
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.DisplayName
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded) return BadRequest(new ApiResponse(400));


            return new UserDto
            {
                DisplayName = user.DisplayName,
                Token = _tokenService.CreateToken(user),
                Email = user.Email
            };

        }




    }
}