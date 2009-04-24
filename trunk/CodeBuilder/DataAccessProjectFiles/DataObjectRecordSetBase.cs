using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace MY_NAMESPACE.Core
{    
    #region DataObjectRecordSetBase
    public class DataObjectRecordSetBase<T> : List<T> where T : DataObjectBase
    {
    }
    #endregion
}