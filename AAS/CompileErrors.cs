using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.CodeDom.Compiler;

namespace AAS
{
    public partial class CompileErrors : Form
    {
        public CompileErrors(CompilerErrorCollection errors)
        {
            InitializeComponent();

            for (int i = 0; i < errors.Count; i++)
            {
                errors[i].ErrorText = errors[i].Line + ": " + errors[i].ErrorText;
            }
            listBox1.DataSource = errors;
            listBox1.DisplayMember = "ErrorText";
        }
    }
}
