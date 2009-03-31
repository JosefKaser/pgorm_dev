using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.BuildEngine;
using Microsoft.Build.Framework;
using PGORM;

namespace PGORMWizard
{
    public class WizardLogger  : ILogger
    {
        private Builder builder;
        public bool HasErrors { get; set; }

        public WizardLogger(Builder p_builder)
        {
            HasErrors = false;
            builder = p_builder;
            builder.CustomLogger = this;
        }
        #region ILogger Members

        public void Initialize(IEventSource eventSource)
        {
            eventSource.ErrorRaised += new BuildErrorEventHandler(eventSource_ErrorRaised);
            eventSource.AnyEventRaised += new AnyEventHandler(eventSource_AnyEventRaised);
        }

        void eventSource_AnyEventRaised(object sender, BuildEventArgs e)
        {
            object o = e;
        }

        void eventSource_ErrorRaised(object sender, BuildErrorEventArgs e)
        {
            HasErrors = true;
            builder.SendMessage(this, BuilderMessageType.Error,
                string.Format("File:{0}\r\nLine Number:{1}\r\n",
                e.File,
                e.LineNumber
                )
                );
        }

        public string Parameters
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void Shutdown()
        {
            throw new NotImplementedException();
        }

        public LoggerVerbosity Verbosity
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
