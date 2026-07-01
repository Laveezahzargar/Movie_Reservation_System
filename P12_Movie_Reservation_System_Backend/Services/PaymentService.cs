using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P12_Movie_Reservation_System_Backend.Common;
using P12_Movie_Reservation_System_Backend.Data.ApplicationDbContext;
using P12_Movie_Reservation_System_Backend.DTOs.Payment;
using P12_Movie_Reservation_System_Backend.Enums;
using P12_Movie_Reservation_System_Backend.Interfaces;
using P12_Movie_Reservation_System_Backend.Models.DomainModels;

namespace P12_Movie_Reservation_System_Backend.Services;

public class PaymentService : IPaymentService
{
    private readonly ApplicationDbContext _context;

    public PaymentService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<PaymentDetailDto>> ProcessPaymentAsync([FromBody]ProcessPaymentDto request)
    {
        var booking = await _context.Bookings.FindAsync(request.BookingId);

        if (booking == null)
            return ApiResponse<PaymentDetailDto>.FailureResponse("Booking not found");

        var payment = new Payment
        {
            BookingId = request.BookingId,
            Amount = request.Amount,
            PaymentMethod = request.PaymentMethod,
            TransactionId = Guid.NewGuid().ToString("N"),
            Status = PaymentStatus.Success
        };

        await _context.Payments.AddAsync(payment);

        booking.Status = "Confirmed";

        await _context.SaveChangesAsync();

        var ticket = new Ticket
        {
            BookingId = request.BookingId,
            TicketNumber = Guid.NewGuid().ToString("N")[..10].ToUpper(),
            QRCodePath = "/qrcodes/sample.png"
        };

        await _context.Tickets.AddAsync(ticket);

        return ApiResponse<PaymentDetailDto>.SuccessResponse(new PaymentDetailDto
        {
            PaymentId = payment.PaymentId,
            Amount = payment.Amount,
            PaymentDate = payment.PaymentDate,
            TransactionId = payment.TransactionId,
            Status = payment.Status,
            BookingId = payment.BookingId
        }, "Payment processed successfully");
    }

    public async Task<ApiResponse<PaymentDetailDto>> GetPaymentByIdAsync(int paymentId)
    {
        var payment = await _context.Payments.FindAsync(paymentId);

        if (payment == null)
            return ApiResponse<PaymentDetailDto>.FailureResponse("Payment not found");

        return ApiResponse<PaymentDetailDto>.SuccessResponse(new PaymentDetailDto
        {
            PaymentId = payment.PaymentId,
            Amount = payment.Amount,
            PaymentDate = payment.PaymentDate,
            TransactionId = payment.TransactionId,
            Status = payment.Status,
            BookingId = payment.BookingId
        }, "Payment retrieved successfully");
    }

    public async Task<ApiResponse<bool>> VerifyPaymentAsync(int paymentId)
    {
        var payment = await _context.Payments.FindAsync(paymentId);

        if (payment == null)
            return ApiResponse<bool>.FailureResponse("Payment not found");

        return ApiResponse<bool>.SuccessResponse(
            payment.Status == PaymentStatus.Success,
            "Payment verification completed");
    }
}