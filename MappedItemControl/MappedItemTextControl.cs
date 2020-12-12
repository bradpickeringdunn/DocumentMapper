using Microsoft.Office.Tools.Word;

namespace MappedItemControl
{
    public class MappedItemTextControl : Microsoft.Office.Tools.Word.PlainTextContentControl
    {
        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            SelectAll();
        }
    }
}
