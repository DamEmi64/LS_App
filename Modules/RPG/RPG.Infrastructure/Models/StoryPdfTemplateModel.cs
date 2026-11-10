namespace RPG.Infrastructure.Models;

public sealed class StoryPdfTemplateModel
{
    public required StoryModelExtended Story { get; init; }
    public required string BackgroundImage { get; init; }
}
