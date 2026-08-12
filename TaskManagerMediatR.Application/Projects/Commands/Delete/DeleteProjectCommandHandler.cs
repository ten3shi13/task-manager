
using TaskManagerMediatR.Application.Shared.Abstractions;
using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;
using TaskManagerMediatR.Application.Shared.Abstractions.Repositories;
using TaskManagerMediatR.Domain.Errors;
using TaskManagerMediatR.Domain.Shared;

namespace TaskManagerMediatR.Application.Projects.Commands.Delete
{
    public sealed class DeleteProjectCommandHandler : ICommandHandler<DeleteProjectCommand, Guid>
    {
        private readonly IProjectRepository _projectRepository;

        public DeleteProjectCommandHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }
        public async Task<Result<Guid>> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            var deletedRows = await _projectRepository.Delete(request.Id, cancellationToken);

            if (deletedRows == 0)
            {
                return Result.Failure<Guid>(DomainErrors.Project.NotFound);
            }

            return Result.Success(request.Id);
        }
    }
}
