﻿using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using JoinRpg.Services.Export.AutoFrontEnd;
using JoinRpg.Services.Export.BackEnds;
using JoinRpg.Services.Export.Internal;
using JoinRpg.Services.Interfaces;

namespace JoinRpg.Services.Export
{
    [UsedImplicitly]
    public class ExportDataServiceImpl : IExportDataService
    {
      private IDictionary<Type, Func<object, string>> DisplayFunctions { get; } =
        new Dictionary<Type, Func<object, string>>();

    public IExportGenerator GetGenerator<T>(ExportType type, IEnumerable<T> data)
      {
        return new TableGenerator<T>(data, GetGeneratorBackend(type), new AutoFrontend<T>(DisplayFunctions));
      }

      public void BindDisplay<T>(Func<T, string> displayFunc)
      {
        DisplayFunctions.Add(typeof(T), arg => displayFunc((T) arg));
      }

      private static IGeneratorBackend GetGeneratorBackend(ExportType type)
      {
        switch (type)
        {
          case ExportType.Csv:
            return new CsvBackend();
          case ExportType.ExcelXml:
            return new ClosedXmlExcelBackend();
          default:
            throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
      }
    }
}
