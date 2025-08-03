using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic;
using Xunit;

namespace BusinessLogic.Test
{
    public class WindowsProcessTest
    {
        [Fact]
        public void IsProgramInstalled_ShouldBeFalse()
        {
            bool expected = false;
            bool actual = WindowsProcess.IsProgramInstalled("xxSuperFakeProgramTest");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IsProgramInstalled_ShouldBeTrue()
        {
            bool expected = true;
            bool actual = WindowsProcess.IsProgramInstalled("winget");

            Assert.Equal(expected, actual);
        }

    }
}
