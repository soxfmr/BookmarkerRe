using System.Windows.Forms;

namespace BookmarkerRe
{
    public class ToolsUtil
    {
        public static string GetSelectedFile(string fileFilter = "")
        {
            string fn = null;

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = fileFilter;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                fn = dialog.FileName;
            }

            return fn;
        }

        public static string GetSaveFileDialog(string fileFilter = "")
        {
            string fn = null;

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = fileFilter;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                fn = dialog.FileName;
            }

            return fn;
        }
    }
}
