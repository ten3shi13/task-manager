using Microsoft.Extensions.Options;
using TaskManagerMediatR.Application.Shared.Abstractions.Caching;
using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;
using TaskManagerMediatR.Application.Shared.Abstractions.Repositories;
using TaskManagerMediatR.Application.Shared.Caching;
using TaskManagerMediatR.Contracts.Projects;
using TaskManagerMediatR.Domain.Errors;
using TaskManagerMediatR.Domain.Models;
using TaskManagerMediatR.Domain.Shared;

namespace TaskManagerMediatR.Application.Projects.Queries.GetById
{
    public sealed class GetProjectQueryHandler : IQueryHandler<GetProjectQuery, ProjectResponse>
    {
        private readonly CacheOptions _options;
        private readonly ICacheService _cacheService;
        private readonly IRequestCoalescer _coalescer;
        private readonly IProjectRepository _projectRepository;
        public GetProjectQueryHandler(
            IOptions<CacheOptions> options,
            ICacheService cacheService,
            IRequestCoalescer coalescer,
            IProjectRepository projectRepository)
        {
            _options = options.Value;
            _coalescer = coalescer;
            _cacheService = cacheService;
            _projectRepository = projectRepository;
        }
        public async Task<Result<ProjectResponse>> Handle(GetProjectQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = CacheKeys.Project(request.Id);

            var cached = await _cacheService.Get<ProjectResponse>(cacheKey, cancellationToken);

            if (cached is not null)
            {
                return Result.Success(cached);
            }

            var response = await _coalescer.GetOrCreateAsync(cacheKey,
                async ct =>
                {
                    var cachedProject = await _cacheService.Get<ProjectResponse>(cacheKey, ct);
                    if (cachedProject is not null)
                        return cachedProject;

                    var project = await _projectRepository.GetById(request.Id, ct);
                    if (project is null)
                        return null;

                    var dto = Map(project);
                    await _cacheService.Set(cacheKey, dto, _options.ProjectExpiration, ct);
                    return dto;
                },
            cancellationToken);

            if (response is null)
                return Result.Failure<ProjectResponse>(DomainErrors.Project.NotFound);

            return response;

        }

        private static ProjectResponse Map(Project project) => new(
            project.Id,
            project.Name,
            project.Description,
            project.CreatedAt,
            project.OwnerId,
            project.Members.Select(m => new ProjectMemberResponse(
                m.UserId,
                m.ProjectRole.ToString(),
                m.JoinedAt)).ToList());
    }
}
