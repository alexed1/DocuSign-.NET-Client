﻿// -----------------------------------------------------------------------
// <copyright file="Error.json.cs" company="DocuSign, Inc.">
// Copyright (c) DocuSign, Inc. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Net;

namespace DocuSign.Integrations.Client
{
    using System;
    using Newtonsoft.Json;

    /// <summary>
    /// Json class for error information
    /// </summary>
    [Serializable]
    public class Error
    {
        /// <summary>
        /// Gets or sets the original http status code
        /// </summary>
        [JsonIgnore] 
        public HttpStatusCode httpStatusCode { get; set; }

        /// <summary>
        /// Gets or sets the error code property
        /// </summary>
        public string errorCode { get; set; }

        /// <summary>
        /// Gets or sets the error message property
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// Deserializes Json text into the object's properties
        /// </summary>
        /// <param name="json">string of Json text</param>
        /// <returns>AccountCreate instance</returns>
        public static Error FromJson(string json)
        {
            return JsonConvert.DeserializeObject<Error>(json);
        }

        /// <summary>
        /// Serializes self
        /// </summary>
        /// <returns>serialized Json text</returns>
        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}