namespace Base;

/// <summary>Defines manual conversions between a DTO and its domain entity.</summary>
public interface IDto<TEntity, TSelf> where TSelf : IDto<TEntity, TSelf>
{
    TEntity ToEntity();
    static abstract TSelf ToDto(TEntity entity);
}
