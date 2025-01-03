using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;

namespace UPPPDGen
{
    public partial class MainWindow : Window
    {
        private void CreateNewDocument(string filePath)
        {
            // Удаляем старый файл, если он существует
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            // Создаём новый документ .docx
            using (var wordDoc = WordprocessingDocument.Create(filePath, DocumentFormat.OpenXml.WordprocessingDocumentType.Document))
            {
                // Добавляем основной документ и тело
                var mainPart = wordDoc.AddMainDocumentPart();
                mainPart.Document = new Document(new Body());
                wordDoc.MainDocumentPart.Document.Save();
            }
        }
        private void ClearDocumentContent(WordprocessingDocument wordDoc)
        {
            // Получаем тело документа
            var body = wordDoc.MainDocumentPart.Document.Body;

            // Очищаем тело документа, удаляя все существующие абзацы и другие элементы
            body.RemoveAllChildren();
        }
        private void CloseWordProcess()
        {
            try
            {
                // Закрываем процесс Word, если он был запущен
                var processes = System.Diagnostics.Process.GetProcessesByName("winword");
                foreach (var process in processes)
                {
                    process.Kill();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при завершении процесса Word: {ex.Message}");
            }
        }

        private DocumentFormat.OpenXml.Wordprocessing.Paragraph WithIndentFirstLine(DocumentFormat.OpenXml.Wordprocessing.Paragraph paragraph, string text)
        {
            // Проверяем, есть ли ParagraphProperties, если нет - создаем
            if (paragraph.ParagraphProperties == null)
            {
                paragraph.ParagraphProperties = new ParagraphProperties();
            }

            // Добавляем отступ первой строки
            var indentation = new Indentation
            {
                FirstLine = (1.25 * 567).ToString() // 1.25 см в twips (1 см = 567 twips)
            };

            paragraph.ParagraphProperties.Indentation = indentation;

            // Теперь обновляем текст
            var run = paragraph.Descendants<DocumentFormat.OpenXml.Wordprocessing.Run>().FirstOrDefault();
            if (run == null)
            {
                run = new DocumentFormat.OpenXml.Wordprocessing.Run();
                paragraph.AppendChild(run);
            }

            run.RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Text>(); // Удаляем старый текст
            run.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Text(text)); // Добавляем новый текст

            return paragraph;
        }

        private DocumentFormat.OpenXml.Wordprocessing.Paragraph WithJustifyAlignment(DocumentFormat.OpenXml.Wordprocessing.Paragraph paragraph, string text)
        {
            if (paragraph.ParagraphProperties == null)
            {
                paragraph.ParagraphProperties = new ParagraphProperties();
            }

            // Устанавливаем выравнивание по ширине
            var alignment = new Justification() { Val = JustificationValues.Both };
            paragraph.ParagraphProperties.Justification = alignment;

            // Теперь обновляем текст
            var run = paragraph.Descendants<DocumentFormat.OpenXml.Wordprocessing.Run>().FirstOrDefault();
            if (run == null)
            {
                run = new DocumentFormat.OpenXml.Wordprocessing.Run();
                paragraph.AppendChild(run);
            }

            run.RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Text>(); // Удаляем старый текст
            run.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Text(text)); // Добавляем новый текст

            return paragraph;
        }

        private DocumentFormat.OpenXml.Wordprocessing.Paragraph WithFontSettings(DocumentFormat.OpenXml.Wordprocessing.Paragraph paragraph, string text)
        {
            // Проверяем, есть ли уже хотя бы один Run в параграфе
            var run = paragraph.Descendants<DocumentFormat.OpenXml.Wordprocessing.Run>().FirstOrDefault();

            if (run == null)
            {
                // Если Run не существует, создаем новый
                run = new DocumentFormat.OpenXml.Wordprocessing.Run();
                paragraph.AppendChild(run);
            }

            // Устанавливаем шрифт и размер
            var runProperties = run.RunProperties ?? new DocumentFormat.OpenXml.Wordprocessing.RunProperties();
            runProperties.FontSize = new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "28" }; // 14 pt = 28 half-points
            runProperties.RunFonts = new DocumentFormat.OpenXml.Wordprocessing.RunFonts() { Ascii = "Times New Roman" }; // Устанавливаем шрифт

            run.RunProperties = runProperties;
            run.RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Text>(); // Удаляем старый текст

            run.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Text(text)); // Добавляем новый текст

            return paragraph;
        }


    }
}
