using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryWpfWindow
{
    public class GridColumnSetting : Attribute
    {
        public GridColumnSetting()
        {
            Visible = true;
            IsReadOnly = false;
        }

        public GridColumnSetting(string displayName) : this()
        {
            DisplayName = displayName;
        }

        public GridColumnSetting(string displayName, bool visible) : this(displayName)
        {
            Visible = visible;
        }

        public GridColumnSetting(string displayName, bool visible, string selectedValue) : this(displayName, visible)
        {
            SelectedValue = selectedValue;
        }

        public GridColumnSetting(string displayName, bool visible, string selectedValue, string displayNameValue) : this(displayName, visible, selectedValue)
        {
            DisplayNameValue = displayNameValue;
        }

        public bool Visible { get; set; }
        public bool IsReadOnly { get; set; }
        public string DisplayName { get; set; }
        public string SelectedValue { get; set; }
        public string DisplayNameValue { get; set; }
    }
}
