namespace Quiz.CSharp.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Quiz.CSharp.Api.Contracts;
using Quiz.CSharp.Api.Services;
using Quiz.Shared.Contracts;

[ApiController]
[Route("api/csharp/progress")]
[Produces("application/json")]
public sealed class ProgressController(IProgressService progressService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<UserProgressResponse>), 200)]
    public async Task<IActionResult> GetUserProgress(CancellationToken cancellationToken)
    {
        var userProgress = await progressService.GetUserProgressAsync(cancellationToken);
        return Ok(new ApiResponse<List<UserProgressResponse>>(userProgress));
    }
}