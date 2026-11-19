using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ProjectTaskManagement.BUS;
using ProjectTaskManagement.DTO;

namespace ProjectTaskManagement.GUI
{
    public class frmCongViec : Form
    {
        private readonly CongViecBUS taskBus = new CongViecBUS();
        private readonly DuAnBUS duAnBus = new DuAnBUS();

        private ComboBox cboFilterDuAn, cboFilterTrangThai;
        private TextBox txtTenTask, txtMoTa;
        private ComboBox cboFormDuAn, cboUuTien, cboFormTrangThai, cboDoiTrangThai;
        private DateTimePicker dtpHanNop;
        private DataGridView dgvTasks;
        private FlowLayoutPanel flpKanban;

        public frmCongViec()
        {
            BuildUI();
            LoadCombos();
            LoadTasks();
        }

        private void BuildUI()
        {
            Controls.Add(new Label { Text = "Dự án:", Left = 20, Top = 18, Width = 55 });
            cboFilterDuAn = new ComboBox { Left = 80, Top = 15, Width = 220, DropDownStyle = ComboBoxStyle.DropDownList };
            cboFilterDuAn.SelectedIndexChanged += (s, e) => LoadTasks();

            Controls.Add(new Label { Text = "Trạng thái:", Left = 315, Top = 18, Width = 70 });
            cboFilterTrangThai = new ComboBox { Left = 390, Top = 15, Width = 140, DropDownStyle = ComboBoxStyle.DropDownList };
            cboFilterTrangThai.Items.AddRange(new object[] { "Tất cả", "To Do", "In Progress", "Review", "Done" });
            cboFilterTrangThai.SelectedIndex = 0;
            cboFilterTrangThai.SelectedIndexChanged += (s, e) => LoadTasks();

            Controls.Add(new Label { Text = "Tên Task:", Left = 20, Top = 52, Width = 65 });
            txtTenTask = new TextBox { Left = 85, Top = 49, Width = 215 };

            Controls.Add(new Label { Text = "Ưu tiên:", Left = 315, Top = 52, Width = 55 });
            cboUuTien = new ComboBox { Left = 375, Top = 49, Width = 110, DropDownStyle = ComboBoxStyle.DropDownList };
            cboUuTien.Items.AddRange(new object[] { "Cao", "Trung bình", "Thấp" });
            cboUuTien.SelectedIndex = 1;

            Controls.Add(new Label { Text = "Hạn nộp:", Left = 500, Top = 52, Width = 60 });
            dtpHanNop = new DateTimePicker { Left = 565, Top = 49, Width = 130, Format = DateTimePickerFormat.Short };

            var btnTaoTask = new Button { Text = "Thêm Task", Left = 710, Top = 47, Width = 100, Height = 28 };
            btnTaoTask.Click += BtnTaoTask_Click;

            Controls.Add(new Label { Text = "Mô tả:", Left = 20, Top = 84, Width = 60 });
            txtMoTa = new TextBox { Left = 85, Top = 81, Width = 400 };

            cboFormDuAn = new ComboBox { Left = 500, Top = 81, Width = 195, DropDownStyle = ComboBoxStyle.DropDownList };
            cboFormTrangThai = new ComboBox { Left = 710, Top = 81, Width = 115, DropDownStyle = ComboBoxStyle.DropDownList };
            cboFormTrangThai.Items.AddRange(new object[] { "To Do", "In Progress", "Review", "Done" });
            cboFormTrangThai.SelectedIndex = 0;

            Controls.Add(new Label { Text = "Đổi trạng thái task chọn:", Left = 20, Top = 116, Width = 145 });
            cboDoiTrangThai = new ComboBox { Left = 170, Top = 113, Width = 140, DropDownStyle = ComboBoxStyle.DropDownList };
            cboDoiTrangThai.Items.AddRange(new object[] { "To Do", "In Progress", "Review", "Done" });
            cboDoiTrangThai.SelectedIndex = 1;

            var btnCapNhatTT = new Button { Text = "Cập nhật trạng thái", Left = 320, Top = 112, Width = 135, Height = 26 };
            btnCapNhatTT.Click += BtnCapNhatTT_Click;

            var tab = new TabControl
            {
                Left = 20,
                Top = 148,
                Width = 870,
                Height = 460,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };

            var tabList = new TabPage("List View");
            dgvTasks = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true
            };
            tabList.Controls.Add(dgvTasks);

            var tabKanban = new TabPage("Kanban View");
            flpKanban = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(10)
            };
            tabKanban.Controls.Add(flpKanban);

            tab.TabPages.Add(tabList);
            tab.TabPages.Add(tabKanban);

            Controls.AddRange(new Control[]
            {
                cboFilterDuAn, cboFilterTrangThai, txtTenTask, cboUuTien, dtpHanNop,
                btnTaoTask, txtMoTa, cboFormDuAn, cboFormTrangThai, cboDoiTrangThai, btnCapNhatTT, tab
            });
        }

        private void LoadCombos()
        {
            var duAns = duAnBus.GetAll();

            var filterList = new List<DuAnDTO> { new DuAnDTO { MaDuAn = 0, TenDuAn = "Tất cả dự án" } };
            filterList.AddRange(duAns);

            cboFilterDuAn.DataSource = filterList;
            cboFilterDuAn.DisplayMember = "TenDuAn";
            cboFilterDuAn.ValueMember = "MaDuAn";

            cboFormDuAn.DataSource = new List<DuAnDTO>(duAns);
            cboFormDuAn.DisplayMember = "TenDuAn";
            cboFormDuAn.ValueMember = "MaDuAn";
        }

        private void LoadTasks()
        {
            int? maDuAn = (cboFilterDuAn.SelectedValue is int id && id > 0) ? (int?)id : null;
            string trangThai = cboFilterTrangThai.Text;

            List<CongViecDTO> list = taskBus.GetTasks(maDuAn, trangThai);
            dgvTasks.DataSource = null;
            dgvTasks.DataSource = list;

            RenderKanban(list);
        }

        private void RenderKanban(List<CongViecDTO> list)
        {
            flpKanban.Controls.Clear();
            string[] cols = { "To Do", "In Progress", "Review", "Done" };

            foreach (string col in cols)
            {
                var panelCol = new Panel
                {
                    Width = 200,
                    Height = 390,
                    BackColor = Color.FromArgb(241, 245, 249),
                    Margin = new Padding(6),
                    AutoScroll = true
                };

                var lblCol = new Label
                {
                    Text = col,
                    Dock = DockStyle.Top,
                    Height = 30,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                    BackColor = Color.FromArgb(203, 213, 225)
                };
                panelCol.Controls.Add(lblCol);

                int y = 36;
                foreach (var item in list)
                {
                    if ((item.TrangThai ?? "To Do") != col) continue;

                    var card = new Panel
                    {
                        Left = 8,
                        Top = y,
                        Width = 176,
                        Height = 72,
                        BackColor = Color.White,
                        BorderStyle = BorderStyle.FixedSingle
                    };
                    card.Controls.Add(new Label
                    {
                        Text = item.TenTask,
                        Left = 6,
                        Top = 6,
                        Width = 164,
                        Height = 36,
                        Font = new Font("Segoe UI", 8.5f, FontStyle.Bold)
                    });
                    card.Controls.Add(new Label
                    {
                        Text = $"Ưu tiên: {item.DoUuTien}",
                        Left = 6,
                        Top = 46,
                        Width = 164,
                        Height = 18,
                        Font = new Font("Segoe UI", 8f)
                    });

                    panelCol.Controls.Add(card);
                    y += 80;
                }

                flpKanban.Controls.Add(panelCol);
            }
        }

        private void BtnTaoTask_Click(object sender, EventArgs e)
        {
            if (!(cboFormDuAn.SelectedValue is int maDuAn)) return;

            var cv = new CongViecDTO
            {
                MaDuAn = maDuAn,
                TenTask = txtTenTask.Text.Trim(),
                MoTa = txtMoTa.Text.Trim(),
                HanNop = dtpHanNop.Value,
                DoUuTien = cboUuTien.Text,
                TrangThai = cboFormTrangThai.Text
            };

            var res = taskBus.Insert(cv);
            MessageBox.Show(res.msg);
            if (res.ok)
            {
                txtTenTask.Clear();
                txtMoTa.Clear();
                LoadTasks();
            }
        }

        private void BtnCapNhatTT_Click(object sender, EventArgs e)
        {
            if (dgvTasks.CurrentRow?.DataBoundItem is CongViecDTO selected)
            {
                var res = taskBus.UpdateTrangThai(selected.MaTask, cboDoiTrangThai.Text);
                MessageBox.Show(res.msg);
                if (res.ok) LoadTasks();
            }
            else
            {
                MessageBox.Show("Chọn 1 task ở bảng List View để đổi trạng thái.");
            }
        }
    }
}