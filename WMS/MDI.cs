using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Printing;

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
            PrintDocument printDocument = new PrintDocument();
            printDocument.PrintPage += (s, ev) =>
            {
                // Define what will be printed
                Font printFont = new Font("Arial", 16);
                ev.Graphics.DrawString($"Weight: {currentWeight}", printFont, Brushes.Black, new PointF(100, 100));
               // ev.Graphics.DrawString($"Barcodescan: {barcodeTable}", printFont, Brushes.Black, new PointF(100, 100));
            };

            try
            {
                // Show a print dialog to select the printer
                PrintDialog printDialog = new PrintDialog();
                printDialog.Document = printDocument;

                if (printDialog.ShowDialog() == DialogResult.OK)
                {
                    // Print the document
                    printDocument.Print();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while printing: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
