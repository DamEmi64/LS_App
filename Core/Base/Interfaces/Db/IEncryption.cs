namespace Base
{
    public interface IEncryption
    {
        string Key { get; }
        string IV { get; }
    }
}
