using System;
using System.Collections.Generic;

namespace DocumentMapper.Models.AuthorsAid
{
    public class Book
    {
        public Book(string title)
        {
            Title = title;
        }

        public string Title { get; private set; }
        public IReadOnlyCollection<Chapter> Chapters() => (IReadOnlyCollection<Chapter>)chapters;

        IList<Chapter> chapters = new List<Chapter>();

        public int PageCount { get; }

        public int wordCount { get; set; }

        public void AddChapter(Chapter chapter)
        {
            chapters.Add(chapter);
        }

        public IList<IDictionary<string, Entity>> Entities { get; set; }

    }
}
