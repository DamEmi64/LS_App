using Base.Entities;
using Serilog;
using System.Domain.Repositories;
using System.Infrastructure.Db;

namespace System.Infrastructure.Repositories
{
    public class DictionaryRepository : IDictionaryRepository
    {
        private readonly SystemContext _context;

        public DictionaryRepository(SystemContext context)
        {
            _context = context;
        }

        public IEnumerable<DictionaryItem> GetAll()
        {
            return _context.Dictionaries;
        }

        public void UpdateDictionaries(IEnumerable<DictionaryItem> items)
        {
            _context.Dictionaries.RemoveRange(_context.Dictionaries);

            foreach (var item in items.OrderBy(x=>x.Dictionary).OrderBy(x=>x.Key))
            {
                if (items.Any(x=>x.Key == item.Key && x.Dictionary == item.Dictionary && item.Id != x.Id))
                {
                    Log.Error($"Key {item.Key} exists in dictionary {item.Dictionary}");
                }

                _context.Dictionaries.Add(item);
            }

            _context.SaveChanges();
        }
    }
}
