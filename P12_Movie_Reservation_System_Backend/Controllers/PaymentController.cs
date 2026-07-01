using Microsoft.AspNetCore.Mvc;
using P12_Movie_Reservation_System_Backend.DTOs.Payment;
using P12_Movie_Reservation_System_Backend.Interfaces;

namespace P12_Movie_Reservation_System_Backend.Controllers;

[ApiController]
[Route("api/payments")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    // POST: /api/payments/process
    [HttpPost("process")]
    public async Task<IActionResult> Process([FromBody] ProcessPaymentDto request)
    {
        var result = await _paymentService.ProcessPaymentAsync(request);
        return Ok(result);
    }

    // GET: /api/payments/{paymentId}
    [HttpGet("{paymentId}")]
    public async Task<IActionResult> GetById(int paymentId)
    {
        var result = await _paymentService.GetPaymentByIdAsync(paymentId);
        return Ok(result);
    }

    // GET: /api/payments/verify/{paymentId}
    [HttpGet("verify/{paymentId}")]
    public async Task<IActionResult> Verify(int paymentId)
    {
        var result = await _paymentService.VerifyPaymentAsync(paymentId);
        return Ok(result);
    }
}