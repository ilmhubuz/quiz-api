namespace Quiz.CSharp.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using Quiz.CSharp.Api.Contracts;
using Quiz.CSharp.Api.Services;
using Quiz.Shared.Contracts;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/csharp/user-progress")]
[Produces("application/json")]
[Authorize] 
public sealed class UserProgressController(IUserProgressService userProgressService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<UserProgressResponse>), 200)]
    public async Task<IActionResult> GetUserProgress(CancellationToken cancellationToken)
    {
        var userProgress = await userProgressService.GetUserProgressAsync(cancellationToken);
        return Ok(new ApiResponse<List<CollectionProgressResponse>>(userProgress));
    }
}