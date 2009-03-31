/*-------------------------------------------------------------------------
 * PGORMLogger.cs
 *
 * This file is part of the PGORM project.
 * http://pgorm.googlecode.com/
 *
 * Copyright (c) 2002-2009, TrueSoftware B.V.
 *
 * IDENTIFICATION
 * 
 *  $Id$
 * 	$HeadURL$
 * 	
 *-------------------------------------------------------------------------
 */
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Microsoft.Build.BuildEngine;
using Microsoft.Build.Framework;

namespace PGORM
{
    public class PGORMLogger : ILogger
    {
        private Builder builder;
        public bool HasErrors { get; set; }
        public List<PGORMLoaggerException> Exceptions = new List<PGORMLoaggerException>();

        public PGORMLogger(Builder p_builder)
        {
            HasErrors = false;
            builder = p_builder;
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
            Exceptions.Add(new PGORMLoaggerException(e));
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