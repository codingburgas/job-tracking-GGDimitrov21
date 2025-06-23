using JobTracking.Domain.DTOs.Request;
using JobTracking.Domain.DTOs.Response;

namespace JobTracking.Application.Contracts
{
    public interface IAuthService
    {
        Task<AuthResponseDTO.AuthResponseDto?> Register(RegisterRequestDto request);
        Task<AuthResponseDTO.AuthResponseDto?> Login(LoginRequestDto request);
    }
}