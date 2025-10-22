using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;

namespace Google
{
    public class GoogleSheetsManager : IGoogleSheetsManager
    {
        private readonly UserCredential _credential;

        public GoogleSheetsManager(UserCredential credential)
        {
            _credential = credential;
        }

        public Spreadsheet GetSpreadSheet(string googleSpreadsheetIdentifier)
        {
            if (string.IsNullOrEmpty(googleSpreadsheetIdentifier))
                throw new ArgumentNullException(nameof(googleSpreadsheetIdentifier));

            using (var sheetsService = new SheetsService(new BaseClientService.Initializer() { HttpClientInitializer = _credential }))
                return sheetsService.Spreadsheets.Get(googleSpreadsheetIdentifier).Execute();
        }

        public Spreadsheet GetSpreadSheet(string googleSpreadsheetIdentifier, string sheetName, bool includeGridData = false)
        {
            if (string.IsNullOrEmpty(googleSpreadsheetIdentifier))
                throw new ArgumentNullException(nameof(googleSpreadsheetIdentifier));

            using (var sheetsService = new SheetsService(new BaseClientService.Initializer() { HttpClientInitializer = _credential }))
            {
                var request = sheetsService.Spreadsheets.Get(googleSpreadsheetIdentifier);
                request.Ranges = new[] { sheetName + "!A:ZZZ" };
                request.IncludeGridData = includeGridData;
                return request.Execute();
            }
        }

        public ValueRange GetSingleValue(string googleSpreadsheetIdentifier, string valueRange)
        {
            if (string.IsNullOrEmpty(googleSpreadsheetIdentifier))
                throw new ArgumentNullException(nameof(googleSpreadsheetIdentifier));
            if (string.IsNullOrEmpty(valueRange))
                throw new ArgumentNullException(nameof(valueRange));

            using (var sheetsService = new SheetsService(new BaseClientService.Initializer() { HttpClientInitializer = _credential }))
            {
                var getValueRequest = sheetsService.Spreadsheets.Values.Get(googleSpreadsheetIdentifier, valueRange);
                return getValueRequest.Execute();
            }
        }

        public BatchGetValuesResponse GetMultipleValues(string googleSpreadsheetIdentifier, string[] ranges)
        {
            if (string.IsNullOrEmpty(googleSpreadsheetIdentifier))
                throw new ArgumentNullException(nameof(googleSpreadsheetIdentifier));
            if (ranges == null || ranges.Length <= 0)
                throw new ArgumentNullException(nameof(ranges));

            using (var sheetsService = new SheetsService(new BaseClientService.Initializer() { HttpClientInitializer = _credential }))
            {
                var getValueRequest = sheetsService.Spreadsheets.Values.BatchGet(googleSpreadsheetIdentifier);
                getValueRequest.Ranges = ranges;
                return getValueRequest.Execute();
            }
        }

        public ValueRange GetValueRangeFromGrid(GridData gridData, IList<GridRange>? merges)
        {
            int rowCount = gridData.RowData?.Count ?? 0;
            int columnCount = gridData.RowData?.Max(r => r.Values?.Count ?? 0) ?? 0;

            object[,] cells = new object[rowCount, columnCount];

            for (int r = 0; r < rowCount; r++)
            {
                var row = gridData.RowData?[r];
                if (row?.Values == null) continue;

                for (int c = 0; c < row.Values.Count; c++)
                {
                    var cell = row.Values[c];
                    cells[r, c] = cell?.FormattedValue ?? "";
                }
            }

            if (merges != null)
            {
                foreach (var merge in merges)
                {
                    int startRow = merge.StartRowIndex ?? 0;
                    int endRow = merge.EndRowIndex ?? startRow + 1;
                    int startCol = merge.StartColumnIndex ?? 0;
                    int endCol = merge.EndColumnIndex ?? startCol + 1;

                    object topLeftValue = cells[startRow, startCol];

                    for (int r = startRow; r < endRow; r++)
                    {
                        for (int c = startCol; c < endCol; c++)
                        {
                            cells[r, c] = topLeftValue;
                        }
                    }
                }
            }

            List<IList<object>> values = new();
            for (int r = 0; r < rowCount; r++)
            {
                List<object> rowValues = new();
                for (int c = 0; c < columnCount; c++)
                {
                    rowValues.Add(cells[r, c]);
                }
                values.Add(rowValues);
            }

            return new ValueRange { Values = values };
        }
    }
}
