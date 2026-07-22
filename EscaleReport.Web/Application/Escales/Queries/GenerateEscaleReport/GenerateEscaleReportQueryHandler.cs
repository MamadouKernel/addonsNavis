using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Application.Escales.Queries.GetEscaleDetail;
using EscaleReport.Web.Domain.Identity;
using MediatR;

namespace EscaleReport.Web.Application.Escales.Queries.GenerateEscaleReport;

public class GenerateEscaleReportQueryHandler(
    ISender mediator,
    IEscalePdfReportGenerator pdfGenerator,
    ICurrentUserService currentUser) : IRequestHandler<GenerateEscaleReportQuery, GenerateEscaleReportResult?>
{
    public async Task<GenerateEscaleReportResult?> Handle(GenerateEscaleReportQuery request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.GenererPdf))
        {
            throw new ForbiddenAccessException(Permissions.GenererPdf);
        }

        var detail = await mediator.Send(new GetEscaleDetailQuery(request.EscaleId), cancellationToken);
        if (detail is null)
        {
            return null;
        }

        var generatedAt = DateTime.UtcNow;
        var pdfBytes = pdfGenerator.Generate(detail, currentUser.UserName ?? "—", generatedAt);

        var safeNavire = string.Join("-", detail.Escale.Navire.Split(Path.GetInvalidFileNameChars()));
        var fileName = $"rapport-escale-{safeNavire}-{generatedAt:yyyyMMdd-HHmm}.pdf";

        return new GenerateEscaleReportResult(pdfBytes, fileName);
    }
}
