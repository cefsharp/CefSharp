// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.WebAuthn
{
    using System.Linq;

    /// <summary>
    /// This domain allows configuring virtual authenticators to test the WebAuthn
    public partial class WebAuthn : DevToolsDomainBase
    {
        public WebAuthn(CefSharp.DevTools.DevToolsClient client)
        {
            _client = (client);
        }

        private CefSharp.DevTools.DevToolsClient _client;
        /// <summary>
        /// Enable the WebAuthn domain and start intercepting credential storage and
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> EnableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("WebAuthn.enable", dict);
            return methodResult;
        }

        /// <summary>
        /// Disable the WebAuthn domain.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DisableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("WebAuthn.disable", dict);
            return methodResult;
        }

        /// <summary>
        /// Creates and adds a virtual authenticator.
        /// </summary>
        public async System.Threading.Tasks.Task<AddVirtualAuthenticatorResponse> AddVirtualAuthenticatorAsync(CefSharp.DevTools.WebAuthn.VirtualAuthenticatorOptions options)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("options", options.ToDictionary());
            var methodResult = await _client.ExecuteDevToolsMethodAsync("WebAuthn.addVirtualAuthenticator", dict);
            return methodResult.DeserializeJson<AddVirtualAuthenticatorResponse>();
        }

        /// <summary>
        /// Removes the given authenticator.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> RemoveVirtualAuthenticatorAsync(string authenticatorId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("authenticatorId", authenticatorId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("WebAuthn.removeVirtualAuthenticator", dict);
            return methodResult;
        }

        /// <summary>
        /// Adds the credential to the specified authenticator.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> AddCredentialAsync(string authenticatorId, CefSharp.DevTools.WebAuthn.Credential credential)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("authenticatorId", authenticatorId);
            dict.Add("credential", credential.ToDictionary());
            var methodResult = await _client.ExecuteDevToolsMethodAsync("WebAuthn.addCredential", dict);
            return methodResult;
        }

        /// <summary>
        /// Returns a single credential stored in the given virtual authenticator that
        public async System.Threading.Tasks.Task<GetCredentialResponse> GetCredentialAsync(string authenticatorId, byte[] credentialId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("authenticatorId", authenticatorId);
            dict.Add("credentialId", ToBase64String(credentialId));
            var methodResult = await _client.ExecuteDevToolsMethodAsync("WebAuthn.getCredential", dict);
            return methodResult.DeserializeJson<GetCredentialResponse>();
        }

        /// <summary>
        /// Returns all the credentials stored in the given virtual authenticator.
        /// </summary>
        public async System.Threading.Tasks.Task<GetCredentialsResponse> GetCredentialsAsync(string authenticatorId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("authenticatorId", authenticatorId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("WebAuthn.getCredentials", dict);
            return methodResult.DeserializeJson<GetCredentialsResponse>();
        }

        /// <summary>
        /// Removes a credential from the authenticator.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> RemoveCredentialAsync(string authenticatorId, byte[] credentialId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("authenticatorId", authenticatorId);
            dict.Add("credentialId", ToBase64String(credentialId));
            var methodResult = await _client.ExecuteDevToolsMethodAsync("WebAuthn.removeCredential", dict);
            return methodResult;
        }

        /// <summary>
        /// Clears all the credentials from the specified device.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> ClearCredentialsAsync(string authenticatorId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("authenticatorId", authenticatorId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("WebAuthn.clearCredentials", dict);
            return methodResult;
        }

        /// <summary>
        /// Sets whether User Verification succeeds or fails for an authenticator.
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetUserVerifiedAsync(string authenticatorId, bool isUserVerified)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("authenticatorId", authenticatorId);
            dict.Add("isUserVerified", isUserVerified);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("WebAuthn.setUserVerified", dict);
            return methodResult;
        }
    }
}