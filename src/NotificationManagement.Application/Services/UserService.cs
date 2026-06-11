using NotificationManagement.Application.DTOs.Auth;
using NotificationManagement.Application.Interfaces;
using NotificationManagement.Domain.Entities;
using NotificationManagement.Domain.Exceptions;
using NotificationManagement.Domain.Interfaces;
using BC = BCrypt.Net.BCrypt;

namespace NotificationManagement.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public UserService(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
        }
        public async Task<string> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);
            if (user == null)
            {
                throw new UserNotFoundException(dto.Email);
            }

            if(!BC.Verify(dto.Password, user.PasswordHash))
            {
                throw new InvalidPasswordException();
            }

            return _jwtTokenGenerator.GenerateToken(user);
        }

        public async Task RegisterAsync(RegisterDto dto)
        {
            var existingUser = _userRepository.GetByEmailAsync(dto.Email);
            if (existingUser != null)
                throw new UserAlreadyExistsException(dto.Email);
            
            var passwordHash = BC.HashPassword(dto.Password);

            var user = new User(dto.Email, dto.UserName, passwordHash);            

            await _userRepository.AddAsync(user);
            
        }
    }
}
