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

            bookMap.EntityTypes.Add(new EntityType("test1")
            {

            });

            var root1 = new Entity("Root Entity1", "test1");
            var child = new Entity("child Entity1", "test1");
            var child2 = new Entity("Entity child 1", "test1");
            var child3 = new Entity("Entity1", "test1");
            var child4 = new Entity("Entity1", "test1");
            var child5 = new Entity("Entity1", "test1");
            var child6 = new Entity("Entity1", "test1");

            child5.AddChildEntity(child6);
            child4.AddChildEntity(child5);
            child3.AddChildEntity(child4);
            child2.AddChildEntity(child3);
            child.AddChildEntity(child2);
            

            bookMap.AddEntity(root1);
            root1.AddChildEntity(child);

            bookMap.AddEntity(new Entity("Entity1", "test1"));
            bookMap.AddEntity(new Entity("Entity2", "test1"));
            bookMap.AddEntity(new Entity("Entity3", "test1"));
            bookMap.AddEntity(new Entity("Entity4", "test1"));
            bookMap.AddEntity(new Entity("Entity5", "test1"));
            bookMap.AddEntity(new Entity("Entity6", "test1"));
            


            
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
