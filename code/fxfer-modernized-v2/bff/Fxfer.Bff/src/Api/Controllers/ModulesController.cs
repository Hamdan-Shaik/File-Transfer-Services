using Fxfer.Bff.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fxfer.Bff.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/modules")]
public sealed class ModulesController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public ModulesController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpGet("selection")]
    [ProducesResponseType(typeof(SelectionItem[]), StatusCodes.Status200OK)]
    public ActionResult<SelectionItem[]> GetSelection([FromQuery] bool activeOnly = true)
    {
        var modules = ReadModules(_configuration)
            .Where(m => !activeOnly || m.IsActive)
            .OrderBy(m => m.DisplayName, StringComparer.OrdinalIgnoreCase)
            .Select(m => new SelectionItem(m.Id, m.DisplayName))
            .ToArray();

        return Ok(modules);
    }

    private static IEnumerable<ModuleConfig> ReadModules(IConfiguration configuration)
    {
        foreach (var child in configuration.GetSection("ReferenceData:Modules").GetChildren())
        {
            if (!Guid.TryParse(child["Id"], out var id) || id == Guid.Empty)
            {
                continue;
            }

            var displayName = child["DisplayName"] ?? string.Empty;

            var isActive = false;
            _ = bool.TryParse(child["IsActive"], out isActive);

            yield return new ModuleConfig(id, displayName, isActive);
        }
    }

    private sealed record ModuleConfig(Guid Id, string DisplayName, bool IsActive);
}

