namespace GraphGenerator.UnitTests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void Generate_Dependency_Graph_For_Core()
        {
            var sut = new DependencyRetriever();

            var results = DependencyRetriever.GetDependencyByAssembly("artifacts\\CoreMain.dll", new List<string>(), false);

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

            var results = DependencyRetriever.GetDependencyByAssembly("artifacts\\FrameworkMain.dll", new List<string>(), false);

            var containsMain = results.Any(x => x.Name == "FrameworkMain");
            var containsDependency= results.Any(x => x.Name == "FrameworkDependency" && x.Found == true);

            Assert.Multiple(() =>
            {
                Assert.That(containsMain, Is.True);
                Assert.That(containsDependency, Is.True);
            });
        }

        [Test]
        public void Generate_Dependency_Graph_By_Wildcards()
        {
            var sut = new DependencyRetriever();

            var results = DependencyRetriever.GetDependencyByAssembly("artifacts\\*.dll", new List<string>(), false);

            var containsMain = results.Any(x => x.Name == "FrameworkMain");
            var containsCore= results.Any(x => x.Name == "CoreMain");
            var containsDependency = results.Any(x => x.Name == "FrameworkDependency" && x.Found == true);
            var containsCoreDependency = results.Any(x => x.Name == "CoreDependency" && x.Found == true);

            Assert.Multiple(() =>
            {
                Assert.That(containsMain, Is.True);
                Assert.That(containsCore, Is.True);
                Assert.That(containsDependency, Is.True);
                Assert.That(containsDependency, Is.True);
            });
        }

        [Test]
        public void Exception_If_File_Does_Not_Exist()
        {
            var sut = new DependencyRetriever();

            Assert.Throws<FileNotFoundException>(() => DependencyRetriever.GetDependencyByAssembly("artifacts\\DoesNotExist.exe", new List<string>(), false));
        }
    }
}