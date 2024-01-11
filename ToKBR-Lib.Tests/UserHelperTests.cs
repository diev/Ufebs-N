using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToKBR.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToKBR.Lib.Tests;

[TestClass()]
public class UserHelperTests
{
    [TestMethod()]
    public void GetAllowedTest()
    {
        string key = "Test";

        AppContext.SetData(key, null);

        if (UserHelper.GetAllowed(key))
        {
            Assert.Fail("null позволил всем!");
        }

        AppContext.SetData(key, "");

        if (UserHelper.GetAllowed(key))
        {
            Assert.Fail("empty позволил всем!");
        }

        AppContext.SetData(key, "*");

        if (!UserHelper.GetAllowed(key))
        {
            Assert.Fail("* не позволила никому!");
        }

        AppContext.SetData(key, Environment.UserName);

        if (!UserHelper.GetAllowed(key))
        {
            Assert.Fail("user не позволен!");
        }

        AppContext.SetData(key, Environment.MachineName);

        if (!UserHelper.GetAllowed(key))
        {
            Assert.Fail("PC не позволен!");
        }
    }
}