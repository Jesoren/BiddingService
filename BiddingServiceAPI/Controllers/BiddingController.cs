using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class BiddingController : ControllerBase
{
    [HttpPost("send-bid")]
    public async Task<IActionResult> SendBid()
    {
        await RabbitMQSender.SendBidMessage();
        return Ok("Bid message sent successfully!");
    }
}
