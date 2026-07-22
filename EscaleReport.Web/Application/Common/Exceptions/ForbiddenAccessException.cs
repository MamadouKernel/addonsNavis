namespace EscaleReport.Web.Application.Common.Exceptions;

public class ForbiddenAccessException(string permissionKey)
    : Exception($"Permission refusée : {permissionKey}");
