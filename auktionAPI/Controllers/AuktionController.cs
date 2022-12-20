using Microsoft.AspNetCore.Mvc;

namespace auktionAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuktionController : ControllerBase
{
    private readonly ILogger<AuktionController> _logger;
    private readonly IAuktionService _auktionService;

    public AuktionController(IAuktionService auktionService, ILogger<AuktionController> logger)
    {
        _logger = logger;
        _auktionService = auktionService;

        var hostName = System.Net.Dns.GetHostName();
        var ips = System.Net.Dns.GetHostAddresses(hostName);
        var _ipaddr = ips.First().MapToIPv4().ToString();
        _logger.LogInformation(1, $"AuktionController responding from {_ipaddr}");
    }

    [HttpGet]
    public async Task<List<Auktion>> Get() =>
        await _auktionService.GetAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Auktion>> Get(string id)
    {
        var auktion = await _auktionService.GetAsync(id);

        if (auktion is null)
        {
            return NotFound();
        }

        return auktion;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Auktion newAuktion)
    {
        await _auktionService.CreateAsync(newAuktion);

        return CreatedAtAction(nameof(Get), new { id = newAuktion.Id }, newAuktion);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Auktion updatedAuktion)
    {
        var auktion = await _auktionService.GetAsync(id);

        if (auktion is null)
        {
            return NotFound();
        }

        updatedAuktion.Id = auktion.Id;

        await _auktionService.UpdateAsync(id, updatedAuktion);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var auktion = await _auktionService.GetAsync(id);

        if (auktion is null)
        {
            return NotFound();
        }

        await _auktionService.RemoveAsync(id);

        return NoContent();
    }
}
