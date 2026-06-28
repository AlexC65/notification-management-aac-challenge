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
        public async Task<string> LoginAsync(LoginRequest dto, CancellationToken ct = default)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email, ct);
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

        public async Task<string> RegisterAsync(RegisterRequest dto, CancellationToken ct = default)
        {
            var existingUser = await _userRepository.GetByEmailAsync(dto.Email, ct);
            if (existingUser != null)
                throw new UserAlreadyExistsException(dto.Email);
            
            var passwordHash = BC.HashPassword(dto.Password);

            var user = new User(dto.Email, dto.UserName, passwordHash);            

            await _userRepository.AddAsync(user);            

            return _jwtTokenGenerator.GenerateToken(user);
        }
    }
}
