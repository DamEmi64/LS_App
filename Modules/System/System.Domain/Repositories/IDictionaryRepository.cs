using Base;

namespace System.Domain.Repositories
{
    public interface IDictionaryRepository
    {
        IEnumerable<DictionaryItem> GetAll();
        void UpdateDictionaries(IEnumerable<DictionaryItem> items);
    }
}