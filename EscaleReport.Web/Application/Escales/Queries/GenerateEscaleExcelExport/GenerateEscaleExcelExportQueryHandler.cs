using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Application.Escales.Queries.GetEscaleDetail;
using EscaleReport.Web.Domain.Identity;
using MediatR;

namespace EscaleReport.Web.Application.Escales.Queries.GenerateEscaleExcelExport;

public class GenerateEscaleExcelExportQueryHandler(
    ISender mediator,
    IEscaleExcelReportGenerator excelGenerator,
    ICurrentUserService currentUser) : IRequestHandler<GenerateEscaleExcelExportQuery, GenerateEscaleExcelExportResult?>
{
    public async Task<GenerateEscaleExcelExportResult?> Handle(GenerateEscaleExcelExportQuery request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.GenererExcel))
        {
            throw new ForbiddenAccessException(Permissions.GenererExcel);
        }

        var detail = await mediator.Send(new GetEscaleDetailQuery(request.EscaleId), cancellationToken);
        if (detail is null)
        {
            return null;
        }

        var excelBytes = excelGenerator.Generate(detail);

        var safeNavire = string.Join("-", detail.Escale.Navire.Split(Path.GetInvalidFileNameChars()));
        var fileName = $"export-escale-{safeNavire}-{DateTime.UtcNow:yyyyMMdd-HHmm}.xlsx";

        return new GenerateEscaleExcelExportResult(excelBytes, fileName);
    }
}
