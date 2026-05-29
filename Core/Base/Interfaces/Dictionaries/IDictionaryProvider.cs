namespace Base;

public interface IDictionaryProvider
{
    IEnumerable<DictionaryItem> GetAll();
    void UpdateDictionaries(IEnumerable<DictionaryItem> items);
}