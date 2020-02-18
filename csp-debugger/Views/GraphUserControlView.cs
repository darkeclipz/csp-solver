using Csp.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Csp.Debugger.Views
{
    public class GraphUserControlView
    {
        private CspModel model;

        public CspModel Model
        {
            get
            {
                return model;
            }
            set
            {
                model = value;
                OnViewUpdate?.Invoke();
            }
        }

        public delegate void ViewUpdate();
        public ViewUpdate OnViewUpdate;
    }
}
