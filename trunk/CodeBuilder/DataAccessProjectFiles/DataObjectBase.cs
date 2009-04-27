using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MY_NAMESPACE.Core
{
    #region EntityStatus
    public enum EntityStatus
	{
		Default,
		Changed,
		New,
		Deleted,
        ChangeMany
	}
	#endregion
	
	#region DataObjectBase
    public class DataObjectBase
    {
        public EntityStatus EntityStatus { get; set; }

        public DataObjectBase()
		{
            EntityStatus = EntityStatus.New;
		}
    }
    #endregion
}
