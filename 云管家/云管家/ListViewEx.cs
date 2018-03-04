using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace 云管家
{
    public partial class ListViewEx : Component
    {
        public ListViewEx()
        {
            InitializeComponent();
        }

        public ListViewEx(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}
