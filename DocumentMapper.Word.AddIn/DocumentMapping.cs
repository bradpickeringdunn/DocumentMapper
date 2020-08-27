using DocumentMapper.Models;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using SysTasks = System.Threading.Tasks;

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

        public static IDictionary<string, MappedItem> FlattenedMappeditems
        {
            get
            {
                if (!_flattenedMappeditems.Any())
                {
                    FlattenListOfMappedItems(Current.MappedItems);
                }
                return _flattenedMappeditems;
            }
        }

        private static Dictionary<string, MappedItem> _flattenedMappeditems = new Dictionary<string, MappedItem>();

        static void FlattenListOfMappedItems(List<MappedItem> mappedItems)
        {
            foreach(var item in mappedItems)
            {
                if (!_flattenedMappeditems.ContainsKey(item.Id.ToString()))
                {
                    _flattenedMappeditems.Add(item.Id.ToString(), item);
                }

                if (item.ChildMappedItems.Any())
                {
                    FlattenListOfMappedItems(item.ChildMappedItems);
                }
            }
        }
                
        public async static SysTasks.Task MapDocumentControlToMappedItem(string mappedItemId = null)
        {
            try
            {
                var controls = string.IsNullOrEmpty(mappedItemId) ? 
                    await GetAllContentControls(Globals.ThisAddIn.Application.ActiveWindow.Document) :
                    await GetAllContentControls(mappedItemId);
                
                controls.ForEach(control =>
                {
                    switch (control.Type)
                    {
                        case WdContentControlType.wdContentControlText:
                            var mappedItem = (MappedItem)DocumentMapping.FlattenedMappeditems[control.Tag];
                            control.LockContents = false;
                            control.LockContentControl = false;
                            control.Range.Text = mappedItem.Name;
                            control.LockContents = true;
                            break;
                    }
                });
            }
            catch(Exception ex)
            {

            }
        }

        public async static SysTasks.Task<List<Microsoft.Office.Interop.Word.ContentControl>> GetAllContentControls(Microsoft.Office.Interop.Word.Document wordDocument)
        {
            var ccList = new List<Microsoft.Office.Interop.Word.ContentControl>();

            foreach (Range range in wordDocument.StoryRanges)
            {
                var rangeStory = range;
                do
                {
                    ccList = GetContentControl(rangeStory.ContentControls);

                    rangeStory = rangeStory.NextStoryRange;
                } while (rangeStory != null);
            }

            return await SysTasks.Task.FromResult(ccList);
        }

        public async static SysTasks.Task<List<Microsoft.Office.Interop.Word.ContentControl>> GetAllContentControls(string mappedItemId)
        {
            var controls = Globals.ThisAddIn.Application.ActiveWindow.Document.SelectContentControlsByTag(mappedItemId);

            var ccList = GetContentControl(controls);

            return await SysTasks.Task.FromResult(ccList);
        }

        public static List<Microsoft.Office.Interop.Word.ContentControl> GetContentControl(ContentControls contentControls)
        {
            var ccList = new List<Microsoft.Office.Interop.Word.ContentControl>();

            try
            {
                foreach (Microsoft.Office.Interop.Word.ContentControl cc in contentControls)
                {
                    ccList.Add(cc);
                }
            }
            catch (COMException)
            {
            }

            return ccList;

        }



    }
}
