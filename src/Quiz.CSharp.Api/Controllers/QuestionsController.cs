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
[RequireSubscription("csharp-quiz")]
public sealed class QuestionsController(IQuestionService questionService) : ControllerBase
  { 
      [HttpPost]
      [Authorize]
      [ProducesResponseType<ApiResponse<QuestionResponse>>(StatusCodes.Status200OK)]
      [ProducesResponseType<ApiResponse<QuestionResponse>>(StatusCodes.Status400BadRequest)]
      public async Task<IActionResult> CreateQuestion(
              [FromBody] CreateQuestionRequest request,
              CancellationToken cancellationToken)
      {
          var result = await questionService.CreateQuestionAsync(
          request.Type,
          request.Subcategory,
          request.Difficulty,
          request.Prompt,
          request.EstimatedTimeMinutes,
          cancellationToken);
          return result.IsSuccess
              ? Ok(new ApiResponse<QuestionResponse>(result.Value))
              : BadRequest(new ApiResponse<QuestionResponse>(
                  Success: false,
                  Message: result.ErrorMessage,
                  Errors: result.Errors));
      }

      [HttpGet]
      [Authorize]
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
  }