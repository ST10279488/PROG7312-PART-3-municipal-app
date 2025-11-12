using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;
using FinalMunicipalApp.Data;
using FinalMunicipalApp.Models;
using Microsoft.Win32;

namespace FinalMunicipalApp
{
    public partial class ReportIssuesWindow : Window
    {
        private List<string> selectedFiles = new List<string>();

        public ReportIssuesWindow()
        {
            InitializeComponent();
            UpdateEngagement();
            CboCategory.SelectedIndex = 0;
        }

        private void BtnAttach_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog()
            {
                Multiselect = true,
                Filter = "Images and Documents|*.png;*.jpg;*.jpeg;*.bmp;*.gif;*.pdf;*.doc;*.docx;*.xls;*.xlsx|All files|*.*"
            };

            if (dlg.ShowDialog() == true)
            {
                foreach (var file in dlg.FileNames)
                {
                    if (!selectedFiles.Contains(file))
                    {
                        selectedFiles.Add(file);
                        LstAttachments.Items.Add(System.IO.Path.GetFileName(file));
                    }
                }
            }
        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            string location = TxtLocation.Text.Trim();
            string category = (CboCategory.SelectedItem as System.Windows.Controls.ComboBoxItem)?.Content.ToString() ?? "Other";
            string description = new TextRange(RtbDescription.Document.ContentStart, RtbDescription.Document.ContentEnd).Text.Trim();

            if (string.IsNullOrEmpty(location))
            {
                MessageBox.Show("Please enter a location.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrEmpty(description))
            {
                MessageBox.Show("Please provide a description of the issue.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var issue = new Issue
            {
                Location = location,
                Category = category,
                Description = description,
                AttachmentFileNames = new List<string>()
            };

            string attachDir = IssueRepository.EnsureAttachmentsFolder();

            foreach (var file in selectedFiles)
            {
                string destFile = System.IO.Path.Combine(attachDir, $"{Guid.NewGuid()}{System.IO.Path.GetExtension(file)}");
                try
                {
                    File.Copy(file, destFile, true);
                    issue.AttachmentFileNames.Add(System.IO.Path.GetFileName(destFile));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to copy attachment: " + System.IO.Path.GetFileName(file) + "\n" + ex.Message, "Attachment Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }

            IssueRepository.Add(issue);

            MessageBox.Show("Thank you — your report has been submitted.", "Submitted", MessageBoxButton.OK, MessageBoxImage.Information);

            TxtLocation.Clear();
            RtbDescription.Document.Blocks.Clear();
            LstAttachments.Items.Clear();
            selectedFiles.Clear();
            CboCategory.SelectedIndex = 0;

            UpdateEngagement();
        }

        private void UpdateEngagement()
        {
            var count = IssueRepository.Issues?.Count ?? 0;
            LblEngagement.Text = count == 0 ? "Be the first to report an issue in your area!" : $"Community reports submitted: {count} — thank you for participating.";
            EngagementProgress.Value = Math.Min(EngagementProgress.Maximum, count % (EngagementProgress.Maximum + 1));
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
