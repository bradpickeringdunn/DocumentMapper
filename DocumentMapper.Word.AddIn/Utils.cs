using DocumentMapper.Models;
using System;
using System.IO;
using System.Xml.Serialization;

namespace DocumentMapper.Word.AddIn
{
    public static class Utils
    {
        public static DocumentMap DocumentMap
        {
            get
            {
                var documentMap = new DocumentMap();
                if (!String.IsNullOrEmpty(ApplicationVariables.GetVariable(ApplicationVariables.DocumentMapFilePath))) {
                    documentMap = LoadDocumentMap();
                }

                return documentMap;
            }
        }

        public static string DocumentMapperFilelocation()
        {
            return ApplicationVariables.GetVariable(ApplicationVariables.DocumentMapFilePath);
        }

        public static bool ActiveDocumentLinkedToDocumentMap()
        {
            return String.IsNullOrEmpty(ApplicationVariables.GetVariable(ApplicationVariables.DocumentMapFilePath)) ? false : true;
        }

        public static DocumentMap LoadDocumentMap()
        {
            var documentMap = new DocumentMap();
            var serializer = new XmlSerializer(typeof(DocumentMap));
            using (var reader = new StreamReader(Utils.DocumentMapperFilelocation()))
            {
                documentMap = (DocumentMap)serializer.Deserialize(reader);
            }

            return documentMap;
        }

        internal static void UnLinkDocumentMap()
        {
            try
            {
                Globals.ThisAddIn.Application.ActiveDocument.Variables[ApplicationVariables.DocumentMapFilePath].Delete();
            }
            catch (Exception ex)
            {
                if (ex.Message != "Object has been deleted.")
                {

                }
            }

            Globals.ThisAddIn.InitializeDocumentMapper();
        }

    

        internal static void SaveDocumentMap(DocumentMap documentMap, string FilePath)
        {

            var writer = new XmlSerializer(typeof(DocumentMap));

            if (String.IsNullOrEmpty(DocumentMapperFilelocation()))
            {
                using (var file = System.IO.File.Create(FilePath))
                {
                    writer.Serialize(file, documentMap);
                }
            }
            else
            {
                using (var file = System.IO.File.Create(FilePath))
                {
                    writer.Serialize(file, documentMap);
                }
            }
            
        }
    }
}
