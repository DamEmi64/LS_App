using Base;
using Base.Entities;
using System.Collections.ObjectModel;

namespace Base
{
    /// <summary>
    ///     Resolver for current connector
    /// </summary>
    public interface IConnectorResolver : IConnector
    {
        /// <summary>
        ///     Get dictionaries by dictionary name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        ReadOnlyCollection<DictionaryItem> GetDictionary(string name);
        
        /// <summary>
        ///     Get operation by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Operation? GetOperation(int id);

        /// <summary>
        ///     Set dictionaries
        /// </summary>
        /// <param name="dictionaries"></param>
        void SetDictionary(IEnumerable<DictionaryItem> dictionaries);
    }
}