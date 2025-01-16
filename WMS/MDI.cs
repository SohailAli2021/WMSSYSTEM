using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Diagnostics;
using Npgsql;
using System.Xml.Linq;
using System.Collections.Generic;

namespace WMS
{
    public partial class MDI : Form
      {
       

        public MDI()
        {
            InitializeComponent();

             txtjoborderbarcode.KeyPress += new KeyPressEventHandler(txtjoborderbarcode_KeyPress);
           
        }

        NpgsqlConnection con = new NpgsqlConnection("Server=localhost;Port=5432;Database=WMS;User Id=postgres;password=1234;");


        void Gridfill()
        {
            con.Open();
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from machine_weight";
            NpgsqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                DataTable dt = new DataTable();
                dt.Load(dr);
                dataGridView1.DataSource = dt;
            }
            cmd.Dispose();
            con.Close();
        }


        private void txtjoborderbarcode_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                

                if (e.KeyChar == (char)13) // Enter key
                {
                    Console.WriteLine("Enter key detected in txtweight.");

                    // Validate weight input
                    string weight = txtweight.Text.Trim();
                    string barcode = txtjoborderbarcode.Text.Trim();

                    if (string.IsNullOrEmpty(weight))
                    {
                        MessageBox.Show("Please enter a valid weight.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (string.IsNullOrEmpty(barcode))
                    {
                        MessageBox.Show("Please enter a valid barcode.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (!double.TryParse(weight, out double parsedWeight))
                    {
                        MessageBox.Show("Weight must be a valid decimal number.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    Console.WriteLine($"Parsed Weight: {parsedWeight}");
                    Console.WriteLine($"Barcode: {barcode}");

                    // Attempt database insertion
                    using (NpgsqlConnection con = new NpgsqlConnection("Server=localhost;Port=5432;Database=WMS;User Id=postgres;Password=1234;"))
                    {
                        try
                        {
                            con.Open();
                            Console.WriteLine("Database connection opened successfully.");

                            string sql = "INSERT INTO machine_weight(live_weight, scanned_barcode) VALUES(@weight, @barcode);";

                            using (var command = new NpgsqlCommand(sql, con))
                            {
                                command.Parameters.AddWithValue("@weight", NpgsqlTypes.NpgsqlDbType.Double, parsedWeight);

                                command.Parameters.AddWithValue("@barcode", NpgsqlTypes.NpgsqlDbType.Varchar, barcode);

                                int rowsAffected = command.ExecuteNonQuery();
                                Console.WriteLine($"Rows affected: {rowsAffected}");

                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Weight and Barcode inserted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    txtweight.Clear();
                                    txtjoborderbarcode.Clear();
                                    txtjoborderbarcode.Focus();
                                    Gridfill();
                                }
                                else
                                {
                                    MessageBox.Show("No rows were inserted. Please try again.", "Insert Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            }
                        }
                        catch (Exception dbEx)
                        {
                            Console.WriteLine($"Database Error: {dbEx}");
                            MessageBox.Show($"Database error: {dbEx.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex}");
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void MDI_Load(object sender, EventArgs e)
        {
            Gridfill();
        }

      

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Setting setting = new Setting();
            setting.Show();
            this.Hide();

        }

        private void button1_Click(object sender, EventArgs e)
        {

            PrintDocument printDoc = new PrintDocument();
            printDoc.PrintPage += new PrintPageEventHandler(PrintDoc_PrintPage);

            PrintPreviewDialog previewDialog = new PrintPreviewDialog
            {
                Document = printDoc
            };

            // Preview the print before sending to printer
            if (previewDialog.ShowDialog() == DialogResult.OK)
            {
                printDoc.Print();
            }
        }



        private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            // Get data from the TextBox and DataGridView
            string weight = txtweight.Text;
            string jobOrderBarcode = txtjoborderbarcode.Text;

            // Example DataGridView first selected row
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridView1.SelectedRows[0];
                string machineWeightId = selectedRow.Cells["machine_weight_id"].Value.ToString();
                string scannedBarcode = selectedRow.Cells["scanned_barcode"].Value.ToString();
                string liveWeight = selectedRow.Cells["live_weight"].Value.ToString();

                // Print the data
                e.Graphics.DrawString("Weight Machine Label ", new Font("Arial", 16, FontStyle.Bold), Brushes.Black,50,130);
                e.Graphics.DrawString($"Machine Weight ID: {machineWeightId}", new Font("Arial", 12), Brushes.Black, 50, 160);
                e.Graphics.DrawString($"Scanned Barcode: {scannedBarcode}", new Font("Arial", 12), Brushes.Black, 50, 190);
                e.Graphics.DrawString($"Live Weight: {liveWeight}", new Font("Arial", 12), Brushes.Black, 50, 220);
            }
            else
            {
                e.Graphics.DrawString("No row selected to print.", new Font("Arial", 12), Brushes.Red, 50, 100);
            }
        }
    }
}

  
 