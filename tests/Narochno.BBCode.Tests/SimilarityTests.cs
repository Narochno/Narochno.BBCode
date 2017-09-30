using System.IO;
using System.Reflection;
using Xunit;

namespace Narochno.BBCode.Tests
{
    public class SimilarityTests
    {
        internal string GetResource(string name)
        {
            Assembly assembly = typeof(SimilarityTests).GetTypeInfo().Assembly;
            string qualifiedName = $"{assembly.GetName().Name}.Files.{name}";
            using (var sr = new StreamReader(assembly.GetManifestResourceStream(qualifiedName)))
            {
                return sr.ReadToEnd();
            }
        }

        [Theory]
        [InlineData("test1.bb", "test1.html")]
        [InlineData("test2.bb", "test2.html")]
        public void TestParseDocuments(string inputFile, string outputFile)
        {
            var input = GetResource(inputFile);

            var parser = new BBCodeParser();

            Assert.Equal(GetResource(outputFile), parser.ToHtml(input));
        }
    }
}
