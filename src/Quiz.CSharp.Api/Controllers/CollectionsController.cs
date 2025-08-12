namespace Quiz.CSharp.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using Quiz.CSharp.Api.Contracts;
using Quiz.CSharp.Api.Services;
using Quiz.Shared.Contracts;

[ApiController]
[Route("api/csharp/collections")]
[Produces("application/json")]
public sealed class CollectionsController(ICollectionService collectionService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<List<CollectionResponse>>), 200)]
    public async Task<IActionResult> GetCollections(CancellationToken cancellationToken)
    {
        var collections = await collectionService.GetCollectionsAsync(cancellationToken);
        return Ok(new ApiResponse<List<CollectionResponse>>(collections));
    }

    [HttpPut("{code}")]
    [ProducesResponseType(typeof(ApiResponse<CollectionResponse>), 200)]
    [ProducesResponseType(typeof(ApiResponse<string>), 404)]
    public async Task<IActionResult> UpdateCollection(string code, [FromBody] Contracts.Requests.UpdateCollectionRequest request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(code))
            return BadRequest(new ApiResponse<string>("Collection code cannot be empty."));

        var updatedCollections = await collectionService.UpdateCollectionAsync(code, request, cancellationToken);
        
        if (updatedCollections.Count == 0)
            return NotFound(new ApiResponse<string>($"No collection found with code '{code}'."));

        return Ok(new ApiResponse<List<CollectionResponse>>(updatedCollections));
    }
}