using System;
using System.Windows.Forms;
using System.Linq;

namespace ProtocolClient
{
    public partial class MapTypeSettingForm : Form
    {
        public string KeyType { get; set; }
        public string ValueType { get; set; }

        private ComboBox keyTypeComboBox;
        private ComboBox valueTypeComboBox;
        private Button okButton;
        private Button cancelButton;

        public MapTypeSettingForm(string keyType, string valueType)
        {
            InitializeComponent();
            InitializeControls();
            InitKeyType(keyType);
            InitValueType(valueType);
        }

        private void InitializeComponent()
        {
            this.Text = "设置Map类型";
            this.Size = new System.Drawing.Size(300, 150);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // 初始化控件
            keyTypeComboBox = new ComboBox();
            valueTypeComboBox = new ComboBox();
            okButton = new Button();
            cancelButton = new Button();

            // 设置控件属性
            keyTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            keyTypeComboBox.Location = new System.Drawing.Point(100, 20);
            keyTypeComboBox.Size = new System.Drawing.Size(150, 21);

            valueTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            valueTypeComboBox.Location = new System.Drawing.Point(100, 50);
            valueTypeComboBox.Size = new System.Drawing.Size(150, 21);

            okButton.Text = "确定";
            okButton.DialogResult = DialogResult.OK;
            okButton.Location = new System.Drawing.Point(70, 80);
            okButton.Click += OkButton_Click;

            cancelButton.Text = "取消";
            cancelButton.DialogResult = DialogResult.Cancel;
            cancelButton.Location = new System.Drawing.Point(170, 80);

            // 添加标签
            Label keyLabel = new Label
            {
                Text = "Key类型:",
                Location = new System.Drawing.Point(20, 23),
                AutoSize = true
            };

            Label valueLabel = new Label
            {
                Text = "Value类型:",
                Location = new System.Drawing.Point(20, 53),
                AutoSize = true
            };

            // 添加控件到窗体
            this.Controls.AddRange(new Control[] {
                keyLabel,
                valueLabel,
                keyTypeComboBox,
                valueTypeComboBox,
                okButton,
                cancelButton
            });
        }
        private void InitKeyType(string str)
        {
            keyTypeComboBox.SelectedIndex = 0;

            if (keyTypeComboBox.Items.Count <= 0)
                return;

            if (str == null)
            {
                return;
            }

            for (int i = 0; i < keyTypeComboBox.Items.Count; i++)
            {
                if (str == keyTypeComboBox.Items[i].ToString())
                {
                    keyTypeComboBox.SelectedIndex = i;
                }
            }
        }
        private void InitValueType(string str)
        {
            valueTypeComboBox.SelectedIndex = 0;

            if (valueTypeComboBox.Items.Count <= 0)
                return;

            if (str == null)
            {
                return;
            }

            for (int i = 0; i < valueTypeComboBox.Items.Count; i++)
            {
                if (str == valueTypeComboBox.Items[i].ToString())
                {
                    valueTypeComboBox.SelectedIndex = i;
                }
            }
        }
        private void InitializeControls()
        {
            var fieldList = Global.FieldTypeList.ToArray();
            var types = fieldList.Where(t => t.ToString() != "map").ToArray();
            keyTypeComboBox.Items.AddRange(Global.MapKeyTypeArray);
            valueTypeComboBox.Items.AddRange(types);
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            KeyType = keyTypeComboBox.SelectedItem.ToString();
            ValueType = valueTypeComboBox.SelectedItem.ToString();
        }
    }
}