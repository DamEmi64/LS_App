using SharedEvents;

namespace Base
{
    public class MediaProviderWrapper : IMediaProviderWrapper
    {
        private readonly IMediaProvider _mediaProvider;
        private readonly Notifier _notifier;

        public MediaProviderWrapper(IMediaProvider mediaProvider, Notifier notifier)
        {
            _mediaProvider = mediaProvider;
            _notifier = notifier;
        }

        public async Task Delete(Guid? id)
        {
            await _mediaProvider.Delete(id);
            await _notifier.Notify(new MediaDeletedEvent(id ?? Guid.Empty));
        }

        public Task<Media?> Load(Guid id, bool removeWebsiteExtras = false) => _mediaProvider.Load(id, removeWebsiteExtras);

        public IAsyncEnumerable<Media?> LoadMany(IEnumerable<Guid> ids, bool removeWebsiteExtras = false) => _mediaProvider.LoadMany(ids, removeWebsiteExtras);

        public async Task<Guid> Save(string content, Guid? id, string? extension = null, string? owner = null)
        {
            var newId = await _mediaProvider.Save(content, id, extension, owner);
            await _notifier.Notify(new MediaSavedEvent(newId, extension ?? "pdf", owner));
            return newId;
        }

        public async Task<Guid> Save(byte[] content, Guid? id, string extension = "pdf", string? owner = null)
        {
            var newId = await _mediaProvider.Save(content, id, extension, owner);
            await _notifier.Notify(new MediaSavedEvent(newId, extension, owner));
            return newId;
        }
    }
}
