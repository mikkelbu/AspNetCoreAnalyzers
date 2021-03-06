namespace AspNetCoreAnalyzers.Tests.ASP008ValidRouteParameterNameTests
{
    using Gu.Roslyn.Asserts;
    using Microsoft.CodeAnalysis.CodeFixes;
    using Microsoft.CodeAnalysis.Diagnostics;
    using NUnit.Framework;

    public class CodeFix
    {
        private static readonly DiagnosticAnalyzer Analyzer = new AttributeAnalyzer();
        private static readonly ExpectedDiagnostic ExpectedDiagnostic = ExpectedDiagnostic.Create(ASP008ValidRouteParameterName.Descriptor);
        private static readonly CodeFixProvider Fix = new TemplateTextFix();

        [TestCase("\"api/orders/{↓id }\"",                         "\"api/orders/{id}\"")]
        [TestCase("\"api/orders/{↓ id}\"",                         "\"api/orders/{id}\"")]
        [TestCase("\"api/orders/{↓ id }\"",                         "\"api/orders/{id}\"")]
        public void When(string before, string after)
        {
            var code = @"
namespace AspBox
{
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    public class OrdersController : Controller
    {
        [HttpGet(""api/orders/{id}"")]
        public IActionResult GetId(string id)
        {
            return this.Ok(id);
        }
    }
}".AssertReplace("\"api/orders/{id}\"", before);

            var fixedCode = @"
namespace AspBox
{
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    public class OrdersController : Controller
    {
        [HttpGet(""api/orders/{id}"")]
        public IActionResult GetId(string id)
        {
            return this.Ok(id);
        }
    }
}".AssertReplace("\"api/orders/{id}\"", after);
            AnalyzerAssert.CodeFix(Analyzer, Fix, ExpectedDiagnostic, code, fixedCode);
        }

        [TestCase("\"api/orders/{↓action}\"")]
        [TestCase("\"api/orders/{↓area}\"")]
        [TestCase("\"api/orders/{↓controller}\"")]
        [TestCase("\"api/orders/{↓handler}\"")]
        [TestCase("\"api/orders/{↓page}\"")]
        public void NoFixWhen(string before)
        {
            var code = @"
namespace AspBox
{
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    public class OrdersController : Controller
    {
        [HttpGet(""api/orders/{id}"")]
        public IActionResult GetId(string id)
        {
            return this.Ok(id);
        }
    }
}".AssertReplace("\"api/orders/{id}\"", before);

            AnalyzerAssert.NoFix(Analyzer, Fix, ExpectedDiagnostic, code);
        }
    }
}
