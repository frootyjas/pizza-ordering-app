using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace pizza_ordering_app
{
    public partial class AdminDashboard : Form
    {
        private int selectedRowIndex = -1;
        private bool isEditing = false;
        private bool isEditingIngredient = false;

   


        public AdminDashboard()
        {
            InitializeComponent();
            InitializeEventHandlers();
            SetupProductGrid();
            SetupIngredientGrid();
            LoadProducts();
            LoadIngredients();


            // Force vertical scrollbar to always appear
            billSummaryPanel.AutoScroll = true;
            billSummaryPanel.AutoScrollMinSize = new Size(0, billSummaryPanel.ClientSize.Height + 1);


       


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

            smallCheckbox.CheckedChanged += SizeCheckBox_CheckedChanged;
            mediumCheckbox.CheckedChanged += SizeCheckBox_CheckedChanged;
            largeCheckbox.CheckedChanged += SizeCheckBox_CheckedChanged;

            btnIngAdd.Click += BtnIngAdd_Click;
            btnIngSave.Click += BtnIngSave_Click;
            btnIngEdit.Click += BtnIngEdit_Click;
            btnIngDelete.Click += BtnIngDelete_Click;
            btnIngCancel.Click += BtnIngCancel_Click;
            dgvIngredients.CellClick += DgvIngredients_CellClick;

            LoadSalesProducts();
            LoadIngredientCheckboxes();
        }




        private void AdminDashboard_Load(object sender, EventArgs e)


        {
            panelSales.Visible = true;  // Show Sales by default
            panelInventory.Visible = false;
            btnCancel.Visible = false;
            btnIngCancel.Visible = false;



 
        }

        private void SetupProductGrid()
        {
            dgvProducts.Columns.Clear();
            dgvProducts.ReadOnly = true;
            dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProducts.MultiSelect = false;
            dgvProducts.AllowUserToAddRows = false;
            dgvProducts.RowTemplate.Height = 60;

            dgvProducts.Columns.Add("id", "ID");
            dgvProducts.Columns["id"].Visible = false;

            dgvProducts.Columns.Add("name", "Name");
            dgvProducts.Columns.Add("price", "Price");

            var imageCol = new DataGridViewImageColumn
            {
                HeaderText = "Image",
                Name = "image",
                ImageLayout = DataGridViewImageCellLayout.Zoom
            };
            dgvProducts.Columns.Add(imageCol);

            dgvProducts.Columns.Add("stocks", "Stocks");
        }


        private void SetupIngredientGrid()
        {
            dgvIngredients.Columns.Clear();
            dgvIngredients.ReadOnly = true;
            dgvIngredients.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvIngredients.MultiSelect = false;
            dgvIngredients.AllowUserToAddRows = false;

            dgvIngredients.Columns.Add("id", "ID");
            dgvIngredients.Columns["id"].Visible = false;
            dgvIngredients.Columns.Add("ingItem", "Item");
            dgvIngredients.Columns.Add("ingServingSize", "Serving Size");
            dgvIngredients.Columns.Add("ingPrice", "Price per Serving");
            dgvIngredients.Columns.Add("ingStockAvailable", "Stock Available");
        }

        private void LoadProducts()
        {
            dgvProducts.Rows.Clear();
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                var cmd = DatabaseHelper.CreateCommand("SELECT id, name, price, image, stocks FROM products", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var imgBytes = (byte[])reader["image"];
                        using (var ms = new MemoryStream(imgBytes))
                        {
                            dgvProducts.Rows.Add(
                                reader["id"],
                                reader["name"],
                                reader["price"],
                                Image.FromStream(ms),
                                reader["stocks"]
                            );
                        }
                    }
                }
            }
            // Update the products label text with the current count
            lblProducts.Text = $"Products ({dgvProducts.Rows.Count})";

        }

        private void LoadSalesProducts()
        {
            flpSalesProducts.Controls.Clear(); // Clear existing controls

            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                var cmd = DatabaseHelper.CreateCommand("SELECT id, name, price, image FROM products", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Read product info
                        int id = Convert.ToInt32(reader["id"]);
                        string name = reader["name"].ToString();
                        decimal price = Convert.ToDecimal(reader["price"]);
                        byte[] imageBytes = (byte[])reader["image"];

                        // Convert image
                        Image productImage;
                        using (var ms = new MemoryStream(imageBytes))
                        {
                            productImage = Image.FromStream(ms);
                        }

                        // Create product panel
                        var productPanel = new Panel
                        {
                            Width = 170,
                            Height = 200,
                            Margin = new Padding(5),
                            BackColor = Color.White
                        };

                        var picBox = new PictureBox
                        {
                            Image = productImage,
                            SizeMode = PictureBoxSizeMode.Zoom,
                            Dock = DockStyle.Top,
                            Height = 100
                        };

                        var nameLabel = new Label
                        {
                            Text = name,
                            TextAlign = ContentAlignment.MiddleCenter,
                            Dock = DockStyle.Top,
                            Height = 30,
                            Font = new Font("Verdana", 9, FontStyle.Bold) 
                        };

                        var priceLabel = new Label
                        {
                            Text = $"₱{price:N2}",
                            TextAlign = ContentAlignment.MiddleCenter,
                            Dock = DockStyle.Top,
                            Height = 25,
                            ForeColor = Color.Green,
                            Font = new Font("Verdana", 8, FontStyle.Regular) 
                        };


                        var btnAddToCart = new Button
                        {
                            Text = "Select",
                            Dock = DockStyle.Bottom,
                            Tag = id // store product id
                        };

                        btnAddToCart.Click += BtnAddToCart_Click;

                        productPanel.Controls.Add(btnAddToCart);

                        // Add controls
                        productPanel.Controls.Add(btnAddToCart);
                        productPanel.Controls.Add(priceLabel);
                        productPanel.Controls.Add(nameLabel);
                        productPanel.Controls.Add(picBox);

                        flpSalesProducts.Controls.Add(productPanel);
                    }
                }
            }
        }

        private void LoadIngredientCheckboxes()
        {
            panelAddOns.Controls.Clear();
            var ingredients = GetAllIngredients();

            int x = 10;
            int y = 10;
            int colCount = 3;
            int colWidth = 130;

            for (int i = 0; i < ingredients.Count; i++)
            {
                var ingredient = ingredients[i];

                var chk = new CheckBox
                {
                    Text = ingredient.Name,
                    Tag = ingredient.Id,
                    AutoSize = true,
                    Location = new Point(x, y)
                };

                panelAddOns.Controls.Add(chk);

                // Move to next column or row
                if ((i + 1) % colCount == 0)
                {
                    x = 10;
                    y += chk.Height + 5;
                }
                else
                {
                    x += colWidth;
                }
            }

        }

        private int selectedProductId = -1;

        public class Ingredient
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public decimal PricePerServing { get; set; }
        }

        public class Product
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
        }

        public class OrderItem
        {
            public int ProductId { get; set; }
            public string ProductName { get; set; }
            public string Size { get; set; }
            public List<int> AddOnIds { get; set; }
            public List<string> AddOnNames { get; set; }
            public int Quantity { get; set; }
            public decimal Total { get; set; }
            public Panel Panel { get; set; }
        }

        private List<OrderItem> currentOrderItems = new List<OrderItem>();

        private void SizeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            if (cb.Checked)
            {
                foreach (CheckBox otherCb in new[] { smallCheckbox, mediumCheckbox, largeCheckbox })
                {
                    if (otherCb != cb)
                    {
                        otherCb.Checked = false;
                    }
                }
            }
        }
        private void btnAddToOrder_Click(object sender, EventArgs e)
        {
            if (selectedProductId == -1)
            {
                MessageBox.Show("Please select a pizza first.");
                return;
            }

            if (!smallCheckbox.Checked && !mediumCheckbox.Checked && !largeCheckbox.Checked)
            {
                MessageBox.Show("Please select a size.");
                return;
            }

            string size = smallCheckbox.Checked ? "Small" : mediumCheckbox.Checked ? "Medium" : "Large";
            int qty = (int)quantity.Value;

            List<Ingredient> selectedAddOns = new List<Ingredient>();
            foreach (Control control in panelAddOns.Controls)
            {
                if (control is CheckBox chk && chk.Checked)
                {
                    int ingredientId = (int)chk.Tag;
                    Ingredient ingredient = GetIngredientById(ingredientId);
                    if (ingredient != null)
                        selectedAddOns.Add(ingredient);
                }
            }

            Product product = GetProductById(selectedProductId);
            if (product == null)
            {
                MessageBox.Show("Product not found.");
                return;
            }

            decimal total = product.Price * qty;
            foreach (var addOn in selectedAddOns)
                total += addOn.PricePerServing * qty;

            List<int> addOnIds = selectedAddOns.Select(a => a.Id).OrderBy(id => id).ToList();
            var existingItem = currentOrderItems.FirstOrDefault(item =>
                item.ProductId == selectedProductId &&
                item.Size == size &&
                item.AddOnIds.OrderBy(id => id).SequenceEqual(addOnIds));

            Console.WriteLine($"Product ID: {selectedProductId}, Size: {size}, Qty: {qty}");

            if (existingItem != null)
            {
                existingItem.Quantity += qty;
                existingItem.Total += total;
                UpdateOrderItemPanel(existingItem);
            }
            else
            {
                List<string> addOnNames = selectedAddOns.Select(a => a.Name).ToList();
                OrderItem newItem = new OrderItem
                {
                    ProductId = selectedProductId,
                    ProductName = product.Name,
                    Size = size,
                    AddOnIds = addOnIds,
                    AddOnNames = addOnNames,
                    Quantity = qty,
                    Total = total,
                    Panel = CreateOrderItemPanel(product.Name, size, qty, addOnNames, total)
                };
                currentOrderItems.Add(newItem);
                // Add the panel to the FlowLayoutPanel
                billSummaryPanel.Controls.Add(newItem.Panel);
                UpdateFinalTotal();

                // Scroll to the new item
                billSummaryPanel.ScrollControlIntoView(newItem.Panel);



            }
            // Force UI refresh
            billSummaryPanel.PerformLayout();
            billSummaryPanel.Refresh();

            // Reset UI
            selectedProductId = -1;
            quantity.Value = 1;
            smallCheckbox.Checked = mediumCheckbox.Checked = largeCheckbox.Checked = false;
            foreach (CheckBox chk in panelAddOns.Controls.OfType<CheckBox>())
                chk.Checked = false;
        }
        private Panel CreateOrderItemPanel(string productName, string size, int quantity, List<string> addOnNames, decimal total)
        {
            Panel itemPanel = new Panel
            {
                Width = billSummaryPanel.Width - 25, // Adjust for scrollbar
                Height = 50,
                Margin = new Padding(5),
                BackColor = Color.White, 
                //BorderStyle = BorderStyle.FixedSingle
            };

            Label lblDetails = new Label
            {
                Text = $"{productName} ({size}) x{quantity} | Add-ons: {GetAddOnsText(addOnNames)} | Total: ₱{total:N2}",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };

            itemPanel.Controls.Add(lblDetails);
            return itemPanel;
        }
        private void UpdateOrderItemPanel(OrderItem item)
        {
            Label lbl = item.Panel.Controls.OfType<Label>().First();
            lbl.Text = $"{item.ProductName} ({item.Size}) x{item.Quantity} | Add-ons: {GetAddOnsText(item.AddOnNames)} | Total: ₱{item.Total:N2}";
        }

        private void UpdateFinalTotal()
        {
            decimal finalTotalValue = currentOrderItems.Sum(item => item.Total);
            finalTotal.Text = $"Total: ₱{finalTotalValue:N2}";
        }



        private string GetAddOnsText(List<string> addOnNames)
        {
            return addOnNames.Count == 0 ? "None" : string.Join(", ", addOnNames);
        }

        private Product GetProductById(int productId)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                var cmd = DatabaseHelper.CreateCommand("SELECT name, price FROM products WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("@id", productId);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                        return new Product
                        {
                            Id = productId,
                            Name = reader["name"].ToString(),
                            Price = Convert.ToDecimal(reader["price"])
                        };
                }
            }
            return null;
        }

        private Ingredient GetIngredientById(int ingredientId)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                var cmd = DatabaseHelper.CreateCommand("SELECT name, price_per_serving FROM ingredients WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("@id", ingredientId);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                        return new Ingredient
                        {
                            Id = ingredientId,
                            Name = reader["name"].ToString(),
                            PricePerServing = Convert.ToDecimal(reader["price_per_serving"])
                        };
                }
            }
            return null;
        }

      

        private void BtnAddToCart_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            int productId = (int)button.Tag;

            if (selectedProductId != productId)
            {
                // New pizza selected: reset quantity to 1
                selectedProductId = productId;
                quantity.Value = 1;
            }
            else
            {
                // Same pizza: increment quantity
                quantity.Value++;
            }
        }


        private List<Ingredient> GetAllIngredients()
        {
            var ingredients = new List<Ingredient>();

            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                var cmd = DatabaseHelper.CreateCommand("SELECT id, name FROM ingredients", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ingredients.Add(new Ingredient
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Name = reader["name"].ToString()
                        });
                    }
                }
            }

            return ingredients;
        }



        private void LoadIngredients()
        {
            dgvIngredients.Rows.Clear();
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                var cmd = DatabaseHelper.CreateCommand("SELECT * FROM ingredients", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dgvIngredients.Rows.Add(
                            reader["id"],
                            reader["name"],
                            reader["serving_size"],
                            reader["price_per_serving"],
                            reader["stock_available"]
                        );
                    }
                }
            }
            // Update the ingredients label text with the current count
            lblIngredients.Text = $"Ingredients ({dgvIngredients.Rows.Count})";

        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (isEditing) return;
            dgvProducts.ReadOnly = false;
            dgvProducts.Rows.Add();
            selectedRowIndex = dgvProducts.Rows.Count - 1;
            dgvProducts.CurrentCell = dgvProducts.Rows[selectedRowIndex].Cells["name"];
            isEditing = true;
            btnCancel.Visible = true;
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (isEditing) return;
            if (dgvProducts.SelectedRows.Count == 0) return;
            selectedRowIndex = dgvProducts.SelectedRows[0].Index;
            dgvProducts.ReadOnly = false;
            isEditing = true;
            btnCancel.Visible = true;
            MessageBox.Show("Editing Product.", "Edit Product", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (selectedRowIndex < 0 || selectedRowIndex >= dgvProducts.Rows.Count) return;
            var row = dgvProducts.Rows[selectedRowIndex];
            if (!ValidateRow(row)) return;

            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                if (row.Cells["id"].Value != null)
                    UpdateProduct(row, conn, Convert.ToInt32(row.Cells["id"].Value));
                else
                    InsertProduct(row, conn);
            }
            LoadProducts();
            LoadSalesProducts();
            ResetEditingState();
            MessageBox.Show("Product saved successfully.", "Save Product", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (isEditing || dgvProducts.SelectedRows.Count == 0) return;

            var row = dgvProducts.SelectedRows[0];
            if (int.TryParse(row.Cells["id"].Value?.ToString(), out int id))
            {
                // Ask for confirmation before deleting
                var confirmResult = MessageBox.Show(
                    "Are you sure you want to delete this product?",
                    "Confirm Deletion",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (confirmResult == DialogResult.Yes)
                {
                    DeleteProduct(id);
                    LoadProducts();
                    LoadSalesProducts();
                }
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            ResetEditingState();
            LoadProducts();
            MessageBox.Show("Cancelled product action.", "Cancel", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ResetEditingState()
        {
            dgvProducts.ReadOnly = true;
            isEditing = false;
            btnCancel.Visible = false;
            selectedRowIndex = -1;
        }

        private bool ValidateRow(DataGridViewRow row)
        {
            if (string.IsNullOrWhiteSpace(row.Cells["name"].Value?.ToString())) return false;
            if (!decimal.TryParse(row.Cells["price"].Value?.ToString(), out _)) return false;
            if (row.Cells["image"].Value == null) return false;
            if (!int.TryParse(row.Cells["stocks"].Value?.ToString(), out _)) return false;
            return true;
        }

        private void DgvProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            selectedRowIndex = e.RowIndex;
            if (dgvProducts.Columns[e.ColumnIndex].Name == "image" && !dgvProducts.ReadOnly)
            {
                HandleImageUpload(e.RowIndex);
            }
        }

        private void HandleImageUpload(int rowIndex)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    dgvProducts.Rows[rowIndex].Cells["image"].Value = Image.FromFile(ofd.FileName);
                }
            }
        }

        private void InsertProduct(DataGridViewRow row, MySqlConnection conn)
        {
            var cmd = DatabaseHelper.CreateCommand("INSERT INTO products (name, price, image, stocks) VALUES (@name, @price, @image, @stocks)", conn);
            AddProductParameters(cmd, row);
            cmd.ExecuteNonQuery();
        }

        private void UpdateProduct(DataGridViewRow row, MySqlConnection conn, int id)
        {
            var cmd = DatabaseHelper.CreateCommand("UPDATE products SET name=@name, price=@price, image=@image, stocks=@stocks WHERE id=@id", conn);
            AddProductParameters(cmd, row);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }

        private void DeleteProduct(int id)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                var cmd = DatabaseHelper.CreateCommand("DELETE FROM products WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }

        private void AddProductParameters(MySqlCommand cmd, DataGridViewRow row)
        {
            cmd.Parameters.AddWithValue("@name", row.Cells["name"].Value);
            cmd.Parameters.AddWithValue("@price", Convert.ToDecimal(row.Cells["price"].Value));
            cmd.Parameters.AddWithValue("@image", ImageToBytes(row.Cells["image"].Value as Image));
            cmd.Parameters.AddWithValue("@stocks", Convert.ToInt32(row.Cells["stocks"].Value));
        }

        private byte[] ImageToBytes(Image img)
        {
            using (var ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }
        }

        private void BtnIngAdd_Click(object sender, EventArgs e)
        {
            if (isEditingIngredient) return;
            dgvIngredients.ReadOnly = false;
            dgvIngredients.Rows.Add();
            selectedRowIndex = dgvIngredients.Rows.Count - 1;
            dgvIngredients.CurrentCell = dgvIngredients.Rows[selectedRowIndex].Cells["ingItem"];
            isEditingIngredient = true;
            btnIngCancel.Visible = true;
        }

        private void BtnIngEdit_Click(object sender, EventArgs e)
        {
            if (isEditingIngredient || dgvIngredients.SelectedRows.Count == 0) return;
            selectedRowIndex = dgvIngredients.SelectedRows[0].Index;
            dgvIngredients.ReadOnly = false;
            isEditingIngredient = true;
            btnIngCancel.Visible = true;
            MessageBox.Show("Editing ingredient.", "Edit Ingredient", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnIngSave_Click(object sender, EventArgs e)
        {
            if (selectedRowIndex < 0 || selectedRowIndex >= dgvIngredients.Rows.Count) return;
            var row = dgvIngredients.Rows[selectedRowIndex];
            if (!ValidateIngredientRow(row)) return;
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                if (row.Cells["id"].Value != null)
                    UpdateIngredient(row, conn, Convert.ToInt32(row.Cells["id"].Value));
                else
                    InsertIngredient(row, conn);
            }
            LoadIngredients();
            ResetIngredientState();
            MessageBox.Show("Ingredient saved successfully.", "Save Ingredient", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnIngDelete_Click(object sender, EventArgs e)
        {
            if (isEditingIngredient || dgvIngredients.SelectedRows.Count == 0) return;

            var row = dgvIngredients.SelectedRows[0];
            if (int.TryParse(row.Cells["id"].Value?.ToString(), out int id))
            {
                // Ask for confirmation before deleting
                var confirmResult = MessageBox.Show(
                    "Are you sure you want to delete this ingredient?",
                    "Confirm Deletion",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (confirmResult == DialogResult.Yes)
                {
                    DeleteIngredient(id);
                    LoadIngredients();
                }
            }
        }

        private void BtnIngCancel_Click(object sender, EventArgs e)
        {
            ResetIngredientState();
            LoadIngredients();
            MessageBox.Show("Cancelled ingredient action.", "Cancel", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void DgvIngredients_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                selectedRowIndex = e.RowIndex;
        }

        private void ResetIngredientState()
        {
            dgvIngredients.ReadOnly = true;
            isEditingIngredient = false;
            btnIngCancel.Visible = false;
            selectedRowIndex = -1;
        }

        private bool ValidateIngredientRow(DataGridViewRow row)
        {
            return !string.IsNullOrWhiteSpace(row.Cells["ingItem"].Value?.ToString()) &&
                   !string.IsNullOrWhiteSpace(row.Cells["ingServingSize"].Value?.ToString()) &&
                   decimal.TryParse(row.Cells["ingPrice"].Value?.ToString(), out _) &&
                   decimal.TryParse(row.Cells["ingStockAvailable"].Value?.ToString(), out _);
        }

        private void InsertIngredient(DataGridViewRow row, MySqlConnection conn)
        {
            var cmd = DatabaseHelper.CreateCommand("INSERT INTO ingredients (name, serving_size, price_per_serving, stock_available) VALUES (@name, @size, @price, @stock)", conn);
            AddIngredientParameters(cmd, row);
            cmd.ExecuteNonQuery();
        }

        private void UpdateIngredient(DataGridViewRow row, MySqlConnection conn, int id)
        {
            var cmd = DatabaseHelper.CreateCommand("UPDATE ingredients SET name=@name, serving_size=@size, price_per_serving=@price, stock_available=@stock WHERE id=@id", conn);
            AddIngredientParameters(cmd, row);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }

        private void DeleteIngredient(int id)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                var cmd = DatabaseHelper.CreateCommand("DELETE FROM ingredients WHERE id=@id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }

        private void AddIngredientParameters(MySqlCommand cmd, DataGridViewRow row)
        {
            cmd.Parameters.AddWithValue("@name", row.Cells["ingItem"].Value?.ToString());
            cmd.Parameters.AddWithValue("@size", row.Cells["ingServingSize"].Value?.ToString());
            cmd.Parameters.AddWithValue("@price", Convert.ToDecimal(row.Cells["ingPrice"].Value));
            cmd.Parameters.AddWithValue("@stock", Convert.ToDecimal(row.Cells["ingStockAvailable"].Value));
        }

        private void BtnSales_Click(object sender, EventArgs e)
        {
            // Show Sales panel, hide Inventory
            panelSales.Visible = true;
            panelInventory.Visible = false;
            LoadSalesProducts();
            LoadIngredientCheckboxes();
        }

        private void BtnInventory_Click(object sender, EventArgs e)
        {
            // Show Inventory panel, hide Sales
            panelSales.Visible = false;
            panelInventory.Visible = true;
        }
        private void btnLogout_Click(object sender, EventArgs e) => ConfirmLogout();

        private void ConfirmLogout()
        {
            if (MessageBox.Show("Logout?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                new Login().Show();
                Close();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void panelSales_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void finalTotalLabel_Click(object sender, EventArgs e)
        {

        }
    }
}
