﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Needletail.Mvc.Communications;
using System.Web.Http;

namespace Needletail.Mvc
{
    public class TwoWayResult : ActionResult
    {

        internal ClientCall Call { get; private set; }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="call">The call to execute on the client</param>
        public TwoWayResult(ClientCall call)
        {
            this.Call = call;
        }

        /// <summary>
        /// The override does not send anything but makes a client call
        /// </summary>
        public override void ExecuteResult(ControllerContext context)
        {
            //make the client call
            if (string.IsNullOrEmpty(this.Call.ClientId))
                RemoteExecution.BroadcastExecuteOnClient(this.Call);
            else
                RemoteExecution.ExecuteOnClient(this.Call,false);
            
            //send just success
            var jsonp = new JavaScriptSerializer().Serialize(new { success = true });
            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.Write(jsonp);
        }
    }
}
