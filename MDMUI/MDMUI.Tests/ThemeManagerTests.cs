using System.Drawing;
using System.Windows.Forms;
using MDMUI.Utility;

namespace MDMUI.Tests;

[TestClass]
public sealed class ThemeManagerTests
{
    [TestMethod]
    public void ApplyTo_DefaultButton_IsSecondarySurface()
    {
        using Form form = new Form();
        using Button button = new Button();
        form.Controls.Add(button);

        ThemeManager.ApplyTo(form);

        Assert.AreEqual(ThemeManager.Palette.Surface.ToArgb(), button.BackColor.ToArgb());
    }

    [TestMethod]
    public void ApplyTo_AcceptButton_IsAccent()
    {
        using Form form = new Form();
        using Button ok = new Button();
        form.Controls.Add(ok);
        form.AcceptButton = ok;

        ThemeManager.ApplyTo(form);

        Assert.AreEqual(ThemeManager.Palette.Accent.ToArgb(), ok.BackColor.ToArgb());
        Assert.AreEqual(Color.White.ToArgb(), ok.ForeColor.ToArgb());
    }

    [TestMethod]
    public void ApplyTo_CustomForeColor_IsPreserved()
    {
        using Form form = new Form();
        using Button danger = new Button();
        danger.ForeColor = Color.Red;
        form.Controls.Add(danger);

        ThemeManager.ApplyTo(form);

        Assert.AreEqual(Color.Red.ToArgb(), danger.ForeColor.ToArgb());
    }
}

