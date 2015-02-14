using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace DrKCrazyAttendance_Instructor
{
    //http://paulstovell.com/blog/dynamic-datagrid
    public class CustomBoundColumn : DataGridBoundColumn
    {
        public string TemplateName { get; set; }

        protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
        {
            var binding = new Binding(((Binding)Binding).Path.Path);
            binding.Source = dataItem;

            var content = new ContentControl();
            content.ContentTemplate = (DataTemplate)cell.FindResource(TemplateName);
            content.SetBinding(ContentControl.ContentProperty, binding);
            return content;
        }

        protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
        {
            return GenerateElement(cell, dataItem);
        }
    }
}
