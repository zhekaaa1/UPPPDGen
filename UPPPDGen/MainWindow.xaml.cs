using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;

namespace UPPPDGen
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string text = targetText.Text; // Получаем текст из TextBox
            string filePath = "C:\\Users\\zheka\\Desktop\\newdoc.docx"; // Путь к файлу

            try
            {
                // Создаем новый документ, если его нет
                if (!File.Exists(filePath))
                {
                    CreateNewDocument(filePath); // Если файл не существует, создаем новый
                }

                // Открываем документ
                using (var wordDoc = WordprocessingDocument.Open(filePath, true))
                {
                    ClearDocumentContent(wordDoc);
                    var body = wordDoc.MainDocumentPart.Document.Body;

                    // Создаем один параграф
                    var paragraph = new DocumentFormat.OpenXml.Wordprocessing.Paragraph();

                    // Применяем все выбранные стили
                    if (indentleft.IsChecked == true)
                    {
                        paragraph = WithIndentFirstLine(paragraph, text);
                    }
                    if (justifytext.IsChecked == true)
                    {
                        paragraph = WithJustifyAlignment(paragraph, text);
                    }
                    if (setfont.IsChecked == true)
                    {
                        paragraph = WithFontSettings(paragraph, text);
                    }

                    // Добавляем один форматированный параграф в тело документа
                    body.AppendChild(paragraph);

                    // Сохраняем изменения
                    wordDoc.MainDocumentPart.Document.Save();
                }

                // Закрытие (очистка ресурсов) для Word
                MessageBox.Show("Новый документ создан и сохранён!");

                // Закрытие процессов Word
                CloseWordProcess();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string filePath = "C:\\Users\\zheka\\Desktop\\newdoc.docx"; // Путь к файлу

            try
            {
                // Открываем документ с помощью программы, ассоциированной с .docx файлами
                Process.Start(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при открытии документа: {ex.Message}");
            }
        }
    }
}
