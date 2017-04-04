using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Final
{
    public class LoadBtnCmd : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private MainData mpd;

        public LoadBtnCmd(MainData maindata)
        {
            this.mpd = maindata;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            mpd.CallApi();
        }

        public void FireCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

       

    
    }
}
