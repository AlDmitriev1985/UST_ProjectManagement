using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;
using LibraryDB;
using LibraryDB.DB;
using Type = System.Type;

namespace UST_ProjectManagement
{
    public static class Class_PrintSetsList
    {
        private static Word.Application wordapp;
        private static Word.Document worddocument;
        private static Word.Paragraph wordparagraph;
        private static string bufferpath;


        public static string CreateWordFile(string filepath, string tempfilepath)
        {
            string result = "";
            FileInfo fileinf = new FileInfo(tempfilepath);

            if (!File.Exists(filepath)) { fileinf.CopyTo(filepath, false); result = filepath; }
            else
            {
                DialogResult ask = MessageBox.Show("Файл с таким именем уже существует.\n\n" +
                    "Заменить его?", "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (ask == DialogResult.Yes)
                {
                    try
                    {
                        fileinf.CopyTo(filepath, true); result = filepath;
                    }
                    catch
                    {
                        MessageBox.Show("Не удалось заменить указанный файл.\n\n" +
                            "Закройте документ и попробуйте еще раз.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace, "Ошибка",MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    string path = CreateNewFilePath(filepath);
                    if (path != "")
                    {
                        CreateWordFile(path, tempfilepath);
                        result = path;
                    }
                    else if (bufferpath != null && bufferpath != "")
                    {
                        CreateWordFile(bufferpath, tempfilepath);
                        result = bufferpath;
                    }
                }
            }
                

            return result;
        }

        private static string CreateNewFilePath(string filepath)
        {
            string[] splitpath = filepath.Split('\\');
            string[] splitname = splitpath[splitpath.Length - 1].Split('.');
            string shortname = "";
            for (int i = 0; i < splitname.Length - 1; i++)
            {
                if (i != splitname.Length - 2)
                {
                    shortname = shortname + splitname[i] + ".";
                }
                else
                {
                    shortname = shortname + splitname[i];
                }
            }
            string newname = shortname + "(1)";
            string newpath = "";

            for(int p = 0; p < splitpath.Length; p++)
            {
                if (p != splitpath.Length - 1)
                {
                    newpath = newpath + splitpath[p] + @"\";
                }
                else
                {
                    newpath = newpath + newname + ".doc";
                }
            }
            if (!File.Exists(newpath)) 
            {
                bufferpath = newpath;
                return newpath; 
            }
            else 
            { 
                CreateNewFilePath(newpath); 
                return ""; 
            }
        }

        public static void OpenWordFile(string filepath)
        {
            if (File.Exists(filepath))
            {
                wordapp = new Word.Application();
                wordapp.Visible = false;
                Object filename = filepath;
                Object confirmConversions = true;
                Object readOnly = false;
                Object addToRecentFiles = true;
                Object passwordDocument = Type.Missing;
                Object passwordTemplate = Type.Missing;
                Object revert = false;
                Object writePasswordDocument = Type.Missing;
                Object writePasswordTemplate = Type.Missing;
                Object format = Type.Missing;
                Object encoding = Type.Missing; ;
                Object oVisible = true;
                Object openConflictDocument = Type.Missing;
                Object openAndRepair = Type.Missing;
                Object documentDirection = Type.Missing;
                Object noEncodingDialog = false;
                Object xmlTransform = Type.Missing;
                //#if OFFICEXP
                //  worddocument=wordapp.Documents.Open2000(ref filename, .....
                //#else
               worddocument = wordapp.Documents.Open (ref filename,
               ref confirmConversions, ref readOnly, ref addToRecentFiles,
               ref passwordDocument, ref passwordTemplate, ref revert,
               ref writePasswordDocument, ref writePasswordTemplate,
               ref format, ref encoding, ref oVisible,
               ref openAndRepair, ref documentDirection, ref noEncodingDialog, ref xmlTransform);
            }
        }

        public static void CreateGrid(string filepath, int columns, int rows, List<ScheduleItem> sections)
        {
            rows -= 4;
            object oMissing = System.Reflection.Missing.Value;
            worddocument.Paragraphs.Add(ref oMissing);


            int p = worddocument.Paragraphs.Count;

            wordparagraph = worddocument.Paragraphs[p];
            Word.Range wordrange = wordparagraph.Range;
            //Добавляем таблицу в начало второго параграфа
            Object defaultTableBehavior = Word.WdDefaultTableBehavior.wdWord9TableBehavior;
            Object autoFitBehavior = Word.WdAutoFitBehavior.wdAutoFitFixed;
            Word.Table wordtable1 = worddocument.Tables.Add(wordrange, rows, columns, ref defaultTableBehavior, ref autoFitBehavior);
          
            wordtable1.Rows.Height = 27;
            wordtable1.Columns.PreferredWidthType = Word.WdPreferredWidthType.wdPreferredWidthPoints;
            wordtable1.Columns[1].Width = 54; 
            wordtable1.Columns[2].Width = 126;
            wordtable1.Columns[3].Width = 220;
            wordtable1.Columns[wordtable1.Columns.Count].Width = 100;

            int t = worddocument.Tables.Count;

            #region --- Head ---
            Word.Range headercell_1 = worddocument.Tables[t].Cell(1, 1).Range;
            headercell_1.Text = "№";
            Word.Range headercell_2 = worddocument.Tables[t].Cell(1, 2).Range;
            headercell_2.Text = "Обозначение";
            Word.Range headercell_3 = worddocument.Tables[t].Cell(1, 3).Range;
            headercell_3.Text = "Наименование";
            Word.Range headercell_4 = worddocument.Tables[t].Cell(1, 4).Range;
            headercell_4.Text = "Примечание";

            Word.Range wordcellrange = worddocument.Tables[t].Range;
            wordcellrange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            wordcellrange.Cells.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
            wordcellrange.Font.Size = 12;
            wordcellrange.Font.Name = "Arial";

            for(int i = 2; i <= worddocument.Tables[t].Rows.Count; i ++)
            {
                Word.Range wordcellrange_SecondColumn = worddocument.Tables[t].Cell(i, 2).Range;
                wordcellrange_SecondColumn.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;

                Word.Range wordcellrange_ThirdColumn = worddocument.Tables[t].Cell(i, 3).Range;
                wordcellrange_ThirdColumn.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
            }


            Word.Range wordcellheaderrange = worddocument.Tables[t].Rows[1].Range;
            wordcellheaderrange.Font.Bold = 1;

            int row = 2;
            string posid = "";
            if (GlobalData.SelectedPosition != null) posid = GlobalData.SelectedPosition.PositionCode;
            else if (GlobalData.SelectedProduct != null) posid = GlobalData.SelectedProduct.ProductCode;
            else if (GlobalData.SelectedTechSolution != null) posid = GlobalData.SelectedTechSolution.TechSolutionCode;
            List<string> grouplist = new List<string>();
            var secOneGroup = sections.GroupBy(x => x.SecOneId).OrderBy(x => x.Key);
            SectionsOne sectionOne = null;

            foreach (var group in secOneGroup)
            {
                if (group.Key != 1 && group.Key != 17)
                {
                    sectionOne = RequestInfo.lb.SectionsOnes.FirstOrDefault(x => x.SectionOneId == group.Key);
                    ///Объединение ячеек
                    object begCell = worddocument.Tables[t].Cell(row, 2).Range.Start;
                    object endCell = worddocument.Tables[t].Cell(row, 4).Range.End;
                    wordcellrange = worddocument.Range(ref begCell, ref endCell);
                    wordcellrange.Select();
                    wordapp.Selection.Cells.Merge();

                    var secOne = RequestInfo.lb.SectionsOnes.First(x => x.SectionOneId == group.Key);
                    string groupName = "";
                    if (GlobalData.SelectedStage.LanguageId == 0) groupName = $"{secOne.SectionOneNum} {secOne.SectionOneNameRus}";
                    else groupName = $"{secOne.SectionOneNum} {secOne.SectionOneNameEng}";

                    Word.Range cell_01 = worddocument.Tables[t].Cell(row, 1).Range;
                    cell_01.Text = sectionOne.SectionOneNum + ".00";

                    Word.Range cell_02 = worddocument.Tables[t].Cell(row, 2).Range;
                    cell_02.Text = sectionOne.SectionOneNameRus;


                    foreach (var secThree in group)
                    {
                        row += 1;
                        Word.Range cell_1 = worddocument.Tables[t].Cell(row, 1).Range;
                        cell_1.Text = secThree.SecThreeNum;

                        Word.Range cell_2 = worddocument.Tables[t].Cell(row, 2).Range;
                        cell_2.Text = posid + " " + secThree.SecThreeTag + secThree.SecThreePostfix;

                        Word.Range cell_3 = worddocument.Tables[t].Cell(row, 3).Range;
                        cell_3.Text = secThree.SecThreeName;
                    }
                    row += 1;
                }
                

            }
            #endregion
        }

        public static void UpdateStemp()
        {
            object oMissing = System.Reflection.Missing.Value;
            worddocument.Paragraphs.Add(ref oMissing);
            Word.HeaderFooter footer;
            Word.Range footerRange;
            Word.Table table;
            Word.Section section;

            string GIP = "-";
            User uGIP = null;
            if (GlobalData.SelectedPosition != null) uGIP = RequestInfo.lb.Users.FirstOrDefault(id => id.UserId == GlobalData.SelectedPosition.PositionUserIdGIP);
            else if (GlobalData.SelectedProduct != null) uGIP = RequestInfo.lb.Users.FirstOrDefault(id => id.UserId == GlobalData.SelectedProduct.UserId);
            else if (GlobalData.SelectedTechSolution != null) uGIP = RequestInfo.lb.Users.FirstOrDefault(id => id.UserId == GlobalData.SelectedTechSolution.UserId);
            if (uGIP != null) GIP = uGIP.UserSurname;

            string GAP = "-";
            User uGAP = null;
            if (GlobalData.SelectedPosition != null) uGAP = RequestInfo.lb.Users.FirstOrDefault(id => id.UserId == GlobalData.SelectedPosition.PositionUserIdGAP);
            if (uGAP != null) GAP = uGAP.UserSurname;

            string stageTag = "-";
            Stage stage = null;
            if (GlobalData.SelectedPosition != null) stage = RequestInfo.lb.Stages.FirstOrDefault(id => id.StageId == GlobalData.SelectedPosition.StageId);
            else if (GlobalData.SelectedProduct != null || GlobalData.SelectedTechSolution != null) stage = GlobalData.SelectedStage;
            if (stage != null)
            {
                stageTag = stage.StageTag;
            }

            Word.Document worddocument1 = worddocument;
            for (int i = 1; i <= worddocument1.Sections.Count; i++)
            {
                try
                {
                    section = worddocument.Sections[i];
                    if (section != null)
                    {
                        footer = section.Footers[Word.WdHeaderFooterIndex.wdHeaderFooterFirstPage];
                        footerRange = footer.Range;
                        if (footerRange.Tables.Count > 0)
                        {
                            table = footerRange.Tables[1];
                            if (GlobalData.SelectedPosition != null) table.Cell(2, 9).Range.Text = GlobalData.SelectedPosition.PositionCode + " ОПЗ.СП";
                            else if (GlobalData.SelectedProduct != null) table.Cell(2, 9).Range.Text = GlobalData.SelectedProduct.ProductCode + " ОПЗ.СП";
                            else if (GlobalData.SelectedTechSolution != null) table.Cell(2, 9).Range.Text = GlobalData.SelectedTechSolution.TechSolutionCode + " ОПЗ.СП";
                            table.Cell(2, 9).VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                            table.Cell(2, 9).Range.Font.Size = 14;
                            table.Cell(2, 9).Range.Font.Name = "Arial";

                            table.Cell(6, 4).Range.Text = GIP;
                            table.Cell(7, 4).Range.Text = GAP;
                            table.Cell(6, 8).Range.Text = stageTag;
                            table.Cell(9, 4).Range.Text = "Городник";
                        }

                        footer = section.Footers[Word.WdHeaderFooterIndex.wdHeaderFooterPrimary];
                        footerRange = footer.Range;
                        if (footerRange.Tables.Count > 0)
                        {
                            table = footerRange.Tables[1];
                            if (GlobalData.SelectedPosition != null) table.Cell(1, 7).Range.Text = GlobalData.SelectedPosition.PositionCode + " ОПЗ.СП";
                            else if (GlobalData.SelectedProduct != null) table.Cell(1, 7).Range.Text = GlobalData.SelectedProduct.ProductCode + " ОПЗ.СП";
                            else if (GlobalData.SelectedTechSolution != null) table.Cell(1, 7).Range.Text = GlobalData.SelectedTechSolution.TechSolutionCode + " ОПЗ.СП";
                            table.Cell(1, 7).VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                            table.Cell(1, 7).Range.Font.Size = 14;
                            table.Cell(1, 7).Range.Font.Name = "Arial";
                        }
                    }
                }
                catch { }
            }
            wordapp.Visible = true;
        }
    }
}
