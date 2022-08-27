using FluentValidation;

namespace BlobContainers.Application.Blobs.v1.Commands.GenerateSASUrl;

public class GenerateSASUrlCommandValidator : AbstractValidator<GenerateSASUrlCommand>
{
	public GenerateSASUrlCommandValidator()
	{
		RuleFor(x => x.BlobName)
			.NotEmpty()
			.WithMessage("Blob name cannot be empty.")
			.NotNull()
			.WithMessage("Blob name cannot be null.");
	}
}

