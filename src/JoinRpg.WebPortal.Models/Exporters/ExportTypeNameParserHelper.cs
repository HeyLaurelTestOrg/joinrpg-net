using JoinRpg.Services.Interfaces;

namespace JoinRpg.Web.Models.Exporters
{
    public static class ExportTypeNameParserHelper
    {
        public static ExportType? ToExportType(string export)
        {
            switch (export)
            {
                case "csv": return ExportType.Csv;
                case "xlsx": return ExportType.ExcelXml;
                default: return null;
            }
        }
    }
}
