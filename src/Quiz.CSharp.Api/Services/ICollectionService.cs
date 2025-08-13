namespace Quiz.CSharp.Api.Services;

using Quiz.CSharp.Api.Contracts;

public interface ICollectionService
{
    Task<List<CollectionResponse>> GetCollectionsAsync(CancellationToken cancellationToken = default);
    Task<List<CollectionResponse>> UpdateCollectionAsync(string code, Contracts.Requests.UpdateCollectionRequest nextRequest,
        CancellationToken cancellationToken = default);
}