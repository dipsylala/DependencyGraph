namespace GraphGenerator.UnitTests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void Generate_Dependency_Graph_For_Core()
        {
            var sut = new DependencyRetriever();

            TestContext.AddTestAttachment("artifacts\\CoreMain.dll", "Main Core Module");
            TestContext.AddTestAttachment("artifacts\\CoreDependency.dll", "Dependency Core Module");

            var results = sut.GetDependencyByAssembly("artifacts\\CoreMain.dll", new List<string>(), false);

            var containsMain = results.Any(x => x.Name == "CoreMain");
            var containsDependency = results.Any(x => x.Name == "CoreDependency" && x.Found == true);

            Assert.Multiple(() =>
            {
                Assert.That(containsMain, Is.True);
                Assert.That(containsDependency, Is.True);
            });
        }

        [Test]
        public void Generate_Dependency_Graph_For_Framework()
        {
            var sut = new DependencyRetriever();

            TestContext.AddTestAttachment("artifacts\\FrameworkMain.dll", "Main Framework Module");
            TestContext.AddTestAttachment("artifacts\\FrameworkDependency.dll", "Dependency Framework Module");

            var results = sut.GetDependencyByAssembly("artifacts\\FrameworkMain.dll", new List<string>(), false);

            var containsMain = results.Any(x => x.Name == "FrameworkMain");
            var containsDependency= results.Any(x => x.Name == "FrameworkDependency" && x.Found == true);

            Assert.Multiple(() =>
            {
                Assert.That(containsMain, Is.True);
                Assert.That(containsDependency, Is.True);
            });
        }

        [Test]
        public void Exception_If_File_Does_Not_Exist()
        {
            var sut = new DependencyRetriever();

            Assert.Throws<FileNotFoundException>(() => sut.GetDependencyByAssembly("artifacts\\DoesNotExist.exe", new List<string>(), false));
        }
    }
}