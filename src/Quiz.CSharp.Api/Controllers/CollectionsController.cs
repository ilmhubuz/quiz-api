namespace Quiz.CSharp.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using Quiz.CSharp.Api.Contracts;
using Quiz.CSharp.Api.Services.Abstractions;
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

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<CollectionResponse>), 200)]
    [ProducesResponseType(typeof(ApiResponse<string>), 404)]
    public async Task<IActionResult> UpdateCollection(int id, [FromBody] UpdateCollectionRequest request,
        CancellationToken cancellationToken)
    {
        var updatedCollections = await collectionService.UpdateCollectionAsync(id, request, cancellationToken);
        return Ok(updatedCollections);
    }
}