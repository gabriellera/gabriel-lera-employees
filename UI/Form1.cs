using gabriel_lera_employees.Services;
using gabriel_lera_employees.Utils;

namespace UI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            dataGridViewResults.AutoGenerateColumns = true;
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            using var openFileDialog = new OpenFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
                Title = "Select Employee CSV"
            };

            if (openFileDialog.ShowDialog() != DialogResult.OK) return;

            string filePath = openFileDialog.FileName;
            lblStatus.Text = $"Loading file: {Path.GetFileName(filePath)}";

            try
            {
                var records = CsvParser.ParseCsv(filePath).ToList();
                var service = new CollaborationService();

                var pairProjects = service.GetAllPairProjectOverlaps(records);

                if (!pairProjects.Any())
                {
                    lblStatus.Text = "No overlapping pairs found.";
                    dataGridViewResults.DataSource = null;
                    return;
                }

                dataGridViewResults.DataSource = pairProjects
                    .OrderByDescending(p => p.DaysWorked)
                    .ToList();

                lblStatus.Text = $"Loaded {pairProjects.Count} records.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading file: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
