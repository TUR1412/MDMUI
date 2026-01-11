using MDMUI.Utility;

namespace MDMUI.Tests;

[TestClass]
public sealed class PasswordEncryptorTests
{
    [TestMethod]
    public void EncryptPassword_Empty_ReturnsEmpty()
    {
        Assert.AreEqual(string.Empty, PasswordEncryptor.EncryptPassword(""));
        Assert.AreEqual(string.Empty, PasswordEncryptor.EncryptPassword(null));
    }

    [TestMethod]
    public void EncryptPassword_SameInput_ProducesStableHash()
    {
        string hash1 = PasswordEncryptor.EncryptPassword("p@ssw0rd");
        string hash2 = PasswordEncryptor.EncryptPassword("p@ssw0rd");

        Assert.IsFalse(string.IsNullOrWhiteSpace(hash1));
        Assert.AreEqual(hash1, hash2);
    }

    [TestMethod]
    public void VerifyPassword_CorrectPassword_ReturnsTrue()
    {
        string storedHash = PasswordEncryptor.EncryptPassword("admin123");
        Assert.IsTrue(PasswordEncryptor.VerifyPassword("admin123", storedHash));
    }

    [TestMethod]
    public void VerifyPassword_WrongPassword_ReturnsFalse()
    {
        string storedHash = PasswordEncryptor.EncryptPassword("admin123");
        Assert.IsFalse(PasswordEncryptor.VerifyPassword("wrong", storedHash));
    }
}
