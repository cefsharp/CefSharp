// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.WebAuthn
{
    using System.Linq;

    /// <summary>
    /// This domain allows configuring virtual authenticators to test the WebAuthn
    /// API.
    /// </summary>
    public partial class WebAuthn : DevToolsDomainBase
    {
        private CefSharp.DevTools.IDevToolsClient _client;
        public WebAuthn(CefSharp.DevTools.IDevToolsClient client)
        {
            _client = (client);
        }

        /// <summary>
        /// Enable the WebAuthn domain and start intercepting credential storage and
        /// retrieval with a virtual authenticator.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> EnableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("WebAuthn.enable", dict);
            return methodResult;
        }

        /// <summary>
        /// Disable the WebAuthn domain.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DisableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("WebAuthn.disable", dict);
            return methodResult;
        }

        partial void ValidateAddVirtualAuthenticator(CefSharp.DevTools.WebAuthn.VirtualAuthenticatorOptions options);
        /// <summary>
        /// Creates and adds a virtual authenticator.
        /// </summary>
        /// <param name = "options">options</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;AddVirtualAuthenticatorResponse&gt;</returns>
        public async System.Threading.Tasks.Task<AddVirtualAuthenticatorResponse> AddVirtualAuthenticatorAsync(CefSharp.DevTools.WebAuthn.VirtualAuthenticatorOptions options)
        {
            ValidateAddVirtualAuthenticator(options);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("options", options.ToDictionary());
            var methodResult = await _client.ExecuteDevToolsMethodAsync("WebAuthn.addVirtualAuthenticator", dict);
            return methodResult.DeserializeJson<AddVirtualAuthenticatorResponse>();
        }

        partial void ValidateRemoveVirtualAuthenticator(string authenticatorId);
        /// <summary>
        /// Removes the given authenticator.
        /// </summary>
        /// <param name = "authenticatorId">authenticatorId</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> RemoveVirtualAuthenticatorAsync(string authenticatorId)
        {
            ValidateRemoveVirtualAuthenticator(authenticatorId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("authenticatorId", authenticatorId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("WebAuthn.removeVirtualAuthenticator", dict);
            return methodResult;
        }

        partial void ValidateAddCredential(string authenticatorId, CefSharp.DevTools.WebAuthn.Credential credential);
        /// <summary>
        /// Adds the credential to the specified authenticator.
        /// </summary>
        /// <param name = "authenticatorId">authenticatorId</param>
        /// <param name = "credential">credential</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> AddCredentialAsync(string authenticatorId, CefSharp.DevTools.WebAuthn.Credential credential)
        {
            ValidateAddCredential(authenticatorId, credential);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("authenticatorId", authenticatorId);
            dict.Add("credential", credential.ToDictionary());
            var methodResult = await _client.ExecuteDevToolsMethodAsync("WebAuthn.addCredential", dict);
            return methodResult;
        }

        partial void ValidateGetCredential(string authenticatorId, byte[] credentialId);
        /// <summary>
        /// Returns a single credential stored in the given virtual authenticator that
        /// matches the credential ID.
        /// </summary>
        /// <param name = "authenticatorId">authenticatorId</param>
        /// <param name = "credentialId">credentialId</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetCredentialResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetCredentialResponse> GetCredentialAsync(string authenticatorId, byte[] credentialId)
        {
            ValidateGetCredential(authenticatorId, credentialId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("authenticatorId", authenticatorId);
            dict.Add("credentialId", ToBase64String(credentialId));
            var methodResult = await _client.ExecuteDevToolsMethodAsync("WebAuthn.getCredential", dict);
            return methodResult.DeserializeJson<GetCredentialResponse>();
        }

        partial void ValidateGetCredentials(string authenticatorId);
        /// <summary>
        /// Returns all the credentials stored in the given virtual authenticator.
        /// </summary>
        /// <param name = "authenticatorId">authenticatorId</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetCredentialsResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetCredentialsResponse> GetCredentialsAsync(string authenticatorId)
        {
            ValidateGetCredentials(authenticatorId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("authenticatorId", authenticatorId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("WebAuthn.getCredentials", dict);
            return methodResult.DeserializeJson<GetCredentialsResponse>();
        }

        partial void ValidateRemoveCredential(string authenticatorId, byte[] credentialId);
        /// <summary>
        /// Removes a credential from the authenticator.
        /// </summary>
        /// <param name = "authenticatorId">authenticatorId</param>
        /// <param name = "credentialId">credentialId</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> RemoveCredentialAsync(string authenticatorId, byte[] credentialId)
        {
            ValidateRemoveCredential(authenticatorId, credentialId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("authenticatorId", authenticatorId);
            dict.Add("credentialId", ToBase64String(credentialId));
            var methodResult = await _client.ExecuteDevToolsMethodAsync("WebAuthn.removeCredential", dict);
            return methodResult;
        }

        partial void ValidateClearCredentials(string authenticatorId);
        /// <summary>
        /// Clears all the credentials from the specified device.
        /// </summary>
        /// <param name = "authenticatorId">authenticatorId</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> ClearCredentialsAsync(string authenticatorId)
        {
            ValidateClearCredentials(authenticatorId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("authenticatorId", authenticatorId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("WebAuthn.clearCredentials", dict);
            return methodResult;
        }

        partial void ValidateSetUserVerified(string authenticatorId, bool isUserVerified);
        /// <summary>
        /// Sets whether User Verification succeeds or fails for an authenticator.
        /// The default is true.
        /// </summary>
        /// <param name = "authenticatorId">authenticatorId</param>
        /// <param name = "isUserVerified">isUserVerified</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetUserVerifiedAsync(string authenticatorId, bool isUserVerified)
        {
            ValidateSetUserVerified(authenticatorId, isUserVerified);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("authenticatorId", authenticatorId);
            dict.Add("isUserVerified", isUserVerified);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("WebAuthn.setUserVerified", dict);
            return methodResult;
        }
    }
}