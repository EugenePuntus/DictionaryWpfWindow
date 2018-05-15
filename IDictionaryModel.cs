using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryWpfWindow
{
    public interface IDictionaryModel
    {
        string Name { get; }

        void Add();

        void Save();

        void Refresh();

        ObservableCollection<object> Data { get; set; }
    }
}
