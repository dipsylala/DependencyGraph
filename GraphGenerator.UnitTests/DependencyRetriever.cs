namespace GraphGenerator.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.IO;

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

        [Test]
        public void TopLevelFlag_Is_Set_For_Single_Assembly()
        {
            var results = DependencyRetriever.GetDependencyByAssembly("artifacts\\CoreMain.dll", new List<string>(), false);

            // determine top-level assemblies
            DependencyRetriever.SetTopLevelAssemblies(results);

            var mainIsTop = results.Any(x => x.Name == "CoreMain" && x.IsTopLevel);
            var depIsTop = results.Any(x => x.Name == "CoreDependency" && x.IsTopLevel);

            Assert.Multiple(() => {
                Assert.That(mainIsTop, Is.True);
                Assert.That(depIsTop, Is.False);
            });
        }

        [Test]
        public void TopLevelFlag_Is_Set_For_Wildcard()
        {
            var results = DependencyRetriever.GetDependencyByAssembly("artifacts\\*.dll", new List<string>(), false);

            DependencyRetriever.SetTopLevelAssemblies(results);

            var frameworkMainTop = results.Any(x => x.Name == "FrameworkMain" && x.IsTopLevel);
            var coreMainTop = results.Any(x => x.Name == "CoreMain" && x.IsTopLevel);
            var frameworkDepTop = results.Any(x => x.Name == "FrameworkDependency" && x.IsTopLevel);
            var coreDepTop = results.Any(x => x.Name == "CoreDependency" && x.IsTopLevel);

            Assert.Multiple(() => {
                Assert.That(frameworkMainTop, Is.True);
                Assert.That(coreMainTop, Is.True);
                Assert.That(frameworkDepTop, Is.False);
                Assert.That(coreDepTop, Is.False);
            });
        }
    }
}