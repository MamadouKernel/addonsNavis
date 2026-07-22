using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Application.Reporting.Queries.GetShiftReport;
using EscaleReport.Web.Domain.Identity;
using MediatR;

namespace EscaleReport.Web.Application.Reporting.Queries.GenerateShiftReport;

public class GenerateShiftReportQueryHandler(
    ISender mediator,
    IShiftReportPdfGenerator pdfGenerator,
    ICurrentUserService currentUser) : IRequestHandler<GenerateShiftReportQuery, GenerateShiftReportResult>
{
    public async Task<GenerateShiftReportResult> Handle(GenerateShiftReportQuery request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.GenererPdf))
        {
            throw new ForbiddenAccessException(Permissions.GenererPdf);
        }

        var report = await mediator.Send(new GetShiftReportQuery(request.Date, request.Shift, request.EscaleId), cancellationToken);

        var generatedAt = DateTime.UtcNow;
        var pdfBytes = pdfGenerator.Generate(report, currentUser.UserName ?? "—", generatedAt);

        var scope = request.EscaleId.HasValue ? "navire" : "tous-navires";
        var fileName = $"rapport-shift-{scope}-{request.Date:yyyyMMdd}-{generatedAt:HHmm}.pdf";

        return new GenerateShiftReportResult(pdfBytes, fileName);
    }
}
