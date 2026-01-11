using System;
using MDMUI.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MDMUI.Tests
{
    [TestClass]
    public class UiSafeTests
    {
        [TestMethod]
        public void Run_WhenActionSucceeds_DoesNotCallOnError()
        {
            bool executed = false;
            bool errorCalled = false;

            UiSafe.Run(
                operation: "test.ok",
                action: () => executed = true,
                onError: (ex, op) => errorCalled = true);

            Assert.IsTrue(executed);
            Assert.IsFalse(errorCalled);
        }

        [TestMethod]
        public void Run_WhenActionThrows_CallsOnErrorAndDoesNotThrow()
        {
            Exception? capturedException = null;
            string? capturedOperation = null;

            UiSafe.Run(
                operation: "test.fail",
                action: () => throw new InvalidOperationException("boom"),
                onError: (ex, op) =>
                {
                    capturedException = ex;
                    capturedOperation = op;
                });

            Assert.IsNotNull(capturedException);
            Assert.IsInstanceOfType(capturedException, typeof(InvalidOperationException));
            Assert.AreEqual("test.fail", capturedOperation);
        }

        [TestMethod]
        public void Wrap_WhenHandlerThrows_CallsOnErrorAndDoesNotThrow()
        {
            Exception? capturedException = null;
            string? capturedOperation = null;

            var handler = UiSafe.Wrap(
                operation: "test.wrap",
                handler: (s, e) => throw new Exception("boom"),
                onError: (ex, op) =>
                {
                    capturedException = ex;
                    capturedOperation = op;
                });

            handler(null, EventArgs.Empty);

            Assert.IsNotNull(capturedException);
            Assert.AreEqual("test.wrap", capturedOperation);
        }
    }
}
