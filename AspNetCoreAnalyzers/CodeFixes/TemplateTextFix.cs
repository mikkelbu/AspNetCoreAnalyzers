namespace AspNetCoreAnalyzers
{
    using System;
    using System.Collections.Immutable;
    using System.Composition;
    using System.Threading;
    using System.Threading.Tasks;
    using Gu.Roslyn.CodeFixExtensions;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CodeActions;
    using Microsoft.CodeAnalysis.CodeFixes;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(TemplateTextFix))]
    [Shared]
    public class TemplateTextFix : CodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds { get; } = ImmutableArray.Create(
            ASP002RouteParameterName.DiagnosticId,
            ASP004RouteParameterType.DiagnosticId,
            ASP005ParameterSyntax.DiagnosticId,
            ASP006ParameterRegex.DiagnosticId,
            ASP008ValidRouteParameterName.DiagnosticId,
            ASP009KebabCaseUrl.DiagnosticId);

        public override FixAllProvider GetFixAllProvider() => null;

        public override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var syntaxRoot = await context.Document.GetSyntaxRootAsync(context.CancellationToken)
                                          .ConfigureAwait(false);

            foreach (var diagnostic in context.Diagnostics)
            {
                if (syntaxRoot.TryFindNodeOrAncestor<LiteralExpressionSyntax>(diagnostic, out _) &&
                    diagnostic.Properties.TryGetValue(nameof(Text), out var text))
                {
                    context.RegisterCodeFix(
                        CodeAction.Create(
                            GetTitle(diagnostic),
                            _ => Fix(_),
                            equivalenceKey: null),
                        diagnostic);

                    async Task<Document> Fix(CancellationToken cancellationToken)
                    {
                        var sourceText = await context.Document.GetTextAsync(cancellationToken)
                                                      .ConfigureAwait(false);
                        return context.Document.WithText(sourceText.Replace(diagnostic.Location.SourceSpan, text));
                    }
                }
            }
        }

        private static string GetTitle(Diagnostic diagnostic)
        {
            switch (diagnostic.Id)
            {
                case ASP002RouteParameterName.DiagnosticId:
                    return "Rename parameter.";
                case ASP004RouteParameterType.DiagnosticId:
                    return "Change type to match symbol.";
                case ASP005ParameterSyntax.DiagnosticId:
                    return "Fix syntax error.";
                case ASP006ParameterRegex.DiagnosticId:
                    return "Escape regex.";
                case ASP008ValidRouteParameterName.DiagnosticId:
                    return "Fix name.";
                case ASP009KebabCaseUrl.DiagnosticId:
                    return "To lowercase.";
                default:
                    throw new InvalidOperationException("Should never get here.");
            }
        }
    }
}
