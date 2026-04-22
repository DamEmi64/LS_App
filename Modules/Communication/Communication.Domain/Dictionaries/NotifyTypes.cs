using Base.Entities;

namespace Communication.Domain.Dictionaries
{
    [Dictionary("Notify types")]
    public class NotifyTypes
    {
        public static DictionaryItem EmailSend => EntityDictionary.Item(1037, "Send Email Successed");
        public static DictionaryItem EmailGenerated => EntityDictionary.Item(1038, "Generation Email Successed");
    }
}
