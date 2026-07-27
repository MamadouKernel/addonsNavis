import path from "node:path";
import { FileBlob, SpreadsheetFile } from "@oai/artifact-tool";

const root = process.cwd();
const inputPath = process.argv[2]
  ? path.resolve(process.argv[2])
  : path.join(
      root,
      "outputs",
      "recette-2026-07-24",
      "EscaleReport_cahier_de_recette_execution_vague3_2026-07-24.xlsx",
    );
const range = process.argv[3] ?? "A86:K217";

const workbook = await SpreadsheetFile.importXlsx(await FileBlob.load(inputPath));
const result = await workbook.inspect({
  kind: "table",
  sheetId: process.argv[4] ?? "Cahier de test",
  range,
  include: "values,formulas",
  tableMaxRows: 240,
  tableMaxCols: 11,
  maxChars: 120000,
});

console.log(result.ndjson);
