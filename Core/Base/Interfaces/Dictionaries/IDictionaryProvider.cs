namespace Base;

/// <summary>
///     Provides access to persisted dictionary items and synchronizes discovered dictionaries.
/// </summary>
public interface IDictionaryProvider
{
    IEnumerable<DictionaryItem> GetAll();
    void UpdateDictionaries(IEnumerable<DictionaryItem> items);
}
