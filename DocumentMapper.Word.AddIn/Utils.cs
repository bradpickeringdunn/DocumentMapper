using DocumentMapper.Models;
using DocumentMapper.Models.AuthorsAid;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

            var entityType = bookMap.AddEntityType("Test1");
            var root1 = new Entity("Root Entity1", entityType.Id);
            var child = new Entity("child Entity1", entityType.Id);
            var child2 = new Entity("Entity child 1", entityType.Id);
            var child3 = new Entity("Entity1", entityType.Id);
            var child4 = new Entity("Entity1", entityType.Id);
            var child5 = new Entity("Entity1", entityType.Id);
            var child6 = new Entity("Entity1", entityType.Id);

            bookMap.AddEntity(root1);
            bookMap.AddEntity(child, root1);
            bookMap.AddEntity(child2, child);

            bookMap.AddEntity(new Entity("Entity1", entityType.Id));
            bookMap.AddEntity(new Entity("Entity2", entityType.Id));
            bookMap.AddEntity(new Entity("Entity3", entityType.Id));
            bookMap.AddEntity(new Entity("Entity4", entityType.Id));
            bookMap.AddEntity(new Entity("Entity5", entityType.Id));
            bookMap.AddEntity(new Entity("Entity6", entityType.Id));
           
            return bookMap;
        }

        public static Entity GetEntity(string entityId)
        {
            Entity entity = default(Entity);

            if (Guid.TryParse(entityId, out var id))
            {
                if (DocumentMapping.CurrentBook.EntityManifest.TryGetValue(id, out var mappeditem))
                {
                    entity = mappeditem;
                }
            }

            return entity;
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
