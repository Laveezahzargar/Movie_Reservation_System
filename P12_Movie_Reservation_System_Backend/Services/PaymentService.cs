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
    private readonly ILogger<PaymentService> _logger;

    public PaymentService(ApplicationDbContext context, ILogger<PaymentService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ApiResponse<PaymentDetailDto>> ProcessPaymentAsync([FromBody] ProcessPaymentDto request)
    {
        _logger.LogInformation(
            "Processing payment for BookingId {BookingId}. Amount: {Amount}, PaymentMethod: {PaymentMethod}.",
            request.BookingId,
            request.Amount,
            request.PaymentMethod);

        var booking = await _context.Bookings.FindAsync(request.BookingId);

        if (booking == null)
        {
            _logger.LogWarning(
                "Payment processing failed. BookingId {BookingId} not found.",
                request.BookingId);

            return ApiResponse<PaymentDetailDto>.FailureResponse("Booking not found");
        }

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

        _logger.LogInformation(
            "Payment processed successfully. PaymentId {PaymentId}, TransactionId {TransactionId}.",
            payment.PaymentId,
            payment.TransactionId);

        var ticket = new Ticket
        {
            BookingId = request.BookingId,
            TicketNumber = Guid.NewGuid().ToString("N")[..10].ToUpper(),
            QRCodePath = "/qrcodes/sample.png"
        };

        await _context.Tickets.AddAsync(ticket);
        await _context.SaveChangesAsync();

        _logger.LogInformation(
            "Ticket generated successfully. TicketNumber {TicketNumber} for BookingId {BookingId}.",
            ticket.TicketNumber,
            request.BookingId);

        return ApiResponse<PaymentDetailDto>.SuccessResponse(
            new PaymentDetailDto
            {
                PaymentId = payment.PaymentId,
                Amount = payment.Amount,
                PaymentDate = payment.PaymentDate,
                TransactionId = payment.TransactionId,
                Status = payment.Status,
                BookingId = payment.BookingId
            },
            "Payment processed successfully");
    }

    public async Task<ApiResponse<PaymentDetailDto>> GetPaymentByIdAsync(int paymentId)
    {
        _logger.LogInformation(
            "Fetching payment with PaymentId {PaymentId}.",
            paymentId);

        var payment = await _context.Payments.FindAsync(paymentId);

        if (payment == null)
        {
            _logger.LogWarning(
                "Payment with PaymentId {PaymentId} was not found.",
                paymentId);

            return ApiResponse<PaymentDetailDto>.FailureResponse("Payment not found");
        }

        _logger.LogInformation(
            "Payment with PaymentId {PaymentId} retrieved successfully.",
            payment.PaymentId);

        return ApiResponse<PaymentDetailDto>.SuccessResponse(
            new PaymentDetailDto
            {
                PaymentId = payment.PaymentId,
                Amount = payment.Amount,
                PaymentDate = payment.PaymentDate,
                TransactionId = payment.TransactionId,
                Status = payment.Status,
                BookingId = payment.BookingId
            },
            "Payment retrieved successfully");
    }

    public async Task<ApiResponse<bool>> VerifyPaymentAsync(int paymentId)
    {
        _logger.LogInformation(
            "Verifying payment with PaymentId {PaymentId}.",
            paymentId);

        var payment = await _context.Payments.FindAsync(paymentId);

        if (payment == null)
        {
            _logger.LogWarning(
                "Payment verification failed. PaymentId {PaymentId} not found.",
                paymentId);

            return ApiResponse<bool>.FailureResponse("Payment not found");
        }

        var isSuccessful = payment.Status == PaymentStatus.Success;

        _logger.LogInformation(
            "Payment verification completed for PaymentId {PaymentId}. VerificationStatus: {VerificationStatus}.",
            paymentId,
            isSuccessful);

        return ApiResponse<bool>.SuccessResponse(
            isSuccessful,
            "Payment verification completed");
    }
}