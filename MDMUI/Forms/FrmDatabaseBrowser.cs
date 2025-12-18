using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MDMUI.Utility;

namespace MDMUI
{
    public partial class FrmDatabaseBrowser : Form
    {
        private readonly string connectionString = DbConnectionHelper.GetConnectionString();
        private readonly SqlConnection connection;
        
        public FrmDatabaseBrowser()
        {
            InitializeComponent();
            
            // 初始化数据库连接
            connection = new SqlConnection(connectionString);
            this.FormClosed += (s, e) =>
            {
                try
                {
                    if (connection.State != ConnectionState.Closed)
                    {
                        connection.Close();
                    }
                    connection.Dispose();
                }
                catch
                {
                    // 忽略关闭异常
                }
            };
            
            // 加载表格列表
            LoadTables();
        }
        
        private void LoadTables()
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                
                // 获取所有表格
                DataTable tables = connection.GetSchema("Tables");
                lstTables.Items.Clear();
                
                foreach (DataRow row in tables.Rows)
                {
                    string tableName = row["TABLE_NAME"].ToString();
                    lstTables.Items.Add(tableName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载表格列表失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void lstTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstTables.SelectedItem != null)
            {
                string tableName = lstTables.SelectedItem.ToString();
                LoadTableData(tableName);
            }
        }
        
        private void LoadTableData(string tableName)
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                
                string safeTableName = (tableName ?? string.Empty).Replace("]", "]]");
                string query = $"SELECT * FROM [{safeTableName}]";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                
                // 显示数据
                dataGridView.DataSource = dataTable;
                
                // 加载表结构
                LoadTableStructure(tableName);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载表 {tableName} 数据失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void LoadTableStructure(string tableName)
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                
                string query = @"
                SELECT
                    COLUMN_NAME as '列名',
                    DATA_TYPE as '数据类型',
                    CHARACTER_MAXIMUM_LENGTH as '最大长度',
                    IS_NULLABLE as '允许Null',
                    COLUMN_DEFAULT as '默认值'
                FROM INFORMATION_SCHEMA.COLUMNS
                WHERE TABLE_NAME = @TableName";

                SqlDataAdapter adapter = new SqlDataAdapter
                {
                    SelectCommand = new SqlCommand(query, connection)
                };
                adapter.SelectCommand.Parameters.AddWithValue("@TableName", tableName);
                DataTable structureTable = new DataTable();
                adapter.Fill(structureTable);
                
                // 显示表结构
                dgvStructure.DataSource = structureTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载表 {tableName} 结构失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void btnExecuteQuery_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtSqlQuery.Text))
                {
                    MessageBox.Show("请输入SQL查询", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string sql = txtSqlQuery.Text.Trim();
                string leadingKeyword = GetLeadingSqlKeyword(sql);
                
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                // 默认当作查询（SELECT/CTE），其他语句需要二次确认，避免误操作导致数据被修改/删除。
                bool isQuery = leadingKeyword == "select" || leadingKeyword == "with";
                if (!isQuery)
                {
                    DialogResult confirm = MessageBox.Show(
                        $"检测到非查询语句（{leadingKeyword}）。该操作可能修改或删除数据。\n\n是否继续执行？",
                        "危险操作确认",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                    if (confirm != DialogResult.Yes)
                    {
                        return;
                    }

                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        int affected = cmd.ExecuteNonQuery();
                        MessageBox.Show($"执行完成，影响行数：{affected}", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    return;
                }

                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                DataTable resultTable = new DataTable();
                adapter.Fill(resultTable);

                // 显示查询结果
                dataGridView.DataSource = resultTable;

                tabControl.SelectedIndex = 0; // 切换到数据标签页
            }
            catch (Exception ex)
            {
                MessageBox.Show("执行SQL查询失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static string GetLeadingSqlKeyword(string sql)
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                return string.Empty;
            }

            string working = sql.TrimStart();

            // 跳过 -- 行注释（尽量少做“聪明解析”，只覆盖常见情况）
            while (working.StartsWith("--"))
            {
                int newline = working.IndexOfAny(new[] { '\r', '\n' });
                if (newline < 0)
                {
                    return string.Empty;
                }
                working = working.Substring(newline).TrimStart();
            }

            int end = 0;
            while (end < working.Length && char.IsLetter(working[end]))
            {
                end++;
            }

            if (end <= 0)
            {
                return string.Empty;
            }

            return working.Substring(0, end).ToLowerInvariant();
        }
        
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            
            // 关闭数据库连接
            if (connection != null && connection.State == ConnectionState.Open)
                connection.Close();
        }
    }
} 
