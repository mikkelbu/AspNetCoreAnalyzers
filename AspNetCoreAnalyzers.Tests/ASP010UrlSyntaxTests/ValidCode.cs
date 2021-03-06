namespace AspNetCoreAnalyzers.Tests.ASP010UrlSyntaxTests
{
    using Gu.Roslyn.Asserts;
    using Microsoft.CodeAnalysis.Diagnostics;
    using NUnit.Framework;

    public class ValidCode
    {
        private static readonly DiagnosticAnalyzer Analyzer = new AttributeAnalyzer();

        [TestCase("\"{value}\"")]
        [TestCase("\"api/orders/{value}\"")]
        [TestCase("\"api/two-words/{value}\"")]
        public void WithParameter(string parameter)
        {
            var code = @"
namespace AspBox
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [ApiController]
    public class OrdersController : Controller
    {
        [HttpGet(""api/{value}"")]
        public IActionResult GetValue(string value)
        {
            return this.Ok(value);
        }
    }
}".AssertReplace("\"api/{value}\"", parameter);
            AnalyzerAssert.Valid(Analyzer, code);
        }
    }
}
