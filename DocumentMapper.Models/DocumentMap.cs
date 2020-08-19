using System;
using System.Collections;
using System.Collections.Generic;

namespace DocumentMapper.Models
{
    [Serializable]
    public class DocumentMap
    {
        public DocumentMap()
        {
            LinkedDocuments = new List<string>();
        }

        /// <summary>
        /// Get's and sets all the documents that are linked to this map.
        /// </summary>
        public List<string> LinkedDocuments { get; set; }
    }
}
