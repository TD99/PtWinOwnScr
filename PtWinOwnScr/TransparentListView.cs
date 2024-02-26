using System.Windows.Forms;

namespace PtWinOwnScr
{
    public class TransparentListView : ListView
    {
        public TransparentListView()
        {
            SetStyle(System.Windows.Forms.ControlStyles.SupportsTransparentBackColor, true);
        }
    }
}
