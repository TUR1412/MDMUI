using MDMUI.Utility;

namespace MDMUI.Tests;

[TestClass]
public sealed class StringHelperTests
{
    [TestMethod]
    public void IsNullOrWhiteSpace_Null_ReturnsTrue()
    {
        Assert.IsTrue(StringHelper.IsNullOrWhiteSpace(null));
    }

    [TestMethod]
    public void EnsureNotNull_Null_ReturnsEmptyString()
    {
        Assert.AreEqual(string.Empty, StringHelper.EnsureNotNull(null));
    }

    [TestMethod]
    public void Truncate_WithinMaxLength_ReturnsOriginal()
    {
        Assert.AreEqual("abc", StringHelper.Truncate("abc", 10));
    }

    [TestMethod]
    public void Truncate_OverMaxLength_ReturnsPrefix()
    {
        Assert.AreEqual("abc", StringHelper.Truncate("abcdef", 3));
    }

    [TestMethod]
    public void CamelCaseToTitle_SplitsUppercase()
    {
        Assert.AreEqual("Hello World", StringHelper.CamelCaseToTitle("helloWorld"));
        Assert.AreEqual("M D M U I", StringHelper.CamelCaseToTitle("MDMUI"));
    }
}
