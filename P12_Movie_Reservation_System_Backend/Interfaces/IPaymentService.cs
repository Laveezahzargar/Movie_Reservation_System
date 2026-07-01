using P12_Movie_Reservation_System_Backend.Common;
using P12_Movie_Reservation_System_Backend.DTOs.Payment;

namespace P12_Movie_Reservation_System_Backend.Interfaces;

public interface IPaymentService
{
    Task<ApiResponse<PaymentDetailDto>> ProcessPaymentAsync(ProcessPaymentDto request);

    Task<ApiResponse<PaymentDetailDto>> GetPaymentByIdAsync(int paymentId);

    Task<ApiResponse<bool>> VerifyPaymentAsync(int paymentId);
}