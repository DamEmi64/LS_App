using Base;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using RPG.Domain.Entities;
using RPG.Infrastructure.Models;

namespace RPG.Infrastructure.External.Firebase
{
    public static class FirebaseExtensions
    {
        public static async Task<FirestoreDb> GetDb()
        {
            var options = AppConfiguration.GetValue<FirebaseOptions>(nameof(FirebaseOptions));
            ArgumentNullException.ThrowIfNull(options, nameof(options));

            var credential = CredentialFactory.FromFile<ICredential>(options.CredentialsPath);
            var builder = new FirestoreClientBuilder
            {
                Credential = credential
            };

            return await FirestoreDb.CreateAsync(options.ProjectId, builder.Build());
        }
        public static FirebaseStory ToFirebase(this StoryModel story)
            => new()
            {
                Id = story.Id.ToString(),
                Title = story.Title,
                Description = story.Description,
                VersionDate = DateTime.Now.ToString("dd.MM.yyyy HH:mm"),
                Chapters = story.Chapters.Select(x => x.Id.ToString()).ToList()
            };

        public static FirebaseChapter ToFirebase(this ChapterModel chapter)
            => new()
            {
                Description = chapter.Description,
                Id = chapter.Id.ToString(),
                Title = chapter.Title,
                Order = chapter.Order,
                Heroes = chapter.Heroes.Select(x => x.ToFirebase()).ToList(),
                Places = chapter.Places.Select(x => x.ToFirebase()).ToList()
            };

        public static FirebasePlayerData ToFirebase(this PlayerData playerData, string hero)
            => new()
            {
                Id = playerData.Id.ToString(),
                PlayerDataId = playerData.Id.ToString(),
                Title = hero,
                Skills = playerData.Skills.Select(x => new FirebaseSkill { Id = x.Id.ToString(), Title = x.Title, Value = x.Value }).ToList()
            };

        public static FirebaseHero ToFirebase(this HeroModel hero)
            => new()
            {
                Description = hero.Description,
                FirstName = hero.FirstName,
                LastName = hero.LastName,
                Player = hero.Player,
                Image = hero.Image.ToString(),
                Id = hero.Id.ToString()
            };

        public static FirebasePlace ToFirebase(this PlaceModel place)
            => new()
            {
                Description = place.Description,
                Title = place.Title,
                Id = place.Id.ToString(),
                Image = place.Image.ToString()
            };
    }
}