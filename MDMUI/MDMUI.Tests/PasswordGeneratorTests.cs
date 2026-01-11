using System.Linq;
using MDMUI.Utility;

namespace MDMUI.Tests;

[TestClass]
public sealed class PasswordGeneratorTests
{
    [TestMethod]
    public void GenerateStrong_Default_HasExpectedLengthAndClasses()
    {
        string password = PasswordGenerator.GenerateStrong();

        Assert.IsTrue(password.Length >= 12);
        Assert.IsTrue(password.Any(char.IsDigit));
        Assert.IsTrue(password.Any(char.IsUpper));
        Assert.IsTrue(password.Any(char.IsLower));
        Assert.IsTrue(password.Any(c => !char.IsLetterOrDigit(c)));
    }

    [TestMethod]
    public void Generate_ExactLength_IsRespected()
    {
        string password = PasswordGenerator.Generate(16);

        Assert.AreEqual(16, password.Length);
        Assert.IsTrue(password.Any(char.IsDigit));
        Assert.IsTrue(password.Any(char.IsUpper));
        Assert.IsTrue(password.Any(char.IsLower));
        Assert.IsTrue(password.Any(c => !char.IsLetterOrDigit(c)));
    }

    [TestMethod]
    public void Generate_LengthBelowFour_IsClampedToFour()
    {
        string password = PasswordGenerator.Generate(2);

        Assert.AreEqual(4, password.Length);
        Assert.IsTrue(password.Any(char.IsDigit));
        Assert.IsTrue(password.Any(char.IsUpper));
        Assert.IsTrue(password.Any(char.IsLower));
        Assert.IsTrue(password.Any(c => !char.IsLetterOrDigit(c)));
    }
}
