using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DocumentMapper.Models.AuthorsAid
{
    public class Chapter
    {
        public Chapter(IReadOnlyDictionary<Guid, EntityType> entityTypes, int chapterNumber, string title, string fileLocation)
        {
            Id = Guid.NewGuid();
            Title = title;
            ChapterNumber = chapterNumber;
            AllEntityTypes = entityTypes;
            FileLocation = fileLocation;
            EntityTypes = new List<EntityType>();
        }

        [JsonProperty("Id")]
        public Guid Id { get; }

        [JsonProperty("chapterNumber")]
        public int ChapterNumber { get; }

        [JsonProperty("entityTypes")]
        public IList<EntityType> EntityTypes { get; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("wordCount")]
        public int WordCount { get; set; }

        [JsonProperty("pageCount")]
        public int PageCount { get; set; }

        [JsonProperty("fileLocation")]
        public string FileLocation { get; }

        [JsonIgnore()]
        public IReadOnlyDictionary<Guid,EntityType> AllEntityTypes { get; }

        public void AddNewEntity(Book book, Entity newEntity)
        {
            book.AddEntity(newEntity);
        }

    }
}