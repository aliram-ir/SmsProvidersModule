using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class SmsController : ControllerBase
{
    private readonly ISmsService _smsService;

    public SmsController(ISmsService smsService)
    {
        _smsService = smsService;
    }

    [HttpPost("send")]
    public async Task<IActionResult> Send([FromQuery] string phone, [FromQuery] string message)
    {
        var result = await _smsService.SendMessageAsync(phone, message);

        if (!result.Success)
            return StatusCode(StatusCodes.Status502BadGateway, result);

        return Ok(result);
    }

    [HttpPost("otp")]
    public async Task<IActionResult> SendOtp([FromQuery] string phone, [FromQuery] string otp)
    {
        await _smsService.SendOtpAsync(phone, otp);
        return Ok(new { Status = "OTP Sent" });
    }

    [HttpPost("send-otp")]
    public async Task<IActionResult> SendOtpTemplate(string phone, string otp)
    {
        var result = await _smsService.SendOtpTemplateAsync(phone, otp);

        if (!result.Success)
            return StatusCode(StatusCodes.Status502BadGateway, result);

        return Ok(result);
    }
}
