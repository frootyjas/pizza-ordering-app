using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace pizza_ordering_app
{
    public partial class AdminDashboard : Form
    {
        private int selectedRowIndex = -1;
        private bool isAdding = false;
        private bool isEditing = false;
        private DataGridViewRow originalRow;

        public AdminDashboard()
        {
            InitializeComponent();
            InitializeEventHandlers();
            SetupProductGrid();
            LoadProducts();
        }

        private void InitializeEventHandlers()
        {
            this.Load += AdminDashboard_Load;
            btnSales.Click += BtnSales_Click;
            btnInventory.Click += BtnInventory_Click;
            btnAdd.Click += BtnAdd_Click;
            btnSave.Click += BtnSave_Click;
            btnEdit.Click += BtnEdit_Click;
            btnDelete.Click += BtnDelete_Click;
            btnCancel.Click += BtnCancel_Click;
            dgvProducts.CellClick += DgvProducts_CellClick;
        }

        private void AdminDashboard_Load(object sender, EventArgs e)
        {
            panelInventory.Visible = false;
            btnCancel.Visible = false;
        }

        private void SetupProductGrid()
        {
            dgvProducts.Columns.Clear();
            dgvProducts.ReadOnly = true;
            dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProducts.MultiSelect = false;
            dgvProducts.AllowUserToAddRows = false;
            dgvProducts.RowTemplate.Height = 60;

            dgvProducts.Columns.Add("name", "Name");
            dgvProducts.Columns.Add("price", "Price");

            var imageCol = new DataGridViewImageColumn
            {
                HeaderText = "Image",
                Name = "image",
                ImageLayout = DataGridViewImageCellLayout.Zoom
            };
            dgvProducts.Columns.Add(imageCol);
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (isEditing || isAdding)
            {
                MessageBox.Show("Please save or cancel current operation first.");
                return;
            }

            isAdding = true;
            btnCancel.Visible = true;
            dgvProducts.ReadOnly = false;
            dgvProducts.Rows.Add();
            selectedRowIndex = dgvProducts.Rows.Count - 1;
            dgvProducts.CurrentCell = dgvProducts.Rows[selectedRowIndex].Cells[0];
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (isAdding || isEditing)
            {
                MessageBox.Show("Finish current operation first.");
                return;
            }

            if (selectedRowIndex < 0) return;

            originalRow = CloneRow(dgvProducts.Rows[selectedRowIndex]);
            isEditing = true;
            btnCancel.Visible = true;
            dgvProducts.ReadOnly = false;
            dgvProducts.Rows[selectedRowIndex].ReadOnly = false;
            dgvProducts.CurrentCell = dgvProducts.Rows[selectedRowIndex].Cells[0];
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (isAdding)
            {
                dgvProducts.Rows.RemoveAt(selectedRowIndex);
            }
            else if (isEditing && originalRow != null)
            {
                dgvProducts.Rows[selectedRowIndex].SetValues(
                    originalRow.Cells["name"].Value,
                    originalRow.Cells["price"].Value,
                    originalRow.Cells["image"].Value
                );
            }

            ResetState();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (selectedRowIndex < 0) return;

            var row = dgvProducts.Rows[selectedRowIndex];
            if (!ValidateRow(row)) return;

            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    SaveProduct(row, conn);
                }

                LoadProducts();
                MessageBox.Show("Product saved successfully!");
                ResetState();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving product: {ex.Message}");
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (isAdding || isEditing || selectedRowIndex < 0)
            {
                MessageBox.Show("Finish current operation first.");
                return;
            }

            var productName = dgvProducts.Rows[selectedRowIndex].Cells["name"].Value?.ToString();
            if (string.IsNullOrEmpty(productName)) return;

            var confirm = MessageBox.Show(
                $"Delete '{productName}'?",
                "Confirm Delete",
                MessageBoxButtons.YesNo
            );

            if (confirm != DialogResult.Yes) return;

            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    new MySqlCommand(
                        "DELETE FROM products WHERE name = @name",
                        conn
                    ).ExecuteNonQueryWithParam("@name", productName);
                }

                LoadProducts();
                MessageBox.Show("Product deleted successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting product: {ex.Message}");
            }
        }

        private void DgvProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != dgvProducts.Columns["image"].Index) return;

            using (var ofd = new OpenFileDialog { Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp" })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    dgvProducts.Rows[e.RowIndex].Cells["image"].Value =
                        Image.FromFile(ofd.FileName);
                }
            }
        }

        private void LoadProducts()
        {
            dgvProducts.Rows.Clear();
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (var reader = new MySqlCommand("SELECT * FROM products", conn).ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var imgBytes = (byte[])reader["image"];
                        using (var ms = new MemoryStream(imgBytes))
                        {
                            dgvProducts.Rows.Add(
                                reader["name"].ToString(),
                                reader["price"],
                                Image.FromStream(ms)
                            );
                        }
                    }
                }
            }
        }

        private DataGridViewRow CloneRow(DataGridViewRow row)
        {
            var newRow = (DataGridViewRow)row.Clone();
            for (int i = 0; i < row.Cells.Count; i++)
            {
                newRow.Cells[i].Value = row.Cells[i].Value;
            }
            return newRow;
        }

        private bool ValidateRow(DataGridViewRow row)
        {
            if (string.IsNullOrWhiteSpace(row.Cells["name"].Value?.ToString()))
            {
                MessageBox.Show("Product name is required.");
                return false;
            }

            if (!decimal.TryParse(row.Cells["price"].Value?.ToString(), out _))
            {
                MessageBox.Show("Valid price is required.");
                return false;
            }

            if (row.Cells["image"].Value == null)
            {
                MessageBox.Show("Product image is required.");
                return false;
            }

            return true;
        }

        private void SaveProduct(DataGridViewRow row, MySqlConnection conn)
        {
            var cmdText = @"
                INSERT INTO products (name, price, image) 
                VALUES (@name, @price, @image)
                ON DUPLICATE KEY UPDATE 
                    price = VALUES(price),
                    image = VALUES(image)";

            using (var cmd = new MySqlCommand(cmdText, conn))
            {
                cmd.ExecuteNonQueryWithParams(
                    ("@name", row.Cells["name"].Value.ToString()),
                    ("@price", Convert.ToDecimal(row.Cells["price"].Value)),
                    ("@image", ImageToBytes(row.Cells["image"].Value as Image))
                );
            }
        }

        private byte[] ImageToBytes(Image img)
        {
            using (var ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }
        }

        private void ResetState()
        {
            isAdding = false;
            isEditing = false;
            selectedRowIndex = -1;
            originalRow = null;
            btnCancel.Visible = false;
            dgvProducts.ReadOnly = true;
        }

        // Other existing methods (BtnSales_Click, BtnInventory_Click, etc.)
        private void BtnSales_Click(object sender, EventArgs e) => panelInventory.Visible = false;
        private void BtnInventory_Click(object sender, EventArgs e) => panelInventory.Visible = true;
    }
}

public static class MySqlExtensions
{
    public static void ExecuteNonQueryWithParam(this MySqlCommand cmd, string name, object value)
    {
        cmd.Parameters.AddWithValue(name, value);
        cmd.ExecuteNonQuery();
    }

    public static void ExecuteNonQueryWithParams(this MySqlCommand cmd, params (string, object)[] parameters)
    {
        foreach (var (name, value) in parameters)
        {
            cmd.Parameters.AddWithValue(name, value);
        }
        cmd.ExecuteNonQuery();
    }
}