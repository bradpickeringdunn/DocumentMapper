using DocumentMapper.Models;

namespace DocumentMapper.Word.AddIn
{
    internal static class DocumentMapping
    {
        public static DocumentMap Current
        {
            get
            {
                if(_currentDocumentMap == null)
                {
                    _currentDocumentMap = (DocumentMap)Utils.LoadDocumentMap();
                }

                return _currentDocumentMap;
            }
        }

        static DocumentMap _currentDocumentMap;
    }
}
