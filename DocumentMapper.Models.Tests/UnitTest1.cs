using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Windows.Controls;

namespace DocumentMapper.Models.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var treeNode = new TreeViewItem();

            var documentMap = new DocumentMap();
            var mappedItem1 = new MappedItem("mappedItem1", documentMap);
            var mappedItem2 = new MappedItem("mappedItem2", documentMap);
            var mappedItem3 = new MappedItem("mappedItem3", documentMap);

            var mappedItem4 = new MappedItem("mappedItem4", documentMap);
            var mappedItem5 = new MappedItem("mappedItem5", documentMap);
            var mappedItem6 = new MappedItem("mappedItem6", documentMap);
            var mappedItem7 = new MappedItem("mappedItem7", documentMap);

            mappedItem1.AddChildMappedItem(mappedItem2);
            mappedItem1.AddChildMappedItem(mappedItem3);

            mappedItem3.AddChildMappedItem(mappedItem4);
            mappedItem3.AddChildMappedItem(mappedItem6);

            mappedItem6.AddChildMappedItem(mappedItem6);
            mappedItem7.AddChildMappedItem(mappedItem6);

            


        }
    }
}
