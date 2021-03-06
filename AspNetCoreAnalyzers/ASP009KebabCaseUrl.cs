namespace AspNetCoreAnalyzers
{
    using Microsoft.CodeAnalysis;

    internal static class ASP009KebabCaseUrl
    {
        public const string DiagnosticId = "ASP009";

        internal static readonly DiagnosticDescriptor Descriptor = new DiagnosticDescriptor(
            id: DiagnosticId,
            title: "Use kebab-cased urls.",
            messageFormat: "Use kebab-cased urls.",
            category: AnalyzerCategory.Routing,
            defaultSeverity: DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: "Use kebab-cased urls.",
            helpLinkUri: HelpLink.ForId(DiagnosticId));
    }
}
