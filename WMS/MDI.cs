using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Printing;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Diagnostics;

namespace WMS
{
    public partial class MDI : Form
      {
        private BindingSource bindingSource;
        private DataTable barcodeTable;
        private string currentWeight = ""; // Variable to temporarily store the weight before adding it to the table

        public MDI()
        {
            InitializeComponent();

            // Initialize the DataTable to store barcode data
            barcodeTable = new DataTable();
            barcodeTable.Columns.Add("Live Weight");
            barcodeTable.Columns.Add("Scanned Barcode");

            // Initialize the BindingSource with the DataTable
            bindingSource = new BindingSource();
            bindingSource.DataSource = barcodeTable;

            // Bind the DataGridView to the BindingSource
            dataGridView1.DataSource = bindingSource;

            // Ensure KeyPress event is connected
            txtjoborderbarcode.KeyPress += txtBarcode_KeyPress;
            txtweight.KeyPress += txtweight_KeyPress;

            // Optionally, set the column width to make sure it's visible
            dataGridView1.Columns[0].Width = 100;
            dataGridView1.Columns[1].Width = 100;
        }

        private void txtweight_KeyPress(object sender, KeyPressEventArgs e)
        {
            // If the Enter key is pressed, process the weight input
            if (e.KeyChar == (char)Keys.Enter)
            {
                string weight = txtweight.Text.Trim();

                // Only add the weight if it's not empty
                if (!string.IsNullOrEmpty(weight))
                {
                    currentWeight = weight; // Temporarily store the weight
                    txtweight.Clear(); // Clear the TextBox for the next input
                    txtjoborderbarcode.Focus(); // Move focus to the barcode TextBox
                }
                else
                {
                    MessageBox.Show("Please enter a valid weight.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void txtBarcode_KeyPress(object sender, KeyPressEventArgs e)
        {
            // If the Enter key is pressed, process the scanned barcode
            if (e.KeyChar == (char)Keys.Enter)
            {
                string scannedBarcode = txtjoborderbarcode.Text.Trim();

                // Only add the barcode if it's not empty and weight is already entered
                if (!string.IsNullOrEmpty(scannedBarcode))
                {
                    if (!string.IsNullOrEmpty(currentWeight))
                    {
                        // Add the weight and barcode to the DataTable
                        barcodeTable.Rows.Add(currentWeight, scannedBarcode);

                        // Clear the temporary weight and TextBox for the next input
                        currentWeight = "";
                        txtjoborderbarcode.Clear();

                        // Refresh the DataGridView to reflect changes
                        dataGridView1.Refresh();
                        txtweight.Focus(); // Move focus back to the weight TextBox
                    }
                    else
                    {
                        MessageBox.Show("Please enter the weight before scanning the barcode.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Setting setting = new Setting();
            setting.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Generate the PDF
                string pdfFilePath = GeneratePdf();

                // Print the PDF
                PrintPdf(pdfFilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GeneratePdf()
        {
            // Define the file path for the PDF
            string filePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "GeneratedDocument.pdf");

            // Create a new PDF document
            PdfDocument document = new PdfDocument();
            document.Info.Title = "Generated Document";

            // Create a page
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont font = new XFont("Arial", 16);
            // Draw content on the PDF
           
            // Draw table headers
            gfx.DrawString($"Weight: {txtweight.Text}", font, XBrushes.Black, new XPoint(100, 100));
            gfx.DrawString($"Barcode: {txtjoborderbarcode.Text}", font, XBrushes.Black, new XPoint(250, 100));
           
            int yPosition = 130; // Start position for rows

            // Iterate through the DataTable and add rows
            foreach (DataRow row in barcodeTable.Rows)
            {
                string weight = row["Live Weight"].ToString();
                string barcode = row["Scanned Barcode"].ToString();
               
                gfx.DrawString(weight, font, XBrushes.Black, new XPoint(100, yPosition));
                gfx.DrawString(barcode, font, XBrushes.Black, new XPoint(250, yPosition));

                yPosition += 30; // Move to the next row
            }

            // Save the document
            document.Save(filePath);

            MessageBox.Show($"PDF generated successfully at {filePath}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            return filePath;
        }


        private void PrintPdf(string filePath)
        {
            // Use the default PDF viewer to print the file
            Process printProcess = new Process();
            printProcess.StartInfo.FileName = filePath;
            printProcess.StartInfo.Verb = "print";
            printProcess.StartInfo.CreateNoWindow = false;
            printProcess.Start();
        }

        
    }
}

