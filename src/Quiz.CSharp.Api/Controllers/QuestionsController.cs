using Quiz.CSharp.Api.Dtos.Question;

namespace Quiz.CSharp.Api.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quiz.CSharp.Api.Contracts;
using Quiz.CSharp.Api.Services;
using Quiz.Shared.Contracts;
using Quiz.Infrastructure.Authentication;

[ApiController]
[Route("api/csharp/questions")]
[Produces("application/json")]
public sealed class QuestionsController(IQuestionService questionService) : ControllerBase
{
    [HttpGet]
    [Authorize]
    [RequireSubscription("csharp-quiz")]
    [ProducesResponseType<PaginatedApiResponse<QuestionResponse>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetQuestions(
        [FromQuery] int collectionId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var questions = await questionService.GetQuestionsByCollectionAsync(collectionId, page, pageSize, cancellationToken);
        return Ok(new PaginatedApiResponse<QuestionResponse>(
            questions.Items.ToList(),
            questions.TotalCount,
            questions.Page,
            questions.PageSize));
    }

    [HttpGet("preview")]
    [AllowAnonymous]
    [ProducesResponseType<ApiResponse<List<QuestionResponse>>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPreviewQuestions(
        [FromQuery] int collectionId,
        CancellationToken cancellationToken)
    {
        var questions = await questionService.GetPreviewQuestionsAsync(collectionId, cancellationToken);
        return Ok(new ApiResponse<List<QuestionResponse>>(questions));
    }
    
    [HttpPut("{collectionId}/{questionId}")]
    [Authorize(Policy = "Admin:Write")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateQuestion(
        [FromRoute] int collectionId,
        [FromRoute] int questionId,
        [FromBody] UpdateQuestionDto questionDto,
        CancellationToken cancellationToken)
    {
        await questionService.UpdateQuestionAsync(collectionId, questionId, questionDto, cancellationToken);
        return Ok();
    }
} 