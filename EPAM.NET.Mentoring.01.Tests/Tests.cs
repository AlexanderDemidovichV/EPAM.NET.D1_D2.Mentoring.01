using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Rhino.Mocks;

namespace EPAM.NET.Mentoring._01.Tests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void CancelWalkAfterStart()
        {
            var stubFileSystemVisitorProvider = MockRepository.GenerateStub<IFileSystemVisitorProvider>();

            var fileSystemVisitor = new FileSystemVisitor(stubFileSystemVisitorProvider, value => true);

            fileSystemVisitor.StartWalk += (sender, args) =>
            {
                if (args.Message == "start")
                    fileSystemVisitor.Cancel = true;
            };

            var result = fileSystemVisitor.Visit("d:/");
            Assert.IsEmpty(result);
        }

        [Test]
        public void ProviderWasCalled()
        {
            var stubFileSystemVisitorProvider = MockRepository.GenerateStub<IFileSystemVisitorProvider>();

            var fileSystemVisitor = new FileSystemVisitor(stubFileSystemVisitorProvider, value => true);

            stubFileSystemVisitorProvider.Stub(provider => provider.GetDirectoryInfo(Arg<string>.Is.Anything));

            fileSystemVisitor.Visit("d:/");

            stubFileSystemVisitorProvider.AssertWasCalled(provider => provider.GetDirectoryInfo(Arg<string>.Is.Anything));
        }

        [Test]
        public void ProviderVerifyAllExpectations()
        {
            var mockFileSystemVisitorProvider = MockRepository.GenerateMock<IFileSystemVisitorProvider>();

            var fileSystemVisitor = new FileSystemVisitor(mockFileSystemVisitorProvider, value => true);

            mockFileSystemVisitorProvider.Expect(provider => provider.GetDirectoryInfo(Arg<string>.Is.Anything));

            fileSystemVisitor.Visit("d:/");

            mockFileSystemVisitorProvider.VerifyAllExpectations();
        }

    }
}
