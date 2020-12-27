using DocumentMapper.Models;
using DocumentMapper.Models.AuthorsAid;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DocumentMapper.Word.AddIn
{
    public static class Utils
    {
        public async static void Await(this Task task)
        {
            await task;
        }

        public static DocumentMap DocumentMap
        {
            get
            {
                var documentMap = new DocumentMap();
                if (!String.IsNullOrEmpty(ApplicationVariables.GetVariable(ApplicationVariables.DocumentMapFilePath))) {
                ///    documentMap = LoadBook();
                }

                return documentMap;
            }
        }

        public static string DocumentMapperFilelocation()
        {
            return ApplicationVariables.GetVariable(ApplicationVariables.DocumentMapFilePath);
        }

        internal static void LinkBookMap(string filePath)
        {
            Globals.ThisAddIn.Application.ActiveDocument.Variables.Add(ApplicationVariables.DocumentMapFilePath, filePath);
            Globals.ThisAddIn.Application.ActiveDocument.Save();
            Globals.ThisAddIn.InitializeDocumentMapper();
        }

        public static bool ActiveDocumentLinkedToDocumentMap()
        {
            return String.IsNullOrEmpty(ApplicationVariables.GetVariable(ApplicationVariables.DocumentMapFilePath)) ? false : true;
        }

        public static Book LoadBook()
        {
            var bookMap = default(Book);
            try
            {
                var file = File.ReadAllText(DocumentMapperFilelocation());
                bookMap = JsonConvert.DeserializeObject<Book>(file);
            }
            catch(Exception ex)
            {

            }

            return bookMap;
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

    

        internal static void SaveBookMap(Book bookMap, string FilePath)
        {
            var json = JsonConvert.SerializeObject(bookMap);
            File.WriteAllText(FilePath, json);
        }
    }
}
