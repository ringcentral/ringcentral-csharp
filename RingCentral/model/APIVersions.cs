using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace RingCentral.Model
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class APIVersions :  IEquatable<APIVersions>
    { 
    
        /// <summary>
        /// Initializes a new instance of the <see cref="APIVersions" /> class.
        /// Initializes a new instance of the <see cref="APIVersions" />class.
        /// </summary>
        /// <param name="Uri">Canonical URI of the API version.</param>
        /// <param name="ApiVersions">Description of product.</param>
        /// <param name="ServerVersion">Server version.</param>
        /// <param name="ServerRevision">Server revision.</param>

        public APIVersions(string Uri = null, List<VersionInfo> ApiVersions = null, string ServerVersion = null, string ServerRevision = null)
        {
            this.Uri = Uri;
            this.ApiVersions = ApiVersions;
            this.ServerVersion = ServerVersion;
            this.ServerRevision = ServerRevision;
            
        }
        
    
        /// <summary>
        /// Canonical URI of the API version
        /// </summary>
        /// <value>Canonical URI of the API version</value>
        [DataMember(Name="uri", EmitDefaultValue=false)]
        public string Uri { get; set; }
    
        /// <summary>
        /// Description of product
        /// </summary>
        /// <value>Description of product</value>
        [DataMember(Name="apiVersions", EmitDefaultValue=false)]
        public List<VersionInfo> ApiVersions { get; set; }
    
        /// <summary>
        /// Server version
        /// </summary>
        /// <value>Server version</value>
        [DataMember(Name="serverVersion", EmitDefaultValue=false)]
        public string ServerVersion { get; set; }
    
        /// <summary>
        /// Server revision
        /// </summary>
        /// <value>Server revision</value>
        [DataMember(Name="serverRevision", EmitDefaultValue=false)]
        public string ServerRevision { get; set; }
    
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class APIVersions {\n");
            sb.Append("  Uri: ").Append(Uri).Append("\n");
            sb.Append("  ApiVersions: ").Append(ApiVersions).Append("\n");
            sb.Append("  ServerVersion: ").Append(ServerVersion).Append("\n");
            sb.Append("  ServerRevision: ").Append(ServerRevision).Append("\n");
            
            sb.Append("}\n");
            return sb.ToString();
        }
  
        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object obj)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            return this.Equals(obj as APIVersions);
        }

        /// <summary>
        /// Returns true if APIVersions instances are equal
        /// </summary>
        /// <param name="other">Instance of APIVersions to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(APIVersions other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.Uri == other.Uri ||
                    this.Uri != null &&
                    this.Uri.Equals(other.Uri)
                ) && 
                (
                    this.ApiVersions == other.ApiVersions ||
                    this.ApiVersions != null &&
                    this.ApiVersions.SequenceEqual(other.ApiVersions)
                ) && 
                (
                    this.ServerVersion == other.ServerVersion ||
                    this.ServerVersion != null &&
                    this.ServerVersion.Equals(other.ServerVersion)
                ) && 
                (
                    this.ServerRevision == other.ServerRevision ||
                    this.ServerRevision != null &&
                    this.ServerRevision.Equals(other.ServerRevision)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            // credit: http://stackoverflow.com/a/263416/677735
            unchecked // Overflow is fine, just wrap
            {
                int hash = 41;
                // Suitable nullity checks etc, of course :)
                
                if (this.Uri != null)
                    hash = hash * 59 + this.Uri.GetHashCode();
                
                if (this.ApiVersions != null)
                    hash = hash * 59 + this.ApiVersions.GetHashCode();
                
                if (this.ServerVersion != null)
                    hash = hash * 59 + this.ServerVersion.GetHashCode();
                
                if (this.ServerRevision != null)
                    hash = hash * 59 + this.ServerRevision.GetHashCode();
                
                return hash;
            }
        }

    }
}
