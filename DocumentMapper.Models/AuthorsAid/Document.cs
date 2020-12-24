using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentMapper.Models.AuthorsAid
{
    public abstract class Document
    {
        public Document(string title)
        {
            Title = title;
        }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("wordCount")]
        public int WordCount { get; set; }

        [JsonProperty("pageCount")]
        public int PageCount { get; set; }
    }
}
