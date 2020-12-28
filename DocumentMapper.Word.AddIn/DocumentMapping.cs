using DocumentMapper.Models;
using DocumentMapper.Models.AuthorsAid;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using SysTasks = System.Threading.Tasks;

namespace DocumentMapper.Word.AddIn
{
    internal static class DocumentMapping
    {
        public static MappedItem AddMappedItem(string itemname, MappedItem parentMappedItem = null)
        {
            MappedItem mappedItem = default(MappedItem);

            var caption = "Add to document map";
            itemname = itemname.Trim();
            if (itemname.Length == 0)
            {
                MessageBox.Show("Cant add spaces to document map", caption, MessageBoxButton.OK);
                itemname = string.Empty;
            }

            if (itemname.Length < 3)
            {
                var message = new StringBuilder("Are you sure you want to add,");
                message.AppendLine(itemname);
                if (MessageBox.Show(message.ToString(), caption, MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                {
                    itemname = string.Empty;
                }
            }

            if (!string.IsNullOrEmpty(itemname))
            {
                try
                {
                    if (parentMappedItem == null)
                    {
                      //  mappedItem = new MappedItem(itemname, DocumentMapping.Current);
                    }
                    else
                    {
                        //mappedItem = new MappedItem(itemname, DocumentMapping.Current, parentMappedItem);
                    }

                   // DocumentMapping.Current.AddMappedItem(mappedItem);

                }
                catch (Exception ex)
                {

                }

            }

            return mappedItem;
        }

        internal static void CreateMapedEntityTextControl(Entity mappedItem)
        {
            try
            {
                var selectedText = Globals.ThisAddIn.Application.Selection;
                //selectedText.InsertBefore($"{mappedItem.Name} ");
                Microsoft.Office.Tools.Word.PlainTextContentControl textControl;

                var vstoDocument = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveDocument);
                textControl = vstoDocument.Controls.AddPlainTextContentControl(mappedItem.Name);
                textControl.Text = mappedItem.Name;
                textControl.LockContents = true;
                textControl.Title = string.Empty;
                textControl.Tag = mappedItem.Id.ToString();
            }
            catch (Exception ex)
            {

            }
        }

        public static Book CurrentBook
        {
            get
            {
                if (_currentDocumentMap == null)
                {
                    _currentDocumentMap = (Book)Utils.LoadBook();
                }

                return _currentDocumentMap;
            }
        }

        static Book _currentDocumentMap;
                
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

                            //var mappedItem = DocumentMapping.Current.MappedItemDictionary[control.Tag];
                            //control.LockContents = false;
                            //control.LockContentControl = false;
                            //control.Range.Text = mappedItem.Name;
                            //control.LockContents = true;
                            break;
                    }
                });
            }
            catch(Exception)
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

        internal async static System.Threading.Tasks.Task RemoveMappedControls(string mappedItemId)
        {
            var controls = await GetAllContentControls(mappedItemId);
            controls.ForEach(control =>
            {
                switch (control.Type)
                {
                    
                    case WdContentControlType.wdContentControlText:

                        control.LockContents = false;
                        control.LockContentControl = false;
                        var range = control.Range;
                        var text = control.Range.Text;
                        control.Delete();

                        Globals.ThisAddIn.Application.ActiveWindow.Document.Range(range.Start, range.End).Text = text;
                        
                        break;
                }
            });
        }
    }
}
