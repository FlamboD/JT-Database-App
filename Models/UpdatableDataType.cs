using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JT_Database_App.Models
{
    internal class UpdatableDataType<T>
    {
        private bool isUpdated;
        public T data { get; private set; }

        public UpdatableDataType(T data)
        {
            this.data = data;
            this.isUpdated = true;
        }

        public UpdatableDataType(T data, bool isUpdated)
        {
            this.data= data;
            this.isUpdated = isUpdated;
        }

        public bool Update(T data)
        {
            this.data = data;
            this.isUpdated = true;
            return this.isUpdated;
        }
    }
}
