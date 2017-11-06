using System;
using System.Reflection;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogic.Tools
{
    public static class ExcelCreator
    {
        private static string ColumnLetter(int intCol)
        {
            var intFirstLetter = ((intCol) / 676) + 64;

            var intSecondLetter = ((intCol % 676) / 26) + 64;

            var intThirdLetter = (intCol % 26) + 65;

            var firstLetter = (intFirstLetter > 64) ? (char)intFirstLetter : ' ';

            var secondLetter = (intSecondLetter > 64) ? (char)intSecondLetter : ' ';

            var thirdLetter = (char)intThirdLetter;

            var result = string.Concat(firstLetter, secondLetter, thirdLetter).Trim();

            return result;
        }

        private static Cell CreateTextCell(string header, UInt32 index, string text)
        {
            var cell = new Cell
            {
                DataType = CellValues.InlineString,

                CellReference = header + index
            };

            var istring = new InlineString();

            var t = new Text { Text = text.ToString() };

            istring.AppendChild(t);

            cell.AppendChild(istring);

            return cell;
        }

        private static Cell CreateCell(string header, UInt32 index, int numb)
        {
            var cell = new Cell
            {
                DataType = CellValues.Number,

                CellReference = header + index
            };

            cell.CellValue = new CellValue(IntegerValue.FromInt64(numb));

            return cell;
        }

        private static object[,] GetObjectArray<T>(IEnumerable<T> objects)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();

            object[,] data = new object[objects.Count() + 1, properties.Length];

            for (int j = 0; j < properties.Count(); j++)
            {
                data[0, j] = properties[j].Name.Replace("_", " ");

                if (properties[j].Name == "FirstNameEng")
                {
                    data[0, j] = properties[j].Name.Replace("FirstNameEng", "Имя(English)");
                }

                if (properties[j].Name == "FirstNameRus")
                {
                    data[0, j] = properties[j].Name.Replace("FirstNameRus", "Имя");
                }

                if (properties[j].Name == "LastNameEng")
                {
                    data[0, j] = properties[j].Name.Replace("LastNameEng", "Фамилия(English)");
                }

                if (properties[j].Name == "LastNameRus")
                {
                    data[0, j] = properties[j].Name.Replace("LastNameRus", "Фамилия");
                }

                if (properties[j].Name == "Phone")
                {
                    data[0, j] = properties[j].Name.Replace("Phone", "Телефон");
                }

                if (properties[j].Name == "City")
                {
                    data[0, j] = properties[j].Name.Replace("City", "Город");
                }

                if (properties[j].Name == "PSExperience")
                {
                    data[0, j] = properties[j].Name.Replace("PSExperience", "Дата начала работы по основной специальности");
                }

                if (properties[j].Name == "EngLevel")
                {
                    data[0, j] = properties[j].Name.Replace("EngLevel", "Уровень владения английским языком");
                }

                if (properties[j].Name == "DesiredSalary")
                {
                    data[0, j] = properties[j].Name.Replace("DesiredSalary", "Желаемая заработная плата");
                }

                if (properties[j].Name == "LastContactDate")
                {
                    data[0, j] = properties[j].Name.Replace("LastContactDate", "Дата последнего общения");
                }

                if (properties[j].Name == "Status")
                {
                    data[0, j] = properties[j].Name.Replace("Status", "Статус");
                }

                if (properties[j].Name == "PrimarySkill")
                {
                    data[0, j] = properties[j].Name.Replace("PrimarySkill", "Основная специализация");
                }

                if (properties[j].Name == "SecondarySkills")
                {
                    data[0, j] = properties[j].Name.Replace("SecondarySkills", "Дополнительная специализация");
                }

                if (properties[j].Name == "PrimarySkillLevel")
                {
                    data[0, j] = properties[j].Name.Replace("PrimarySkillLevel", "Уровень знания основной специализации");
                }

                if (properties[j].Name == "ProjectName")
                {
                    data[0, j] = properties[j].Name.Replace("ProjectName", "Название проекта");
                }

                if (properties[j].Name == "VacancyName")
                {
                    data[0, j] = properties[j].Name.Replace("VacancyName", "Название вакансии");
                }

                if (properties[j].Name == "RequestDate")
                {
                    data[0, j] = properties[j].Name.Replace("RequestDate", "Дата запроса");
                }

                if (properties[j].Name == "StartDate")
                {
                    data[0, j] = properties[j].Name.Replace("StartDate", "Дата старта проекта");
                }

                if (properties[j].Name == "Link")
                {
                    data[0, j] = properties[j].Name.Replace("Link", "Описание требований");
                }

                if (properties[j].Name == "Experience")
                {
                    data[0, j] = properties[j].Name.Replace("Experience", "Опыт работы");
                }

                if (properties[j].Name == "CloseDate")
                {
                    data[0, j] = properties[j].Name.Replace("CloseDate", "Дата закрытия");
                }

                for (int i = 0; i < objects.Count(); i++)
                {
                    data[i + 1, j] = properties[j].GetValue(objects.ElementAt(i), null);
                }
            }

            return data;
        }

        public static byte[] GenerateExcel<T>(IEnumerable<T> objects)
        {
            var data = GetObjectArray<T>(objects);

            PropertyInfo[] properties = typeof(T).GetProperties();

            var stream = new MemoryStream();

            var document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook);

            var workbookpart = document.AddWorkbookPart();

            workbookpart.Workbook = new Workbook();

            var worksheetPart = workbookpart.AddNewPart<WorksheetPart>();

            var sheetData = new SheetData();

            var workSheet = new Worksheet(sheetData);

            worksheetPart.Worksheet = workSheet;

            var sheets = document.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());

            var sheet = new Sheet()
            {
                Id = document.WorkbookPart.GetIdOfPart(worksheetPart),

                SheetId = 1,

                Name = "Sheet 1"
            };

            sheets.AppendChild(sheet);

            uint rowIndex = 0;

            var cellIndex = 0;

            for (int i = 0; i< objects.Count()+1; i++)
            {
                var row = new Row { RowIndex = ++rowIndex };

                sheetData.AppendChild(row);

                for (int j = 0; j< properties.Length; j++)
                {
                    var cell = cellIndex++;

                    if (data[i, j] != null)
                    {
                        if (properties[j].PropertyType == typeof(System.Nullable<DateTime>) && i != 0)
                        {
                            data[i, j] = data[i, j].ToString().Substring(0, 10);
                        }
                    }
                    else data[i, j] = "";
                    if ((properties[j].PropertyType == typeof(System.Nullable<int>) || 
                        properties[j].PropertyType == typeof(int)) && i != 0 && data[i, j].ToString() != "")
                    {
                        row.AppendChild(CreateCell(ColumnLetter(cell), rowIndex, (int)data[i, j]));
                    }
                    else
                    {
                        row.AppendChild(CreateTextCell(ColumnLetter(cell), rowIndex, data[i, j].ToString()));
                    }
                }

                cellIndex = 0;
            }
            
            workbookpart.Workbook.Save();

            document.Close();

            return stream.ToArray();
        }
    }
}
