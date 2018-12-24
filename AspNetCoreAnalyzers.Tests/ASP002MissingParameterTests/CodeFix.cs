namespace AspNetCoreAnalyzers.Tests.ASP002MissingParameterTests
{
    using Gu.Roslyn.Asserts;
    using Microsoft.CodeAnalysis.CodeFixes;
    using Microsoft.CodeAnalysis.Diagnostics;
    using NUnit.Framework;

    public class CodeFix
    {
        private static readonly DiagnosticAnalyzer Analyzer = new AttributeAnalyzer();
        private static readonly ExpectedDiagnostic ExpectedDiagnostic = ExpectedDiagnostic.Create(ASP002MissingParameter.Descriptor);
        private static readonly CodeFixProvider Fix = new TemplateTextFix();

        [TestCase("\"api/{↓value}\"",       "\"api/{text}\"")]
        [TestCase("\"api/{↓value:alpha}\"", "\"api/{text:alpha}\"")]
        public void WhenWrongName(string before, string after)
        {
            var code = @"
namespace ValidCode
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [ApiController]
    public class OrdersController : Controller
    {
        [HttpGet(""api/{↓value}"")]
        public IActionResult GetValue(string text)
        {
            return this.Ok(text);
        }
    }
}".AssertReplace("\"api/{↓value}\"", before);

            var fixedCode = @"
namespace ValidCode
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [ApiController]
    public class OrdersController : Controller
    {
        [HttpGet(""api/{text}"")]
        public IActionResult GetValue(string text)
        {
            return this.Ok(text);
        }
    }
}".AssertReplace("\"api/{text}\"", after);

            AnalyzerAssert.CodeFix(Analyzer, Fix, ExpectedDiagnostic, code, fixedCode);
        }

        [TestCase("\"api/{text1}/{↓value}\"",               "\"api/{text1}/{text2}\"")]
        [TestCase("\"api/{text1:regex(\\\\d+)}/{↓value}\"", "\"api/{text1:regex(\\\\d+)}/{text2}\"")]
        [TestCase("@\"api/{text1:regex(\\d+)}/{↓value}\"",  "@\"api/{text1:regex(\\d+)}/{text2}\"")]
        public void WhenWrongNameSecondParameter(string before, string after)
        {
            var code = @"
namespace ValidCode
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [ApiController]
    public class OrdersController : Controller
    {
        [HttpGet(""api/{text1}/{↓value}"")]
        public IActionResult GetValue(string text1, string text2)
        {
            return this.Ok(text1 + text2);
        }
    }
}".AssertReplace("\"api/{text1}/{↓value}\"", before);

            var fixedCode = @"
namespace ValidCode
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [ApiController]
    public class OrdersController : Controller
    {
        [HttpGet(""api/{text1}/{text2}"")]
        public IActionResult GetValue(string text1, string text2)
        {
            return this.Ok(text1 + text2);
        }
    }
}".AssertReplace("\"api/{text1}/{text2}\"", after);

            AnalyzerAssert.CodeFix(Analyzer, Fix, ExpectedDiagnostic, code, fixedCode);
        }
    }
}
