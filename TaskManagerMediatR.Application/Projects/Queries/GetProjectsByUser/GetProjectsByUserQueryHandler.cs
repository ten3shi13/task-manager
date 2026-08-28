using TaskManagerMediatR.Application.Shared.Abstractions.Caching;
using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;
using TaskManagerMediatR.Application.Shared.Abstractions.Repositories;
using TaskManagerMediatR.Application.Shared.Caching;
using TaskManagerMediatR.Application.Shared.Filters;
using TaskManagerMediatR.Contracts.Projects;
using TaskManagerMediatR.Domain.Shared;

namespace TaskManagerMediatR.Application.Projects.Queries.GetProjectsByUser
{
    public sealed class GetProjectsByUserQueryHandler : IQueryHandler<GetProjectsByUserQuery, PagedList<ProjectResponse>>
    {
        private readonly ICacheService _cache;
        private readonly IProjectRepository _projectRepository;
        private readonly ICacheVersionService _cacheVersionService;
        private readonly IQueryCachePolicyFactory _cachePolicyFactory;

        public GetProjectsByUserQueryHandler(
            ICacheService cache,
            IProjectRepository projectRepository,
            ICacheVersionService cacheVersionService,
            IQueryCachePolicyFactory cachePolicyFactory)
        {
            _cache = cache;
            _projectRepository = projectRepository;
            _cachePolicyFactory = cachePolicyFactory;
            _cacheVersionService = cacheVersionService;
        }

        public async Task<Result<PagedList<ProjectResponse>>> Handle(GetProjectsByUserQuery request, CancellationToken cancellationToken)
        {
            var queryType = ProjectsByUserClassifier.Classify(request);

            var policy = _cachePolicyFactory.CreateForProjects();

            if (!policy.ShouldCache(queryType))
                return await GetFromDatabase(request, cancellationToken);

            var expiration = policy.GetExpiration(queryType);

            var versionKey = CacheKeys.UserProjectsVersion(request.OwnerId);

            var version = await _cacheVersionService.Get(versionKey, cancellationToken);

            var cacheKey = CacheKeys.UserProjects(request, version);

            var cached = await _cache.Get<PagedList<ProjectResponse>>(cacheKey, cancellationToken);

            if (cached is not null)
                return Result.Success(cached);

            var result = await GetFromDatabase(request, cancellationToken);

            if (result.IsFailure)
                return result;

            await _cache.Set(cacheKey, result.Value, expiration, cancellationToken);

            return result;

        }

        private async Task<Result<PagedList<ProjectResponse>>> GetFromDatabase(GetProjectsByUserQuery request, CancellationToken cancellationToken)
        {
            var query = _projectRepository.GetFiltered(
                request.Search,
                request.OwnerId,
                request.MemberId,
                request.SortBy,
                request.SortOrder);

            var projected = query.Select(p => new ProjectResponse(
                p.Id,
                p.Name,
                p.Description,
                p.CreatedAt,
                p.OwnerId,
                p.Members.Select(m => new ProjectMemberResponse(
                    m.UserId,
                    m.ProjectRole.ToString(),
                    m.JoinedAt)).ToList()));

            var page = await PagedList<ProjectResponse>.CreateAsync(
                projected,
                request.Page,
                request.PageSize,
                cancellationToken);

            return Result.Success(page);
        }
    }
}
