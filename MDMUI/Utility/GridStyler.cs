using System.Drawing;
using System.Windows.Forms;

namespace MDMUI.Utility
{
    public static class GridStyler
    {
        public static void Apply(DataGridView grid)
        {
            if (grid == null) return;

            ThemePalette palette = ThemeManager.Palette;

            grid.BorderStyle = BorderStyle.None;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grid.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            if (grid.ColumnHeadersHeight < 34)
            {
                grid.ColumnHeadersHeight = 36;
            }

            if (grid.RowTemplate.Height < 30)
            {
                grid.RowTemplate.Height = 34;
            }

            grid.AlternatingRowsDefaultCellStyle.BackColor = palette.SurfaceAlt;
            grid.AlternatingRowsDefaultCellStyle.ForeColor = palette.TextPrimary;
            grid.DefaultCellStyle.ForeColor = palette.TextPrimary;
            grid.DefaultCellStyle.SelectionBackColor = palette.AccentSoft;
            grid.DefaultCellStyle.SelectionForeColor = palette.TextPrimary;
            grid.ColumnHeadersDefaultCellStyle.BackColor = palette.SurfaceAlt;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = palette.TextPrimary;

            grid.DefaultCellStyle.Font = ThemeManager.CreateBodyFont(grid.Font?.Size ?? 9f);
            grid.ColumnHeadersDefaultCellStyle.Font = ThemeManager.CreateBodyFont((grid.Font?.Size ?? 9f) + 0.5f, FontStyle.Bold);
        }
    }
}

